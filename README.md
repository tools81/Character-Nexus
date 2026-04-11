# Character Nexus

[![tools81](https://img.shields.io/badge/Created_by_-tools81-blue)](https://github.com/tools81)

Character Nexus is a TTRPG character creation application. A React frontend presents a dynamically generated form per ruleset, and an ASP.NET 8 API backend handles schema generation, character serialization, and storage via Azure Blob Storage.

---

## Architecture

### React Frontend (`CharacterNexus/ClientApp`)

The frontend is a React + TypeScript SPA using React Hook Form for all form state management.

**Key flows:**

- On load, the frontend fetches the ruleset's `Form.json` schema from the API. This schema drives every aspect of the form — field types, labels, options, validations, and calculated values.
- React Hook Form (`useForm`, `useWatch`, `setValue`, `getValues`) manages all field state. Every bonus, characteristic, and derived stat is wired through this form state so the entire sheet is consistent at all times.
- Bonus state is held in two React state arrays — `bonusAdjustments` and `bonusCharacteristics` — which are updated as the user makes selections. Dedicated hooks translate those arrays into actual form field changes.

**Key hooks:**

| Hook | Purpose |
|---|---|
| `useBonusAdjustments` | Diffs added/removed numeric adjustments and applies them as `+value` / `-value` to the target field. |
| `useBonusCharacteristics` | Diffs added/removed characteristics and sets or clears string/array/boolean fields accordingly. |
| `useConditionalBonuses` | Watches condition fields and promotes or demotes conditional bonuses into the active bonus state when conditions are met or no longer met. |
| `useFieldCalculations` | Watches dependency fields and re-evaluates formula-based derived stats (e.g. Hit Points, Defense) whenever their inputs change. |

**Key components:**

| Component | Purpose |
|---|---|
| `InputSelect` | Dropdown that fires bonus/characteristic/user-choice logic on change via `data-` attributes on the hidden `<select>`. |
| `InputNumber` | Number input that can carry bonus adjustments. |
| `InputSwitch` | Toggle input that can carry bonus adjustments. |
| `LeftCollapsiblePane` | Collapsible side panel showing `pinnedStat` derived fields (e.g. HP, Defense, Hit Dice). |
| `RightCollapsiblePane` | Collapsible side panel showing ruleset instructions. |

---

### ASP.NET Backend (`CharacterNexus`)

The API is an ASP.NET 8 application. Ruleset implementations are injected as `IRuleset` singletons at startup (`Startup.cs`).

**Key endpoints (via `RulesetController`):**
- `GET /api/ruleset` — returns all registered rulesets
- `GET /api/ruleset/newcharacter` — returns the `Form.json` schema for the active ruleset
- `POST /api/character/save` — deserializes the form submission into a typed `ICharacter` model and persists it
- `GET /api/character/load` — retrieves a stored character and serializes it back to the React form shape

**Ruleset registration:**

Each ruleset is registered in `Startup.cs`:

```csharp
services.AddSingleton<IRuleset, EverydayHeroes.Ruleset>();
```

And mapped in `appsettings.json` so the UI can identify it by display name:

```json
"MappingsRuleset": {
  "Everyday Heroes": "EverydayHeroes"
}
```

---

### Azure Blob Storage (`Storage.AzureBlobStorage`)

Characters are persisted as JSON blobs. The `IStorage` interface abstracts the storage layer so it can be swapped. The Azure implementation is injected in `Startup.cs`:

```csharp
services.AddSingleton<IStorage, AzureBlobStorage.Storage>();
```

Connection credentials are resolved via Azure Key Vault (configured in `appsettings.json` under `KeyVault`).

---

## Adding a New Ruleset

### 1. Create the project

Copy `Ruleset.Template` to a new project named `Ruleset.MyRulesetName`. Update all `Template` namespace references to `MyRulesetName`.

### 2. Add JSON data files

In the `Json/` folder, define each ruleset concept as a JSON array. Each file maps to a C# model class you create.

Common patterns:

- A selection that numerically modifies a field → add a `BonusAdjustments` property (list of `BonusAdjustment`)
- A selection that sets or adds to another characteristic → add a `BonusCharacteristics` property (list of `BonusCharacteristic`)
- A selection that is only available when a condition is met → add a `Prerequisites` property (list of `Prerequisite`)
- A selection that prompts the user to pick from further options → add a `UserChoices` property (list of `UserChoice`)

See the [Bonus System](#bonus-system) section below for the full data contract.

### 3. Create C# model classes

Create a class for each JSON concept (e.g. `Class`, `Background`, `Skill`). Classes that represent selectable options should include the relevant bonus properties from the `Utility` project.

The `Character` model must include every field that will be on the character sheet, plus `Id` (Guid) and `Image`.

### 4. Implement `GenerateFormSchema`

`GenerateFormSchema.InitializeSchema()` is called at build time (via `SchemaGenerator`) and writes `Json/Character/Form.json`. This file is the source of truth for the React form.

The schema is a JSON object with a `fields` array. Each field object describes one input:

```json
{
  "name": "background",
  "id": "background",
  "label": "Background",
  "type": "select",
  "className": "form-control",
  "options": [
    {
      "value": "Scholar",
      "label": "Scholar",
      "bonusAdjustments": "[{\"type\":\"attribute\",\"name\":\"Intelligence\",\"value\":1}]",
      "bonusCharacteristics": "[{\"type\":\"skill\",\"value\":\"History\"}]",
      "userChoices": null
    }
  ]
}
```

**Field types** recognized by the React renderer: `hidden`, `text`, `number`, `select`, `checkbox`, `switch`, `textarea`, `image`, `array`, `modifiableitem`, `accordion`, `listgroup`, `divider`, `linebreak`, `card`, `div`.

**Special field properties:**

| Property | Effect |
|---|---|
| `pinnedStat: true` | Field is excluded from the main form render and shown in the left derived-stats panel instead. It is still registered as a hidden input so React Hook Form tracks it. |
| `calculation: "<formula>"` | Field value is computed at runtime. Use `[fieldName]` tokens to reference other fields. Use `values.fieldName` for dynamic key lookups (e.g. `values.attribute[someKey]`). |
| `prerequisites` | Array of `Prerequisite` objects. The field is disabled until all prerequisites pass. |

Use `JsonConvert.SerializeObject(list, _jsonSettings)` (camelCase contract resolver) when embedding bonus data as `data-` attribute strings on option elements.

After modifying `GenerateFormSchema.cs`, rebuild the `SchemaGenerator` project — its post-build event runs the generator and rewrites all `Form.json` files.

### 5. Implement `CharacterJsonConverter`

`CharacterJsonConverter` is a Newtonsoft `JsonConverter` subclass with two jobs:

- **`ReadJson`** — maps the flat React form submission (keyed by field `name`) into the structured `Character` model.
- **`WriteJson`** — serializes a `Character` back to the flat form shape so a saved character can be loaded into the React form.

These two methods must be exact inverses of each other. If `ReadJson` reads `formData["attribute"]["Strength"]` into `character.Attributes["Strength"].Value`, then `WriteJson` must write it back under the same key.

### 6. Update `Ruleset.cs`

Set the static properties on the `Ruleset` class:

```csharp
public string Name => "My Ruleset";           // Display name
public string RulesetName => "Ruleset.MyRulesetName"; // Project name
public string ImageSource => "...";           // Character image placeholder URL
public string LogoSource => "...";            // Ruleset logo URL
public string FormResource => "Ruleset.MyRulesetName.Json.Character.Form.json"; // Embedded resource path
public string Instructions => "...";          // HTML shown in the right panel
public string Stylesheet => "...";            // Optional CSS class for theming
```

### 7. Register in SchemaGenerator

Add the namespace reference to `SchemaGenerator/Program.cs` and call `InitializeSchema`:

```csharp
MyRulesetName.GenerateFormSchema.InitializeSchema();
```

### 8. Register in the API

In `CharacterNexus/Startup.cs`:

```csharp
services.AddSingleton<IRuleset, MyRulesetName.Ruleset>();
```

In `CharacterNexus/appsettings.json`:

```json
"MappingsRuleset": {
  "My Ruleset Name": "MyRulesetName"
}
```

---

## Bonus System

The bonus system is the primary mechanism for making selections on the character sheet affect other fields automatically.

### BonusAdjustment

Adds or subtracts a numeric value from a target field when a selection is made.

```json
{
  "type": "attribute",
  "name": "Strength",
  "value": 2
}
```

- `type` — the top-level form field name (e.g. `"attribute"`, `"wealthlevel"`)
- `name` — sub-field name, combined as `type.name` (e.g. `"attribute.Strength"`). Leave empty if `type` is the full path.
- `value` — integer delta, positive to add, negative to subtract

When a selection is changed, the old adjustment is subtracted and the new one is added. The `useBonusAdjustments` hook diffs the active list and applies only the delta.

### BonusCharacteristic

Sets a value on a target field when a selection is made. The behavior depends on the target field type:

- **String/hidden input** — sets the field directly to `value`
- **Array field** — appends `{ value }` to the array. If `value` is a comma-separated list, each item is appended individually. Supports quantity via `"ItemName|3"` syntax.
- **Boolean object** — sets `fieldName.value` to `true`

```json
{
  "type": "skill",
  "value": "History,Arcana"
}
```

When the selection is removed, the characteristic is reversed using the same field-type detection logic.

### Conditional Bonuses

Both `BonusAdjustment` and `BonusCharacteristic` support a `conditions` array. A conditional bonus is only applied when **all** conditions evaluate to true (AND logic).

```json
{
  "type": "talent",
  "value": "Crush",
  "conditions": [
    { "type": "Class", "name": "strong", "formula": "=== 'Brawler'" },
    { "type": "Level", "formula": ">= 3" }
  ]
}
```

Each condition:
- `type` + `name` resolve to a form field path (`type.name`, lowercased)
- `formula` is a JavaScript expression fragment appended to the field value, e.g. `>= 3`, `=== 'Brawler'`
- If the resolved field is an array, `=== "X"` is evaluated as `.includes("X")`

The `useConditionalBonuses` hook watches all condition fields and promotes/demotes bonuses into the active state as values change.

### UserChoice

When a selection should prompt the user to pick from a further set of options, use `UserChoices`. When the input is changed, a modal opens with the specified choices.

```json
{
  "type": "skill",
  "label": "Choose a skill",
  "count": 2,
  "category": "Characteristic",
  "choices": ["History", "Arcana", "Athletics"]
}
```

- `type` — the field type the chosen values will be applied to
- `count` — how many selections the user must make
- `category` — `"Characteristic"` to apply as `BonusCharacteristic`, `"Adjustment"` for `BonusAdjustment`
- `choices` — array of available option values

### Prerequisite

Prerequisites control whether a field or option is available based on current form values.

```json
{
  "type": "level",
  "formula": ">= 5"
}
```

- `type` + `name` resolve to a form field path (same convention as `BonusCondition`)
- `formula` — JavaScript expression fragment evaluated against the current field value
- `logicalOr` — nested list of prerequisites where any one passing is sufficient

If prerequisites are not met, the field is rendered as disabled. The `useDisableEngine` hook evaluates all prerequisites reactively.

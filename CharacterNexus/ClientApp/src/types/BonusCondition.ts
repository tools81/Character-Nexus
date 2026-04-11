export interface BonusCondition {
    type: string;    // maps to the form field type/path segment, e.g. "Archetype" or "level"
    name?: string;   // optional sub-path, combined as "type.name" (matching C# BonusCondition)
    formula: string; // same syntax as Prerequisite.formula, e.g. ">= 4" or "=== \"Fighter\""
}
import { useState, useEffect, useRef, useCallback } from "react";
import { UseFormUnregister, UseFormGetValues, UseFormSetValue, UseFormWatch } from "react-hook-form";
import { UserChoices, UserChoice } from "../types/UserChoice";
import { BonusAdjustments } from "../types/BonusAdjustment";
import { BonusCharacteristics } from "../types/BonusCharacteristic";
import { forEachFieldInstance, selectNameFor, parseJsonAttr, findOption } from "../utils/schemaFieldInstances";
import { rowIndexIn, shiftRowPath } from "../utils/arrayRowPaths";

export type ChoicesByOrigin = Record<string, UserChoices>;

interface UseUserChoicesArgs {
  unregister: UseFormUnregister<any>;
  getValues: UseFormGetValues<any>;
  setValue: UseFormSetValue<any>;
  watch: UseFormWatch<any>;
  setBonusCharacteristics: React.Dispatch<React.SetStateAction<BonusCharacteristics>>;
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>;
}

const buildChoiceFields = (origin: string, choices: UserChoices): any[] => {
  const fields: any[] = [];

  choices.forEach((item: UserChoice) => {
    item.choices.forEach((choice: any, index: number) => {
      const name = `choice.${item.type}.${origin}.${index}`;
      const description = item.choiceDescriptions?.[index];

      if (item.category === "Characteristic") {
        const bonusChar = { origin, type: item.type, value: choice };
        fields.push({
          id: name,
          key: name,
          name,
          origin,
          choiceType: item.type,
          label: choice,
          description,
          type: "switch",
          defaultValue: false,
          bonusCharacteristics: JSON.stringify([bonusChar])
        });
      }
      if (item.category === "Adjustment") {
        const bonusAdj = { type: item.type, name: choice, value: 0 };
        fields.push({
          id: name,
          key: name,
          name,
          origin,
          choiceType: item.type,
          label: choice,
          description,
          type: "stepper",
          bonusAdjustments: JSON.stringify([bonusAdj]),
          defaultValue: 0
        });
      }
    });
  });

  return fields;
};

const withOrigin = (choices: UserChoices, origin: string): UserChoices =>
  choices.map(c => ({ ...c, origin }));

// Every select whose saved value is an option that grants user choices
const collectSavedChoices = (
  fields: any[],
  getValues: UseFormGetValues<any>
): Map<string, UserChoices> => {
  const result = new Map<string, UserChoices>();

  forEachFieldInstance(fields, getValues, (field, name, inArrayRow) => {
    if (field.type !== "select" && field.type !== "modifiableitem") return;
    const origin = selectNameFor(name, inArrayRow);
    const choices = parseJsonAttr(findOption(field.options, getValues(origin))?.userChoices);
    if (choices?.length) result.set(origin, withOrigin(choices, origin));
  });

  return result;
};

const fieldNamesFor = (origin: string, choices: UserChoices) =>
  buildChoiceFields(origin, choices).map(f => f.name);

export function useUserChoices({
  unregister,
  getValues,
  setValue,
  watch,
  setBonusCharacteristics,
  setBonusAdjustments,
}: UseUserChoicesArgs) {
  // Choices currently offered, keyed by the name of the field that produced them
  const [choicesByOrigin, setChoicesByOrigin] = useState<ChoicesByOrigin>({});
  const [choiceFields, setChoiceFields] = useState<any[]>([]);

  // Source of truth for the state above; kept in a ref so updates can read it synchronously
  const choicesRef = useRef<Map<string, UserChoices>>(new Map());
  // Track which fields have already been initialized to avoid resetting user-entered values
  const initializedRef = useRef<Set<string>>(new Set());
  const watchedRef = useRef<Set<string>>(new Set());

  const commit = useCallback(() => {
    const entries = Array.from(choicesRef.current.entries());
    setChoicesByOrigin(Object.fromEntries(entries));
    setChoiceFields(entries.flatMap(([origin, choices]) => buildChoiceFields(origin, choices)));
  }, []);

  const forgetField = useCallback((name: string) => {
    unregister(name);
    initializedRef.current.delete(name);
    watchedRef.current.delete(name);
  }, [unregister]);

  // Replace the choices offered by a single origin field. Passing an empty
  // list clears that origin, which collapses its choice panel.
  const setOriginChoices = useCallback((origin: string, choices: UserChoices) => {
    const oldChoices = choicesRef.current.get(origin);
    if (!oldChoices && choices.length === 0) return;

    if (oldChoices) {
      const oldNames = new Set(fieldNamesFor(origin, oldChoices));
      // Drop bonuses granted by this origin's old choices; the bonus hooks undo
      // their effect on the form values when they leave state.
      setBonusCharacteristics(prev => prev.filter(c => !c.origin || !oldNames.has(c.origin)));
      setBonusAdjustments(prev => prev.filter(a => !a.origin || !oldNames.has(a.origin)));
      // Unregister ONLY fields from this origin — preserve all other origins' fields
      oldNames.forEach(forgetField);
    }

    if (choices.length > 0) choicesRef.current.set(origin, choices);
    else choicesRef.current.delete(origin);
    commit();
  }, [forgetField, commit, setBonusCharacteristics, setBonusAdjustments]);

  // Rebuild choice panels for a freshly loaded character, keeping its saved
  // choice values. Returns the rebuilt choice fields so their bonuses can be restored.
  const restoreChoices = useCallback((schemaFields: any[]): any[] => {
    choicesRef.current = collectSavedChoices(schemaFields, getValues);
    const fields = Array.from(choicesRef.current.entries())
      .flatMap(([origin, choices]) => buildChoiceFields(origin, choices));

    // Values came from the saved character, so don't overwrite them with defaults
    initializedRef.current = new Set(fields.map(f => f.name));
    watchedRef.current = new Set();
    commit();
    return fields;
  }, [getValues, commit]);

  // Call before removing row `index` of array `arrayName`: drops that row's
  // choices and moves later rows' choices (and their values) up by one.
  // Bonus state for these choices is handled by the caller.
  const shiftChoicesForRemovedRow = useCallback((arrayName: string, index: number) => {
    const affected = Array.from(choicesRef.current.entries()).filter(([origin]) => {
      const row = rowIndexIn(origin, arrayName);
      return row !== null && row >= index;
    });
    if (affected.length === 0) return;

    // Snapshot values of rows that move, before unregistering anything
    const moving = affected
      .filter(([origin]) => rowIndexIn(origin, arrayName)! > index)
      .map(([origin, choices]) => {
        const newOrigin = shiftRowPath(origin, arrayName, index);
        const values = fieldNamesFor(origin, choices).map(name => ({
          newName: shiftRowPath(name, arrayName, index),
          value: getValues(name),
        }));
        return { newOrigin, choices: withOrigin(choices, newOrigin), values };
      });

    affected.forEach(([origin, choices]) => {
      fieldNamesFor(origin, choices).forEach(forgetField);
      choicesRef.current.delete(origin);
    });

    moving.forEach(({ newOrigin, choices, values }) => {
      choicesRef.current.set(newOrigin, choices);
      values.forEach(({ newName, value }) => {
        setValue(newName, value);
        initializedRef.current.add(newName);
      });
    });
    commit();
  }, [getValues, setValue, forgetField, commit]);

  // Initialize values for NEW fields only — don't reset already-initialized fields
  useEffect(() => {
    if (!choiceFields.length) return;
    choiceFields.forEach((field) => {
      if (!initializedRef.current.has(field.name)) {
        setValue(field.name, field.defaultValue ?? 0);
        initializedRef.current.add(field.name);
      }
    });
  }, [choiceFields, setValue]);

  // Watch dynamic choice fields
  useEffect(() => {
    if (!choiceFields.length) return;
    choiceFields.forEach((field) => {
      if (!watchedRef.current.has(field.name)) {
        watch(field.name);
        watchedRef.current.add(field.name);
      }
    });
  }, [choiceFields, watch]);

  return { choicesByOrigin, choiceFields, setOriginChoices, restoreChoices, shiftChoicesForRemovedRow };
}

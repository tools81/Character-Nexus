import { useState, useEffect, useRef } from "react";
import { UseFormUnregister, UseFormSetValue, UseFormWatch } from "react-hook-form";
import { UserChoices, UserChoice } from "../types/UserChoice";
import { UseUserChoiceModalReturn } from "./useModal";

export function useUserChoices(
  unregister: UseFormUnregister<any>,
  setValue: UseFormSetValue<any>,
  watch: UseFormWatch<any>,
  userChoiceModal: UseUserChoiceModalReturn
) {
  const [userChoices, setUserChoices] = useState<UserChoices>([]);
  const [choiceFields, setChoiceFields] = useState<any[]>([]);

  // Track field names per origin so we only clean up same-origin fields on re-selection
  const originFieldsRef = useRef<Map<string, string[]>>(new Map());
  // Track which fields have already been initialized to avoid resetting user-entered values
  const initializedRef = useRef<Set<string>>(new Set());
  const watchedRef = useRef<Set<string>>(new Set());

  const openUserChoiceModal = (choices: UserChoices) => {
    // Determine which origins the new choices belong to
    const newOrigins = Array.from(new Set(choices.map(c => c.origin).filter(Boolean) as string[]));
    const newOriginsSet = new Set(newOrigins);

    // Unregister ONLY fields from the same origin(s) — preserve all other origins' fields
    newOrigins.forEach(origin => {
      const oldNames = originFieldsRef.current.get(origin) ?? [];
      oldNames.forEach(name => {
        unregister(name);
        initializedRef.current.delete(name);
      });
      originFieldsRef.current.delete(origin);
    });

    // Filter choiceFields to remove same-origin old fields, keeping everything else
    setChoiceFields(prev => prev.filter(f => {
      const parts = f.name?.split(".") ?? [];
      return parts.length < 3 || !newOriginsSet.has(parts[2]);
    }));

    setUserChoices(choices);
    userChoiceModal.open(choices);
  };

  // Build choice fields for the current userChoices and APPEND them (don't replace)
  useEffect(() => {
    if (!userChoices || userChoices.length === 0) return;

    const newFields: any[] = [];

    userChoices.forEach((item: UserChoice) => {
      const origin = item.origin ?? "unknown";
      const itemNames: string[] = [];

      item.choices.forEach((choice: any, index: any) => {
        const name = `choice.${item.type}.${origin}.${index}`;

        if (item.category === "Characteristic") {
          const bonusChar = { origin, type: item.type, value: choice };
          newFields.push({
            id: name,
            key: name,
            name,
            label: choice,
            type: "switch",
            defaultValue: false,
            bonusCharacteristics: JSON.stringify([bonusChar])
          });
        }
        if (item.category === "Adjustment") {
          const bonusAdj = { type: item.type, name: choice, value: 0 };
          newFields.push({
            id: name,
            key: name,
            name,
            label: choice,
            type: "number",
            bonusAdjustments: JSON.stringify([bonusAdj]),
            defaultValue: 0
          });
        }
        itemNames.push(name);
      });

      // Track names for this origin so we can clean them up on re-selection
      const existing = originFieldsRef.current.get(origin) ?? [];
      originFieldsRef.current.set(origin, [...existing, ...itemNames]);
    });

    // Append new fields — same-origin old fields were already removed in openUserChoiceModal
    setChoiceFields(prev => [...prev, ...newFields]);
  }, [userChoices]);

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

  return { userChoices, setUserChoices, choiceFields, openUserChoiceModal };
}

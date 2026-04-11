import { useEffect, useRef } from "react";
import { BonusCharacteristics } from "../types/BonusCharacteristic";
import { UseFormGetValues, UseFormSetValue, FieldValues } from "react-hook-form";

export function useBonusCharacteristics(
  bonusCharacteristics: BonusCharacteristics,
  getValues: UseFormGetValues<FieldValues>,
  setValue: UseFormSetValue<FieldValues>
) {
  const prevRef = useRef<BonusCharacteristics>([]);

  useEffect(() => {
    if (!Array.isArray(bonusCharacteristics)) return;

    const prev = prevRef.current;
    prevRef.current = bonusCharacteristics;

    // Characteristics removed since last run (in prev but not in new)
    const removed = prev.filter(p =>
      !bonusCharacteristics.some(
        c => c.origin === p.origin && c.type === p.type && c.value === p.value
      )
    );

    // Characteristics added since last run (in new but not in prev)
    const added = bonusCharacteristics.filter(c =>
      !prev.some(
        p => p.origin === c.origin && p.type === c.type && p.value === c.value
      )
    );

    // Undo removed characteristics first (reverse order)
    for (const char of [...removed].reverse()) {
      handleRemoveCharacteristic(getValues, setValue, char.type, char.value);
    }

    // Apply only newly added characteristics
    for (const char of added) {
      let fieldName = char.type;
      let choice = char.value;
      const periodIndex = choice.indexOf(".");
      if (periodIndex > -1) {
        fieldName = `${fieldName}.${choice.substring(0, periodIndex)}`;
        choice = choice.substring(periodIndex + 1);
      }
      handleSetFieldValue(getValues, setValue, fieldName, choice);
    }
  }, [bonusCharacteristics]);
}

const handleRemoveCharacteristic = (
  getValues: UseFormGetValues<FieldValues>,
  setValue: UseFormSetValue<FieldValues>,
  type: string,
  value: string
) => {
  let fieldName = type;
  let choice = value;
  const periodIndex = choice.indexOf(".");
  if (periodIndex > -1) {
    fieldName = `${fieldName}.${choice.substring(0, periodIndex)}`;
    choice = choice.substring(periodIndex + 1);
  }

  const currentValue = getValues(fieldName);

  if (Array.isArray(currentValue)) {
    const items = choice.split(",").map(s => s.trim()).filter(Boolean);
    const itemValues = items.map(item => {
      const pipeIndex = item.indexOf("|");
      return pipeIndex > -1 ? item.substring(0, pipeIndex) : item;
    });
    const newValue = currentValue.filter(
      (v: any) => !itemValues.includes(typeof v === "object" && v !== null ? v.value : v)
    );
    setValue(fieldName, newValue, { shouldDirty: true, shouldTouch: true, shouldValidate: true });
    return;
  }

  if (typeof currentValue === "string" || currentValue === null) {
    setValue(fieldName, "", { shouldDirty: true, shouldTouch: true, shouldValidate: true });
    return;
  }

  if (typeof currentValue === "object" && currentValue !== null) {
    setValue(`${fieldName}.${choice}`, false, { shouldDirty: true, shouldTouch: true, shouldValidate: true });
  }
};

const handleSetFieldValue = (
  getValues: UseFormGetValues<FieldValues>,
  setValue: UseFormSetValue<FieldValues>,
  fieldName: string,
  choice: string
) => {
  if (!fieldName || typeof fieldName !== "string") return;

  const currentValue = getValues(fieldName);

  // ─────────────────────────────────────────────
  // ARRAY FIELD (checkbox group, multi-select)
  // ─────────────────────────────────────────────
  if (Array.isArray(currentValue)) {
    const items = choice.split(",").map(s => s.trim()).filter(Boolean);
    let updated = [...currentValue];

    for (const item of items) {
      const pipeIndex = item.indexOf("|");
      const itemValue = pipeIndex > -1 ? item.substring(0, pipeIndex) : item;
      const quantity = pipeIndex > -1 ? parseInt(item.substring(pipeIndex + 1), 10) : null;

      const existingIndex = updated.findIndex(
        (v: any) => v === itemValue || v?.value === itemValue
      );

      if (existingIndex > -1) {
        if (quantity != null) {
          const existingQty = updated[existingIndex]?.quantity ?? 0;
          updated[existingIndex] = { ...updated[existingIndex], quantity: existingQty + quantity };
        }
      } else {
        updated = [...updated, quantity != null ? { value: itemValue, quantity } : { value: itemValue }];
      }
    }

    setValue(fieldName, updated, { shouldDirty: true, shouldTouch: true, shouldValidate: true });
    return;
  }

  // ─────────────────────────────────────────────
  // CHECKBOX (boolean)
  // ─────────────────────────────────────────────
  if (typeof currentValue === "object" && currentValue !== null) {
    let isBooleanObject = true;
    for (const key in currentValue) {
      if (Object.prototype.hasOwnProperty.call(currentValue, key)) {
        if (typeof currentValue[key] !== "boolean") {
          isBooleanObject = false;
        }
      }
    }
    if (isBooleanObject) {
      setValue(`${fieldName}.${choice}`, true, {
        shouldDirty: true,
        shouldTouch: true,
        shouldValidate: true
      });
      return;
    }
  }

  // ─────────────────────────────────────────────
  // PLAIN STRING / PRIMITIVE FIELD (e.g. hidden inputs like defensemodifier)
  // ─────────────────────────────────────────────
  if (typeof currentValue === "string" || currentValue === null) {
    setValue(fieldName, choice, { shouldDirty: true, shouldTouch: true, shouldValidate: true });
    return;
  }

  // ─────────────────────────────────────────────
  // EMPTY / UNINITIALIZED FIELD
  // ─────────────────────────────────────────────
  if (currentValue === undefined) {
    const pipeIndex = choice.indexOf("|");
    const itemValue = pipeIndex > -1 ? choice.substring(0, pipeIndex) : choice;
    const quantity = pipeIndex > -1 ? parseInt(choice.substring(pipeIndex + 1), 10) : null;

    setValue(
      fieldName,
      [quantity != null ? { value: itemValue, quantity } : { value: itemValue }],
      { shouldDirty: true, shouldTouch: true, shouldValidate: true }
    );
  }
};

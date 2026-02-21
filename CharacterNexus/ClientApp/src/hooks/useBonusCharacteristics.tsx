import { useEffect } from "react";
import { BonusCharacteristics } from "../types/BonusCharacteristic";
import { UseFormGetValues, UseFormSetValue, FieldValues } from "react-hook-form";

export function useBonusCharacteristics(
  bonusCharacteristics: BonusCharacteristics,
  getValues: UseFormGetValues<FieldValues>,
  setValue: UseFormSetValue<FieldValues>
) {

  useEffect(() => {
    if (!Array.isArray(bonusCharacteristics)) return;

    for (const characteristic of bonusCharacteristics) {
      const periodIndex: number = characteristic.value.indexOf(".");

      if (periodIndex > -1)
      {
        // If the value of the choice string contains a period, treat it as a sub name for an array. Use the value after the period to set the field.
        characteristic.type = `${characteristic.type}.${characteristic.value.substring(0, periodIndex)}`;
        characteristic.value = characteristic.value.substring(periodIndex + 1);
      }

      handleSetFieldValue(characteristic.type, characteristic.value);
    }

    console.log("Bonus Characteristics:", bonusCharacteristics);
  }, [bonusCharacteristics]);

  const handleSetFieldValue = (fieldName: string, choice: string) => {
    if (!fieldName || typeof fieldName !== "string") return;

    const currentValue = getValues(fieldName);

    // ─────────────────────────────────────────────
    // ARRAY FIELD (checkbox group, multi-select)
    // ─────────────────────────────────────────────
    if (Array.isArray(currentValue)) {
      const exists = currentValue.some(
        (v: any) => v === choice || v?.value === choice
      );      

      if (!exists) {
        setValue(
          fieldName,
          [...currentValue, { value: choice }],
          {
            shouldDirty: true,
            shouldTouch: true,
            shouldValidate: true
          }
        );
      }
      return;
    }

    // ─────────────────────────────────────────────
    // CHECKBOX (boolean)
    // ─────────────────────────────────────────────
    if (typeof currentValue === "object") {
      let isBooleanObject = true;
      for (const key in currentValue) {
        if (Object.prototype.hasOwnProperty.call(currentValue, key)) {
          if (typeof currentValue[key] !== 'boolean') {
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
    // EMPTY / UNINITIALIZED FIELD
    // Assume array intent (defensive default)
    // ─────────────────────────────────────────────
    if (currentValue === undefined) {
      setValue(
        fieldName,
        [{ value: choice }],
        {
          shouldDirty: true,
          shouldTouch: true,
          shouldValidate: true
        }
      );
    }
  };
}


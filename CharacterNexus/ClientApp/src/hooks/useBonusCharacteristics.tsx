import { useEffect } from "react";
import { BonusCharacteristics } from "../types/BonusCharacteristic";

export function useBonusCharacteristics(
  bonusCharacteristics: BonusCharacteristics,
  getValues: (field: string) => any,
  setValue: (field: string, value: any) => void
) {
  useEffect(() => {
    if (!Array.isArray(bonusCharacteristics)) return;

    for (const characteristic of bonusCharacteristics) {
      handleSetArrayValue(characteristic.type, characteristic.value);
    }
  }, [bonusCharacteristics, getValues, setValue]);

  const handleSetArrayValue = (array: string, choice: string) => {
    // Guard against undefined or non-string array names which can cause
    // react-hook-form internals to call string methods like `startsWith` on
    // an undefined value (observed as a runtime error when inputs update).
    if (!array || typeof array !== 'string') return;

    const current = Array.isArray(getValues(array)) ? getValues(array) : [];
    const exists = Array.isArray(current) && current.some((i: any) => i && i.value === choice);
    if (!exists) {
      setValue(array, [...current, { value: choice }]);
    }
  };
}

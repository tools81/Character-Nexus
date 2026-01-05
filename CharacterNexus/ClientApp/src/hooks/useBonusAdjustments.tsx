import { useEffect } from "react";
import { BonusAdjustments } from "../types/BonusAdjustment";

export function useBonusAdjustments(
  bonusAdjustments: BonusAdjustments,
  getValues: (field: string) => any,
  setValue: (field: string, value: any) => void
) {
  useEffect(() => {
    if (!Array.isArray(bonusAdjustments)) return;

    for (const adjustment of bonusAdjustments) {
      handleSetArrayValue(adjustment.type, adjustment.value);
    }
  }, [bonusAdjustments, getValues, setValue]);

  const handleSetArrayValue = (array: string, value: number) => {
    // Guard against undefined or non-string array names which can cause
    // react-hook-form internals to call string methods like `startsWith` on
    // an undefined value (observed as a runtime error when inputs update).
    if (!array || typeof array !== 'string') return;

    const current = Array.isArray(getValues(array)) ? getValues(array) : [];
    const exists = Array.isArray(current) && current.some((i: any) => i && i.value === value);
    if (!exists) {
      setValue(array, [...current, { value: value }]);
    }
  };
}

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
      handleSetValue(adjustment.type, adjustment.name, adjustment.value);
    }

    console.log("Bonus Adjustments:", bonusAdjustments);
  }, [bonusAdjustments]);

  const handleSetValue = (type: string, name: string, bonusValue: number) => {
    // Guard against undefined or non-string array names which can cause
    // react-hook-form internals to call string methods like `startsWith` on
    // an undefined value (observed as a runtime error when inputs update).
    if (!type || typeof type !== 'string') return;

    if (name && typeof name === 'string' && name !== "") {
      type = `${type}.${name}`;
    }

    const current = getValues(type);
    setValue(type, +current + +bonusValue);
    // const exists = current.some((i: any) => i && i.value === bonusValue);

    // if (!exists) {
    //   setValue(type, [...current, { value: bonusValue }]);
    // }
  };
}

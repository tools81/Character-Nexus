import { useEffect } from "react";
import { BonusAdjustments } from "../types/BonusAdjustment";
import { toCamelCase } from "../utils/toCamelCase";

export function useBonusAdjustments(
  bonusAdjustments: BonusAdjustments,
  getValues: (field: string) => any,
  setValue: (field: string, value: any) => void
) {
  useEffect(() => {
    if (!Array.isArray(bonusAdjustments)) return;

    for (const adjustment of bonusAdjustments) {
      const type = toCamelCase(adjustment.type);
      const current = getValues(type);
      const next = current + adjustment.value;
      if (next !== current) {
        setValue(type, next);
      }
    }
  }, [bonusAdjustments, getValues, setValue]);
}

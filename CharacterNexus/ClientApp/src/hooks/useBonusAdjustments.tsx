import { useEffect, useRef } from "react";
import { BonusAdjustments } from "../types/BonusAdjustment";

/**
 * Walks the form values object case-insensitively and returns the actual
 * registered key path (e.g. "HitPointBase" → "hitpointbase").
 * Falls back to the original segment when no match is found.
 */
function resolveFieldPath(allValues: any, path: string): string {
  const parts = path.split(".");
  const resolved: string[] = [];
  let current = allValues;
  for (const part of parts) {
    if (current == null || typeof current !== "object") {
      resolved.push(part);
      continue;
    }
    const key = Object.keys(current).find(k => k.toLowerCase() === part.toLowerCase());
    resolved.push(key ?? part);
    current = key != null ? current[key] : undefined;
  }
  return resolved.join(".");
}

export function useBonusAdjustments(
  bonusAdjustments: BonusAdjustments,
  getValues: (field: string) => any,
  setValue: (field: string, value: any) => void
) {
  const prevRef = useRef<BonusAdjustments>([]);

  useEffect(() => {
    if (!Array.isArray(bonusAdjustments)) return;

    const prev = prevRef.current;
    prevRef.current = bonusAdjustments;

    // Adjustments removed since last run
    const removed = prev.filter(p =>
      !bonusAdjustments.some(
        a => a.origin === p.origin && a.type === p.type && a.name === p.name && a.value === p.value
      )
    );

    // Adjustments added since last run
    const added = bonusAdjustments.filter(a =>
      !prev.some(
        p => p.origin === a.origin && p.type === a.type && p.name === a.name && p.value === a.value
      )
    );

    // Undo removed adjustments first (subtract)
    for (const adj of [...removed].reverse()) {
      applyAdjustment(adj.type, adj.name, -adj.value);
    }

    // Apply only newly added adjustments
    for (const adj of added) {
      applyAdjustment(adj.type, adj.name, adj.value);
    }
  }, [bonusAdjustments]);

  const applyAdjustment = (type: string, name: string, bonusValue: number) => {
    if (!type || typeof type !== 'string') return;

    let fieldType = type;
    if (name && typeof name === 'string' && name !== "") {
      fieldType = `${fieldType}.${name}`;
    }

    const allValues = (getValues as unknown as () => any)();
    const fieldPath = resolveFieldPath(allValues, fieldType);

    const current = getValues(fieldPath) ?? 0;
    setValue(fieldPath, +current + +bonusValue);
  };
}

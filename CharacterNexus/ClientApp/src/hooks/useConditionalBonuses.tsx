import { useEffect, useRef } from "react";
import { UseFormGetValues, UseFormWatch } from "react-hook-form";
import { BonusAdjustment, BonusAdjustments } from "../types/BonusAdjustment";
import { BonusCharacteristic, BonusCharacteristics } from "../types/BonusCharacteristic";
import { BonusCondition } from "../types/BonusCondition";

/**
 * Resolves a BonusCondition to a form field path.
 * Mirrors the prerequisite path convention: "type.name" when name is present,
 * otherwise just "type" — lowercased to match registered field names.
 */
function conditionFieldPath(condition: BonusCondition): string {
  const base = condition.type ?? "";
  const sub = condition.name ?? "";
  const path = (base && sub) ? `${base}.${sub}` : (base || sub);
  return path.toLowerCase();
}

/**
 * Evaluates all conditions on a bonus item (AND logic).
 * Handles array fields via Array.includes when the current value is an array.
 */
function evaluateConditions(
  conditions: BonusCondition[],
  getValues: UseFormGetValues<any>
): boolean {
  for (const condition of conditions) {
    const fieldPath = conditionFieldPath(condition);
    const value = getValues(fieldPath);

    let result: boolean;
    try {
      if (Array.isArray(value)) {
        // For array fields, rewrite "=== X" as ".includes(X)" so a class list
        // like ["Fighter","Rogue"] satisfies === "Fighter".
        const includesMatch = condition.formula.match(/^===\s*(.+)$/);
        if (includesMatch) {
          const target = Function(`return ${includesMatch[1]}`)();
          result = value.includes(target);
        } else {
          const expr = `${JSON.stringify(value)}${condition.formula}`;
          result = Function(`return ${expr}`)();
        }
      } else {
        const expr = `${JSON.stringify(value)}${condition.formula}`;
        result = Function(`return ${expr}`)();
      }
    } catch {
      result = false;
    }

    if (!result) return false;
  }
  return true;
}

/**
 * Watches all condition fields from pending conditional bonuses and keeps the
 * active bonus state arrays in sync as form values change.
 *
 * Bonuses without conditions are never touched — they flow through the
 * existing useBonusAdjustments / useBonusCharacteristics hooks unchanged.
 */
export function useConditionalBonuses(
  bonusAdjustments: BonusAdjustments,
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>,
  bonusCharacteristics: BonusCharacteristics,
  setBonusCharacteristics: React.Dispatch<React.SetStateAction<BonusCharacteristics>>,
  getValues: UseFormGetValues<any>,
  watch: UseFormWatch<any>
) {
  // Track which conditional bonus keys are currently active so we can diff
  // against them on each re-evaluation.
  const activeAdjustmentKeys = useRef<Set<string>>(new Set());
  const activeCharacteristicKeys = useRef<Set<string>>(new Set());

  useEffect(() => {
    // Collect the unique set of condition fields we need to watch.
    const conditionFields = new Set<string>();

    for (const adj of bonusAdjustments) {
      if (adj.conditions?.length) adj.conditions.forEach(c => conditionFields.add(conditionFieldPath(c)));
    }
    for (const char of bonusCharacteristics) {
      if (char.conditions?.length) char.conditions.forEach(c => conditionFields.add(conditionFieldPath(c)));
    }

    if (conditionFields.size === 0) return;

    const evaluate = () => {
      // ── Adjustments ────────────────────────────────────────────────────────
      const nextAdjKeys = new Set<string>();
      const toAddAdj: BonusAdjustment[] = [];
      const toRemoveAdjKeys: string[] = [];

      bonusAdjustments.forEach((adj, idx) => {
        if (!adj.conditions?.length) return; // unconditional — leave alone

        const key = `${adj.origin ?? ""}:conditional:adj:${idx}`;
        const qualifies = evaluateConditions(adj.conditions, getValues);

        if (qualifies) {
          nextAdjKeys.add(key);
          if (!activeAdjustmentKeys.current.has(key)) {
            toAddAdj.push({ ...adj, origin: key, conditions: undefined });
          }
        } else {
          if (activeAdjustmentKeys.current.has(key)) {
            toRemoveAdjKeys.push(key);
          }
        }
      });

      if (toAddAdj.length > 0 || toRemoveAdjKeys.length > 0) {
        activeAdjustmentKeys.current = nextAdjKeys;
        setBonusAdjustments(prev => {
          const filtered = prev.filter(a => !toRemoveAdjKeys.includes(a.origin ?? ""));
          return [...filtered, ...toAddAdj];
        });
      }

      // ── Characteristics ────────────────────────────────────────────────────
      const nextCharKeys = new Set<string>();
      const toAddChar: BonusCharacteristic[] = [];
      const toRemoveCharKeys: string[] = [];

      bonusCharacteristics.forEach((char, idx) => {
        if (!char.conditions?.length) return;

        const key = `${char.origin ?? ""}:conditional:char:${idx}`;
        const qualifies = evaluateConditions(char.conditions, getValues);

        if (qualifies) {
          nextCharKeys.add(key);
          if (!activeCharacteristicKeys.current.has(key)) {
            toAddChar.push({ ...char, origin: key, conditions: undefined });
          }
        } else {
          if (activeCharacteristicKeys.current.has(key)) {
            toRemoveCharKeys.push(key);
          }
        }
      });

      if (toAddChar.length > 0 || toRemoveCharKeys.length > 0) {
        activeCharacteristicKeys.current = nextCharKeys;
        setBonusCharacteristics(prev => {
          const filtered = prev.filter(c => !toRemoveCharKeys.includes(c.origin ?? ""));
          return [...filtered, ...toAddChar];
        });
      }
    };

    // Run immediately and subscribe to future changes on condition fields.
    evaluate();
    const subscription = watch((_, { name }) => {
      if (name && conditionFields.has(name)) evaluate();
    });

    return () => subscription.unsubscribe();
  }, [bonusAdjustments, bonusCharacteristics]);
}

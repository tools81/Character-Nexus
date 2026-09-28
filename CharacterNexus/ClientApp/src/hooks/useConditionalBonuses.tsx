import { useCallback, useEffect, useRef } from "react";
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
 * Pairs each conditional bonus with a stable key: its origin plus its position
 * among that origin's conditional bonuses. Keys don't shift when bonuses from
 * other origins are added or removed.
 */
export function conditionalEntries<T extends { origin?: string; conditions?: BonusCondition[] }>(
  bonuses: T[],
  kind: "adj" | "char"
): { key: string; bonus: T }[] {
  const counts = new Map<string, number>();
  const result: { key: string; bonus: T }[] = [];
  for (const bonus of bonuses) {
    if (!bonus.conditions?.length) continue;
    const origin = bonus.origin ?? "";
    const n = counts.get(origin) ?? 0;
    counts.set(origin, n + 1);
    result.push({ key: `${origin}:conditional:${kind}:${n}`, bonus });
  }
  return result;
}

/**
 * Returns the active (condition-satisfied) copies for a list of bonuses, as the
 * hook would add them to state. Used to rebuild state for a loaded character.
 */
export function activeConditionalCopies<T extends { origin?: string; conditions?: BonusCondition[] }>(
  bonuses: T[],
  kind: "adj" | "char",
  getValues: UseFormGetValues<any>
): { key: string; copy: T }[] {
  return conditionalEntries(bonuses, kind)
    .filter(({ bonus }) => evaluateConditions(bonus.conditions!, getValues))
    .map(({ key, bonus }) => ({ key, copy: { ...bonus, origin: key, conditions: undefined } }));
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

    // Nothing to evaluate and nothing active to clean up
    if (conditionFields.size === 0 && activeAdjustmentKeys.current.size === 0 && activeCharacteristicKeys.current.size === 0) return;

    const evaluate = () => {
      // ── Adjustments ────────────────────────────────────────────────────────
      const nextAdjKeys = new Set<string>();
      const toAddAdj: BonusAdjustment[] = [];

      for (const { key, bonus } of conditionalEntries(bonusAdjustments, "adj")) {
        if (!evaluateConditions(bonus.conditions!, getValues)) continue;
        nextAdjKeys.add(key);
        if (!activeAdjustmentKeys.current.has(key)) {
          toAddAdj.push({ ...bonus, origin: key, conditions: undefined });
        }
      }
      // Remove copies whose condition failed or whose source bonus is gone
      const toRemoveAdjKeys = Array.from(activeAdjustmentKeys.current).filter(k => !nextAdjKeys.has(k));

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

      for (const { key, bonus } of conditionalEntries(bonusCharacteristics, "char")) {
        if (!evaluateConditions(bonus.conditions!, getValues)) continue;
        nextCharKeys.add(key);
        if (!activeCharacteristicKeys.current.has(key)) {
          toAddChar.push({ ...bonus, origin: key, conditions: undefined });
        }
      }
      const toRemoveCharKeys = Array.from(activeCharacteristicKeys.current).filter(k => !nextCharKeys.has(k));

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
    if (conditionFields.size === 0) return;
    const subscription = watch((_, { name }) => {
      if (name && conditionFields.has(name)) evaluate();
    });

    return () => subscription.unsubscribe();
  }, [bonusAdjustments, bonusCharacteristics]);

  // Replace the active key sets (used when rebuilding state for a loaded character)
  const resetActiveKeys = useCallback((adjKeys: string[], charKeys: string[]) => {
    activeAdjustmentKeys.current = new Set(adjKeys);
    activeCharacteristicKeys.current = new Set(charKeys);
  }, []);

  // Rename or drop (return null) active keys, e.g. when array rows shift
  const rewriteActiveKeys = useCallback((rewrite: (key: string) => string | null) => {
    const apply = (keys: Set<string>) =>
      new Set(Array.from(keys).map(rewrite).filter((k): k is string => k !== null));
    activeAdjustmentKeys.current = apply(activeAdjustmentKeys.current);
    activeCharacteristicKeys.current = apply(activeCharacteristicKeys.current);
  }, []);

  return { resetActiveKeys, rewriteActiveKeys };
}

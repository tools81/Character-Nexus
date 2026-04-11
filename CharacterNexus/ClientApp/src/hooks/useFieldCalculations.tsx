import { useEffect, useRef } from "react";

function getValueCaseInsensitive(allValues: any, path: string): any {
  const parts = path.split(".");
  let current = allValues;
  for (const part of parts) {
    if (current == null || typeof current !== "object") return undefined;
    const key = Object.keys(current).find(k => k.toLowerCase() === part.toLowerCase());
    if (key === undefined) return undefined;
    current = current[key];
  }
  return current;
}

export function useFieldCalculations(
  schema: any,
  getValues: (field: string) => any,
  setValue: (field: string, value: any) => void,
  watch?: (callback: () => void) => { unsubscribe: () => void }
) {
  // If watch is not provided, the hook cannot function
  if (!watch) return;

  const calculatedFieldsRef = useRef<Array<{ name: string; calculation: string }> | null>(null);
  const prevDependencyValuesRef = useRef<Record<string, any>>({});

  useEffect(() => {
    if (!schema || !Array.isArray(schema.fields)) return;

    // Collect all fields with calculations (only once per schema change)
    const calculatedFields: Array<{ name: string; calculation: string }> = [];
    const dependencyNames = new Set<string>();

    const collectCalculatedFields = (field: any) => {
      if (field.calculation) {
        calculatedFields.push({ name: field.name, calculation: field.calculation });

        // Extract field names from brackets [fieldName], skipping JS property access like obj[prop]
        const matches = field.calculation.match(/(?<![.\w])\[([^\]]+)\]/g);
        if (matches) {
          console.log(`Field "${field.name}" depends on:`, matches.map((m: string) => m.slice(1, -1)));
          matches.forEach((match: string) => {
            const depName = match.slice(1, -1); // Remove brackets
            dependencyNames.add(depName);
          });
        }
      }
      
      // Recurse into nested components (for accordion, listgroup, etc.)
      if (field.component && field.component.items) {
        field.component.items.forEach((item: any) => collectCalculatedFields(item));
      }
      if (Array.isArray(field.items)) {
        field.items.forEach((item: any) => collectCalculatedFields(item));
      }
    };

    schema.fields.forEach((field: any) => {
      collectCalculatedFields(field);
    });

    if (calculatedFields.length === 0) return;

    calculatedFieldsRef.current = calculatedFields;

    // Seed prevDependencyValuesRef with current values so the first watch
    // callback correctly detects changes (important for loaded characters).
    const allValues = (getValues as () => any)();
    dependencyNames.forEach(depName => {
      prevDependencyValuesRef.current[depName] = getValueCaseInsensitive(allValues, depName);
    });

    // Run immediately so derived stats are populated on mount.
    for (const { name: fieldName, calculation } of calculatedFields) {
      evaluateCalculation(fieldName, calculation);
    }

    // Watch only the dependency fields and recalculate when they change
    const unsubscribe = watch(() => {
      if (!calculatedFieldsRef.current) return;

      // Check if any dependency values have changed
      let hasChanged = false;
      for (const { calculation } of calculatedFieldsRef.current) {
        const matches = calculation.match(/(?<![.\w])\[([^\]]+)\]/g);
        if (matches) {
          for (const match of matches) {
            const depName = match.slice(1, -1);
            const newValue = getValueCaseInsensitive((getValues as () => any)(), depName);
            const oldValue = prevDependencyValuesRef.current[depName];
            if (newValue !== oldValue) {
              prevDependencyValuesRef.current[depName] = newValue;
              hasChanged = true;
            }
          }
        }
      }

      // Only recalculate if a dependency actually changed
      if (hasChanged) {
        for (const { name: fieldName, calculation } of calculatedFieldsRef.current) {
          evaluateCalculation(fieldName, calculation);
        }
      }
    });

    return () => {
      if (unsubscribe) unsubscribe.unsubscribe();
    };
  }, [schema, watch, getValues, setValue]);

  const evaluateCalculation = (fieldName: string, calculation: string) => {
    if (!calculation) return;

    // Only match [token] when NOT preceded by an identifier char or dot,
    // so that JS property access like values.attribute[mod] is left alone.
    const tokenRegex = /(?<![.\w])\[([^\]]+)\]/g;
    let calcFields = calculation.match(tokenRegex);
    if (calcFields) {
      const snapshot = (getValues as () => any)();
      let formula = calculation;
      calcFields.forEach((calcField: string) => {
        const depName = calcField.slice(1, -1); // Remove the square brackets
        const depValue = getValueCaseInsensitive(snapshot, depName) ?? 0;
        // If the value is numeric (including numeric strings from hidden inputs),
        // substitute as a plain number so + performs arithmetic, not concatenation.
        const numericValue = Number(depValue);
        const substituted = (depValue !== "" && depValue !== null && !isNaN(numericValue))
          ? String(numericValue)
          : JSON.stringify(depValue ?? "");
        formula = formula.replace(new RegExp(calcField.replace(/[.*+?^${}()|[\]\\]/g, "\\$&"), "g"), substituted);
      });

      try {
        // eslint-disable-next-line no-new-func
        const newValue = new Function("values", "return " + formula)(snapshot);
        const currentValue = getValues(fieldName);
        if (currentValue !== newValue) {
          setValue(fieldName, newValue);
        }
      } catch (e) {
        console.warn("Error evaluating calculation for field:", fieldName, "formula:", formula, e);
      }
    }
  };
}
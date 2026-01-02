import { useEffect, useRef } from "react";

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

        // Extract field names from brackets [fieldName]
        const matches = field.calculation.match(/\[([^\]]+)\]/g);
        if (matches) {
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

    // Watch only the dependency fields and recalculate when they change
    const unsubscribe = watch(() => {
      if (!calculatedFieldsRef.current) return;

      // Check if any dependency values have changed
      let hasChanged = false;
      for (const { calculation } of calculatedFieldsRef.current) {
        const matches = calculation.match(/\[([^\]]+)\]/g);
        if (matches) {
          for (const match of matches) {
            const depName = match.slice(1, -1);
            const newValue = getValues(depName);
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

    let calcFields = calculation.match(/\[([^\]]+)\]/g);
    if (calcFields) {
      let formula = calculation;
      calcFields.forEach((calcField: string) => {
        const depName = calcField.slice(1, -1); // Remove the square brackets
        const depValue = getValues(depName) !== undefined ? getValues(depName) : 0;
        formula = formula.replace(calcField, JSON.stringify(depValue));
      });

      try {
        // eslint-disable-next-line no-new-func
        const newValue = Function("return " + formula)();
        const currentValue = getValues(fieldName);
        console.log(`Calculating field: ${fieldName}, formula: ${formula}, newValue: ${newValue}, currentValue: ${currentValue}`);
        if (currentValue !== newValue) {
          setValue(fieldName, newValue);
        }
      } catch (e) {
        console.warn("Error evaluating calculation for field:", fieldName, "formula:", formula, e);
      }
    }
  };
}
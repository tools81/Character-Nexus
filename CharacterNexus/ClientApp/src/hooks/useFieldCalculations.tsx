import { useEffect } from 'react';
import { useFormContext } from 'react-hook-form';

const useFieldCalculations = (schema: any, set: Set<unknown>) => {
  const { watch, setValue, getValues } = useFormContext();

  useEffect(() => {
    if (!schema) return;

    const subscription = watch((value, { name }) => {
      schema.fields.forEach((field: any) => {
        if (field.calculation) {
          let calcFields = field.calculation.match(/\[([^\]]+)\]/g);

          if (calcFields) {
            let calculation = field.calculation;

            calcFields.forEach((calcField: string) => {
              let fieldName = calcField.slice(1, -1); // Remove the square brackets

              if (!set.has(fieldName)) {
                watch(fieldName);
                set.add(fieldName);
              }

              let calcValue = getValues(fieldName) || 0;
              calculation = calculation.replace(calcField, calcValue);
            });

            // Only set the value if it has changed to avoid infinite loop
            const newValue = Function("return " + calculation)();
            if (getValues(field.name) !== newValue) {
              setValue(field.name, newValue);
            }
          }
        }
      });
    });
    return () => subscription.unsubscribe();
  }, [watch, setValue, schema]);
};

export default useFieldCalculations;
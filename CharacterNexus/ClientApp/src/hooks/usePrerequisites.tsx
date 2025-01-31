import { useEffect } from 'react';
import { useFormContext } from 'react-hook-form';

export const HandlePrerequisite = (schema: any, set: Set<unknown>) => {
  const { watch, setValue, getValues } = useFormContext();

  useEffect(() => {
    if (!schema) return;

    let meetsPrerequisites = true;
    const subscription = watch((value, { name }) => {
      schema.fields.forEach((field: any) => {
        if (field.prerequisites) {
          field.prerequisites.forEach((prerequisite: any) => {
            let fieldName = prerequisite.type ? prerequisite.type + "." + prerequisite.name : prerequisite.name;

            if (!set.has(fieldName)) {
              watch(fieldName);
              set.add(fieldName);
            }

            // Only set the value if it has changed to avoid infinite loop
            const meetsPrerequisite = Function("return " + getValues(field.name) + prerequisite.formula)();
            if (!meetsPrerequisite) {
              meetsPrerequisites = false;
            }
          });          
        }

        field.disabled = !meetsPrerequisites;
      });
    });
    return () => subscription.unsubscribe();
  }, [watch, setValue, schema]);
};
import { useEffect, useState, useRef } from 'react';
import { useFormContext, useWatch } from 'react-hook-form';
import { Prerequisite } from '../types/Prerequisite';

// Accept optional form methods so the hook can be used either with FormProvider or directly
export const usePrerequisites = (schema: any, set: any, methods?: any) => {
  const formMethods = methods ? methods : useFormContext();
  const { watch, setValue, getValues } = formMethods;
  const [, setDisabledState] = useState<Record<string, boolean>>({}); // trigger re-renders
  const prevDisabledRef = useRef<string>('');

  useEffect(() => {
    if (!schema) return;
    // Pre-collect prerequisite field names and subscribe to them once so
    // we don't call `watch` repeatedly inside the subscription callback
    const prereqNames = new Set<string>();
    const collectNames = (field: any) => {
      const component = field && field.component ? field.component : field;
      if (!component || !component.prerequisites) return;
      try {
        const arr = JSON.parse(component.prerequisites) as Prerequisite[];
        for (const prerequisite of arr) {
          const fieldName = prerequisite.type != ''
            ? prerequisite.type + '.' + prerequisite.name
            : prerequisite.name;
          prereqNames.add(fieldName);
        }
      } catch (e) {
        // ignore parse errors
      }
      if (component && Array.isArray(component.items)) {
        for (const child of component.items) collectNames(child);
      }
    };

    if (Array.isArray(schema.fields)) {
      schema.fields.forEach((group: any) => {
        if (Array.isArray(group.items)) {
          group.items.forEach((field: any) => collectNames(field));
        }
      });
    }

    // register those names with watch and the provided set
    prereqNames.forEach((name) => {
      try {
        watch(name);
      } catch (e) {
        // watch may throw if the name is invalid; ignore
      }
      if (set && !set.has(name)) set.add(name);
    });

    // Subscribe to form changes and evaluate prerequisites
    const subscription = watch(() => {
      const disabledUpdates: Record<string, boolean> = {};

      // Recursive function to process nested fields/items
      const evaluateField = (field: any) => {
        let meetsPrerequisites = true;

        // component may be field.component or the field itself
        const component = field && field.component ? field.component : field;

        // If the component has prerequisites, evaluate them
        if (component && component.prerequisites != null && component.prerequisites !== 'null' && component.prerequisites !== '') {
          for (const prerequisite of JSON.parse(component.prerequisites) as Prerequisite[]) {
            const fieldName = prerequisite.type != ''
              ? prerequisite.type + "." + prerequisite.name
              : prerequisite.name;

            const value = getValues(fieldName);
            const expr = `${JSON.stringify(value)}${prerequisite.formula}`;
            let meetsPrerequisite = true;
            try {
              // eslint-disable-next-line no-new-func
              meetsPrerequisite = Function('return ' + expr)();
            } catch (e) {
              meetsPrerequisite = true;
              console.warn('Error evaluating prerequisite', expr, e);
            }

            if (!meetsPrerequisite) {
              meetsPrerequisites = false;
              break;
            }
          }

          const disabled = !meetsPrerequisites;
          const componentKey = (component && component.name) ? component.name : JSON.stringify(component);
          if (component) {
            component.disabled = disabled;
            disabledUpdates[componentKey] = disabled;
          }

          // If the component itself has items (e.g., listgroup), recurse into them
          if (component && Array.isArray(component.items)) {
            for (const childField of component.items) {
              evaluateField(childField);
            }
          }
        }        
      };

      schema.fields.forEach((group: any) => {
        if (Array.isArray(group.items)) {
          group.items.forEach((field: any) => {
            evaluateField(field);
          });
        }
      });

      // Only trigger a re-render if disabled state actually changed
      try {
        const json = JSON.stringify(disabledUpdates);
        if (json !== prevDisabledRef.current) {
          prevDisabledRef.current = json;
          setDisabledState(disabledUpdates);
        }
      } catch (err) {
        // Fallback: if stringify fails for some reason, still set state
        setDisabledState(disabledUpdates);
      }
    });

    return () => {
      if (subscription) subscription.unsubscribe();
    };
  }, [watch, setValue, schema, methods]);
};
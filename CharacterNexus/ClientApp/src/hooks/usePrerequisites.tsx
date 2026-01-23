import { useEffect, useRef, useState } from 'react';
import { useFormContext } from 'react-hook-form';
import { Prerequisite } from '../types/Prerequisite';

type DisabledMap = Record<string, boolean>;

export const usePrerequisites = (
  schema: any,
  prereqSet?: Set<string>,
  methods?: any
) => {
  const formMethods = methods ?? useFormContext();
  const { watch, getValues } = formMethods;

  const [disabledMap, setDisabledMap] = useState<DisabledMap>({});
  const prevJsonRef = useRef<string>('');

  /* --------------------------------------------
   * Collect prerequisite field names (once)
   * ------------------------------------------ */
  useEffect(() => {
    if (!schema?.fields) return;

    const names = new Set<string>();

    const collect = (field: any) => {
      const component = field?.component ?? field;
      if (!component?.prerequisites) return;

      try {
        const prereqs = JSON.parse(component.prerequisites) as Prerequisite[];
        prereqs.forEach(p => {
          const name = p.type ? `${p.type}.${p.name}` : p.name;
          names.add(name);
        });
      } catch {
        /* ignore */
      }

      component?.items?.forEach(collect);
    };

    schema.fields.forEach((group: any) =>
      group.items?.forEach(collect)
    );

    names.forEach(n => {
      watch(n);
      prereqSet?.add(n);
    });
  }, [schema, watch, prereqSet]);

  /* --------------------------------------------
   * Evaluate ALL prerequisites
   * ------------------------------------------ */
  useEffect(() => {
    if (!schema?.fields) return;

    const evaluateAll = () => {
      const nextMap: DisabledMap = {};

      const evaluateField = (field: any) => {
        const component = field?.component ?? field;
        if (!component) return;

        let enabled = true;

        if (component.prerequisites) {
          try {
            const prereqs = JSON.parse(component.prerequisites) as Prerequisite[];

            for (const p of prereqs) {
              const name = p.type ? `${p.type}.${p.name}` : p.name;
              const value = getValues(name);
              const expr = `${JSON.stringify(value)}${p.formula}`;

              const result = Function(`return ${expr}`)();
              if (!result) {
                enabled = false;
                break;
              }
            }
          } catch {
            enabled = true;
          }
        }

        const isInput =
          component.type !== 'accordion' &&
          component.type !== 'group' &&
          component.type !== 'section';

        if (isInput && component.name) {
          nextMap[component.name] = !enabled;
        }


        component.items?.forEach(evaluateField);
      };

      schema.fields.forEach((group: any) =>
        group.items?.forEach(evaluateField)
      );

      const json = JSON.stringify(nextMap);
      if (json !== prevJsonRef.current) {
        prevJsonRef.current = json;
        setDisabledMap(nextMap);
      }
    };

    evaluateAll();
    const sub = watch(evaluateAll);

    return () => sub.unsubscribe();
  }, [schema, watch, getValues]);

  return disabledMap;
};

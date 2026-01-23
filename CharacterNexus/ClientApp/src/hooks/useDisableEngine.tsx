import { useEffect, useRef, useState } from "react";
import { useFormContext } from "react-hook-form";

interface Prerequisite {
  name: string;
  type?: string;
  formula?: string;
}

export function useDisableEngine(schema: any) {
  const methods = useFormContext();
  if (!methods) {
    throw new Error("useDisableEngine must be used inside a FormProvider");
  }

  const { watch, getValues } = methods;

  const prevRef = useRef<string>("");

  const evaluateField = (field: any, next: Record<string, boolean>) => {
    if (!field) return;

    if (Array.isArray(field.items)) {
      field.items.forEach((f: any) => evaluateField(f, next));
    }

    if (Array.isArray(field.children)) {
      field.children.forEach((f: any) => evaluateField(f, next));
    }

    if (field.component) {
      evaluateField(field.component, next);
    }

    if (!field.name) return;

    let enabled = true;

    if (field.prerequisites && field.prerequisites !== "null") {
      try {
        const prereqs = JSON.parse(field.prerequisites);

        for (const p of prereqs) {
          const depName = p.type ? `${p.type}.${p.name}` : p.name;
          const value = getValues(depName);
          const expr = `${JSON.stringify(value)}${p.formula ?? ""}`;

          // eslint-disable-next-line no-new-func
          if (!Function(`return ${expr}`)()) {
            enabled = false;
            break;
          }
        }
      } catch {
        /* ignore */
      }
    }

    next[field.name.toLowerCase()] = !enabled;
  };

  const buildDisabledMap = () => {
    if (!schema?.fields) return {};

    const next: Record<string, boolean> = {};
    schema.fields.forEach((field: any) => evaluateField(field, next));
    return next;
  };

  const [disabledMap, setDisabledMap] = useState<Record<string, boolean>>(
    () => buildDisabledMap()
  );

  useEffect(() => {
    const sub = watch(() => {
      const next = buildDisabledMap();
      const json = JSON.stringify(next);

      if (json !== prevRef.current) {
        prevRef.current = json;
        setDisabledMap(next);
      }
    });

    return () => sub.unsubscribe();
  }, [watch, schema]);

  return disabledMap;
}

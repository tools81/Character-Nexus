import { useMemo } from "react";
import { Control, FieldValues, useWatch } from "react-hook-form";

export interface DependsOn {
  field: string;
  value: any;
}

export interface FieldConfig {
  name: string;
  dependsOn?: DependsOn;
}

const isFieldVisible = (
  field: FieldConfig,
  values: Record<string, any>
): boolean => {
  // No dependency → always visible
  if (!field.dependsOn) return true;

  const { field: dependsOnField, value: expectedValue } = field.dependsOn;
  const actualValue = values?.[dependsOnField];

  return actualValue === expectedValue;
};

const buildVisibilityMap = (
  fields: FieldConfig[],
  values: Record<string, any>
): Record<string, boolean> => {
  const map: Record<string, boolean> = {};

  for (const field of fields) {
    if (!field.name) continue;

    map[field.name.toLowerCase()] = isFieldVisible(field, values);
  }

  return map;
};

export const useVisibilityEngine = (
  fields: FieldConfig[],
  control: Control<FieldValues>
) => {
  // Watch ALL values so dependencies react instantly
  const values = useWatch({ control });

  const visibilityMap = useMemo(
    () => buildVisibilityMap(fields, values ?? {}),
    [fields, values]
  );

  return {
    visibilityMap,
    isVisible: (fieldName: string) => visibilityMap[fieldName.toLowerCase()] !== false,
  };
};

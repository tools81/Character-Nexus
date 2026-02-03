import { FieldValues, UseFormGetValues, UseFormSetValue, UseFormUnregister } from "react-hook-form";


export const handleRemoveBonusAdjustment = (getValues: UseFormGetValues<FieldValues>, setValue: UseFormSetValue<FieldValues>, type: string, name: string, increment: number) => {
  if (getValues(type) === undefined) return;
  
  if (name && typeof name === 'string' && name !== "") {
    type = `${type}.${name}`;
  }
  setValue(type, Number(getValues(type)) - increment);
};

export const handleRemoveArrayValue = (
  getValues: UseFormGetValues<FieldValues>,
  unregister: UseFormUnregister<FieldValues>,
  arrayFieldName: any,
  choice: any
) => {
  const currentValues = getValues(arrayFieldName);

  const indexToRemove = currentValues.findIndex(
    (field: HTMLSelectElement) => field.value === choice
  );

  if (indexToRemove !== -1) {
    unregister(`${arrayFieldName}.${indexToRemove}`);
  }
};

export const handleRemoveFieldValue = (
  getValues: UseFormGetValues<FieldValues>,
  setValue: UseFormSetValue<FieldValues>,
  unregister: UseFormUnregister<FieldValues>,
  fieldName: string,
  choice?: any
) => {
  const currentValue = getValues(fieldName);

  // ─────────────────────────────────────────────
  // ARRAY FIELD (checkbox group, multi-select)
  // ─────────────────────────────────────────────
  if (Array.isArray(currentValue)) {
    const indexToRemove = currentValue.findIndex(
      (v: any) => v === choice || v?.value === choice
    );

    if (indexToRemove !== -1) {
      unregister(`${fieldName}.${indexToRemove}`);
    }

    return;
  }

  // ─────────────────────────────────────────────
  // CHECKBOX (boolean)
  // ─────────────────────────────────────────────
  if (typeof currentValue === "boolean") {
    setValue(`${fieldName}.${choice}`, false, {
      shouldDirty: true,
      shouldTouch: true,
      shouldValidate: true
    });
    return;
  }

  // ─────────────────────────────────────────────
  // FALLBACK (single select / input)
  // ─────────────────────────────────────────────
  if (currentValue !== undefined) {
    setValue(fieldName, null, {
      shouldDirty: true,
      shouldTouch: true,
      shouldValidate: true
    });
  }
};

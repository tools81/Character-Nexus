import { FieldValues, UseFormGetValues, UseFormSetValue, UseFormUnregister } from "react-hook-form";


export const handleRemoveBonusAdjustment = (getValues: UseFormGetValues<FieldValues>, setValue: UseFormSetValue<FieldValues>, type: string, field: string, increment: number) => {
  setValue(type, Number(getValues(type)) - increment);
};

export const handleRemoveArrayValue = (
  getValues: UseFormGetValues<FieldValues>,
  unregister: UseFormUnregister<FieldValues>,
  arrayField: any,
  choice: any
) => {
  const currentValues = getValues(arrayField);

  const indexToRemove = currentValues.findIndex(
    (field: HTMLSelectElement) => field.value === choice
  );

  if (indexToRemove !== -1) {
    unregister(`${arrayField}.${indexToRemove}`);
  }
};
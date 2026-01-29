import { ChangeEvent } from "react";
import { UseFormRegister, FieldValues, useWatch, UseFormGetValues, UseFormSetValue, UseFormUnregister } from "react-hook-form";
import { BonusAdjustments } from "../types/BonusAdjustment";
import { handleRemoveBonusAdjustment, handleRemoveFieldValue } from "../hooks/useBonus";

interface Props {
  register: UseFormRegister<FieldValues>,
  unregister: UseFormUnregister<FieldValues>;
  getValues: UseFormGetValues<FieldValues>;
  setValue: UseFormSetValue<FieldValues>;
  name: string;
  includeLabel: boolean;
  label: string;
  defaultValue: string;
  className: string;
  inputBonusAdjustments: any;
  bonusAdjustments: BonusAdjustments;
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>;
  validation?: {
    required?: boolean;
    min?: number;
    max?: number;
  };
  disabled?: boolean;
  visible?: boolean;
}

const InputNumber = ({
  register,
  unregister,
  getValues,
  setValue,
  name,
  includeLabel,
  label,
  defaultValue,
  className,
  inputBonusAdjustments,
  bonusAdjustments,
  setBonusAdjustments,
  validation,
  disabled,
  visible = true
}: Props) => {
  var watchedValue = useWatch({ name });
  var value = watchedValue !== undefined ? watchedValue : "";
  
  return (
    <div key={name} className="mb-3" {...(!visible ? { style: { display: 'none' } } : {})}>
      {includeLabel && (
        <>
          <label>{label}</label>
          <br />
        </>
      )}
      <input
        id={name}
        type="number"
        className={className}
        style={{ maxWidth: "100px" }}
        data-bonusadjustments={inputBonusAdjustments}
        {...register(name, { disabled })}
        disabled={disabled}
        min = {validation?.min}
        max = {validation?.max}
        value={value ? value : defaultValue}
        onChange={(event) =>
          handleInputChange(
            event,
            bonusAdjustments,
            setBonusAdjustments,
            getValues,
            setValue,
            unregister
          )
        }
      ></input>
    </div>
  );
};

const handleInputChange = (
  event: ChangeEvent<HTMLInputElement>,
  bonusAdjustments: any,
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>,
  getValues: UseFormGetValues<FieldValues>,
  setValue: UseFormSetValue<FieldValues>,
  unregister: UseFormUnregister<FieldValues>
) => {
  if (event.target.value == null) {
    return;
  }

  setValue(event.target.name, event.target.value, {
    shouldDirty: true,
    shouldTouch: true,
  });

  const fieldElement = event.target;
  const fieldBonusAdjustmentsString = fieldElement.getAttribute(
    "data-bonusadjustments"
  );

  const fieldBonusAdjustments = fieldBonusAdjustmentsString
    ? JSON.parse(fieldBonusAdjustmentsString)
    : null;

  if (fieldBonusAdjustments) {
    // Remove bonuses provided by the component if user changes selection
    for (const adjustment of bonusAdjustments.reverse()) {
      handleRemoveBonusAdjustment(
        getValues,
        setValue,
        adjustment.type,
        adjustment.name,
        adjustment.value
      );
    }

    //Remove items previously added by the same input
    setBonusAdjustments(
      bonusAdjustments.filter((a: any) => a.origin !== event.target.name)
    );

    for (const adjustment of fieldBonusAdjustments) {
      setBonusAdjustments((prevBonusAdjustments) => [
        ...prevBonusAdjustments,
        {
          origin: event.target.name,
          type: adjustment.type,
          name: adjustment.name,
          value: event.target.valueAsNumber,
        },
      ]);
    }
  }
};

export default InputNumber;

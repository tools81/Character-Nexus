import { ChangeEvent } from "react";
import { UseFormRegister, FieldValues, useWatch, UseFormGetValues, UseFormSetValue, UseFormUnregister } from "react-hook-form";
import { BonusAdjustments } from "../types/BonusAdjustment";
import { handleRemoveBonusAdjustment } from "../hooks/useBonus";
import { InputGroup } from "react-bootstrap";
import DiceRollButton from "./DiceRollButton";

interface Props {
  register: UseFormRegister<FieldValues>,
  unregister: UseFormUnregister<FieldValues>;
  getValues: UseFormGetValues<FieldValues>;
  setValue: UseFormSetValue<FieldValues>;
  name: string;
  includeLabel: boolean;
  label: string;
  defaultValue: any;
  className: string;
  image?: string;
  inputBonusAdjustments: any;
  bonusAdjustments: BonusAdjustments;
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>;
  validation?: {
    required?: boolean;
    min?: number;
    max?: number;
  };
  dice?: boolean;
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
  image,
  inputBonusAdjustments,
  bonusAdjustments,
  setBonusAdjustments,
  validation,
  dice,
  disabled,
  visible = true
}: Props) => {
  var watchedValue = useWatch({ name });
  var value = watchedValue !== undefined && watchedValue !== null ? watchedValue : "";

  // Call register exactly once to avoid duplicate field entries
  const { onChange: rhfOnChange, onBlur, name: fieldName, ref } = register(name, { disabled });

  const controlledValue = value !== "" ? value : (defaultValue ?? "");

  const handleDiceRoll = () => {
    const min = validation?.min ?? 1;
    const max = validation?.max ?? 20;
    setValue(name, Math.floor(Math.random() * (max - min + 1)) + min);
  };

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    rhfOnChange(event);
    handleInputChange(event, bonusAdjustments, setBonusAdjustments, getValues, setValue, unregister);
  };

  return (
    <div key={name} className="mb-3" {...(!visible ? { style: { display: 'none' } } : {})}>
      {includeLabel && (
        <>
          <label>{label}</label>
          <br />
        </>
      )}
      {image && (
        <InputGroup>
          <InputGroup.Text>
            <img src={image} alt="" style={{ maxHeight: "32px", objectFit: "cover" }} />
          </InputGroup.Text>
          <input
            id={name}
            type="number"
            className={className}
            style={{ maxWidth: "100px" }}
            data-bonusadjustments={inputBonusAdjustments}
            ref={ref}
            name={fieldName}
            onBlur={onBlur}
            disabled={disabled}
            min={validation?.min}
            max={validation?.max}
            value={controlledValue}
            onChange={handleChange}
          />
          {dice && <DiceRollButton onClick={handleDiceRoll} />}
        </InputGroup>
      )}
      {!image && (
        <span style={{ display: "inline-flex", alignItems: "center", gap: "4px" }}>
          <input
            id={name}
            type="number"
            className={className}
            style={{ maxWidth: "100px" }}
            data-bonusadjustments={inputBonusAdjustments}
            ref={ref}
            name={fieldName}
            onBlur={onBlur}
            disabled={disabled}
            min={validation?.min}
            max={validation?.max}
            value={controlledValue}
            onChange={handleChange}
          />
          {dice && <DiceRollButton onClick={handleDiceRoll} />}
        </span>
      )}
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
  if (event.target.value == null || Number.isNaN(event.target.value) || event.target.value === "") {
    return;
  }

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

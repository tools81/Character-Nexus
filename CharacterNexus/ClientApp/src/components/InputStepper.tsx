import { UseFormRegister, FieldValues, useWatch, UseFormSetValue } from "react-hook-form";
import { BonusAdjustments } from "../types/BonusAdjustment";

interface Props {
  register: UseFormRegister<FieldValues>;
  setValue: UseFormSetValue<FieldValues>;
  name: string;
  label: string;
  inputBonusAdjustments: any;
  bonusAdjustments: BonusAdjustments;
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>;
  min?: number;
  max?: number;
}

const InputStepper = ({
  register,
  setValue,
  name,
  label,
  inputBonusAdjustments,
  bonusAdjustments,
  setBonusAdjustments,
  min,
  max,
}: Props) => {
  register(name);
  const watched = useWatch({ name });
  const current: number = typeof watched === "number" ? watched : 0;

  const applyAdjustment = (newValue: number) => {
    setValue(name, newValue);

    const parsed = inputBonusAdjustments
      ? (typeof inputBonusAdjustments === "string" ? JSON.parse(inputBonusAdjustments) : inputBonusAdjustments)
      : null;

    if (parsed) {
      setBonusAdjustments(prev => {
        const filtered = prev.filter((a: any) => a.origin !== name);
        if (newValue === 0) return filtered;
        return [
          ...filtered,
          ...parsed.map((adj: any) => ({
            origin: name,
            type: adj.type,
            name: adj.name,
            value: newValue,
          })),
        ];
      });
    }
  };

  const decrement = () => {
    if (min === undefined ||current > min) applyAdjustment(current - 1);
  };

  const increment = () => {
    if (max === undefined || current < max) applyAdjustment(current + 1);
  };

  return (
    <div className="mb-3">
      <label>{label}</label>
      <br />
      <div className="input-stepper">
        <button
          type="button"
          className="input-stepper__btn"
          onClick={decrement}
          disabled={min !== undefined && current <= min}
          aria-label="Decrease"
        >
          −
        </button>
        <span className="input-stepper__value">{current}</span>
        <button
          type="button"
          className="input-stepper__btn"
          onClick={increment}
          disabled={max !== undefined && current >= max}
          aria-label="Increase"
        >
          +
        </button>
      </div>
    </div>
  );
};

export default InputStepper;

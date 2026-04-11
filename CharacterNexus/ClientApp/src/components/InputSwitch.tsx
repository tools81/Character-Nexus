import { ChangeEvent } from "react";
import { useWatch } from "react-hook-form";
import {
  UseFormRegister,
  FieldValues,
  UseFormGetValues,
  UseFormSetValue
} from "react-hook-form";
import { BonusAdjustments } from "../types/BonusAdjustment";
import { BonusCharacteristics } from "../types/BonusCharacteristic";
import { handleRemoveBonusAdjustment } from "../hooks/useBonus";
import { Prerequisites } from "../types/Prerequisite";

interface Props {
  register: UseFormRegister<FieldValues>;
  getValues: UseFormGetValues<FieldValues>;
  setValue: UseFormSetValue<FieldValues>;
  id: string;
  name: string;
  includeLabel: boolean;
  label: string;
  inputBonusCharacteristics: any;
  bonusCharacteristics: BonusCharacteristics;
  setBonusCharacteristics: React.Dispatch<
    React.SetStateAction<BonusCharacteristics>
  >;
  inputBonusAdjustments: any;
  bonusAdjustments: BonusAdjustments;
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>;
  prerequisites: Prerequisites;
  disabled?: boolean;
  visible?: boolean;
}

const InputSwitch = ({
  register,
  getValues,
  setValue,
  id,
  name,
  label,
  inputBonusCharacteristics,
  bonusCharacteristics,
  setBonusCharacteristics,
  inputBonusAdjustments,
  bonusAdjustments,
  setBonusAdjustments,
  prerequisites,
  disabled,
  visible = true
}: Props) => {
  var watchedValue = useWatch({ name });
  var checked = watchedValue === true;

  return (
    <div className="form-check form-switch" {...(!visible ? { style: { display: 'none' } } : {})}>
      <input
        className="form-check-input"
        type="checkbox"
        role="switch"
        id={id}
        data-bonusadjustments={inputBonusAdjustments}
        data-bonuscharacteristics={inputBonusCharacteristics}
        data-prerequisites={prerequisites}        
        {...register(name, { disabled })}
        disabled={disabled}
        checked={checked}
        onChange={(event) =>
          handleInputChange(
            event,
            bonusCharacteristics,
            setBonusCharacteristics,
            bonusAdjustments,
            setBonusAdjustments,
            getValues,
            setValue
          )
        }
      />
      <label className="form-check-label" htmlFor={id}>
        <b>{label}</b>
      </label>
    </div>
  );
};

const handleInputChange = (
  event: ChangeEvent<HTMLInputElement>,
  bonusCharacteristics: any,
  setBonusCharacteristics: React.Dispatch<
    React.SetStateAction<BonusCharacteristics>
  >,
  bonusAdjustments: any,
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>,
  getValues: UseFormGetValues<FieldValues>,
  setValue: UseFormSetValue<FieldValues>
) => {
  if (event.target.value == null) {
    return;
  }

  setValue(event.target.name, event.target.checked, {
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
  const fieldBonusCharacteristicsString = fieldElement.getAttribute(
    "data-bonuscharacteristics"
  );
  const fieldBonusCharacteristics = fieldBonusCharacteristicsString
    ? JSON.parse(fieldBonusCharacteristicsString)
    : null;

  if (fieldBonusAdjustments) {
    const filteredAdjustments = bonusAdjustments.filter((a: any) => a.origin !== event.target.name);
    const newAdjustments = event.target.checked
      ? fieldBonusAdjustments.map((adjustment: any) => ({
          origin: event.target.name,
          type: adjustment.Type,
          name: adjustment.Name,
          value: adjustment.Value,
        }))
      : [];
    setBonusAdjustments([...filteredAdjustments, ...newAdjustments]);
  }

  if (fieldBonusCharacteristics) {
      const filteredCharacteristics = bonusCharacteristics.filter((c: any) => c.origin !== event.target.name);
      const newCharacteristics = event.target.checked
        ? fieldBonusCharacteristics.map((characteristic: any) => ({
            origin: event.target.name,
            type: characteristic.type,
            value: characteristic.value,
          }))
        : [];
      setBonusCharacteristics([...filteredCharacteristics, ...newCharacteristics]);
    }
};

export default InputSwitch;

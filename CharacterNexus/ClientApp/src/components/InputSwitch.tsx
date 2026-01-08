import { ChangeEvent } from "react";
import { useWatch } from "react-hook-form";
import {
  UseFormRegister,
  FieldValues,
  UseFormGetValues,
  UseFormSetValue,
  UseFormUnregister
} from "react-hook-form";
import { BonusAdjustments } from "../types/BonusAdjustment";
import { BonusCharacteristics } from "../types/BonusCharacteristic";
import { toCamelCase } from "../utils/toCamelCase";
import { handleRemoveBonusAdjustment, handleRemoveArrayValue } from "../hooks/useBonus";

interface Props {
  register: UseFormRegister<FieldValues>;
  unregister: UseFormUnregister<FieldValues>;
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
  disabled?: boolean;
}

const InputSwitch = ({
  register,
  unregister,
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
  disabled
}: Props) => {
  var watchedValue = useWatch({ name });
  var checked = watchedValue === true;
  
  return (
    <div className="form-check form-switch">
      <input
        className="form-check-input"
        type="checkbox"
        role="switch"
        id={id}
        data-bonusadjustments={inputBonusAdjustments}
        data-bonuscharacteristics={inputBonusCharacteristics}
        disabled={disabled}
        {...register(name)}
        checked={checked}
        onChange={(event) =>
          handleInputChange(
            event,
            bonusCharacteristics,
            setBonusCharacteristics,
            bonusAdjustments,
            setBonusAdjustments,
            getValues,
            setValue,
            unregister
          )
        }
      />
      <label className="form-check-label" htmlFor={id}>
        {label}
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
  setValue: UseFormSetValue<FieldValues>,
  unregister: UseFormUnregister<FieldValues>
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
    //Remove bonuses provided by the component if user changes selection
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

    if (event.target.checked) {
      for (const adjustment of fieldBonusAdjustments) {
        setBonusAdjustments((prevBonusAdjustments) => [
          ...prevBonusAdjustments,
          {
            origin: event.target.name,
            type: adjustment.Type,
            name: adjustment.Name,
            value: adjustment.Value,
          },
        ]);
      }
    }
  }

  if (fieldBonusCharacteristics) {
      //Remove components from field array where the origin is this input. So bonus is removed when changing value again.
      for (const characteristic of bonusCharacteristics.reverse()) {
        handleRemoveArrayValue(
          getValues,
          unregister,
          characteristic.type,
          characteristic.value
        );
      }
  
      //Remove items previously added by the same input
      setBonusCharacteristics(
        bonusCharacteristics.filter((c: any) => c.origin !== event.target.name)
      );
      
      if (event.target.checked) {
        //Add to the existing state of characteristics
        for (const characteristic of fieldBonusCharacteristics) {
          setBonusCharacteristics((existingCharacteristics) => [
            ...existingCharacteristics,
            {
              origin: event.target.name,
              type: characteristic.type,
              value: characteristic.value,
            },
          ]);
        }
      }
    }
};

export default InputSwitch;

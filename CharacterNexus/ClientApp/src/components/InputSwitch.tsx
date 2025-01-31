import { ChangeEvent } from "react";
import {
  UseFormRegister,
  FieldValues,
  UseFormGetValues,
  UseFormSetValue,
} from "react-hook-form";
import { BonusAdjustments } from "../types/BonusAdjustment";
import { BonusCharacteristics } from "../types/BonusCharacteristic";
import { toCamelCase } from "../utils/toCamelCase";
import { handleRemoveBonusAdjustment } from "../hooks/useBonus";
//import { HandlePrerequisite } from "../hooks/usePrerequisites";

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
  setBonusAdjustments
}: Props) => {
  return (
    <div className="form-check form-switch">
      <input
        className="form-check-input"
        type="checkbox"
        role="switch"
        id={id}
        data-bonusadjustments={inputBonusAdjustments}
        data-bonuscharacteristics={inputBonusCharacteristics}
        //TODO: Replace 'powers' with a variable
        {...register(`powers.${name}`)}
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
  setValue: UseFormSetValue<FieldValues>
) => {
  if (event.target.value == null) {
    return;
  }

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
        toCamelCase(adjustment.type),
        toCamelCase(adjustment.name),
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
};

export default InputSwitch;

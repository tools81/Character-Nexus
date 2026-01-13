import { ChangeEvent } from "react";
import {
  UseFormRegister,
  FieldValues,
  UseFormGetValues,
  UseFormSetValue,
  UseFormUnregister
} from "react-hook-form";
import { BonusAdjustments } from "../types/BonusAdjustment";
import { BonusCharacteristics } from "../types/BonusCharacteristic";
import { UserChoices } from "../types/UserChoice";
import { toCamelCase } from "../utils/toCamelCase";
import { handleRemoveBonusAdjustment, handleRemoveArrayValue } from "../hooks/useBonus";
// import RandomSelectButton from "./RandomSelectButton";

interface Props {
  register: UseFormRegister<FieldValues>;
  unregister: UseFormUnregister<FieldValues>;
  getValues: UseFormGetValues<FieldValues>;
  setValue: UseFormSetValue<FieldValues>;
  name: string;
  includeLabel: boolean;
  label: string;
  options: any;
  className: string;
  bonusCharacteristics: BonusCharacteristics;
  setBonusCharacteristics: React.Dispatch<
    React.SetStateAction<BonusCharacteristics>
  >;
  bonusAdjustments: BonusAdjustments;
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>;
  userChoices: UserChoices;
  setUserChoices: React.Dispatch<React.SetStateAction<UserChoices>>;
  openUserChoiceModal: (choices: UserChoices) => void;
  disabled?: boolean;
}

const InputSelect = ({
  register,
  unregister,
  getValues,
  setValue,
  name,
  includeLabel,
  label,
  options,
  className,
  bonusCharacteristics,
  setBonusCharacteristics,
  bonusAdjustments,
  setBonusAdjustments,
  userChoices,
  setUserChoices,
  openUserChoiceModal,
  disabled
}: Props) => {
  return (
    <>
      {includeLabel && (
        <>
          <label>{label}</label>
          <br />
        </>
      )}
      <select
        id={name}
        className={className}
        aria-label={label}
        disabled={disabled}
        {...register(`${name}.value`)}
        onChange={(event) =>
          handleSelectChange(event, bonusCharacteristics, setBonusCharacteristics, bonusAdjustments, setBonusAdjustments, userChoices, setUserChoices, openUserChoiceModal, getValues, setValue, unregister)
        }
      >
        <option value="">Select...</option>
        {options.map((option: any) => (
          <option
            key={option.value}
            id={option.value}
            value={option.value}
            data-bonusadjustments={option.bonusAdjustments}
            data-bonuscharacteristics={option.bonusCharacteristics}
            data-userchoices={option.userChoices}
          >          
            {option.label}
          </option>
        ))}
      </select>
      {/* <RandomSelectButton selectName={`${name}.value`} options={options} setValue={setValue} /> */}      
      <div className="pb-3" />
    </>
  );
};

const handleSelectChange = (
  event: ChangeEvent<HTMLSelectElement>,
  bonusCharacteristics: any,
  setBonusCharacteristics: React.Dispatch<
    React.SetStateAction<BonusCharacteristics>
  >,
  bonusAdjustments: any,
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>,
  userChoices: any,
  setUserChoices: React.Dispatch<React.SetStateAction<UserChoices>>,
  openUserChoiceModal: (choices: UserChoices) => void,
  getValues: UseFormGetValues<FieldValues>,
  setValue: UseFormSetValue<FieldValues>,
  unregister: UseFormUnregister<FieldValues>
) => {
  const selectElement = event.target;
  const selectedOption = selectElement.options[selectElement.selectedIndex];

  const selectBonusAdjustmentsString = selectedOption.getAttribute(
    "data-bonusadjustments"
  );
  const selectBonusAdjustments = selectBonusAdjustmentsString
    ? JSON.parse(selectBonusAdjustmentsString)
    : null;

  const selectBonusCharacteristicsString = selectedOption.getAttribute(
    "data-bonuscharacteristics"
  );
  const selectBonusCharacteristics = selectBonusCharacteristicsString
    ? JSON.parse(selectBonusCharacteristicsString)
    : null;

  const selectUserChoiceString = selectedOption.getAttribute(
    "data-userchoices"
  );
  const selectUserChoices = selectUserChoiceString
    ? JSON.parse(selectUserChoiceString)
    : null;

  if (event.target.value == null) {
    return;
  }

  if (selectBonusAdjustments) {
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

    for (const adjustment of selectBonusAdjustments) {
      setBonusAdjustments((prevBonusAdjustments) => [
        ...prevBonusAdjustments,
        {
          origin: event.target.name,
          type: adjustment.type,
          name: adjustment.name,
          value: adjustment.value,
        },
      ]);
    }
  }

  if (selectBonusCharacteristics) {
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

    //Add to the existing state of characteristics
    for (const characteristic of selectBonusCharacteristics) {
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

  if (selectUserChoices) {
    const value = selectUserChoices as UserChoices | null;

    //Add to the existing state of user choices
    for (const choice of selectUserChoices as UserChoices) {
      choice.origin = event.target.name;
    }

    if (value && openUserChoiceModal) {
      openUserChoiceModal(value);
    }    
  }
};

export default InputSelect;

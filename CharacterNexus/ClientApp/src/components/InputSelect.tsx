import { ChangeEvent } from "react";
import {
  UseFormRegister,
  FieldValues,
  UseFormGetValues,
  UseFormSetValue,
  UseFormUnregister
} from "react-hook-form";
import { BonusAdjustment, BonusAdjustments } from "../types/BonusAdjustment";
import { BonusCharacteristic } from "../types/BonusCharacteristic";
import { BonusCharacteristics } from "../types/BonusCharacteristic";
import { UserChoice } from "../types/UserChoice";
import { UserChoices } from "../types/UserChoice";
import { toCamelCase } from "../utils/toCamelCase";
import { handleRemoveBonusAdjustment, handleRemoveArrayValue } from "../hooks/useBonus";
import { useModal } from "../hooks/useModal";
// import RandomSelectButton from "./RandomSelectButton";

interface Props {
  register: UseFormRegister<FieldValues>;
  unregister: UseFormUnregister<FieldValues>;
  getValues: UseFormGetValues<FieldValues>;
  setValue: UseFormSetValue<FieldValues>;
  renderField: (component: any, includeLabel?: boolean, disabled?: boolean) => React.ReactNode;
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
  disabled?: boolean;
}

const InputSelect = ({
  register,
  unregister,
  getValues,
  setValue,
  renderField,
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
  disabled
}: Props) => {
  const userChoiceModal = useModal();
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
          handleSelectChange(event, bonusCharacteristics, setBonusCharacteristics, bonusAdjustments, setBonusAdjustments, userChoices, setUserChoices, userChoiceModal, getValues, setValue, unregister)
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
      <userChoiceModal.Modal>
        {({ userChoices: userChoices, close }) => (
          <>
            <h2>Choice: </h2>

            {userChoices.map((item: UserChoice) => (
              <p>Choose {item.count}</p>
            ))}

            {userChoices.map((item: UserChoice) => item.category === "Characteristic" && item.choices.map((choice, index) => {
              const bonusCharacteristic: BonusCharacteristic = { 
                      type: item.type,
                      value: choice
                    };

              const field = { 
                id: `choice.${item.type}.${index}`.trim(), 
                key: `choice.${item.type}.${index}`.trim(),
                name: `choice.${item.type}.${index}`.trim(), 
                label: choice, 
                type: "switch", 
                bonusCharacteristics: JSON.stringify([bonusCharacteristic])};
              
              return (renderField(field));
            }))} 

            {userChoices.map((item: UserChoice) => item.category === "Adjustment" && item.choices.map((choice, index) => {
              const bonusAdjustment: BonusAdjustment = { 
                      type: item.type,
                      name: choice,
                      value: 0
                    };

              const field = { 
                id: `${item.type}.${index}`.trim(), 
                name: `${item.type}.${index}`.trim(), 
                label: choice, 
                type: "number", 
                bonusAdjustments: [bonusAdjustment]};
              
              return (renderField(field));
            }))} 

            <button onClick={close}>Close</button>
          </>
        )}
      </userChoiceModal.Modal>
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
  userChoice: any,
  setUserChoice: React.Dispatch<React.SetStateAction<UserChoices>>,
  userChoiceModal: ReturnType<typeof useModal>,
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

    if (value) {
      userChoiceModal.open(value);
    }
    // Update user choices state
    // setUserChoice((prevChoices) => ({
    //   ...prevChoices,
    //   ...selectUserChoices
    // }));
  }
};

export default InputSelect;

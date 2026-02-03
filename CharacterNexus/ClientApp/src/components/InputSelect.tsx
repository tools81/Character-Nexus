import { ChangeEvent, forwardRef, useEffect, useRef, useState } from "react";
import {
  UseFormRegister,
  FieldValues,
  UseFormGetValues,
  UseFormSetValue,
  UseFormUnregister,
  useWatch
} from "react-hook-form";
import { BonusAdjustments } from "../types/BonusAdjustment";
import { BonusCharacteristics } from "../types/BonusCharacteristic";
import { UserChoices } from "../types/UserChoice";
import { toCamelCase } from "../utils/toCamelCase";
import { handleRemoveBonusAdjustment, handleRemoveFieldValue } from "../hooks/useBonus";

interface Props {
  register: UseFormRegister<FieldValues>;
  unregister: UseFormUnregister<FieldValues>;
  getValues: UseFormGetValues<FieldValues>;
  setValue: UseFormSetValue<FieldValues>;
  name: string;
  includeLabel: boolean;
  label: string;
  options: any[];
  className: string;
  bonusCharacteristics: BonusCharacteristics;
  setBonusCharacteristics: React.Dispatch<React.SetStateAction<BonusCharacteristics>>;
  bonusAdjustments: BonusAdjustments;
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>;
  userChoices: UserChoices;
  setUserChoices: React.Dispatch<React.SetStateAction<UserChoices>>;
  openUserChoiceModal: (choices: UserChoices) => void;
  disabled?: boolean;
  visible?: boolean;
}

/* =======================================================
   Main InputSelect Component with Custom UI Wrapper
======================================================= */
const InputSelect = forwardRef<HTMLSelectElement, Props>((props, ref) => {
  const {
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
    disabled,
    visible = true
  } = props;

  const selectRef = useRef<HTMLSelectElement | null>(null);
  const [selectedValue, setSelectedValue] = useState<any>(null);
  const [open, setOpen] = useState(false);
  const wrapperRef = useRef<HTMLDivElement>(null);

  const watchedValue = useWatch({ name });

  const normalizedValue =
    watchedValue && typeof watchedValue === "object" && "value" in watchedValue
      ? (watchedValue as any).value
      : watchedValue;

  useEffect(() => {
    const match =
      options.find(o => String(o.value) === String(normalizedValue)) ?? null;

    setSelectedValue(match);
  }, [normalizedValue, options]);


  useEffect(() => {
    const close = (e: MouseEvent) => {
      if (wrapperRef.current && !wrapperRef.current.contains(e.target as Node)) {
        setOpen(false);
      }
    };
    document.addEventListener("mousedown", close);
    return () => document.removeEventListener("mousedown", close);
  }, []);  

  // Handle clicks on the custom UI
  const handleCustomChange = (option: any) => {
    const isArrayIndexField = /\.\d+$/.test(name);

    setValue(name, isArrayIndexField ? option.value : option.value, {
      shouldDirty: true,
      shouldTouch: true,
      shouldValidate: true
    });

    // Update hidden select and trigger change event so existing handlers run
    if (selectRef.current) {
      selectRef.current.value = option.value;
      selectRef.current.dispatchEvent(
        new Event("change", { bubbles: true })
      );
    }

    setSelectedValue(option);
    setOpen(false);
  };

  return (
    <>
    <div {...(!visible ? { style: { display: 'none' } } : {})}>
      {includeLabel && (
        <>
          <label>{label}</label>
          <br />
        </>
      )}
    </div>

      {/* ================= Custom Select UI ================= */}
      <div
        ref={wrapperRef}
        className={`custom-select ${className}`}
        aria-disabled={disabled}
        {...(!visible ? { style: { display: 'none' } } : {})}
      >
        {/* Selected value */}
        <div
          className="custom-select__control"
          onClick={() => !disabled && setOpen((v) => !v)}
        >
          {selectedValue ? (
            <div className="custom-select__value">
              {selectedValue.image && (
                <img src={selectedValue.image} alt="" />
              )}
              <span>{selectedValue.label}</span>
            </div>
          ) : (
            <span>Select…</span>
          )}
        </div>

        {/* Dropdown */}
        {open && (
          <div className="custom-select__menu">
            {options.map((option) => (
              <div
                key={option.value}
                className="custom-select__option"
                onClick={() => handleCustomChange(option)}
              >
                {option.image && (
                  <img src={option.image} alt="" />
                )}
                <div>
                  <strong>{option.label}</strong>
                  {option.description && (
                    <div
                      className="description"
                      dangerouslySetInnerHTML={{
                        __html: option.description,
                      }}
                    />
                  )}
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
      
      {/* ================= Hidden Select ================= */}
      <div className="sr-only">
        <select
          id={name}
          className={className}
          aria-label={label}
          {...register(name, { disabled })}
          disabled={disabled}
          style={{ display: "none" }}
          ref={(el) => {
            selectRef.current = el;
            if (typeof ref === "function") ref(el);
            else if (ref) (ref as React.MutableRefObject<HTMLSelectElement | null>).current = el;
          }}
          onChange={(event) =>
            handleSelectChange(
              event,
              bonusCharacteristics,
              setBonusCharacteristics,
              bonusAdjustments,
              setBonusAdjustments,
              userChoices,
              setUserChoices,
              openUserChoiceModal,
              getValues,
              setValue,
              unregister
            )
          }
        >
          <option value="">Select...</option>
          {options.map(option => (
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
      </div>
      <div className="pb-3" />
    </>
  );
});

export default InputSelect;

const handleSelectChange = (
  event: ChangeEvent<HTMLSelectElement>,
  bonusCharacteristics: any,
  setBonusCharacteristics: React.Dispatch<React.SetStateAction<BonusCharacteristics>>,
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

  console.log("Element:", selectElement, "Selected:", selectedOption);

  const selectBonusAdjustmentsString = selectedOption.getAttribute("data-bonusadjustments");
  const selectBonusAdjustments = selectBonusAdjustmentsString
    ? JSON.parse(selectBonusAdjustmentsString)
    : null;

  const selectBonusCharacteristicsString = selectedOption.getAttribute("data-bonuscharacteristics");
  const selectBonusCharacteristics = selectBonusCharacteristicsString
    ? JSON.parse(selectBonusCharacteristicsString)
    : null;

  const selectUserChoiceString = selectedOption.getAttribute("data-userchoices");
  const selectUserChoices = selectUserChoiceString
    ? JSON.parse(selectUserChoiceString)
    : null;

  if (event.target.value == null) return;

  if (selectBonusAdjustments) {
    for (const adjustment of bonusAdjustments.reverse()) {
      handleRemoveBonusAdjustment(
        getValues,
        setValue,
        toCamelCase(adjustment.type),
        toCamelCase(adjustment.name),
        adjustment.value
      );
    }

    setBonusAdjustments(bonusAdjustments.filter((a: any) => a.origin !== event.target.name));

    for (const adjustment of selectBonusAdjustments) {
      setBonusAdjustments(prev => [
        ...prev,
        { origin: event.target.name, type: adjustment.type, name: adjustment.name, value: adjustment.value },
      ]);
    }
  }

  if (selectBonusCharacteristics) {
    for (const characteristic of bonusCharacteristics.reverse()) {
      handleRemoveFieldValue(getValues, setValue, unregister, characteristic.type, characteristic.value);
    }

    setBonusCharacteristics(bonusCharacteristics.filter((c: any) => c.origin !== event.target.name));

    for (const characteristic of selectBonusCharacteristics) {
      setBonusCharacteristics(prev => [
        ...prev,
        { origin: event.target.name, type: characteristic.type, value: characteristic.value },
      ]);
    }
  }

  if (selectUserChoices) {
    const value = selectUserChoices as UserChoices | null;
    for (const choice of selectUserChoices as UserChoices) {
      choice.origin = event.target.name;
    }

    if (value && openUserChoiceModal) {
      openUserChoiceModal(value);
    }
  }  
};

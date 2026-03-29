import { ChangeEvent, forwardRef, useEffect, useMemo, useRef, useState } from "react";
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
import { handleRemoveBonusAdjustment, handleRemoveFieldValue } from "../hooks/useBonus";
import { usePrerequisites } from "../hooks/usePrerequisites";
import DisabledPrereqWrapper from "./DisabledPrereqWrapper";

interface Props {
  register: UseFormRegister<FieldValues>;
  unregister: UseFormUnregister<FieldValues>;
  getValues: UseFormGetValues<FieldValues>;
  setValue: UseFormSetValue<FieldValues>;
  name: string;            // e.g. "weapons.0.mods.0"
  itemFieldName: string; // e.g. "weapons.0"
  label: string;
  options: any[];          // pre-filtered mod options for this weapon
  className: string;
  bonusAdjustments: BonusAdjustments;
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>;
  bonusCharacteristics: BonusCharacteristics;
  setBonusCharacteristics: React.Dispatch<React.SetStateAction<BonusCharacteristics>>;
}

const InputModSelect = forwardRef<HTMLSelectElement, Props>((props, ref) => {
  const {
    register,
    unregister,
    getValues,
    setValue,
    name,
    itemFieldName,
    label,
    options,
    className,
    bonusAdjustments,
    setBonusAdjustments,
    bonusCharacteristics,
    setBonusCharacteristics
  } = props;

  const selectRef = useRef<HTMLSelectElement | null>(null);
  const [selectedValue, setSelectedValue] = useState<any>(null);
  const [open, setOpen] = useState(false);
  const wrapperRef = useRef<HTMLDivElement>(null);

  const watchedValue = useWatch({ name });

  useEffect(() => {
    const match = options.find(o => String(o.value) === String(watchedValue)) ?? null;
    setSelectedValue(match);
  }, [watchedValue, options]);

  useEffect(() => {
    const close = (e: MouseEvent) => {
      if (wrapperRef.current && !wrapperRef.current.contains(e.target as Node)) {
        setOpen(false);
      }
    };
    document.addEventListener("mousedown", close);
    return () => document.removeEventListener("mousedown", close);
  }, []);

  const prereqSchema = useMemo(() => ({
    fields: [{ items: options.map(o => ({ name: o.value, prerequisites: o.prerequisites ?? null, type: "modselect" })) }]
  }), [options]);

  const disabledMap = usePrerequisites(prereqSchema);

  const handleCustomChange = (option: any) => {
    setValue(name, option.value, { shouldDirty: true, shouldTouch: true, shouldValidate: true });

    if (selectRef.current) {
      selectRef.current.value = option.value;
      selectRef.current.dispatchEvent(new Event("change", { bubbles: true }));
    }

    setSelectedValue(option);
    setOpen(false);
  };

  return (
    <>
      <span style={{ display: "flex", flex: 1, alignItems: "center", gap: "4px" }}>
        <div ref={wrapperRef} className={`custom-select ${className}`} style={{ flex: 1 }}>
          <div className="custom-select__control" onClick={() => setOpen(v => !v)}>
            {selectedValue ? (
              <div className="custom-select__value">
                <span>{selectedValue.label}</span>
              </div>
            ) : (
              <span>Select Mod…</span>
            )}
          </div>

          {open && (
            <div className="custom-select__menu">
              {options.map(option => {
                const isDisabled = disabledMap[option.value] ?? false;
                return (
                  <div
                    key={option.value}
                    className="custom-select__option"
                    onClick={() => !isDisabled && handleCustomChange(option)}
                  >
                    <DisabledPrereqWrapper component={{ prerequisites: option.prerequisites }} disabled={isDisabled}>
                      <strong>{option.label}</strong>
                      {option.description && (
                        <div className="description">{option.description}</div>
                      )}
                    </DisabledPrereqWrapper>
                  </div>
                );
              })}
            </div>
          )}
        </div>
      </span>

      <div className="sr-only">
        <select
          id={name}
          className={className}
          aria-label={label}
          {...register(name)}
          style={{ display: "none" }}
          ref={(el) => {
            selectRef.current = el;
            if (typeof ref === "function") ref(el);
            else if (ref) (ref as React.MutableRefObject<HTMLSelectElement | null>).current = el;
          }}
          onChange={(event) =>
            handleModSelectChange(
              event,
              itemFieldName,
              bonusAdjustments,
              setBonusAdjustments,
              bonusCharacteristics,
              setBonusCharacteristics,
              getValues,
              setValue,
              unregister
            )
          }
        >
          <option value="">Select Mod...</option>
          {options.map(option => (
            <option
              key={option.value}
              value={option.value}
              data-bonusadjustments={option.bonusAdjustments}
              data-bonuscharacteristics={option.bonusCharacteristics}
            >
              {option.label}
            </option>
          ))}
        </select>
      </div>
    </>
  );
});

export default InputModSelect;

const handleModSelectChange = (
  event: ChangeEvent<HTMLSelectElement>,
  itemFieldName: string,
  bonusAdjustments: BonusAdjustments,
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>,
  bonusCharacteristics: BonusCharacteristics,
  setBonusCharacteristics: React.Dispatch<React.SetStateAction<BonusCharacteristics>>,
  getValues: UseFormGetValues<FieldValues>,
  setValue: UseFormSetValue<FieldValues>,
  unregister: UseFormUnregister<FieldValues>
) => {
  const selectedOption = event.target.options[event.target.selectedIndex];
  const origin = event.target.name; // e.g. "weapons.0.mods.0"

  const rawAdjustmentsStr = selectedOption.getAttribute("data-bonusadjustments");
  const rawAdjustments = rawAdjustmentsStr ? JSON.parse(rawAdjustmentsStr) : null;

  const rawCharacteristicsStr = selectedOption.getAttribute("data-bonuscharacteristics");
  const rawCharacteristics = rawCharacteristicsStr ? JSON.parse(rawCharacteristicsStr) : null;

  // Remove ALL current adjustments from form values (reset-and-reapply pattern)
  for (const adj of [...bonusAdjustments].reverse()) {
    handleRemoveBonusAdjustment(getValues, setValue, adj.type, adj.name, adj.value);
  }

  // Remove characteristics for this specific origin
  for (const char of [...bonusCharacteristics].filter(c => c.origin === origin).reverse()) {
    handleRemoveFieldValue(getValues, setValue, unregister, char.type, char.value);
  }

  // Update state: drop old entries for this origin, add new ones
  setBonusAdjustments(prev => {
    const kept = prev.filter(a => a.origin !== origin);
    if (!rawAdjustments) return kept;
    const added = rawAdjustments.map((adj: any) => ({
      origin,
      type: `${itemFieldName}.${adj.type}`, // e.g. "weapons.0.damage"
      name: adj.name ?? "",
      value: adj.value
    }));
    return [...kept, ...added];
  });

  setBonusCharacteristics(prev => {
    const kept = prev.filter(c => c.origin !== origin);
    if (!rawCharacteristics) return kept;
    const added = rawCharacteristics.map((char: any) => ({
      origin,
      type: `${itemFieldName}.${char.type}`, // e.g. "weapons.0.effects"
      value: char.value
    }));
    return [...kept, ...added];
  });
};

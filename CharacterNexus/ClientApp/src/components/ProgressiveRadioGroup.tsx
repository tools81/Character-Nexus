import React from "react";
import { UseFormRegister, UseFormSetValue, UseFormWatch } from "react-hook-form";

interface Props {
  name: string;
  count: number;
  label?: string;
  includeLabel?: boolean;
  register: UseFormRegister<any>;
  setValue: UseFormSetValue<any>;
  watch: UseFormWatch<any>;
  disabled?: boolean;
  visible?: boolean;
}

const ProgressiveRadioGroup: React.FC<Props> = ({
  name,
  count,
  label,
  includeLabel = true,
  register,
  setValue,
  watch,
  disabled = false,
  visible = true
}) => {
  const value: number = Number(watch(name) ?? 0);

  // Extract RHF props (no duplicate handlers)
  const { ref, ...rest } = register(name);

  const handleClick = (index: number) => {
    const newValue = index + 1;

    setValue(name, newValue, {
      shouldDirty: true,
      shouldTouch: true,
      shouldValidate: true
    });
  };

  return (
    <div>
      <div className="d-flex gap-2">
        {Array.from({ length: count }).map((_, index) => {
          const checked = index < value;

          return (
            <label className="progressive-radio">
              <input
                key={index}
                type="checkbox"
                checked={checked}
                disabled={disabled}
                onChange={() => handleClick(index)}
              />
              <span className="radio-ui" />
            </label>
          );
        })}
        {includeLabel && (
          <label className=""><b>{label}</b></label>
        )}
      </div>

      {/* Hidden field registered with RHF */}
      <input type="hidden" ref={ref} {...rest} />
    </div>
  );
};

export default ProgressiveRadioGroup;

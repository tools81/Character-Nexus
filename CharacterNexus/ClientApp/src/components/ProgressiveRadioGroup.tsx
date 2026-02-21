import React from "react";
import { UseFormRegister, UseFormSetValue, UseFormWatch } from "react-hook-form";

interface Props {
  name: string;
  count: number;
  minimum: number;
  label?: string;
  includeLabel?: boolean;
  defaultValue: number;
  register: UseFormRegister<any>;
  setValue: UseFormSetValue<any>;
  watch: UseFormWatch<any>;
  disabled?: boolean;
  visible?: boolean;
}

const ProgressiveRadioGroup: React.FC<Props> = ({
  name,
  count,
  minimum = 0,
  label,
  includeLabel = true,
  defaultValue = 0,
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
          const readonly = disabled || index + 1 < minimum

          return (
            <label className="progressive-radio">
              <input
                key={index}
                name={name + index}
                type="checkbox"
                checked={checked}
                disabled={readonly}
                onChange={() => {
                  if (readonly) return;
                  if (index == value - 1) {
                    handleClick(-1);
                  }
                  else {
                    handleClick(index);
                  }
                }}
              />
              <span className="radio-ui" />
            </label>
          );
        })}
        {includeLabel && (
          <label><b>{label}</b></label>
        )}
      </div>

      {/* Hidden field registered with RHF */}
      <input type="hidden" 
        value={value ? value : defaultValue} 
        ref={ref} 
        {...rest} />
    </div>
  );
};

export default ProgressiveRadioGroup;

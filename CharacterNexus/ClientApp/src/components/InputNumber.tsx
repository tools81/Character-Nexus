import { UseFormRegister, FieldValues, useWatch } from "react-hook-form";

interface Props {
  register: UseFormRegister<FieldValues>,
  name: string;
  includeLabel: boolean;
  label: string;
  defaultValue: string;
  className: string;
  validation?: {
    required?: boolean;
    min?: number;
    max?: number;
  };
  disabled?: boolean;
  visible?: boolean;
}

const InputNumber = ({
  register,
  name,
  includeLabel,
  label,
  defaultValue,
  className,
  validation,
  disabled,
  visible = true
}: Props) => {
  var watchedValue = useWatch({ name });
  var value = watchedValue !== undefined ? watchedValue : "";
  
  return (
    <div key={name} className="mb-3" {...(!visible ? { style: { display: 'none' } } : {})}>
      {includeLabel && (
        <>
          <label>{label}</label>
          <br />
        </>
      )}
      <input
        id={name}
        type="number"
        className={className}
        style={{ maxWidth: "100px" }}
        {...register(name, { disabled })}
        disabled={disabled}
        min = {validation?.min}
        max = {validation?.max}
        value={value ? value : defaultValue}
      ></input>
    </div>
  );
};

export default InputNumber;

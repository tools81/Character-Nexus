import { UseFormRegister, FieldValues, useWatch } from "react-hook-form";

interface Props {
  register: UseFormRegister<FieldValues>,
  name: string;
  includeLabel: boolean;
  label: string;
  defaultValue: string;
  className: string;
  disabled?: boolean;
}

const InputNumber = ({
  register,
  name,
  includeLabel,
  label,
  defaultValue,
  className,
  disabled
}: Props) => {
  var watchedValue = useWatch({ name });
  var value = watchedValue !== undefined ? watchedValue : "";
  
  return (
    <div key={name} className="mb-3">
      {includeLabel && (
        <>
          <label>{label}</label>
          <br />
        </>
      )}
      <input
        id={name}
        type="number"
        defaultValue={defaultValue}
        className={className}
        style={{ maxWidth: "100px" }}
        disabled={disabled}
        {...register(name)}
        value={value ? value : 0}
      ></input>
    </div>
  );
};

export default InputNumber;

import { UseFormRegister, FieldValues } from "react-hook-form";

interface Props {
  register: UseFormRegister<FieldValues>,
  name: string;
  includeLabel: boolean;
  label: string;
  defaultValue: string;
  className: string;
  disabled?: boolean;
  visible?: boolean;
}

const InputTextArea = ({
  register,
  name,
  includeLabel,
  label,
  defaultValue,
  className,
  disabled,
  visible = true
}: Props) => {
  return (
    <div key={name} className="mb-3" {...(!visible ? { style: { display: 'none' } } : {})}>
      {includeLabel && (
        <>
          <label>{label}</label>
          <br />
        </>
      )}
      <textarea
        id={name}
        defaultValue={defaultValue}
        className={className}
        {...register(name, { disabled })}
        disabled={disabled}
      ></textarea>
    </div>
  );
};

export default InputTextArea;

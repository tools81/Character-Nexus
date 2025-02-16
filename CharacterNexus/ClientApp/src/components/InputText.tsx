import { UseFormRegister, FieldValues } from "react-hook-form";

interface Props {
  register: UseFormRegister<FieldValues>,
  name: string;
  includeLabel: boolean;
  label: string;
  defaultValue: string;
  className: string;
}

const InputText = ({
  register,
  name,
  includeLabel,
  label,
  defaultValue,
  className,
}: Props) => {
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
        defaultValue={defaultValue}
        className={className}
        {...register(name)}
      ></input>
    </div>
  );
};

export default InputText;

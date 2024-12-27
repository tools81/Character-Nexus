import { UseFormRegister, FieldValues } from "react-hook-form";

interface Props {
  register: UseFormRegister<FieldValues>,
  name: string;
  className: string;
  defaultValue: string;
}

const InputHidden = ({
  register,
  name,
  className,
  defaultValue
}: Props) => {
  return (
    <input
      id={name}
      className={className}
      defaultValue={defaultValue}
      style={{ display: "none" }}
      {...register(name)}
    ></input>
  );
};

export default InputHidden;

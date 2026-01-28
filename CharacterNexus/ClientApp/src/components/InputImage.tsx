import { ChangeEvent } from "react";
import { UseFormRegister, FieldValues } from "react-hook-form";

interface Props {
  register: UseFormRegister<FieldValues>,
  name: string;
  label: string;
  className: string;
  imagePreview: string | null;
  setImagePreview: React.Dispatch<
      React.SetStateAction<string | null>
    >;
  setImageData: React.Dispatch<
      React.SetStateAction<File | null>
    >;
  disabled?: boolean;
  visible?: boolean;
}

const InputImage = ({
  register,
  name,
  label,
  className,
  imagePreview,
  setImagePreview,
  setImageData, 
  disabled,
  visible = true
}: Props) => {
    return (
        <>
          <div key={name} className="mb-3" {...(!visible ? { style: { display: 'none' } } : {})}>
            <label htmlFor={name} className="form-label">
              {label}
            </label>
            <input
              className={className}
              type="file"
              id={name}
              accept="image/*"
              {...register(name, { disabled })}
              disabled={disabled}
              onChange={(event) => handleImageUpload(event, setImagePreview, setImageData)}
            />
          </div>
          <div className="mt-3" {...(!visible ? { style: { display: 'none' } } : {})}>
            {imagePreview ? (
              <img
                src={imagePreview}
                alt="Your Image"
                className="img-fluid pb-3"
              />
            ) : (
              <p>No image selected</p>
            )}
          </div>
        </>
      );
};

const handleImageUpload = (event: ChangeEvent<HTMLInputElement>, setImagePreview: React.Dispatch<
    React.SetStateAction<string | null>
  >, setImageData: React.Dispatch<
  React.SetStateAction<File | null>
>) => {
    if (event.target.files && event.target.files.length > 0) {
      setImageData(event.target.files[0]);

      const reader = new FileReader();
      reader.onloadend = () => {
        setImagePreview(reader.result as string);
      };
      reader.readAsDataURL(event.target.files[0]);
    }
  };

export default InputImage;

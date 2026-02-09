import React, { useState } from "react";
import ReactDatePicker from "react-datepicker";
import { UseFormRegister, FieldValues } from "react-hook-form";
import "react-datepicker/dist/react-datepicker.css";

interface Props {
  register: UseFormRegister<FieldValues>,
  name: string;
  includeLabel: boolean;
  label: string;
  defaultValue: string;
  disabled?: boolean;
  visible?: boolean;
}

const InputDatePicker = ({
  register,
  name,
  includeLabel,
  label,
  defaultValue,
  disabled,
  visible = true
}: Props) => {
  const [selectedDate, setSelectedDate] = useState<Date | null>(defaultValue ? new Date(defaultValue) : null);
  const { onChange } = register(name, { disabled });

  const handleDateChange = (date: Date | null) => {
    setSelectedDate(date);
    onChange({ target: { value: date } });
  };

  return (
    <div key={name} className="mb-3" {...(!visible ? { style: { display: 'none' } } : {})}>
      {includeLabel && (
        <>
          <label>{label}</label>
          <br />
        </>
      )}
      <ReactDatePicker
        placeholderText="Select date"
        onChange={handleDateChange}
        selected={selectedDate}
        disabled={disabled}
      />
    </div>
  );
};

export default InputDatePicker;
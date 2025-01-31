import React from 'react';
import { FieldValues, useFormContext, UseFormSetValue } from 'react-hook-form';
import { GiPerspectiveDiceSixFacesRandom } from "react-icons/gi";

interface RandomSelectButtonProps {
  selectName: string;
  options: { value: string; label: string }[];
  setValue: UseFormSetValue<FieldValues>;
}

const RandomSelectButton: React.FC<RandomSelectButtonProps> = ({ selectName, options, setValue }) => {
  const handleRandomSelect = () => {
    const randomIndex = Math.floor(Math.random() * options.length);
    const randomValue = options[randomIndex].value;
    setValue(selectName, randomValue);
  };

  return (
    <button type="button" onClick={handleRandomSelect}>
      <GiPerspectiveDiceSixFacesRandom />
    </button>
  );
};

export default RandomSelectButton;
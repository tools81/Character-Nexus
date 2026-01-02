import { useState, useEffect, ChangeEvent, useCallback, useRef } from "react";
import { useForm, useFieldArray, FormProvider } from "react-hook-form";
import { useRulesetContext } from "../components/RulesetContext";
import { useNavigate } from "react-router-dom";

import { BonusAdjustments } from "../types/BonusAdjustment";
import { BonusCharacteristics } from "../types/BonusCharacteristic";
import { getURLParameter } from "../utils/getUrlParameter";
import { getFileNameFromUrl } from "../utils/getFileNameFromUrl";
import InputText from "../components/InputText";
import InputTextArea from "../components/InputTextArea";
import InputNumber from "../components/InputNumber";
import InputHidden from "../components/InputHidden";
import InputSelect from "../components/InputSelect";
import InputSwitch from "../components/InputSwitch";
import InputImage from "../components/InputImage";
import FormGroup from "../components/FormGroup";
import FormListGroup from "../components/FormListGroup";
import FormAccordion from "../components/FormAccordion";
import DisabledPrereqWrapper from "../components/DisabledPrereqWrapper";
import { useFieldCalculations } from "../hooks/useFieldCalculations";
import { usePrerequisites } from "../hooks/usePrerequisites";
import { useBonusCharacteristics } from "../hooks/useBonusCharacteristics";
import { useBonusAdjustments } from "../hooks/useBonusAdjustments";

const BASE_URL = `${window.location.protocol}//${window.location.host}`;

const DynamicForm = () => {
  const navigate = useNavigate();
  const { ruleset } = useRulesetContext();
  const [schema, setSchema] = useState<any>(null);
  const [bonusCharacteristics, setBonusCharacteristics] = useState<BonusCharacteristics>([]);
  const [bonusAdjustments, setBonusAdjustments] = useState<BonusAdjustments>([]);
  const [imagePreview, setImagePreview] = useState<string | null>(null);
  const [imageData, setImageData] = useState<File | null>(null);
  const watchedRef = useRef<Set<string>>(new Set());
  const prereqSetRef = useRef<Set<string>>(new Set());
  
  const methods = useForm();
  const {
    register,
    unregister,
    handleSubmit,
    watch,
    control,
    getValues,
    setValue,
    reset,
    formState: { errors },
  } = methods;

  interface Props {
    field: any;
  }

  function navigateToRulesetDashboard() {
    navigate("/ruleset");
  } 

  const fetchDataAndReset = useCallback(
    async (name: string) => {
      try {
        const response = await fetch(
          `${BASE_URL}/api/character/load?ruleset=${encodeURIComponent(
            ruleset.name
          )}&characterName=${encodeURIComponent(name)}`,
          {
            method: "GET",
          }
        );
        const data = await response.json();
        reset(data);

        const imageUrl = data.image;
        const fileResponse = await fetch(imageUrl, { method: "GET" });
        const blob = await fileResponse.blob();
        const file = new File([blob], getFileNameFromUrl(data.image), {
          type: blob.type,
        });

        // Set the value programmatically
        setValue("image", file, { shouldDirty: true });

        // Dispatch a native change event manually
        const inputElement = document.getElementById(
          "image"
        ) as HTMLInputElement;

        if (inputElement) {
          // Create a DataTransfer object to simulate file selection
          const dataTransfer = new DataTransfer();
          dataTransfer.items.add(file);

          // Assign the DataTransfer object to the input element's files
          inputElement.files = dataTransfer.files;

          // Dispatch the change event manually
          const event = new Event("change", { bubbles: true });
          inputElement.dispatchEvent(event);
        }

        //setValue("image", file);
      } catch (error) {
        console.error("Error fetching character:", error);
      }
    },
    [reset]
  );

  useEffect(() => {
    const fetchSchema = async () => {
      try {
        const response = await fetch(
          `${BASE_URL}/api/character/new?ruleset=${encodeURIComponent(
            ruleset.name
          )}`
        );
        setSchema(await response.json());
      } catch (e: any) {
        console.error("Error fetching schema:", e);
      }
    };

    if (schema == null) {
      fetchSchema();

      var character = getURLParameter("character");
      if (character !== "" && character !== null) {
        fetchDataAndReset(character);
      }
    }
  }, []);

  useBonusAdjustments(bonusAdjustments, getValues, setValue);
  useBonusCharacteristics(bonusCharacteristics, getValues, setValue);
  usePrerequisites(schema, prereqSetRef.current, methods);
  useFieldCalculations(schema, getValues, setValue, watch);

  const onSubmit = async (data: any) => {
    if (imageData != null) {
      const formData = new FormData();
      formData.append(
        "Image",
        imageData,
        data.name + "." + imageData.name.split(".").pop()
      );
      formData.append("JsonData", JSON.stringify(data));

      await fetch(
        `${BASE_URL}/api/character/save?ruleset=${encodeURIComponent(
          ruleset.name
        )}`,
        {
          method: "POST",
          body: formData,
        }
      )
        .then(() => navigateToRulesetDashboard())
        .catch((error) => console.error("Error:", error));
    }
  };  

  const FieldArray = ({ field }: Props) => {
    const { fields, append, remove } = useFieldArray({
      name: field.name,
      control,
    });

    const handleAddSelect = (component: any) => {
      // Append a new item select input
      append({ value: "" });
    };

    return (
      <div>
        <label>{field.label}</label>

        {fields.map((item, index) => {
          const childComponent = { ...field.component, name: `${field.name}.${index}` };

          return (
            <div key={item.id} className="input-group">
              {renderField(childComponent, false, !!childComponent.disabled)}
              <button
                type="button"
                className="btn btn-outline-secondary"
                onClick={() => remove(index)}
              >
                Remove
              </button>
            </div>
          );
        })}
        <section>
          <button
            type="button"
            onClick={() => handleAddSelect(field.component)}
          >
            Add
          </button>
        </section>
        <div className="pb-3" />
      </div>
    );
  };

  const renderField = (field: any, includeLabel: boolean = true, disabled: boolean = false) => {
    const justTrue = true;

    switch (field.type) {
      case "hidden":
        return (
          <InputHidden
            register={register}
            name={field.name}
            className={field.className}
            defaultValue={field.default}
          />
        );
      case "text":
        return (
          <DisabledPrereqWrapper component={field} disabled={disabled}>
            <InputText
              register={register}
              name={field.name}
              includeLabel={includeLabel}
              label={field.label}
              defaultValue={field.default}
              className={field.className}
              disabled={disabled}
            />
          </DisabledPrereqWrapper>
        );
      case "textarea":
        return (
          <DisabledPrereqWrapper component={field} disabled={disabled}>
            <InputTextArea
              register={register}
              name={field.name}
              includeLabel={includeLabel}
              label={field.label}
              defaultValue={field.default}
              className={field.className}
              disabled={disabled}
            />
          </DisabledPrereqWrapper>
        );
      case "number":
        return (
          <DisabledPrereqWrapper component={field} disabled={disabled}>
            <InputNumber
              register={register}
              name={field.name}
              includeLabel={includeLabel}
              label={field.label}
              defaultValue={field.default}
              className={field.className}
              disabled={disabled}
            />
          </DisabledPrereqWrapper>
        );
      case "switch":
        return (
          <DisabledPrereqWrapper component={field} disabled={disabled}>
            <InputSwitch
              register={register}
              getValues={getValues}
              setValue={setValue}
              id={field.id}
              name={field.name}
              includeLabel={includeLabel}
              label={field.label}
              inputBonusCharacteristics={field.bonusCharacteristics}
              bonusCharacteristics={bonusCharacteristics}
              setBonusCharacteristics={setBonusCharacteristics}
              inputBonusAdjustments={field.bonusAdjustments}
              bonusAdjustments={bonusAdjustments}
              setBonusAdjustments={setBonusAdjustments}
              disabled={disabled}
            />
          </DisabledPrereqWrapper>
        );
      case "select":
        return (
          <DisabledPrereqWrapper component={field} disabled={disabled}>
            <InputSelect
              register={register}
              unregister={unregister}
              getValues={getValues}
              setValue={setValue}
              name={field.name}
              includeLabel={includeLabel}
              label={field.label}
              options={field.options}
              className={field.className}
              bonusCharacteristics={bonusCharacteristics}
              setBonusCharacteristics={setBonusCharacteristics}
              bonusAdjustments={bonusAdjustments}
              setBonusAdjustments={setBonusAdjustments}
              disabled={disabled}
            />
          </DisabledPrereqWrapper>
        );
      case "listgroup":
        return (
          <FormListGroup
            renderField={renderField}
            items={field.items}
            disabled={disabled}
          />
        );
      case "group":
        return (
          <FormGroup
            renderField={renderField}
            name={field.name}
            includeLabel={includeLabel}
            label={field.label}
            children={field.children}
            disabled={disabled}
          />
        );
      case "textblock":
        return (
          <div key={field.name} className="mb-3">
            <p>{field.text}</p>
          </div>
        );
      case "div":
        return (
          <div
            key={field.name}
            className={field.className}
            dangerouslySetInnerHTML={{
              __html: field.children.map((childField: any) => childField.text),
            }}
          ></div>
        );
      case "card":
        return (
          <div className="card">
            <div className="card-body">{field.text}</div>
          </div>
        );
      case "array":
        return <FieldArray field={field} />;
      case "image":
        return (
          <DisabledPrereqWrapper component={field} disabled={disabled}>
            <InputImage
              register={register}
              name={field.name}
              label={field.label}
              className={field.className}
              imagePreview={imagePreview}
              setImagePreview={setImagePreview}
              setImageData={setImageData}
              disabled={disabled}
            />
          </DisabledPrereqWrapper>
        );
      case "accordion":
        return (
          <FormAccordion
            includeLabel={includeLabel}
            field={field}
            renderField={renderField}
            disabled={disabled}
          />
        );
      default:
        return null;
    }
  };

  // register dependsOn watches once (avoid calling watch during render)
  useEffect(() => {
    if (!schema) return;

    for (const field of schema.fields) {
      if (field.dependsOn) {
        const name = field.dependsOn.field;
        if (!watchedRef.current.has(name)) {
          watch(name);
          watchedRef.current.add(name);
        }
      }      
    }    
  }, [schema, watch]);

  if (!schema) return <div>Loading...</div>;

  return (
    <FormProvider {...methods}>
      <form onSubmit={handleSubmit(onSubmit)}>
        <h2>{schema.title}</h2>
        {schema.fields.map((field: any) => {
          const shouldRenderField = field.dependsOn
            ? getValues(field.dependsOn.field) == field.dependsOn.value
            : true;

          const disabled = !!field.disabled;

          return shouldRenderField ? renderField(field, true, disabled) : null;
        })}
        <button type="submit">Submit</button>
      </form>
    </FormProvider>
  );
};

export default DynamicForm;
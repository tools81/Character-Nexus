import React, { useState, useEffect } from "react";
import { useForm, useFieldArray, Controller } from "react-hook-form";
import { useRulesetContext } from "./RulesetContext";

const BASE_URL = `${window.location.protocol}//${window.location.host}`;

// interface Validation {
//   required: boolean;
//   min: number;
//   max: number;
// }

// interface Option {
//   value: string;
//   label: string;
// }

// interface Children {
//   name: string;
//   label: string;
//   type: string;
//   text: string;
// }

// interface DependsOn {
//   field: string;
//   value: string;
// }

// interface Field {
//   name: string;
//   label: string;
//   type: string;
//   default: string;
//   validation: Validation;
//   options: Option[];
//   className: string;
//   children: Children[];
//   dependsOn: DependsOn;
// }

// interface Schema {
//   title: string;
//   fields: Field[];
// }

const DynamicForm = () => {
  const { ruleset } = useRulesetContext();
  //const [schema, setSchema] = useState<Schema>({ title: "Empty", fields: []});
  const [schema, setSchema] = useState<any>(null);
  const {
    register,
    handleSubmit,
    watch,
    control,
    formState: { errors }
  } = useForm();

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
        console.error("Error fetching schema:", e)
      }
    };

    if (schema == null) {
      fetchSchema();
    }
  }, []);

  const onSubmit = (data : any) => {
    console.log(data);
  };

    // const onSubmit = (data) => {
    //   fetch(`${BASE_URL}/api/character/save`, {
    //     method: "POST",
    //     headers: {
    //       "Content-Type": "application/json",
    //     },
    //     body: JSON.stringify(data),
    //   })
    //     .then((response) => response.json())
    //     .then((data) => console.log("Success:", data))
    //     .catch((error) => console.error("Error:", error));
    // };

  const FieldArrayComponent = ( field : any, control : any, register : any, errors : any ) => {
    const { fields, append, remove } = useFieldArray({
      control,
      name: field.name,
    });

    return (
      <div>
        <label>{field.label}</label>
        {fields.map((item, index) => (
          <div key={item.id}>
            <input
              type={field.items.type}
              {...register(`${field.name}[${index}]`, {
                required: field.items.validation.required,
                pattern: field.items.validation.pattern,
              })}
            />
            <button type="button" onClick={() => remove(index)}>
              Remove
            </button>
            {errors[field.name] && errors[field.name][index] && (
              <span>This field is required</span>
            )}
          </div>
        ))}
      </div>
    );
  };

  const renderField = (field : any) => {
    switch (field.type) {
      case "text":
        return (
          <div key={field.name} className="mb-3">
            <label>{field.label}</label>
            <input id={field.name} defaultValue={field.default}></input>
          </div>
        );
      case "number":
        return (
          <div key={field.name} className="mb-3">
            <label>{field.label}</label>
            <input id={field.name} type="number" defaultValue={field.default}></input>
          </div>
        );
      case "select":
        return (
          <div key={field.name} className="mb-3">
            <label>{field.label}</label>
            <select
              id={field.name}
              name={field.name}
              className={field.className}
              aria-label={field.label}
            >
              <option value="">Select...</option>
              {field.options.map((option: any) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </div>
        );
      case "textBlock":
        return (
          <div key={field.name} className="mb-3">
            <p>{field.text}</p>
          </div>
        );
      case "div":
        return (
          <div key={field.name} className={field.className}>
            {field.children.map((childField: any) => renderField(childField))}
          </div>
        );
      case "array":
        return (
          <FieldArrayComponent
            key={field.name}
            field={field}
            control={control}
            register={register}
            errors={errors}
          />
        );
      case "file":
        return (
          <div className="mb-3">
            <label htmlFor={field.name} className="form-label">
              {field.label}
            </label>
            <input
              className={field.className}
              type={field.type}
              id={field.name}
            />
          </div>
        );
      default:
        return null;
    }
  };

  if (!schema) {
    return <div>Loading...</div>;
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <h2>{schema.title}</h2>
      {schema.fields.map((field : any) => {
        const dependsOnField = field.dependsOn
          ? watch(field.dependsOn.field)
          : true;
        const shouldRenderField = field.dependsOn
          ? dependsOnField === field.dependsOn.value
          : true;

        return shouldRenderField ? (
          renderField(field)
        ) : null;
      })}
      <button type="submit">Submit</button>
    </form>
  );
};

export default DynamicForm;

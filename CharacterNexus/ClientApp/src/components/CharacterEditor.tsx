import React, { useState, useEffect } from "react";
import { useForm, useFieldArray, Controller } from "react-hook-form";
import { useRulesetContext } from "./RulesetContext";

const BASE_URL = `${window.location.protocol}//${window.location.host}`;

const DynamicForm = () => {
  const { ruleset } = useRulesetContext();
  const [schema, setSchema] = useState<any>(null);
  const {
    register,
    handleSubmit,
    watch,
    control,
    getValues,
    formState: { errors },
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
        console.error("Error fetching schema:", e);
      }
    };

    if (schema == null) {
      fetchSchema();
    }
  }, []);

  const onSubmit = (data: any) => {
    console.log(data);
  };

  //const onSubmit: SubmitHandler<FormValues> = (data) => console.log(data);

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

  const FieldArrayComponent = (
    field: any,
    control: any,
    register: any,
  ) => {
    const { fields, append, remove } = useFieldArray({
      control,
      name: field.name,
    });

    return (
      <div>
        <label>{field.label}</label>
        <ul>
          {fields.map((item, index) => (
            <li key={item.id}>
              <Controller
                control={control}
                name={`weapon[${index}]`}
                render={({ field }) => (
                  <select {...field}>
                    <option value="">Select...</option>
                    <option value="option1">Option 1</option>
                  </select>
                )}
              />
              {/* {renderField(field.component)} */}
              {/* <input
              type={field.items.type}
              {...register(`${field.name}[${index}]` as const, {
                required: field.items.validation.required,
                pattern: field.items.validation.pattern,
              })}
            /> */}
              <button type="button" onClick={() => remove(index)}>
                Remove
              </button>
            </li>
          ))}
        </ul>
        <section>
          <button type="button" onClick={() => append({ value: "Select..." })}>
            Add
          </button>
        </section>
      </div>
    );
  };

  const renderField = (field: any) => {
    switch (field.type) {
      case "text":
        return (
          <div key={field.name} className="mb-3">
            <label>{field.label}</label>
            <input
              id={field.name}
              defaultValue={field.default}
              {...register(field.name)}
            ></input>
          </div>
        );
      case "textarea":
        return (
          <div key={field.name} className="mb-3">
            <label>{field.label}</label>
            <textarea
              id={field.name}
              defaultValue={field.default}
              
              {...register(field.name)}
            ></textarea>
          </div>
        );
      case "number":
        return (
          <div key={field.name} className="mb-3">
            <label>{field.label}</label>
            <input
              id={field.name}
              type="number"
              defaultValue={field.default}
              {...register(field.name)}
            ></input>
          </div>
        );
      case "switch":
        return (
          <div className="form-check form-switch">
            <input
              className="form-check-input"
              type="checkbox"
              role="switch"
              id={field.id}
              {...register(field.name)}
            />
            <label className="form-check-label" htmlFor={field.id}>
              {field.label}
            </label>
          </div>
        );
      case "select":
        return (
          <div key={field.name} className="mb-3">
            <label>{field.label}</label>
            <select
              id={field.name}
              className={field.className}
              aria-label={field.label}
              {...register(field.name)}
            >
              <option value="">Select...</option>
              {field.options.map((option: any) => (
                <option key={option.value} id={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </div>
        );
      case "listgroup":
        return (
          <ul className="list-group">
            {field.items.map((childItem: any) => (
              <li className="list-group-item" id={childItem.component.name} key={childItem.component.name}>
                {renderField(childItem.component)}
                {renderField(childItem.text)}
              </li>
            ))}
          </ul>
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
            dangerouslySetInnerHTML={{ __html: field.children.map((childField: any) => childField.text) }}
          >

          </div>
        );
      case "card":
        return (
          <div className="card">
            <div className="card-body">
              {field.text}
            </div>
          </div>
        );
      case "array":
        return (
          <FieldArrayComponent
            key={field.name}
            field={field}
            control={control}
            register={register}
          />
        );
      case "file":
        return (
          <div key={field.name} className="mb-3">
            <label htmlFor={field.name} className="form-label">
              {field.label}
            </label>
            <input
              className={field.className}
              type={field.type}
              id={field.name}
              {...register(field.name)}
            />
          </div>
        );
      case "accordion":
        return (
          <div className="mb-3">
            <label htmlFor={field.id} className="form-label">
              {field.label}
            </label>
            <div className="accordion" id={field.id}>
              {field.items.map((childItem: any) => (
                <div className="accordion-item">
                  <h2 className="accordion-header">
                    <button
                      className="accordion-button collapsed"
                      type="button"
                      data-bs-toggle="collapse"
                      data-bs-target={`#${childItem.name}`}
                      aria-expanded="false"
                      aria-controls={childItem.name}
                    >
                      {childItem.header}
                    </button>
                  </h2>
                  <div
                    id={childItem.name}
                    className="accordion-collapse collapse"
                    data-bs-parent={`#${field.id}`}
                  >
                    <div className="accordion-body">
                      {renderField(childItem.component)}
                    </div>
                  </div>
                </div>
              ))}
            </div>
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
      {schema.fields.map((field: any) => {
        //const dependsOnField = field.dependsOn
        //  ? watch(field.dependsOn.field)
        //  : true;
        //const shouldRenderField = field.dependsOn
        //  ? dependsOnField === field.dependsOn.value
        //  : true;
        if (field.dependsOn) {
          watch(field.dependsOn.field);
        }

        const shouldRenderField = field.dependsOn
          ? getValues(field.dependsOn.field) == field.dependsOn.value
          : true;

        return shouldRenderField ? renderField(field) : null;
      })}
      <button type="submit">Submit</button>
    </form>
  );
};

export default DynamicForm;

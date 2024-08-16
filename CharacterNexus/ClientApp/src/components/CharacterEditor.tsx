import React, { useState, useEffect, ChangeEvent } from "react";
import { useForm, useFieldArray, Controller } from "react-hook-form";
import { useRulesetContext } from "./RulesetContext";
import { useNavigate } from "react-router-dom";

const BASE_URL = `${window.location.protocol}//${window.location.host}`;

const DynamicForm = () => {
  const navigate = useNavigate();
  const { ruleset } = useRulesetContext();
  const [schema, setSchema] = useState<any>(null);
  const [imagePreview, setImagePreview] = useState<string | null>(null);
  const [imageData, setImageData] = useState<File | null>(null);
  const {
    register,
    handleSubmit,
    watch,
    control,
    getValues,
    formState: { errors },
  } = useForm();

  function navigateToRulesetDashboard() {
    navigate("/rulesetdashboard");
  }

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

  const handleImageUpload = (event: ChangeEvent<HTMLInputElement>) => {
    if (event.target.files && event.target.files.length > 0) {
      setImageData(event.target.files[0]);

      const reader = new FileReader();
      reader.onloadend = () => {
        setImagePreview(reader.result as string);
      };
      reader.readAsDataURL(event.target.files[0]);
    }
  };

  const onSubmit = async (data: any) => {
    var imageApiResponse;
    if (imageData != null) {
      const formData = new FormData();
      formData.append(
        "image",
        imageData,
        data.name + "." + imageData.name.split(".").pop()
      );

      try {
        const response = await fetch(
          `${BASE_URL}/api/image/upload?ruleset=${encodeURIComponent(
            ruleset.name
          )}`,
          {
            method: "POST",
            body: formData
          }
        );

        if (!response.ok) {
          throw new Error("Network response was not ok");
        }

        imageApiResponse = await response.json();
      } catch (error) {
        console.error("Error uploading image:", error);
      }
    }

    var dataWithImage = { ...data, image: imageApiResponse.url};

    console.log(
      `${BASE_URL}/api/character/save?ruleset=${encodeURIComponent(
        ruleset.name
      )}`
    );

    await fetch(
      `${BASE_URL}/api/character/save?ruleset=${encodeURIComponent(
        ruleset.name
      )}`,
      {
        method: "POST",
        headers: {
          Accept: "application/json, text/plain, */*",
          'Content-Type': "application/json"
        },
        body: JSON.stringify(dataWithImage),
      }
    )
      .then((response) => response.json())
      .then(() => navigateToRulesetDashboard())
      .catch((error) => console.error("Error:", error));
  };

  interface Props {
    field: any;
  }

  const FieldArray = ({ field }: Props) => {
    const { fields, append, remove } = useFieldArray({
      name: field.name,
      control,
    });
    return (
      <div>
        <label>{field.label}</label>
        {fields.map((item, index) => (
          <div key={item.id} className="input-group">
            {(field.component.name = `${field.name}.${index}`)}
            {renderField(field.component, false)}
            <button
              type="button"
              className="btn btn-outline-secondary"
              onClick={() => remove(index)}
            >
              Remove
            </button>
          </div>
        ))}
        <section>
          <button type="button" onClick={() => append({ value: "Select..." })}>
            Add
          </button>
        </section>
        <div className="pb-3" />
      </div>
    );
  };

  const renderField = (field: any, includeLabel: boolean = true) => {
    switch (field.type) {
      case "hidden":
        return (
          <input
            id={field.name}
            className={field.className}
            style={{ visibility: "hidden" }}
            {...register(field.name)}
          ></input>
        );
      case "text":
        return (
          <div key={field.name} className="mb-3">
            {includeLabel && (
              <>
                <label>{field.label}</label>
                <br />
              </>
            )}
            <input
              id={field.name}
              defaultValue={field.default}
              className={field.className}
              {...register(field.name)}
            ></input>
          </div>
        );
      case "textarea":
        return (
          <div key={field.name} className="mb-3">
            {includeLabel && (
              <>
                <label>{field.label}</label>
                <br />
              </>
            )}
            <textarea
              id={field.name}
              defaultValue={field.default}
              className={field.className}
              {...register(field.name)}
            ></textarea>
          </div>
        );
      case "number":
        return (
          <div key={field.name} className="mb-3">
            {includeLabel && (
              <>
                <label>{field.label}</label>
                <br />
              </>
            )}
            <input
              id={field.name}
              type="number"
              defaultValue={field.default}
              className={field.className}
              style={{ maxWidth: "100px" }}
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
              {...register(`powers.${field.name}`)}
            />
            <label className="form-check-label" htmlFor={field.id}>
              {field.label}
            </label>
          </div>
        );
      case "select":
        return (
          <>
            {includeLabel && (
              <>
                <label>{field.label}</label>
                <br />
              </>
            )}
            <select
              id={field.name}
              className={field.className}
              aria-label={field.label}
              {...register(field.name)}
            >
              <option value="">Select...</option>
              {field.options.map((option: any) => (
                <option
                  key={option.value}
                  id={option.value}
                  value={option.value}
                >
                  {option.label}
                </option>
              ))}
            </select>
            <div className="pb-3" />
          </>
        );
      case "listgroup":
        return (
          <ul className="list-group">
            {field.items.map((childItem: any) => (
              <li
                className="list-group-item"
                id={childItem.component.name}
                key={childItem.component.name}
              >
                {renderField(childItem.component)}
                {renderField(childItem.text)}
              </li>
            ))}
          </ul>
        );

      case "group":
        return (
          <>
            <label htmlFor={field.name} className="form-label">
              {field.label}
            </label>
            <br />
            <div key={field.name} className="input-group">
              {field.children.map((childField: any) => (
                <div className="form-group p-2">{renderField(childField)}</div>
              ))}
            </div>
          </>
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
        break;
      case "image":
        return (
          <>
            <div key={field.name} className="mb-3">
              <label htmlFor={field.name} className="form-label">
                {field.label}
              </label>
              <input
                className={field.className}
                type="file"
                id={field.name}
                accept="image/*"
                {...register(field.name)}
                onChange={handleImageUpload}
              />
            </div>
            <div className="mt-3">
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
      case "accordion":
        return (
          <div className="mb-3">
            {includeLabel && (
              <>
                <label>{field.label}</label>
                <br />
              </>
            )}
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

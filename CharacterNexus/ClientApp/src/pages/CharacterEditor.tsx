import { useState, useEffect, useRef } from "react";
import { useForm, useFieldArray, FormProvider } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../store/configureStore";
import { fetchCharacterSchema, fetchCharacterByName, saveCharacter } from "../store/slices/characterSlice";
import { BonusAdjustments } from "../types/BonusAdjustment";
import { BonusCharacteristics } from "../types/BonusCharacteristic";
import { UserChoices, UserChoice } from "../types/UserChoice";
import { getFileNameFromUrl } from "../utils/getFileNameFromUrl";
import { getURLParameter } from "../utils/getUrlParameter";
import InputText from "../components/InputText";
import InputTextArea from "../components/InputTextArea";
import InputNumber from "../components/InputNumber";
import InputHidden from "../components/InputHidden";
import InputSelect from "../components/InputSelect";
import InputSwitch from "../components/InputSwitch";
import InputImage from "../components/InputImage";
import InputDatePicker from "../components/InputDatePicker";
import ProgressiveRadioGroup from "../components/ProgressiveRadioGroup";
import FormGroup from "../components/FormGroup";
import FormListGroup from "../components/FormListGroup";
import FormAccordion from "../components/FormAccordion";
import DisabledPrereqWrapper from "../components/DisabledPrereqWrapper";
import RightCollapsiblePane from "../components/RightCollapsiblePane";
import { useFieldCalculations } from "../hooks/useFieldCalculations";
import { useBonusCharacteristics } from "../hooks/useBonusCharacteristics";
import { useBonusAdjustments } from "../hooks/useBonusAdjustments";
import { useModal } from "../hooks/useModal";
import { useDisableEngine } from "../hooks/useDisableEngine";
import { useVisibilityEngine } from "../hooks/useVisibilityEngine";

const CharacterEditor: React.FC = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const { currentRuleset } = useAppSelector((state: { ruleset: any; }) => state.ruleset);
  const { schema, currentCharacter, isLoading: charLoading, error: charError } = useAppSelector((state: { character: any; }) => state.character);

  const [bonusCharacteristics, setBonusCharacteristics] = useState<BonusCharacteristics>([]);
  const [bonusAdjustments, setBonusAdjustments] = useState<BonusAdjustments>([]);
  const [userChoices, setUserChoices] = useState<UserChoices>([]);
  const [choiceFields, setChoiceFields] = useState<any[]>([]);
  const modalFieldNamesRef = useRef<string[]>([]);
  const [imagePreview, setImagePreview] = useState<string | null>(null);
  const [imageData, setImageData] = useState<File | null>(null);
  const watchedRef = useRef<Set<string>>(new Set());

  const methods = useForm({ shouldUnregister: false });
  const { register, unregister, handleSubmit, watch, control, getValues, setValue, reset } = methods;

  const userChoiceModal = useModal();

  // Open modal with choices
  const openUserChoiceModal = (choices: UserChoices) => {
    modalFieldNamesRef.current.forEach((name) => {
      unregister(name);
      setValue(name, undefined);
    });
    modalFieldNamesRef.current = [];
    setChoiceFields([]);
    setUserChoices(choices);
    userChoiceModal.open(choices);
  };

  // Load schema on ruleset change
  useEffect(() => {
    if (!currentRuleset) return;
    dispatch(fetchCharacterSchema(currentRuleset.name));
  }, [currentRuleset, dispatch]);

  // Load character if query param exists
  useEffect(() => {
    if (!currentRuleset || !schema) return;

    const characterName = getURLParameter("character");
    if (characterName) {
      dispatch(fetchCharacterByName({ rulesetName: currentRuleset.name, characterName }));
    }
  }, [currentRuleset, schema, dispatch]);

  // When currentCharacter loads, reset form
  useEffect(() => {
    if (!currentCharacter) return;

    reset(currentCharacter);

    // Load image from URL if exists
    if (currentCharacter.image) {
      fetch(currentCharacter.image)
        .then((res) => res.blob())
        .then((blob) => {
          const file = new File([blob], getFileNameFromUrl(currentCharacter.image), { type: blob.type });
          setValue("image", file, { shouldDirty: true });
          setImageData(file);
          setImagePreview(URL.createObjectURL(file));
        });
    }
  }, [currentCharacter, reset, setValue]);

  // Build choice fields dynamically
  useEffect(() => {
    if (!userChoices || userChoices.length === 0) return;

    const newFields: any[] = [];
    const newNames: string[] = [];

    userChoices.forEach((item: UserChoice) => {
      item.choices.forEach((choice: any, index: any) => {
        const name = `choice.${item.type}.${item.origin}.${index}`;
        if (item.category === "Characteristic") {
          const bonusChar = { type: item.type, value: choice };
          newFields.push({
            id: name,
            key: name,
            name,
            label: choice,
            type: "switch",
            defaultValue: false,
            bonusCharacteristics: JSON.stringify([bonusChar])
          });
        }
        if (item.category === "Adjustment") {
          const bonusAdj = { type: item.type, name: choice, value: 0 };
          newFields.push({
            id: name,
            key: name,
            name,
            label: choice,
            type: "number",
            bonusAdjustments: JSON.stringify([bonusAdj]),
            defaultValue: 0
          });
        }
        newNames.push(name);
      });
    });

    modalFieldNamesRef.current = newNames;
    setChoiceFields(newFields);
  }, [userChoices]);

  // Initialize form values for choice fields
  useEffect(() => {
    if (!choiceFields.length) return;
    choiceFields.forEach((field) => {
      setValue(field.name, field.defaultValue ?? null, { shouldDirty: false, shouldTouch: false, shouldValidate: false });
    });
  }, [choiceFields, setValue]);

  // Watch dynamic fields
  useEffect(() => {
    if (!choiceFields.length) return;
    choiceFields.forEach((field) => {
      if (!watchedRef.current.has(field.name)) {
        watch(field.name);
        watchedRef.current.add(field.name);
      }
    });
  }, [choiceFields, watch]);

  useBonusAdjustments(bonusAdjustments, getValues, setValue);
  useBonusCharacteristics(bonusCharacteristics, getValues, setValue); 
  useFieldCalculations(schema, getValues, setValue, watch);  

  // Submit handler
  const onSubmit = async (data: any) => {
    if (!currentRuleset) return;

    console.log("Data: ", data);
    await dispatch(saveCharacter({ rulesetName: currentRuleset.name, characterData: data, imageFile: imageData ?? undefined }));

    navigate("/ruleset");
  };

  // Field rendering helper
  const FieldArray = ({ field, disabledMap, visibilityMap, isVisible }: { field: any, disabledMap: Record<string, boolean>, visibilityMap: Record<string, boolean>, isVisible: (fieldName: string) => boolean }) => {
    const { fields, append, remove } = useFieldArray({ name: field.name, control });

    const handleAddSelect = (component: any) => append({ value: "" });

    return (
      <div {...!isVisible(field.name) ? { style: { display: 'none' } } : {}}>
        <label>{field.label}</label>
        {fields.map((item, index) => {
          const childComponent = { ...field.component, name: `${field.name}.${index}` };
          return (
            <div key={item.id} className="input-group">
              {renderField(childComponent, disabledMap, visibilityMap, isVisible, false )}
              <button type="button" className="btn btn-outline-secondary" onClick={() => remove(index)}>
                Remove
              </button>
            </div>
          );
        })}
        <section>
          <button type="button" className="add-button" onClick={() => handleAddSelect(field.component)}>
            Add
          </button>
        </section>
        <div className="pb-3" />
      </div>
    );
  };

  const renderField = (
    field: any,
    disabledMap: Record<string, boolean>,
    visibilityMap: Record<string, boolean>,
    isVisible: (name: string) => boolean,
    includeLabel: boolean = true
  ) => {
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
      case "divider":
        return <hr />;
      case "linebreak":
        return <br />;
      case "text":
        return (
          <DisabledPrereqWrapper component={field} disabled={disabledMap?.[field.name] === true}>
            <InputText
              register={register}
              name={field.name}
              includeLabel={includeLabel}
              label={field.label}
              defaultValue={field.default}
              className={field.className}
              disabled={disabledMap?.[field.name] === true}
              visible={isVisible(field.name)}
            />
          </DisabledPrereqWrapper>
        );
      case "textarea":
        return (
          <DisabledPrereqWrapper component={field} disabled={disabledMap?.[field.name] === true}>
            <InputTextArea
              register={register}
              name={field.name}
              includeLabel={includeLabel}
              label={field.label}
              defaultValue={field.default}
              className={field.className}
              disabled={disabledMap?.[field.name] === true}
              visible={isVisible(field.name)}
            />
          </DisabledPrereqWrapper>
        );
      case "number":
        return (
          <DisabledPrereqWrapper component={field} disabled={disabledMap?.[field.name] === true}>
            <InputNumber
              register={register}
              unregister={unregister}
              getValues={getValues}
              setValue={setValue}
              name={field.name}
              includeLabel={includeLabel}
              label={field.label}
              defaultValue={field.default}
              className={field.className}
              image={field.image}
              inputBonusAdjustments={field.bonusAdjustments}
              bonusAdjustments={bonusAdjustments}
              setBonusAdjustments={setBonusAdjustments}
              validation={field.validation}
              disabled={disabledMap?.[field.name] === true}
              visible={isVisible(field.name)}
            />
          </DisabledPrereqWrapper>
        );
      case "switch":
        return (
          <DisabledPrereqWrapper component={field} disabled={disabledMap?.[field.name] === true}>
            <InputSwitch
              register={register}
              unregister={unregister}
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
              prerequisites={field.prerequisites}
              disabled={disabledMap?.[field.name] === true} 
              visible={isVisible(field.name)}
            />
          </DisabledPrereqWrapper>
        );
      case "select": 
        return (
          <DisabledPrereqWrapper component={field} disabled={disabledMap?.[field.name] === true}>
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
              userChoices={userChoices}
              setUserChoices={setUserChoices}
              openUserChoiceModal={openUserChoiceModal}
              disabled={disabledMap?.[field.name] === true}
              visible={isVisible(field.name)}
            />
          </DisabledPrereqWrapper>
        );
      case "date":
        return (
          <DisabledPrereqWrapper component={field} disabled={disabledMap?.[field.name] === true}>
            <InputDatePicker
              register={register}
              name={field.name}
              includeLabel={includeLabel}
              label={field.label}
              defaultValue={field.default}
              disabled={disabledMap?.[field.name] === true}
              visible={isVisible(field.name)}
            />
          </DisabledPrereqWrapper>
        );
      case "radiogroup":
        return (
          <DisabledPrereqWrapper component={field} disabled={disabledMap?.[field.name] === true}>
            <ProgressiveRadioGroup
              register={register}
              setValue={setValue}
              watch={watch}
              name={field.name}
              count={field.count}
              minimum={field.minimum}
              includeLabel={includeLabel}
              defaultValue={field.default}
              label={field.label}
              disabled={disabledMap?.[field.name] === true}
              visible={isVisible(field.name)}
            />
          </DisabledPrereqWrapper>
        );
      case "listgroup":
        return (
          <FormListGroup
            renderField={renderField}
            items={field.items}
            disabledMap={disabledMap}
            visibilityMap={visibilityMap}
            isVisible={isVisible}
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
            disabledMap={disabledMap}
            visibilityMap={visibilityMap}
            isVisible={isVisible}
          />
        );
      case "textblock":
        return (
          <div key={field.name} className="mb-3">
            {field.image && <img src={field.image} alt="" />}
            &nbsp;
            <div dangerouslySetInnerHTML={{
              __html: field.text,
            }}
            ></div>
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
        return <FieldArray field={field} disabledMap={disabledMap} visibilityMap={visibilityMap} isVisible={isVisible} />;
      case "image":
        return (
          <DisabledPrereqWrapper component={field} disabled={disabledMap?.[field.name] === true}
>
            <InputImage
              register={register}
              name={field.name}
              label={field.label}
              className={field.className}
              imagePreview={imagePreview}
              setImagePreview={setImagePreview}
              setImageData={setImageData}
              disabled={disabledMap?.[field.name] === true}
              visible={isVisible(field.name)}
            />
          </DisabledPrereqWrapper>
        );
      case "accordion":
        return (
          <FormAccordion
            includeLabel={includeLabel}
            field={field}
            renderField={renderField}
            disabledMap={disabledMap}
            visibilityMap={visibilityMap}
            isVisible={isVisible}
          />
        );
      default:
        return null;
    }
  };

  if (charLoading || !schema) return <div>Loading...</div>;
  if (charError) return <div>Error: {charError}</div>;

  return (
    <FormProvider {...methods}>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="d-flex mb-3">
          {currentRuleset && <img src={currentRuleset.logoSource} alt={currentRuleset.name} className="p-2" />}
        </div>

        <FormContents
          schema={schema}
          control={control}
          renderField={renderField}
          choiceFields={choiceFields}
          userChoiceModal={userChoiceModal}
        />

        <div className="center-container">
          <button className="submit-button" type="submit">Submit</button>
        </div>
        <br />
      </form>
      {currentRuleset && <RightCollapsiblePane title={currentRuleset.name} htmlContent={currentRuleset.instructions} />}
    </FormProvider>
  );
};

const FormContents = ({
  schema,
  control,
  renderField,
  choiceFields,
  userChoiceModal,
}: any) => {
  const disabledMap = useDisableEngine(schema);
  const { visibilityMap, isVisible } = useVisibilityEngine(
    schema.fields,
    control
  );
  
  return (
    <>
      {schema.fields.map((field: any) =>
        renderField(field, disabledMap, visibilityMap, isVisible, field.includeLabel ?? true)
      )}

      <userChoiceModal.Modal>
        {({ userChoices, close }: any) => (
          <>
            <h2>Choice:</h2>

            {userChoices.map((item: any, index: number) => (
              <div key={item.type}>
                <p>Choose {item.count}</p>

                {choiceFields
                  .filter((field: any) =>
                    field.name?.startsWith(`choice.${item.type}`)
                  )
                  .map((field: any) => (
                    <div key={field.id}>
                      {renderField(
                        field,
                        disabledMap,
                        visibilityMap,
                        isVisible,
                        field.includeLabel ?? true
                      )}
                    </div>
                  ))}

                {index < userChoices.length - 1 && (
                  <hr className="my-3" />
                )}
              </div>
            ))}
            <br />
            <button className="center-container" onClick={close}>Close</button>
          </>
        )}
      </userChoiceModal.Modal>
    </>
  );
};


export default CharacterEditor;

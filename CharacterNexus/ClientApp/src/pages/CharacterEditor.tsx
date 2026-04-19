import { useState, useEffect, useMemo, useRef } from "react";
import { useForm, useFieldArray, useWatch, FormProvider, useFormContext } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../store/configureStore";
import { saveCharacter } from "../store/slices/characterSlice";
import { BonusAdjustments } from "../types/BonusAdjustment";
import { BonusCharacteristics } from "../types/BonusCharacteristic";
import { UserChoices } from "../types/UserChoice";
import InputText from "../components/InputText";
import InputTextArea from "../components/InputTextArea";
import InputNumber from "../components/InputNumber";
import InputStepper from "../components/InputStepper";
import InputHidden from "../components/InputHidden";
import InputSelect from "../components/InputSelect";
import InputModSelect from "../components/InputModSelect";
import InputSwitch from "../components/InputSwitch";
import InputImage from "../components/InputImage";
import InputDatePicker from "../components/InputDatePicker";
import ProgressiveRadioGroup from "../components/ProgressiveRadioGroup";
import FormGroup from "../components/FormGroup";
import FormListGroup from "../components/FormListGroup";
import FormAccordion from "../components/FormAccordion";
import DisabledPrereqWrapper from "../components/DisabledPrereqWrapper";
import RightCollapsiblePane from "../components/RightCollapsiblePane";
import StatsBar from "../components/StatsBar";
import { useMediaQuery } from "../hooks/useMediaQuery";
import { useFieldCalculations } from "../hooks/useFieldCalculations";
import { useBonusCharacteristics } from "../hooks/useBonusCharacteristics";
import { useBonusAdjustments } from "../hooks/useBonusAdjustments";
import { useConditionalBonuses } from "../hooks/useConditionalBonuses";
import { handleRemoveFieldValue } from "../hooks/useBonus";
import { useModal } from "../hooks/useModal";
import { useDisableEngine } from "../hooks/useDisableEngine";
import { useVisibilityEngine, isFieldVisible } from "../hooks/useVisibilityEngine";
import { useCharacterLoader } from "../hooks/useCharacterLoader";
import { useUserChoices } from "../hooks/useUserChoices";
import { useRulesetTheme } from "../hooks/useRulesetTheme";

/* =======================================================
   ArrayItem — handles a single non-modifiableitem array
   entry, including an optional quantity input when the
   selected option carries a quantity property.
======================================================= */
interface ArrayItemProps {
  component: any;
  fieldName: string;
  disabledMap: Record<string, boolean>;
  visibilityMap: Record<string, boolean>;
  isVisible: (field: any) => boolean;
  renderField: (field: any, disabledMap: Record<string, boolean>, visibilityMap: Record<string, boolean>, isVisible: (field: any) => boolean, includeLabel?: boolean) => React.ReactNode;
  onRemove: () => void;
}

const ArrayItem = ({ component, fieldName, disabledMap, visibilityMap, isVisible, renderField, onRemove }: ArrayItemProps) => {
  const { register, getValues, setValue } = useFormContext();
  const valueFieldName = `${fieldName}.value`;
  const watchedValue = useWatch({ name: valueFieldName });
  const watchedQuantity = useWatch({ name: `${fieldName}.quantity` });
  const prevValueRef = useRef<string | undefined>(undefined);

  const selectedOption = component.options?.find((o: any) => o.value === watchedValue);
  const hasQuantity = selectedOption?.quantity != null || watchedQuantity != null;

  useEffect(() => {
    const prevValue = prevValueRef.current;
    prevValueRef.current = watchedValue;

    if (selectedOption?.quantity != null) {
      if (!prevValue) {
        // On mount: only set default quantity if none already exists (e.g. from bonus characteristics)
        const existingQty = getValues(`${fieldName}.quantity`);
        if (existingQty == null) {
          setValue(`${fieldName}.quantity`, selectedOption.quantity, { shouldDirty: true });
        }
      } else {
        // User changed selection: reset to the new option's default quantity
        setValue(`${fieldName}.quantity`, selectedOption.quantity, { shouldDirty: true });
      }
    }
  }, [watchedValue]);

  const childComponent = { ...component, name: valueFieldName };

  return (
    <div className="input-group">
      {renderField(childComponent, disabledMap, visibilityMap, isVisible, false)}
      {hasQuantity && (
        <input
          type="number"
          className="quantity-input"
          min={0}
          {...register(`${fieldName}.quantity`, { valueAsNumber: true })}
        />
      )}
      <button type="button" className="btn btn-outline-secondary" onClick={onRemove}>
        Remove
      </button>
    </div>
  );
};

/* =======================================================
   FieldArray — module-level so React never remounts it
   due to CharacterEditor re-renders
======================================================= */
interface FieldArrayProps {
  field: any;
  disabledMap: Record<string, boolean>;
  visibilityMap: Record<string, boolean>;
  isVisible: (fieldName: string) => boolean;
  renderField: (
    field: any,
    disabledMap: Record<string, boolean>,
    visibilityMap: Record<string, boolean>,
    isVisible: (name: string) => boolean,
    includeLabel?: boolean
  ) => React.ReactNode;
}

const FieldArray = ({ field, disabledMap, visibilityMap, isVisible, renderField }: FieldArrayProps) => {
  const { control } = useFormContext();
  const { fields, append, remove } = useFieldArray({ name: field.name, control });

  const handleAddSelect = () => append({ value: "" });

  return (
    <div {...!isVisible(field.name) ? { style: { display: 'none' } } : {}}>
      <label>{field.label}</label>
      {fields.map((item, index) => {
        const childComponent = { ...field.component, name: `${field.name}.${index}` };
        if (field.component.type === "modifiableitem") {
          childComponent.onRemove = () => remove(index);
          return <div key={item.id}>{renderField(childComponent, disabledMap, visibilityMap, isVisible, false)}</div>;
        }
        return (
          <ArrayItem
            key={item.id}
            component={field.component}
            fieldName={`${field.name}.${index}`}
            disabledMap={disabledMap}
            visibilityMap={visibilityMap}
            isVisible={isVisible}
            renderField={renderField}
            onRemove={() => remove(index)}
          />
        );
      })}
      <section>
        <button type="button" className="add-button" onClick={() => handleAddSelect()}>
          Add
        </button>
      </section>
      <div className="pb-3" />
    </div>
  );
};

/* =======================================================
   ModifiableItem — module-level so React never remounts
   it due to CharacterEditor re-renders. Form methods come
   from context; state is passed via props.
======================================================= */
interface ModifiableItemProps {
  field: any;
  bonusAdjustments: BonusAdjustments;
  setBonusAdjustments: React.Dispatch<React.SetStateAction<BonusAdjustments>>;
  bonusCharacteristics: BonusCharacteristics;
  setBonusCharacteristics: React.Dispatch<React.SetStateAction<BonusCharacteristics>>;
  userChoices: UserChoices;
  setUserChoices: React.Dispatch<React.SetStateAction<UserChoices>>;
  openUserChoiceModal: (choices: UserChoices) => void;
}

const ModifiableItem = ({
  field,
  bonusAdjustments,
  setBonusAdjustments,
  bonusCharacteristics,
  setBonusCharacteristics,
  userChoices,
  setUserChoices,
  openUserChoiceModal,
}: ModifiableItemProps) => {
  const { register, unregister, getValues, setValue, control } = useFormContext();

  const itemFieldName = field.name; // e.g. "weapons.0"
  const watchedItemData = useWatch({ name: itemFieldName });
  const watchedItem = watchedItemData?.value;

  const selectedItemOption = field.options?.find((o: any) => o.value === watchedItem);
  const itemValueFieldName = `${itemFieldName}.value`;

  const modSets: string[] = useMemo(
    () => selectedItemOption?.modSets ? JSON.parse(selectedItemOption.modSets) : [],
    [selectedItemOption]
  );

  const availableModOptions = useMemo(
    () => (field.modOptions ?? []).filter((mod: any) => modSets.includes(mod.value)),
    [field.modOptions, modSets]
  );

  const watchedMods: any[] = watchedItemData?.mods ?? [];
  const itemDisplayLabel = useMemo(() => {
    if (!selectedItemOption) return undefined;
    const prefixes = watchedMods
      .map((mod: any) => {
        const modValue = typeof mod === "object" ? mod?.value ?? mod : mod;
        return availableModOptions.find((o: any) => o.value === modValue)?.prefix;
      })
      .filter(Boolean);
    return prefixes.length > 0
      ? `${prefixes.join(" ")} ${selectedItemOption.label}`
      : undefined;
  }, [watchedMods, selectedItemOption, availableModOptions]);

  useEffect(() => {
    if (!selectedItemOption?.stats) return;
    const stats: { label: string; value: string; field?: string }[] = JSON.parse(selectedItemOption.stats);
    for (const stat of stats) {
      if (!stat.field) continue;
      const numeric = Number(stat.value);
      setValue(`${itemFieldName}.${stat.field}`, isNaN(numeric) ? stat.value : numeric);
    }
  }, [watchedItem]);

  const { fields: modFields, append: appendMod, remove: removeMod } = useFieldArray({
    name: `${itemFieldName}.mods`,
    control
  });

  const handleRemoveMod = (modIndex: number) => {
    const modOrigin = `${itemFieldName}.mods.${modIndex}`;

    setBonusAdjustments(prev => prev.filter(a => a.origin !== modOrigin));

    const charToRemove = bonusCharacteristics.filter(c => c.origin === modOrigin);
    for (const char of [...charToRemove].reverse()) {
      handleRemoveFieldValue(getValues, setValue, unregister, char.type, char.value);
    }
    setBonusCharacteristics(prev => prev.filter(c => c.origin !== modOrigin));

    removeMod(modIndex);
  };

  return (
    <div className="border rounded p-2 mb-2">
      <div className="input-group">
        <InputSelect
          register={register}
          unregister={unregister}
          getValues={getValues}
          setValue={setValue}
          name={itemValueFieldName}
          includeLabel={false}
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
          displayLabel={itemDisplayLabel}
        />
        <button type="button" className="btn btn-outline-secondary" onClick={field.onRemove}>
          Remove
        </button>
      </div>
      {modFields.length > 0 && (
        <div className="ms-3">
          {modFields.map((item, modIndex) => (
            <div key={item.id} className="input-group mb-1">
              <InputModSelect
                register={register}
                unregister={unregister}
                getValues={getValues}
                setValue={setValue}
                name={`${itemFieldName}.mods.${modIndex}`}
                itemFieldName={itemFieldName}
                label="Mod"
                options={availableModOptions}
                className={field.className}
                bonusAdjustments={bonusAdjustments}
                setBonusAdjustments={setBonusAdjustments}
                bonusCharacteristics={bonusCharacteristics}
                setBonusCharacteristics={setBonusCharacteristics}
              />
              <button type="button" className="btn btn-outline-secondary" onClick={() => handleRemoveMod(modIndex)}>
                Remove Mod
              </button>
            </div>
          ))}
        </div>
      )}
      {modSets.length > 0 && (
        <button type="button" className="add-button" onClick={() => appendMod({ value: "" })}>
          Add Mod
        </button>
      )}
      {selectedItemOption?.stats && (() => {
        const stats: { label: string; value: string; field?: string }[] = JSON.parse(selectedItemOption.stats);
        return (
          <div className="d-flex flex-wrap gap-3 mt-2">
            {stats.map(stat => {
              const liveValue = stat.field ? watchedItemData?.[stat.field] : undefined;
              const displayValue = liveValue != null ? liveValue : stat.value;
              return <span key={stat.label}><small>{stat.label}</small> {displayValue}</span>;
            })}
          </div>
        );
      })()}
    </div>
  );
};

/* =======================================================
   CharacterEditor
======================================================= */
const CharacterEditor: React.FC = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const { currentRuleset } = useAppSelector((state: { ruleset: any }) => state.ruleset);
  const { schema, isLoading: charLoading, error: charError } = useAppSelector((state: { character: any }) => state.character);

  const [bonusCharacteristics, setBonusCharacteristics] = useState<BonusCharacteristics>([]);
  const [bonusAdjustments, setBonusAdjustments] = useState<BonusAdjustments>([]);
  const [openPane, setOpenPane] = useState<"right" | null>(null);
  const handleRightToggle = () => setOpenPane(prev => prev === "right" ? null : "right");

  const methods = useForm({ shouldUnregister: false });
  const { register, unregister, handleSubmit, watch, control, getValues, setValue, reset } = methods;

  useRulesetTheme(currentRuleset);

  const userChoiceModal = useModal();
  const { imagePreview, imageData, setImagePreview, setImageData } = useCharacterLoader(reset, setValue);
  const { userChoices, setUserChoices, choiceFields, openUserChoiceModal } = useUserChoices(unregister, setValue, watch, userChoiceModal);

  useBonusAdjustments(bonusAdjustments, getValues, setValue);
  useBonusCharacteristics(bonusCharacteristics, getValues, setValue);
  useConditionalBonuses(bonusAdjustments, setBonusAdjustments, bonusCharacteristics, setBonusCharacteristics, getValues, watch);
  useFieldCalculations(schema, getValues, setValue, watch);

  // Submit handler
  const onSubmit = async (data: any) => {
    if (!currentRuleset) return;

    console.log("Data: ", data);
    await dispatch(saveCharacter({ rulesetName: currentRuleset.name, characterData: data, imageFile: imageData ?? undefined }));

    navigate("/ruleset");
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
      case "stepper":
        return (
          <InputStepper
            register={register}
            setValue={setValue}
            name={field.name}
            label={field.label}
            inputBonusAdjustments={field.bonusAdjustments}
            bonusAdjustments={bonusAdjustments}
            setBonusAdjustments={setBonusAdjustments}
            min={field.validation?.min}
            max={field.validation?.max}
          />
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
              defaultValue={field.defaultValue ?? field.default}
              className={field.className}
              image={field.image}
              inputBonusAdjustments={field.bonusAdjustments}
              bonusAdjustments={bonusAdjustments}
              setBonusAdjustments={setBonusAdjustments}
              validation={field.validation}
              dice={field.dice}
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
              dice={field.dice}
              disabled={disabledMap?.[field.name] === true}
              visible={isVisible(field)}
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
      case "modifiableitem":
        return (
          <ModifiableItem
            field={field}
            bonusAdjustments={bonusAdjustments}
            setBonusAdjustments={setBonusAdjustments}
            bonusCharacteristics={bonusCharacteristics}
            setBonusCharacteristics={setBonusCharacteristics}
            userChoices={userChoices}
            setUserChoices={setUserChoices}
            openUserChoiceModal={openUserChoiceModal}
          />
        );
      case "array":
        return (
          <FieldArray
            field={field}
            disabledMap={disabledMap}
            visibilityMap={visibilityMap}
            isVisible={isVisible}
            renderField={renderField}
          />
        );
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
      {schema && (
        <StatsBar
          schema={schema}
          onInstructionsToggle={currentRuleset?.instructions ? handleRightToggle : undefined}
          instructionsOpen={openPane === "right"}
        />
      )}
      {currentRuleset && (
        <RightCollapsiblePane
          title={currentRuleset.name}
          htmlContent={currentRuleset.instructions}
          isOpen={openPane === "right"}
          onToggle={handleRightToggle}
        />
      )}
      <form onSubmit={handleSubmit(onSubmit)} className="character-editor-form">
        <FormContents
          schema={schema}
          control={control}
          renderField={renderField}
          choiceFields={choiceFields}
          userChoiceModal={userChoiceModal}
        />
        <div className="center-container">
          <button className="submit-button" type="submit">Save Character</button>
        </div>
        <br />
      </form>
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
  const { register } = useFormContext();
  const isMobile = useMediaQuery("(max-width: 767px)");
  const disabledMap = useDisableEngine(schema);
  const { visibilityMap, isVisible, values } = useVisibilityEngine(
    schema.fields,
    control
  );

  // Derive ordered unique tab list from schema; fields without a tab go to a default tab
  const tabs: string[] = useMemo(() => {
    const seen = new Set<string>();
    const result: string[] = [];
    for (const field of schema.fields) {
      if (field.pinnedStat || !field.tab) continue;
      if (!seen.has(field.tab)) {
        seen.add(field.tab);
        result.push(field.tab);
      }
    }
    return result;
  }, [schema]);

  const hasTabs = tabs.length > 0;
  const [activeTab, setActiveTab] = useState<string>(() => tabs[0] ?? "");

  const resolveVisible = (fieldOrName: any): boolean => {
    if (typeof fieldOrName === 'object' && fieldOrName !== null) {
      return isFieldVisible(fieldOrName, values);
    }
    return isVisible(fieldOrName);
  };

  const renderHiddenRegistrations = () =>
    schema.fields
      .filter((field: any) => field.pinnedStat)
      .map((field: any) => (
        <InputHidden
          key={field.name}
          register={register}
          name={field.name}
          className={field.className}
          defaultValue={field.default}
        />
      ));

  const renderTabFields = (tab: string) =>
    schema.fields
      .filter((field: any) => !field.pinnedStat && field.tab === tab)
      .map((field: any) =>
        renderField(field, disabledMap, visibilityMap, resolveVisible, field.includeLabel ?? true)
      );

  const renderUnlabelledFields = () =>
    schema.fields
      .filter((field: any) => !field.pinnedStat && !field.tab)
      .map((field: any) =>
        renderField(field, disabledMap, visibilityMap, resolveVisible, field.includeLabel ?? true)
      );

  return (
    <>
      {renderHiddenRegistrations()}

      {hasTabs ? (
        <>
          {/* Tab bar: scrollable pill tabs on mobile, full labelled bar on desktop */}
          {isMobile ? (
            <div className="tab-bar tab-bar--mobile">
              <div className="tab-bar__scroll">
                {tabs.map(tab => (
                  <button
                    key={tab}
                    type="button"
                    className={`tab-bar__pill${activeTab === tab ? " active" : ""}`}
                    onClick={() => setActiveTab(tab)}
                  >
                    {tab}
                  </button>
                ))}
              </div>
            </div>
          ) : (
            <div className="tab-bar tab-bar--desktop">
              {tabs.map(tab => (
                <button
                  key={tab}
                  type="button"
                  className={`tab-bar__tab${activeTab === tab ? " active" : ""}`}
                  onClick={() => setActiveTab(tab)}
                >
                  {tab}
                </button>
              ))}
            </div>
          )}

          {/* Active tab content */}
          <div className="tab-content">
            {renderTabFields(activeTab)}
          </div>
        </>
      ) : (
        renderUnlabelledFields()
      )}

      <userChoiceModal.Modal>
        {({ userChoices, close }: any) => (
          <>
            <h2>Choice:</h2>

            {userChoices.map((item: any, index: number) => (
              <div key={item.type}>
                <p>{item.label ?? `Choose ${item.count}`}</p>

                {choiceFields
                  .filter((field: any) =>
                    field.name?.startsWith(`choice.${item.type}.${item.origin}.`)
                  )
                  .map((field: any) => (
                    <ChoiceFieldWithDescription
                      key={field.id}
                      field={field}
                      disabledMap={disabledMap}
                      visibilityMap={visibilityMap}
                      isVisible={isVisible}
                      renderField={renderField}
                    />
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


function ChoiceFieldWithDescription({ field, disabledMap, visibilityMap, isVisible, renderField }: {
  field: any;
  disabledMap: any;
  visibilityMap: any;
  isVisible: any;
  renderField: any;
}) {
  const [open, setOpen] = useState(false);

  return (
    <div>
      <div className="d-flex align-items-center gap-2">
        <div className="flex-grow-1">
          {renderField(field, disabledMap, visibilityMap, isVisible, field.includeLabel ?? true)}
        </div>
        {field.description && (
          <button
            type="button"
            className="btn btn-link btn-sm p-0 flex-shrink-0"
            onClick={() => setOpen(o => !o)}
            aria-expanded={open}
          >
            {open ? "▲" : "▼"}
          </button>
        )}
      </div>
      {field.description && open && (
        <small className="d-block mb-2 mt-1" dangerouslySetInnerHTML={{ __html: field.description }} />
      )}
    </div>
  );
}

export default CharacterEditor;

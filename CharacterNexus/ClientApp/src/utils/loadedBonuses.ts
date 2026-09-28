import { UseFormGetValues } from "react-hook-form";
import { BonusAdjustments } from "../types/BonusAdjustment";
import { BonusCharacteristics } from "../types/BonusCharacteristic";
import { forEachFieldInstance, selectNameFor, parseJsonAttr, findOption } from "./schemaFieldInstances";

/* Rebuild the bonus entries a loaded character's saved selections would have
   put into state. Each case mirrors the entry shape produced by the matching
   input's change handler, so later changes replace these entries exactly as
   they would in the session that made the original selections.

   Only inputs whose bonuses are applied solely by user action are included
   (selects, mods and user choices). Plain switches and numbers are skipped: a
   saved true/number can come from a default or another bonus, in which case
   their own bonuses were never applied and recording them would be wrong. */
export const collectLoadedBonuses = (
  schemaFields: any[],
  choiceFields: any[],
  getValues: UseFormGetValues<any>
): { adjustments: BonusAdjustments; characteristics: BonusCharacteristics } => {
  const adjustments: BonusAdjustments = [];
  const characteristics: BonusCharacteristics = [];

  forEachFieldInstance(schemaFields, getValues, (field, name, inArrayRow) => {
    switch (field.type) {
      case "select":
      case "modifiableitem": {
        // InputSelect → handleSelectChange
        const origin = selectNameFor(name, inArrayRow);
        const option = findOption(field.options, getValues(origin));
        if (!option) break;

        parseJsonAttr(option.bonusAdjustments)?.forEach((adj: any) =>
          adjustments.push({
            origin,
            type: adj.type,
            name: adj.name,
            value: adj.value,
            conditions: adj.conditions ?? undefined,
          })
        );
        parseJsonAttr(option.bonusCharacteristics)?.forEach((char: any) =>
          characteristics.push({ origin, type: char.type, value: char.value })
        );

        // InputModSelect → handleModSelectChange
        if (field.type === "modifiableitem") {
          const mods = getValues(`${name}.mods`);
          if (!Array.isArray(mods)) break;
          mods.forEach((mod, modIndex) => {
            const modOrigin = `${name}.mods.${modIndex}`;
            const modOption = findOption(field.modOptions, mod);
            if (!modOption) return;

            parseJsonAttr(modOption.bonusAdjustments)?.forEach((adj: any) =>
              adjustments.push({ origin: modOrigin, type: `${name}.${adj.type}`, name: adj.name ?? "", value: adj.value })
            );
            parseJsonAttr(modOption.bonusCharacteristics)?.forEach((char: any) =>
              characteristics.push({ origin: modOrigin, type: `${name}.${char.type}`, value: char.value })
            );
          });
        }
        break;
      }
    }
  });

  // User choice switches (InputSwitch) and steppers (InputStepper)
  for (const field of choiceFields) {
    const value = getValues(field.name);
    if (field.type === "switch" && value === true) {
      parseJsonAttr(field.bonusCharacteristics)?.forEach((char: any) =>
        characteristics.push({ origin: field.name, type: char.type, value: char.value })
      );
    }
    if (field.type === "stepper" && Number(value)) {
      parseJsonAttr(field.bonusAdjustments)?.forEach((adj: any) =>
        adjustments.push({ origin: field.name, type: adj.type, name: adj.name, value: Number(value) })
      );
    }
  }

  return { adjustments, characteristics };
};

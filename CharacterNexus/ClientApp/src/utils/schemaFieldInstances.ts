import { UseFormGetValues } from "react-hook-form";

/* Walk the schema and call visit() for every concrete input with the form name
   it is rendered under. Array rows are expanded from the current form values
   and visited with the row path ("<array>.<index>") and inArrayRow = true. */
export const forEachFieldInstance = (
  fields: any[],
  getValues: UseFormGetValues<any>,
  visit: (field: any, name: string, inArrayRow: boolean) => void
) => {
  const walk = (field: any) => {
    if (!field) return;
    switch (field.type) {
      case "array": {
        const component = field.component;
        const items = getValues(field.name);
        if (!component || !Array.isArray(items)) break;
        items.forEach((_, index) => visit(component, `${field.name}.${index}`, true));
        break;
      }
      case "group":
        field.children?.forEach(walk);
        break;
      case "listgroup":
        field.items?.forEach((item: any) => walk(item.component));
        break;
      case "accordion":
        field.items?.forEach((item: any) => {
          walk(item.embedField);
          walk(item.component);
        });
        break;
      default:
        if (field.name) visit(field, field.name, false);
    }
  };

  fields.forEach(walk);
};

// Array rows keep their selected option under "<row>.value"
export const selectNameFor = (name: string, inArrayRow: boolean) =>
  inArrayRow ? `${name}.value` : name;

export const parseJsonAttr = (value: any): any[] | null => {
  if (!value) return null;
  if (Array.isArray(value)) return value;
  try {
    const parsed = JSON.parse(value);
    return Array.isArray(parsed) ? parsed : null;
  } catch {
    return null;
  }
};

export const findOption = (options: any[] | undefined, value: any) => {
  const normalized = value && typeof value === "object" && "value" in value ? value.value : value;
  if (normalized == null || normalized === "") return undefined;
  return options?.find(o => String(o.value) === String(normalized));
};

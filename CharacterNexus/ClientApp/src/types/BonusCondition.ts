export interface BonusCondition {
    field: string;   // dot-path into the form, e.g. "class" or "attributes.strength"
    formula: string; // same syntax as Prerequisite.formula, e.g. ">= 4" or "=== \"Fighter\""
}
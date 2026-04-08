import { BonusCondition } from "./BonusCondition";

export interface BonusCharacteristic {
    origin?: string;
    type: string;
    value: string;
    conditions?: BonusCondition[];
}

export interface BonusCharacteristics extends Array<BonusCharacteristic> {}
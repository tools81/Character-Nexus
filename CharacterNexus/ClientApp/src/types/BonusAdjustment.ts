import { BonusCondition } from "./BonusCondition";

export interface BonusAdjustment {
    origin?: string;
    type: string;
    name: string;
    value: number;
    conditions?: BonusCondition[];
}

export interface BonusAdjustments extends Array<BonusAdjustment> {}
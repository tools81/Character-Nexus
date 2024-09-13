export interface BonusAdjustment {
    type: string;
    name: string;
    value: number;
}

export interface BonusAdjustments extends Array<BonusAdjustment> {}
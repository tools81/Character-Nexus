export interface BonusAdjustment {
    origin: string;
    type: string;
    name: string;
    value: number;
}

export interface BonusAdjustments extends Array<BonusAdjustment> {}
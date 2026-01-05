export interface BonusCharacteristic {
    origin?: string;
    type: string;
    value: string;
}

export interface BonusCharacteristics extends Array<BonusCharacteristic> {}
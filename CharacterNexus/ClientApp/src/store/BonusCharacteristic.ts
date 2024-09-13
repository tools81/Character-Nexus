export interface BonusCharacteristic {
    type: string;
    value: string;
}

export interface BonusCharacteristics extends Array<BonusCharacteristic> {}
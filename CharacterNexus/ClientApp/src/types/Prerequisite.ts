export interface Prerequisite {
    type: string;
    name: string;
    formula: string;
    logicalOr?: Array<Prerequisite>;
}

export interface Prerequisites extends Array<Prerequisite> {}
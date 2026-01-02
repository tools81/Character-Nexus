export interface Prerequisite {
    type: string;
    name: string;
    formula: string;
}

export interface Prerequisites extends Array<Prerequisite> {}
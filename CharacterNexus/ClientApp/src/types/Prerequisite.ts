export interface Prerequisite {
    type: string;
    value: any;
}

export interface Prerequisites extends Array<Prerequisite> {}
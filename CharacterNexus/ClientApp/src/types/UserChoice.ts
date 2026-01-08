export interface UserChoice {
    origin?: string;
    type: string;
    choices: string[];
    count: number;
    category: string;
    value: string;
}

export interface UserChoices extends Array<UserChoice> {}
export interface UserChoice {
    type: string;
    choices: string[];
    count: number;
    category: string;
    value: string;
}

export interface UserChoices extends Array<UserChoice> {}
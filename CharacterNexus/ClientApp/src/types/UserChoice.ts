export interface UserChoice {
    origin?: string;
    label?: string;
    type: string;
    choices: string[];
    choiceDescriptions?: string[];
    count: number;
    category: string;
    value: string;
}

export interface UserChoices extends Array<UserChoice> {}
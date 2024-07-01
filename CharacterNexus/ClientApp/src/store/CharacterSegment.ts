// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface CharacterSegmentState {
    isLoading: boolean;
}

export interface CharacterSegment {
    id: any;
    name: string;
    imageUrl: string;
    level: number;
    details: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.


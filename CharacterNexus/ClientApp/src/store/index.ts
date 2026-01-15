import * as Ruleset from '../types/Ruleset';
import 'bootstrap/dist/css/bootstrap.min.css';
import { combineReducers } from '@reduxjs/toolkit';
import characterSegmentReducer from './slices/characterSegmentSlice';
import characterReducer from './slices/characterSlice';
import rulesetReducer from './slices/rulesetSlice';

// The top-level state object
export interface ApplicationState {
    ruleset: Ruleset.RulesetState | undefined;
}

// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
export const reducers : any = {
    characterSegment: characterSegmentReducer,
    character: characterReducer,
    ruleset: rulesetReducer
};

export type RootState = ReturnType<typeof reducers>;

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => ApplicationState): void;
}

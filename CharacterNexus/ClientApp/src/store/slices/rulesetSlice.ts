import { createSlice, PayloadAction, createAsyncThunk } from '@reduxjs/toolkit';

export interface Ruleset {
  name: string;
  rulesetName: string;
  imageSource: string;
  logoSource: string;
  instructions: string;
}

export interface RulesetState {
  rulesets: Ruleset[];           // all available rulesets
  currentRuleset: Ruleset | null; // the selected ruleset
  isLoading: boolean;
  error?: string | null;
}

const defaultRuleset: Ruleset = {
  name: '',
  rulesetName: '',
  imageSource: '',
  logoSource: '',
  instructions: '',
};

const initialState: RulesetState = {
  rulesets: [],
  currentRuleset: null,
  isLoading: false,
  error: null,
};

const BASE_URL =
  process.env.REACT_APP_API_BASE ||
  `${window.location.protocol}//${window.location.host}`;

// Async thunk to fetch all rulesets
export const fetchRulesets = createAsyncThunk<
  Ruleset[],
  void,
  { rejectValue: string }
>('ruleset/fetchRulesets', async (_, thunkAPI) => {
  try {
    const response = await fetch(`${BASE_URL}/api/ruleset/rulesets`);
    console.log(response);
    if (!response.ok) throw new Error('Failed to fetch rulesets');
    const data = (await response.json()) as Ruleset[];
    return data;
  } catch (error: any) {
    return thunkAPI.rejectWithValue(error.message);
  }
});

const rulesetSlice = createSlice({
  name: 'ruleset',
  initialState,
  reducers: {
    setRulesets(state, action: PayloadAction<Ruleset[]>) {
      state.rulesets = action.payload;
    },
    addRuleset(state, action: PayloadAction<Ruleset>) {
      state.rulesets.push(action.payload);
    },
    updateRuleset(state, action: PayloadAction<Ruleset>) {
      const index = state.rulesets.findIndex(r => r.name === action.payload.name);
      if (index >= 0) state.rulesets[index] = action.payload;
    },
    setCurrentRuleset(state, action: PayloadAction<Ruleset>) {
      state.currentRuleset = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchRulesets.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(fetchRulesets.fulfilled, (state, action) => {
        state.rulesets = action.payload;
        state.isLoading = false;
      })
      .addCase(fetchRulesets.rejected, (state, action) => {
        state.error = action.payload || 'Failed to fetch rulesets';
        state.isLoading = false;
      });
  },
});

export const { setRulesets, addRuleset, updateRuleset, setCurrentRuleset } =
  rulesetSlice.actions;

export default rulesetSlice.reducer;

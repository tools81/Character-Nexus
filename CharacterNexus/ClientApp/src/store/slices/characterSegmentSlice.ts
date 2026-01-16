import { createSlice, PayloadAction, createAsyncThunk } from '@reduxjs/toolkit';

export interface CharacterSegment {
  id: string;
  name: string;
  level: number;
  levelName: string;
  image: string;
  details: string;
  characterSheet?: any;
}

interface CharacterSegmentState {
  characterSegments: CharacterSegment[];
  isLoading: boolean;
  error?: string | null;
}

const initialState: CharacterSegmentState = {
  characterSegments: [],
  isLoading: false,
  error: null,
};

const BASE_URL =
  process.env.REACT_APP_API_BASE ||
  `${window.location.protocol}//${window.location.host}`;

// Async thunk to fetch characters
export const fetchCharacters = createAsyncThunk<
  CharacterSegment[],
  string, // ruleset name
  { rejectValue: string }
>('character/fetchCharacters', async (rulesetName, thunkAPI) => {
  try {
    const response = await fetch(
      `${BASE_URL}/api/ruleset/characters?ruleset=${encodeURIComponent(rulesetName)}`
    );
    if (!response.ok) throw new Error('Failed to fetch characters');
    const data = (await response.json()) as CharacterSegment[];
    return data;
  } catch (error: any) {
    return thunkAPI.rejectWithValue(error.message);
  }
});

// Async thunk to delete a character
export const deleteCharacter = createAsyncThunk<
  string, // return deleted character id
  { id: string; rulesetName: string },
  { rejectValue: string }
>('character/deleteCharacter', async ({ id, rulesetName }, thunkAPI) => {
  try {
    const response = await fetch(
      `${BASE_URL}/api/character/delete?ruleset=${encodeURIComponent(
        rulesetName
      )}&characterName=${encodeURIComponent(id)}`,
      { method: 'DELETE' }
    );
    if (!response.ok) throw new Error('Failed to delete character');
    return id;
  } catch (error: any) {
    return thunkAPI.rejectWithValue(error.message);
  }
});

const characterSegmentSlice = createSlice({
  name: 'characterSegment',
  initialState,
  reducers: {
    addCharacterSegment(state, action: PayloadAction<CharacterSegment>) {
      state.characterSegments.push(action.payload);
    },
    updateCharacterSegment(state, action: PayloadAction<CharacterSegment>) {
      const index = state.characterSegments.findIndex((c) => c.id === action.payload.id);
      if (index >= 0) state.characterSegments[index] = action.payload;
    },
    setCharacterSegments(state, action: PayloadAction<CharacterSegment[]>) {
      state.characterSegments = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder
      // Fetch characters
      .addCase(fetchCharacters.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(fetchCharacters.fulfilled, (state, action) => {
        state.characterSegments = action.payload;
        state.isLoading = false;
      })
      .addCase(fetchCharacters.rejected, (state, action) => {
        state.error = action.payload || 'Failed to fetch characters';
        state.isLoading = false;
      })

      // Delete character
      .addCase(deleteCharacter.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(deleteCharacter.fulfilled, (state, action) => {
        state.characterSegments = state.characterSegments.filter((c) => c.id !== action.payload);
        state.isLoading = false;
      })
      .addCase(deleteCharacter.rejected, (state, action) => {
        state.error = action.payload || 'Failed to delete character';
        state.isLoading = false;
      });
  },
});

export const { addCharacterSegment, updateCharacterSegment, setCharacterSegments } = characterSegmentSlice.actions;
export default characterSegmentSlice.reducer;

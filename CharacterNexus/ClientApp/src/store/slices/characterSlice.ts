import { createSlice, createAsyncThunk, PayloadAction } from '@reduxjs/toolkit';

interface CharacterSchema {
  fields: any[];
}

interface CharacterState {
  schema: CharacterSchema | null;
  currentCharacter: any | null;
  isLoading: boolean;
  error: string | null;
}

const initialState: CharacterState = {
  schema: null,
  currentCharacter: null,
  isLoading: false,
  error: null
};

const BASE_URL =
  process.env.REACT_APP_API_BASE ||
  `${window.location.protocol}//${window.location.host}`;

// Fetch schema for a ruleset (new character)
export const fetchCharacterSchema = createAsyncThunk<
  CharacterSchema,
  string,
  { rejectValue: string }
>('character/fetchSchema', async (rulesetName, { rejectWithValue }) => {
  try {
    const response = await fetch(
      `${BASE_URL}/api/character/new?ruleset=${encodeURIComponent(
        rulesetName
      )}`
    );
    if (!response.ok) throw new Error('Failed to fetch character schema');
    return (await response.json()) as CharacterSchema;
  } catch (err: any) {
    return rejectWithValue(err.message);
  }
});

// Load saved character by name
export const fetchCharacterByName = createAsyncThunk<
  any,
  { rulesetName: string; characterName: string },
  { rejectValue: string }
>('character/fetchByName', async ({ rulesetName, characterName }, { rejectWithValue }) => {
  try {
    const response = await fetch(
      `${BASE_URL}/api/character/load?ruleset=${encodeURIComponent(
        rulesetName
      )}&characterName=${encodeURIComponent(characterName)}`
    );
    if (!response.ok) throw new Error('Failed to fetch character');
    return await response.json();
  } catch (err: any) {
    return rejectWithValue(err.message);
  }
});

// Save character
export const saveCharacter = createAsyncThunk<
  void,
  { rulesetName: string; characterData: any; imageFile?: File },
  { rejectValue: string }
>('character/save', async ({ rulesetName, characterData, imageFile }, { rejectWithValue }) => {
  try {
    const formData = new FormData();
    if (imageFile) {
      formData.append('Image', imageFile, `${characterData.name}.${imageFile.name.split('.').pop()}`);
    }
    formData.append('JsonData', JSON.stringify(characterData));

    const response = await fetch(
      `${BASE_URL}/api/character/save?ruleset=${encodeURIComponent(
        rulesetName
      )}`,
      { method: 'POST', body: formData }
    );
    if (!response.ok) throw new Error('Failed to save character');
  } catch (err: any) {
    return rejectWithValue(err.message);
  }
});

const characterSlice = createSlice({
  name: 'character',
  initialState,
  reducers: {
    clearCurrentCharacter(state) {
      state.currentCharacter = null;
      state.schema = null;
      state.error = null;
    }
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchCharacterSchema.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(fetchCharacterSchema.fulfilled, (state, action: PayloadAction<CharacterSchema>) => {
        state.isLoading = false;
        state.schema = action.payload;
      })
      .addCase(fetchCharacterSchema.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload || 'Unknown error fetching schema';
      })

      .addCase(fetchCharacterByName.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(fetchCharacterByName.fulfilled, (state, action: PayloadAction<any>) => {
        state.isLoading = false;
        state.currentCharacter = action.payload;
      })
      .addCase(fetchCharacterByName.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload || 'Unknown error loading character';
      })

      .addCase(saveCharacter.pending, (state) => {
        state.isLoading = true;
        state.error = null;
      })
      .addCase(saveCharacter.fulfilled, (state) => {
        state.isLoading = false;
      })
      .addCase(saveCharacter.rejected, (state, action) => {
        state.isLoading = false;
        state.error = action.payload || 'Unknown error saving character';
      });
  }
});

export const { clearCurrentCharacter } = characterSlice.actions;
export default characterSlice.reducer;

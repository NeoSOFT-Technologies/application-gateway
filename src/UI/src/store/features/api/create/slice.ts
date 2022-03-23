import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import error from "../../../../utils/error";
import { IApiFormData } from "../../../../types/api/index";
import { addApiDataService } from "../../../../services/api/api";

interface IAddApiState {
  apiAdded?: boolean;
  loading: boolean;
  error?: string | null;
}
const initialState: IAddApiState = {
  apiAdded: false,
  loading: false,
  error: null,
};

export const addNewApi = createAsyncThunk(
  "api/createapi",
  async (conditions: IApiFormData) => {
    try {
      const response = await addApiDataService(conditions);
      console.log(response);
      return response.data;
    } catch (err) {
      return err;
    }
  }
);

const slice = createSlice({
  name: "addapi",
  initialState,
  reducers: {},
  extraReducers(builder): void {
    builder.addCase(addNewApi.pending, (state) => {
      state.loading = true;
    });
    builder.addCase(addNewApi.fulfilled, (state, action) => {
      state.loading = false;
      state.apiAdded = true;
    });
    builder.addCase(addNewApi.rejected, (state, action) => {
      state.loading = false;
      // action.payload contains error information
      state.error = error(action.payload);
    });
  },
});

export default slice.reducer;

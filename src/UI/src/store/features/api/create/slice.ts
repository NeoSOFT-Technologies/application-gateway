import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
// import error from "../../../../utils/error";
import { addApiDataService } from "../../../../services/api/api";
import { IAddApiState, IApiFormData } from ".";
import axios, { AxiosError } from "axios";

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
      return response.data;
    } catch (err) {
      const myError = err as Error | AxiosError;
      if (axios.isAxiosError(myError) && myError.response)
        throw myError.response.data.Errors[0];
      else throw myError.message;
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
    builder.addCase(addNewApi.fulfilled, (state) => {
      state.loading = false;
      state.apiAdded = true;
    });
    builder.addCase(addNewApi.rejected, (state, action) => {
      state.loading = false;
      action.payload = action.error;
    });
  },
});

export default slice.reducer;

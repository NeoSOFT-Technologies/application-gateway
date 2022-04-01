import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
// import error from "../../../../utils/error";
import { addApiDataService } from "../../../../services/api/api";
// import { AxiosError } from "axios";
import { IAddApiState, IApiFormData } from ".";
import { AxiosError } from "axios";

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
      const myError = err as AxiosError;
      console.log(typeof myError.message);
      // if (myError !== undefined) throw myError.response?.data.Errors[0];
      // else
      throw myError;
      //  const ErrorResponse = myError.response?.data;
      // return ErrorResponse;
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
      console.log("actiOn", action);
      action.payload = action.error;
    });
  },
});

export default slice.reducer;

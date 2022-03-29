import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import error from "../../../../utils/error";
import { IApiGetByIdState } from "../../../../types/api/index";
import { getApiByIdService } from "../../../../services/api/api";

const initialState: IApiGetByIdState = {
  data: null,
  loading: false,
  error: null,
};

export const getApiById = createAsyncThunk(
  "api/getApiById",
  async (Id: string) => {
    try {
      const response = await getApiByIdService(Id);
      // console.log(response);
      return response?.data;
    } catch (err) {
      const myError = err as Error;
      // console.log("");
      throw myError;
    }
  }
);

const slice = createSlice({
  name: "getApiById",
  initialState,
  reducers: {},
  extraReducers(builder): void {
    builder.addCase(getApiById.pending, (state) => {
      state.loading = true;
    });
    builder.addCase(getApiById.fulfilled, (state, action) => {
      state.loading = false;
      state.data = action.payload.Data;
    });
    builder.addCase(getApiById.rejected, (state, action) => {
      state.loading = false;
      // action.payload contains error information
      state.error = error(action.payload);
      action.payload = action.error;
    });
  },
});

export default slice.reducer;

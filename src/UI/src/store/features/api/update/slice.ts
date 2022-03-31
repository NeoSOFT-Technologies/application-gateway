import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import error from "../../../../utils/error";
import {
  getApiByIdService,
  updateApiService,
} from "../../../../services/api/api";
import { initialState } from "./payload";
import { IGetApiByIdData } from ".";

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
export const updateApi = createAsyncThunk(
  "api/update",
  async (data: IGetApiByIdData) => {
    // const { data } = conditions;
    try {
      const response = await updateApiService(data);
      console.log(response);
      return response.data;
    } catch (err) {
      return err;
    }
  }
);

const slice = createSlice({
  name: "apiUpdate",
  initialState,
  reducers: {
    setForm: (state, action) => {
      console.log("action - ", action.payload);
      state.data.form = action.payload; // OnChange
      console.log("form slice - ", state.data.form);
    },
    setFormError: (state, action) => {
      state.data.errors = action.payload; // OnChange
      console.log("error slice - ", state.data.errors);
    },
  },
  extraReducers(builder): void {
    builder.addCase(getApiById.pending, (state) => {
      state.loading = true;
    });
    builder.addCase(getApiById.fulfilled, (state, action) => {
      state.loading = false;
      state.data.form = action.payload.Data;
      // console.log("form slice1 - ", state.data.form);
    });
    builder.addCase(getApiById.rejected, (state, action) => {
      state.loading = false;
      // action.payload contains error information
      state.error = error(action.payload);
      action.payload = action.error;
    });
    builder.addCase(updateApi.pending, (state) => {
      state.loading = true;
    });
    builder.addCase(updateApi.fulfilled, (state, action) => {
      state.loading = false;
      state.data = action.payload;
    });
    builder.addCase(updateApi.rejected, (state, action) => {
      state.loading = false;
      // action.payload contains error information
      state.error = error(action.payload);
    });
  },
});

export const { setForm, setFormError } = slice.actions;
export default slice.reducer;

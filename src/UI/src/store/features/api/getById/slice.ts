import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import error from "../../../../utils/error";
import { IApiGetByIdState } from "../../../../types/api/index";
import { getApiByIdService } from "../../../../services/api/api";

const initialState: IApiGetByIdState = {
  data: {
    form: {
      apiId: "",
      apiName: "",
      listenPath: "",
      stripListenPath: "",
      targetUrl: "",
      isActive: true,
      rateLimit: {
        rate: 0,
        per: 0,
        isDisabled: false,
      },
      versioningInfo: { location: 0, key: "" },
      defaultVersion: "",
      version: { name: "", overrideTarget: "", expires: "" },
      isQuotaDisabled: false,
    },
    errors: null,
  },
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
  reducers: {
    setForm: (state, action) => {
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
      console.log("form slice1 - ", state.data.form);
    });
    builder.addCase(getApiById.rejected, (state, action) => {
      state.loading = false;
      // action.payload contains error information
      state.error = error(action.payload);
      action.payload = action.error;
    });
  },
});

export const { setForm, setFormError } = slice.actions;
export default slice.reducer;

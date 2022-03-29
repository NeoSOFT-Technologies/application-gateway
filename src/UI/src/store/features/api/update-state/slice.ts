import { createSlice } from "@reduxjs/toolkit";
import { IUpdateState } from "../../../../types/api";

const initialState: IUpdateState = {
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
  errors: {
    apiId: "",
    apiName: "",
    listenPath: "",
    stripListenPath: "",
    targetUrl: "",
    isActive: "",
    rate: "",
    perSecond: "",
    versioningInfo: "",
    defaultVersion: "",
    version: "",
    isQuotaDisabled: "",
  },
};
export const userSlice = createSlice({
  name: "api",
  initialState,
  reducers: {
    setForm: (state, action) => {
      state.form = action.payload; // OnChange
      console.log("form slice - ", state.form);
    },
    setFormError: (state, action) => {
      state.errors = action.payload; // OnChange
      console.log("error slice - ", state.errors);
    },
  },
});

export const { setForm, setFormError } = userSlice.actions;

export default userSlice.reducer;

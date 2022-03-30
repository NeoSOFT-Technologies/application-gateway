import { createSlice } from "@reduxjs/toolkit";
import { IUpdateState } from "../../../../types/api";

const initialState: IUpdateState = {
  form: {
    ApiId: "",
    Name: "",
    ListenPath: "",
    StripListenPath: false,
    TargetUrl: "",
    IsActive: true,
    IsInternal: false,
    Protocol: "",
    RateLimit: {
      Rate: 0,
      Per: 0,
      IsDisabled: false,
    },
    Blacklist: [],
    Whitelist: [],
    VersioningInfo: {
      Location: 0,
      Key: "",
    },
    DefaultVersion: "",
    Versions: [
      {
        Name: "",
        OverrideTarget: "",
        Expires: "",
        GlobalRequestHeaders: {},
        GlobalRequestHeadersRemove: [],
        GlobalResponseHeaders: {},
        GlobalResponseHeadersRemove: [],
        ExtendedPaths: null,
      },
    ],
    AuthType: "",
    OpenidOptions: {
      Providers: [],
    },
    LoadBalancingTargets: [],
    IsQuotaDisabled: false,
  },
  errors: null,
  // errors: {
  //   apiId: "",
  //   apiName: "",
  //   listenPath: "",
  //   stripListenPath: "",
  //   targetUrl: "",
  //   isActive: "",
  //   rate: "",
  //   perSecond: "",
  //   versioningInfo: "",
  //   defaultVersion: "",
  //   version: "",
  //   isQuotaDisabled: "",
  // },
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

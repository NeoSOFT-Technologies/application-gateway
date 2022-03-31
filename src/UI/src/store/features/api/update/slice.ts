import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import error from "../../../../utils/error";
import { updateApiService } from "../../../../services/api/api";
import { IGetApiByIdData, IApiUpdateState } from "../../../../types/api";

// interface IConditions {
//   data: IApiUpdateFormData;
// }
const initialState: IApiUpdateState = {
  // data: null,
  data: {
    apiId: "",
    name: "",
    listenPath: "",
    stripListenPath: false,
    targetUrl: "",
    isActive: true,
    isInternal: false,
    protocol: "",
    rateLimit: {
      rate: 0,
      per: 0,
      isDisabled: false,
    },
    blacklist: [],
    whitelist: [],
    versioningInfo: {
      location: "",
      key: "",
    },
    defaultVersion: "",
    versions: [
      {
        name: "",
        overrideTarget: "",
        expires: "",
        globalRequestHeaders: {
          additionalProp1: "",
          additionalProp2: "",
          additionalProp3: "",
        },
        globalRequestHeadersRemove: [],
        globalResponseHeaders: {
          additionalProp1: "",
          additionalProp2: "",
          additionalProp3: "",
        },
        globalResponseHeadersRemove: [],
        extendedPaths: {
          methodTransforms: [],
          urlRewrites: [],
          validateJsons: [],
          transformHeaders: [],
          transformResponseHeaders: [],
          transform: [],
          transformResponse: [],
        },
      },
    ],
    authType: "",
    openidOptions: {
      providers: [],
    },
    loadBalancingTargets: [],
    isQuotaDisabled: false,
  },

  loading: false,
  error: null,
};
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
  reducers: {},
  extraReducers(builder): void {
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

export default slice.reducer;

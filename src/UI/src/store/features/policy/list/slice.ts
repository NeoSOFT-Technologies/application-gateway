import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { policyListService } from "../../../../services/Policy/policy";
import { IPolicyListState } from "./index";
import error from "../../../../utils/error";
interface IConditions {
  currentPage: number;
}

const initialState: IPolicyListState = {
  data: null,
  loading: false,
  error: null,
};

export const getPolicyList = createAsyncThunk(
  "policy/list",
  async (conditions: IConditions) => {
    const { currentPage } = conditions;
    try {
      const response = await policyListService(currentPage);
      console.log(response);
      return response.data;
    } catch (err) {
      return err;
    }
  }
);

const slice = createSlice({
  name: "policy",
  initialState,
  reducers: {},
  extraReducers(builder): void {
    builder.addCase(getPolicyList.pending, (state) => {
      state.loading = true;
    });
    builder.addCase(getPolicyList.fulfilled, (state, action) => {
      state.loading = false;
      state.data = {
        Policies: action.payload.Data.Policies,
        TotalCount: Math.ceil(action.payload.TotalCount / 3),
      };
    });
    builder.addCase(getPolicyList.rejected, (state, action) => {
      state.loading = false;
      // action.payload contains error information
      state.error = error(action.payload);
    });
  },
});

export default slice.reducer;

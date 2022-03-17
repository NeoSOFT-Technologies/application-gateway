import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import error from "../../../../utils/error";
import { IPolicyListState } from "../../../../types/Policy/index";
import { policyListService } from "../../../../services/Policy/policy";
interface IConditions {
  currentPage: number;
}

const initialState: IPolicyListState = {
  data: null,
  loading: false,
  error: null,
};

export const getPolicyList = createAsyncThunk(
  "api/list",
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
  name: "api",
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
        TotalCount: action.payload.TotalCount,
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

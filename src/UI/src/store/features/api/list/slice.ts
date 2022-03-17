import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import error from "../../../../utils/error";
import { IApiListState } from "../../../../types/api/index";
import { apiListService } from "../../../../services/api/api";

interface IConditions {
  currentPage: number;
}

const initialState: IApiListState = {
  data: null,
  loading: false,
  error: null,
};

export const getApiList = createAsyncThunk(
  "api/list",
  async (conditions: IConditions) => {
    const { currentPage } = conditions;
    try {
      const response = await apiListService(currentPage);
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
    builder.addCase(getApiList.pending, (state) => {
      state.loading = true;
    });
    builder.addCase(getApiList.fulfilled, (state, action) => {
      state.loading = false;
      state.data = {
        Apis: action.payload.Data.Apis,
        TotalCount: action.payload.TotalCount,
      };
    });
    builder.addCase(getApiList.rejected, (state, action) => {
      state.loading = false;
      // action.payload contains error information
      state.error = error(action.payload);
    });
  },
});

export default slice.reducer;

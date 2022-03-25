import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import error from "../../../../utils/error";
import { IKeyListState } from "../../../../types/key/index";
import { keyListService } from "../../../../services/key/key";

interface IConditions {
  currentPage: number;
}

const initialState: IKeyListState = {
  data: null,
  loading: false,
  error: null,
};

export const getKeyList = createAsyncThunk(
  "key/list",
  async (conditions: IConditions) => {
    const { currentPage } = conditions;
    try {
      const response = await keyListService(currentPage);
      console.log(response);
      return response.data;
    } catch (err) {
      return err;
    }
  }
);

const slice = createSlice({
  name: "key",
  initialState,
  reducers: {},
  extraReducers(builder): void {
    builder.addCase(getKeyList.pending, (state) => {
      state.loading = true;
    });
    builder.addCase(getKeyList.fulfilled, (state, action) => {
      state.loading = false;
      state.data = {
        Keys: action.payload.Data.Keys,
        TotalCount: Math.ceil(action.payload.TotalCount / 3),
      };
    });
    builder.addCase(getKeyList.rejected, (state, action) => {
      state.loading = false;
      // action.payload contains error information
      state.error = error(action.payload);
    });
  },
});

export default slice.reducer;

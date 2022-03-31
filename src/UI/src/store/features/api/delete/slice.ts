import { createAsyncThunk, createSlice, current } from "@reduxjs/toolkit";
import error from "../../../../utils/error";
import { deleteApiDataService } from "../../../../services/api/api";
import { AxiosError } from "axios";
import { IDeleteApiState } from ".";

const initialState: IDeleteApiState = {
  isDeleted: false,
  loading: false,
  error: null,
};
export const deleteApi = createAsyncThunk(
  "api/deleteapi",
  async (Id: string) => {
    try {
      const response = await deleteApiDataService(Id);
      return response.data;
    } catch (err) {
      console.log(err);
      const myError = err as AxiosError;
      throw myError;
    }
  }
);

const slice = createSlice({
  name: "deleteapi",
  initialState,
  reducers: {},
  extraReducers(builder): void {
    builder.addCase(deleteApi.pending, (state) => {
      state.loading = true;
      state.isDeleted = false;
    });
    builder.addCase(deleteApi.fulfilled, (state, action) => {
      state.loading = false;
      state.isDeleted = true;
      console.log("state ", current(state));
    });
    builder.addCase(deleteApi.rejected, (state, action) => {
      state.loading = false;
      state.isDeleted = false;
      // action.payload contains error information
      state.error = error(action.payload);
      action.payload = action.error;
    });
  },
});

export default slice.reducer;

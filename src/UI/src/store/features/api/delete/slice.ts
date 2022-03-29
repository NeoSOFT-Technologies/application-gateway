import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import error from "../../../../utils/error";
import { deleteApiDataService } from "../../../../services/api/api";

interface IAddApiState {
  isDeleted?: boolean;
  loading: boolean;
  error?: string | null;
}

const initialState: IAddApiState = {
  isDeleted: false,
  loading: false,
  error: null,
};

export const deleteApi = createAsyncThunk(
  "api/deleteapi",
  async (Id: string) => {
    try {
      const response = await deleteApiDataService(Id);
      console.log(response);
      return response.data;
    } catch (err) {
      console.log(err);
      const myError = err as Error;
      // console.log("");
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

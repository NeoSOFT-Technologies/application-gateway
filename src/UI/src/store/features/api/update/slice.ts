import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import error from "../../../../utils/error";
import { updateApiService } from "../../../../services/api/api";
import { IApiUpdateFormData } from "../../../../types/api";

interface IConditions {
  data: IApiUpdateFormData;
}
const initialState: any = {
  data: null,
  loading: false,
  error: null,
};
export const updateApi = createAsyncThunk(
  "api/update",
  async (conditions: IConditions) => {
    const { data } = conditions;
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

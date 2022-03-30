import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import error from "../../../../utils/error";
import { deleteApiDataService } from "../../../../services/api/api";
import { IDeleteApiState } from "../../../../types/api/index";
import { AxiosError } from "axios";
// import store from "../../..";
// import { useLocation } from "react-router-dom";
// import { getApiList } from "../list/slice";
// import { getApiList } from "../list/slice";
// import { RootState } from "../../..";
// import { useAppSelector } from "../../../hooks";

// const apiList: IApiListState = useAppSelector(
//   (state: RootState) => state.apiList
// );
// const [state1, setstate] = useState(store);
// console.log("store: ", store.getState().apiList);
const initialState: IDeleteApiState = {
  isDeleted: false,
  loading: false,
  error: null,
  data: null, // store.getState().apiList.data
};

// const initialState: IApiListState = {
//   data: null,
//   loading: false,
//   error: null,
// };
console.log();
export const deleteApi = createAsyncThunk(
  "api/deleteapi",
  async (Id: string) => {
    try {
      const response = await deleteApiDataService(Id);
      console.log(response);
      return response.data;
    } catch (err) {
      console.log(err);
      const myError = err as AxiosError;
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
      // state.data = action.payload;
      state.isDeleted = true;

      // state.data = store.getState().apiList;
      // listData.data = listData.data?.Apis.filter(
      //   (item) => item.Id !== action.meta.arg
      // );
      // console.log(state.data?.Apis);
      // state.data = state.data?.Apis.filter(
      //   (item) => item.Id !== action.meta.arg
      // );
      // {
      //   Apis: state.data?.Apis.filter((item) => item.Id !== action.meta.arg),
      // };
      //  {action..data?.data?.Apis.filter(
      //   (item) => item.Id !== action.meta.arg
      // );}
      // console.log(action.payload + "\n" + action);
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

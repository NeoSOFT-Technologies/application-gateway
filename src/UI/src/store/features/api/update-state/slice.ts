import { createSlice } from "@reduxjs/toolkit";
const initialState: any = {
  form: {
    apiName: "Demo Api Name",
    listenPath: "/Demo Listen Path/",
    targetUrl: "https://google.com",
  },
  errors: {
    apiName: "",
    listenPath: "",
    targetUrl: "",
  },
};
export const userSlice = createSlice({
  name: "api",
  initialState,
  reducers: {
    setForm: (state, action) => {
      state.form = action.payload; // OnChange
      console.log("form - ", state.form);
    },
    setFormError: (state, action) => {
      state.errors = action.payload; // OnChange
      console.log("error - ", state.errors);
    },
  },
});

export const { setForm, setFormError } = userSlice.actions;

export default userSlice.reducer;

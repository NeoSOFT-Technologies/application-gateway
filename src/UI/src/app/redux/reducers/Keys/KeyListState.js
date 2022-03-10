const initialState = {
  list: [],
  count: 0,
  loading: false,
  error: "",
};
const setKeyList = (state = initialState, action) => {
  switch (action.type) {
    case "getKeys": {
      initialState.list = [action.payload.Data.KeyDto];
      return initialState;
    }
    case "Key_LOADING": {
      return {
        ...state.initialState,
        loading: true,
      };
    }
    case "KEY_LOADING_FAILURE": {
      console.log("loading data failed");
      return {
        loading: false,
        list: [],
        error: action.payload,
      };
    }
    default:
      return state;
  }
};
export default setKeyList;

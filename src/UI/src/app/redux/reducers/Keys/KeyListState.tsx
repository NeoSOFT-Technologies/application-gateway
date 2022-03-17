const initialState = {
  list: [],
  count: 0,
  loading: false,
  error: "",
  totalCount: 0,
};
const setKeyList = (state = initialState, action) => {
  switch (action.type) {
    case "getKeys": {
      //initialState.list = [action.payload.Data.KeyDto];
      initialState.list = [action.payload.Data.Keys];
      initialState.count = Math.ceil(action.payload.TotalCount / 3);
      initialState.totalCount = action.payload.TotalCount;
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

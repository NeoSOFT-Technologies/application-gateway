const initialState = {
  list: [],
  count: 0,
  loading: false,
};
const setKeyList = (state = initialState, action) => {
  switch (action.type) {
    case "getKeys": {
      //initialState.list = [action.payload.Data.KeyDto];
      initialState.list = [action.payload.listData];
      initialState.count = action.payload.countList;
      return initialState;
    }
    case "Key_LOADING": {
      return {
        ...state.initialState,
        loading: true,
      };
    }
    default:
      return state;
  }
};
export default setKeyList;

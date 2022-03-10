const initialState = {
  list: [],
  count: 0,
  loading: false,
  totalCount: 0,
};
const setAPIList = (state = initialState, action) => {
  switch (action.type) {
    case "getAPIs": {
      initialState.list = [action.payload.listData];
      initialState.count = action.payload.countList;
      initialState.totalCount = action.payload.total;
      return initialState;
    }
    case "API_LOADING": {
      return {
        ...state.initialState,
        loading: true,
      };
    }
    default:
      return state;
  }
};
export default setAPIList;

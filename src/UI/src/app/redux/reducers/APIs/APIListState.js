const initialState = {
  list: [],
  count: 0,
  loading: false,
  error: "",
  //totalCount: 0,
};
const setAPIList = (state = initialState, action) => {
  switch (action.type) {
    case "getAPIs": {
      initialState.list = [action.payload.Data.Apis];
      initialState.count = Math.ceil(action.payload.Data.Apis.length / 3);
      //initialState.list = [action.payload.listData];
      //initialState.count = action.payload.total;
      //initialState.count = action.payload.countList;
      //initialState.totalCount = action.payload.total;
      return initialState;
    }
    case "API_LOADING": {
      return {
        ...state.initialState,
        loading: true,
      };
    }
    case "API_LOADING_FAILURE": {
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
export default setAPIList;

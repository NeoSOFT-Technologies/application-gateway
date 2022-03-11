const initialState = {
  list: [],
  count: 0,
  loading: false,
  error: "",
  totalCount: 0,
};
const setPolicyList = (state = initialState, action) => {
  switch (action.type) {
    case "getPolicies": {
      initialState.list = [action.payload.listData];
      initialState.count = action.payload.countList;
      initialState.totalCount = action.payload.total;
      return initialState;
    }
    case "POLICY_LOADING": {
      return {
        ...state.initialState,
        loading: true,
      };
    }
    case "POLICY_LOADING_FAILURE": {
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
export default setPolicyList;

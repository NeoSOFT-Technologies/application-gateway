const initialState = {
  list: [],
  count: 0,
  loading: false,
};
const setPolicyList = (state = initialState, action) => {
  switch (action.type) {
    case "getPolicies": {
      initialState.list = [action.payload.Data];
      return initialState;
    }
    case "API_LOADING": {
      console.log("loading");
      return {
        ...state.initialState,
        loading: true,
      };
    }
    default:
      return state;
  }
};
export default setPolicyList;

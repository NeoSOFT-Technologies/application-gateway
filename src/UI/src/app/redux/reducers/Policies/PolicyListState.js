const initialState = {
  list: [],
  count: 0,
  loading: false,
  error: "",
};
const setPolicyList = (state = initialState, action) => {
  switch (action.type) {
    case "getPolicies": {
      initialState.list = [action.payload.Data];
      initialState.count = initialState.list[0].length;
      console.log(initialState.count);
      initialState.error = "";
      return initialState;
    }
    case "POLICY_LOADING": {
      console.log("loading");
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

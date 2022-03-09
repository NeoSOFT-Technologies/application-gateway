const initialState = {
  list: [],
  count: 0,
  loading: false,
};
const setPolicyList = (state = initialState, action) => {
  switch (action.type) {
    case "getPolicies": {
      //return { list: [...state.list, action.payload.Data.Apis] };
      //return action.payload.Data.Apis;
      // return {
      //   list: action.payload.Data,
      //   loading: false,
      // };
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

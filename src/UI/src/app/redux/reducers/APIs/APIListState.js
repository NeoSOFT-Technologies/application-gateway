const initialState = {
  list: [],
  count: 0,
  loading: false,
};
const setAPIList = (state = initialState, action) => {
  switch (action.type) {
    case "getAPIs": {
      initialState.list = [action.payload.Data.Apis];
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

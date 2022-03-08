const data = {
  list: null,
  count: 0,
};
const initialState = {
  list: data,
  loading: false,
};
const setKeyList = (state = initialState, action) => {
  switch (action.type) {
    case "getKeys": {
      return {
        list: action.payload.Data.KeyDto,
        loading: false,
      };
    }
    case "Key_LOADING": {
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
export default setKeyList;

// const setKeyList = (state = { list: [], count: 0 }, action) => {
//   switch (action.type) {
//     case "getKeys":
//       return action.payload.Data.KeyDto;
//     default:
//       return state;
//   }
// };
// export default setKeyList;

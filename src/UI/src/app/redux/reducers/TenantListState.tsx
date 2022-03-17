const setTenantList = (state = { list: [], count: 0 }, action) => {
  switch (action.type) {
    case "getTenant":
      return action.payload;
    default:
      return state;
  }
};
export default setTenantList;

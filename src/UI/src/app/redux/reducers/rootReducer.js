import { combineReducers } from "redux";
import setAdminLogin from "./AdminLoginState";
import setTenantLogin from "./TenantLoginState";
import setUserData from "./userDataState";
import setTenantList from "./TenantListState";
import setAPIList from "./APIs/APIListState";
const rootReducer = combineReducers({
  setAdminLogin,
  setTenantLogin,
  setUserData,
  setTenantList,
  setAPIList,
});
export default rootReducer;

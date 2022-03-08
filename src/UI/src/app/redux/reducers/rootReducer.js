import { combineReducers } from "redux";
import setAdminLogin from "./AdminLoginState";
import setTenantLogin from "./TenantLoginState";
import setUserData from "./userDataState";
import setTenantList from "./TenantListState";
import setAPIList from "./APIs/APIListState";
import setPolicyList from "./Policy/PolicyListState";
import setKeyList from "./Keys/KeyListState";
const rootReducer = combineReducers({
  setAdminLogin,
  setTenantLogin,
  setUserData,
  setTenantList,
  setAPIList,
  setPolicyList,
  setKeyList,
});
export default rootReducer;

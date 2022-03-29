import { configureStore, ThunkAction, Action } from "@reduxjs/toolkit";
import landingReducer from "./landing/slice";
import tenantListReducer from "./features/admin/tenant-list/slice";
import userDatatReducer from "./user-data/slice";
import tenantUserListReducer from "./features/tenant/tenant-user-list/slice";
import addNewTenantReducer from "./features/admin/add-tenant/slice";
import deleteTenantReducer from "./features/admin/delete-tenant/slice";
import updateTenantReducer from "./features/tenant/update-tenant/slice";
import addNewUserReducer from "./features/tenant/add-user/slice";
import apiListReducer from "./features/api/list/slice";
import policyListReducer from "./features/policy/list/slice";
import keyListReducer from "./features/key/list/slice";
import addApiReducer from "./features/api/create/slice";
import deleteApiReducer from "./features/api/delete/slice";
import getApiByIdReducer from "./features/api/getById/slice";
import apiBReducer from "./features/api/update/bslice";

const store = configureStore({
  reducer: {
    landing: landingReducer,
    tenantList: tenantListReducer,
    userData: userDatatReducer,
    tenantUserList: tenantUserListReducer,
    addNewTenant: addNewTenantReducer,
    deleteTenant: deleteTenantReducer,
    updateTenant: updateTenantReducer,
    addNewUser: addNewUserReducer,
    apiList: apiListReducer,
    policyList: policyListReducer,
    keyList: keyListReducer,
    addApi: addApiReducer,
    deleteApi: deleteApiReducer,
    getApiById: getApiByIdReducer,
    updateApiState: apiBReducer,
  },
});

export type AppDispatch = typeof store.dispatch;

export type RootState = ReturnType<typeof store.getState>;

export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;
export default store;

import React, { Suspense, lazy } from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import Spinner from "./components/loader/Loader";
// import { AdminGuard, TenantGuard } from "./utils/Authgaurd";
const Error404 = lazy(() => import("./pages/error-pages/Error404"));
const Error401 = lazy(() => import("./pages/error-pages/Error401"));
const Error500 = lazy(() => import("./pages/error-pages/Error500"));
const Login = lazy(() => import("./pages/login/Login"));
const TenantList = lazy(
  () => import("./pages/features/Admin/tenant-list/TenantList")
);
const APIList = lazy(() => import("./pages/features/api/list/APIList"));
const CreateApi = lazy(() => import("./pages/features/api/create/CreateApi"));
const RegisterTenant = lazy(
  () => import("./pages/features/Admin/register-tenant/RegisterTenant")
);
const PolicyList = lazy(
  () => import("./pages/features/policy/list/PolicyList")
);
const CreateKey = lazy(() => import("./pages/features/key/create/CreateKey"));
const CreatePolicy = lazy(
  () => import("./pages/features/policy/create/CreatePolicy")
);
const KeyList = lazy(() => import("./pages/features/key/list/KeyList"));
const Dashboard = lazy(() => import("./pages/features/Dashboard"));
const UpdateApi = lazy(() => import("./pages/features/api/update/Update"));
const TenantDetails = lazy(
  () => import("./pages/features/Admin/tenant-details/TenantDetails")
);
function AppRoutes() {
  return (
    <Suspense fallback={<Spinner />}>
      <Routes>
        <Route path="/login-page" element={<Login />} />
        <Route path="/error-pages/error-404" element={<Error404 />} />
        <Route path="/error-pages/error-500" element={<Error500 />} />
        <Route path="/error-pages/error-401" element={<Error401 />} />
        <Route path="/tenantlist" element={<TenantList />} />
        <Route path="/api/list" element={<APIList />} />
        <Route path="/policy/list" element={<PolicyList />} />
        <Route path="/key/list" element={<KeyList />} />
        <Route path="/api/create" element={<CreateApi />} />
        <Route path="/key/create" element={<CreateKey />} />
        <Route path="/policy/create" element={<CreatePolicy />} />
        <Route path="/registertenant" element={<RegisterTenant />} />
        <Route path="/dashboard" element={<Dashboard />} />
        <Route path="/api/update" element={<UpdateApi />} />
        <Route path="/tenantdetail" element={<TenantDetails />} />
        {/**********************************************************/}
        <Route path="*" element={<Navigate to="/login-page" />} />
      </Routes>
    </Suspense>
  );
}
export default AppRoutes;

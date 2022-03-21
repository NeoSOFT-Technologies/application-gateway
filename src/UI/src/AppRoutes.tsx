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
const APIList = lazy(() => import("./pages/features/APIs/List/APIList"));
const PolicyList = lazy(
  () => import("./pages/features/Policies/List/PolicyList")
);
const KeyList = lazy(() => import("./pages/features/Keys/List/KeyList"));
const Dashboard = lazy(() => import("./pages/features/Dashboard"));

function AppRoutes() {
  return (
    <Suspense fallback={<Spinner />}>
      <Routes>
        <Route path="/login-page" element={<Login />} />
        <Route path="/error-pages/error-404" element={<Error404 />} />
        <Route path="/error-pages/error-500" element={<Error500 />} />
        <Route path="/error-pages/error-401" element={<Error401 />} />
        <Route path="/tenantlist" element={<TenantList />} />
        <Route path="/apilist" element={<APIList />} />
        <Route path="/policylist" element={<PolicyList />} />
        <Route path="/keylist" element={<KeyList />} />
        <Route path="/dashboard" element={<Dashboard />} />
        {/**********************************************************/}
        <Route path="*" element={<Navigate to="/login-page" />} />
      </Routes>
    </Suspense>
  );
}
export default AppRoutes;

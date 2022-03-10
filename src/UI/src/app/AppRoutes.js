import React, { Component, Suspense, lazy } from "react";
import { Routes, Route, Navigate } from "react-router-dom";
import Spinner from "../app/shared/Spinner";
const Policies = lazy(() => import("./Components/Policies/List/PolicyList"));
const APIList = lazy(() => import("./Components/APIs//List/APIList"));
const KeyList = lazy(() => import("./Components/Keys/List/KeyList"));
const TenantList = lazy(() => import("./Components/TenantList"));
const Dashboard = lazy(() => import("./Components/Dashboard"));
const Login = lazy(() => import("./user-pages/Login"));

class AppRoutes extends Component {
  render() {
    return (
      <Suspense fallback={<Spinner />}>
        <Routes>
          <Route exact path="/policies" element={<Policies />} />
          <Route exact path="/apis" element={<APIList />} />
          <Route exact path="/keys" element={<KeyList />} />
          <Route path="/tenantlist" element={<TenantList />} />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/login" element={<Login />} />
          <Route path="*" element={<Navigate to="/login" />} />
        </Routes>
      </Suspense>
    );
  }
}

export default AppRoutes;

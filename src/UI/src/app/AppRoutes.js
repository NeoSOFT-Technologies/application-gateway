import React, { Component, Suspense, lazy } from "react";
import { Routes, Route } from "react-router-dom";
import Spinner from "../app/shared/Spinner";
const Policies = lazy(() => import("./Components/Policies/List/PolicyList"));
const APIList = lazy(() => import("./Components/APIs//List/APIList"));
const KeyList = lazy(() => import("./Components/Keys/List/KeyList"));
const TenantList = lazy(() => import("./Components/TenantList"));

class AppRoutes extends Component {
  render() {
    return (
      <Suspense fallback={<Spinner />}>
        <Routes>
          <Route exact path="/policies" element={<Policies />} />
          <Route exact path="/apis" element={<APIList />} />
          <Route exact path="/keys" element={<KeyList />} />
          <Route path="/tenantlist" element={<TenantList />} />
          {/* <Route path="*" element={<Navigate to="/apis" />} /> */}
        </Routes>
      </Suspense>
    );
  }
}

export default AppRoutes;

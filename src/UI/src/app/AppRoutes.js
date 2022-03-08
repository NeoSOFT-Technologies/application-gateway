import React, { Component, Suspense, lazy } from "react";
import { Routes, Route } from "react-router-dom";
import Spinner from "../app/shared/Spinner";
const Policies = lazy(() => import("./dashboard/Policy/Policies"));
const APIList = lazy(() => import("./dashboard/APIs/APIList"));
const KeyList = lazy(() => import("./dashboard/Keys/KeyList"));
const TenantList = lazy(() => import("./dashboard/TenantList"));

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

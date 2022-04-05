import React from "react";
import GlobalLimit from "../../../common-settings/global-limit/GlobalLimit";
import AccessList from "./api-access-rights/AccessList";
import ApiAccess from "./api-access/ApiAccess";
export default function AccessRights() {
  return (
    <div>
      <div className="col-lg-12 grid-margin stretch-card">
        <div>
          <div className="align-items-center">
            <div className="pt-2">
              <AccessList />
              <GlobalLimit />
              <ApiAccess />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

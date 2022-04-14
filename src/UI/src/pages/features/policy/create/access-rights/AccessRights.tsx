import React from "react";
import GlobalLimit from "../../../common-settings/global-limit/GlobalLimit";
import AccessList from "./api-access-rights/AccessList";
import ApiAccess from "./api-access/ApiAccess";
export default function AccessRights() {
  // const disabled = false;
  return (
    <div>
      <div>
        <div>
          <div className="align-items-center">
            <div className="pt-2">
              <AccessList />
              <GlobalLimit isDisabled={false} />
              <ApiAccess />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

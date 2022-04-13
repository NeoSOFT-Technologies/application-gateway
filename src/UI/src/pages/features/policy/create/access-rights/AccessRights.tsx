import React from "react";
import { useAppSelector } from "../../../../../store/hooks";
// import { useAppSelector } from "../../../../../store/hooks";
import GlobalLimit from "../../../common-settings/global-limit/GlobalLimit";
import AccessList from "./api-access-rights/AccessList";
import ApiAccess from "./api-access/ApiAccess";
export default function AccessRights() {
  const state = useAppSelector((RootState) => RootState.createPolicyState);
  return (
    <div>
      <div>
        <div>
          <div className="align-items-center">
            <div className="pt-2">
              <AccessList />
              <GlobalLimit isDisabled={false} />
              {state.data.form.apIs?.length > 0 ? <ApiAccess /> : <></>}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

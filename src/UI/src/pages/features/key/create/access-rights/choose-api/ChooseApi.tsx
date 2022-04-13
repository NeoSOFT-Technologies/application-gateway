import React from "react";
import GlobalLimit from "../../../../common-settings/global-limit/GlobalLimit";
import AccessList from "./api-access-rights/AccessList";
import ApiAccess from "./api-access/ApiAccess";

export default function ChooseApi() {
  return (
    <div>
      <h4>Choose Api</h4>
      <AccessList />
      <GlobalLimit
        isDisabled={true}
        policyId="e9420aa1-eec5-4dfc-8ddf-2bc989a9a47f"
      />
      <ApiAccess />
    </div>
  );
}

import React from "react";
import GlobalLimit from "../../../../common-settings/global-limit/GlobalLimit";
import AccessList from "./api-access-rights/AccessList";
import ApiAccess from "./api-access/ApiAccess";

export default function ChooseApi() {
  return (
    <div>
      <h4>Choose Api</h4>
      <AccessList />
      <GlobalLimit />
      <ApiAccess />
    </div>
  );
}

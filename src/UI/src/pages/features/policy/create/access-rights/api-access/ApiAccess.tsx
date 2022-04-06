import React from "react";
import GlobalLimit from "../../../../common-settings/global-limit/GlobalLimit";
import PathBased from "../../../../common-settings/path-based-permission/PathBased";

export default function ApiAccess() {
  return (
    <div>
      <h5>api access Setting</h5>
      <GlobalLimit />
      <PathBased />
    </div>
  );
}

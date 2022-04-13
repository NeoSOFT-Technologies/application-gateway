import React from "react";
import { useAppSelector } from "../../../../../../store/hooks";
import PathBased from "../../../../common-settings/path-based-permission/PathBased";
// import GlobalLimit from "../../../../common-settings-policy/global-limit/GlobalLimit";
// import PathBased from "../../../../common-settings/path-based-permission/PathBased";

export default function ApiAccess() {
  const state = useAppSelector((RootState) => RootState.createPolicyState);
  console.log("states", state);
  return (
    <>
      {state.data.form.apIs !== null &&
      state.data.form.apIs?.length > 0 &&
      Array.isArray(state.data.form.apIs) ? (
        (state.data.form.apIs as any[]).map((data: any, index: number) => {
          const { apIs } = data;
          console.log(apIs);
          return (
            <tr key={index}>
              <PathBased />
            </tr>
          );
        })
      ) : (
        <></>
      )}
    </>
  );
}

import React from "react";
import { useAppSelector } from "../../../../../../store/hooks";
import PathBased from "../../../../common-settings/path-based-permission/PathBased";
// import GlobalLimit from "../../../../common-settings-policy/global-limit/GlobalLimit";
// import PathBased from "../../../../common-settings/path-based-permission/PathBased";

export default function ApiAccess() {
  const state = useAppSelector((RootState) => RootState.createPolicyState);
  const apistate = useAppSelector((RootState) => RootState.updateApiState);
  console.log("states", state);
  return (
    <>
      <br /> <br />
      <div className="card col-lg-12 grid-margin stretch-card">
        {state.data.form.ApIs !== null &&
        state.data.form.ApIs?.length > 0 &&
        Array.isArray(state.data.form.ApIs) ? (
          (state.data.form.ApIs as any[]).map((data: any, index: number) => {
            // const { apIs } = data;
            // console.log(apIs);
            return (
              <tr key={index}>
                <PathBased
                  policystate={state}
                  apistate={apistate}
                  apidata={data[index]}
                  indexdata={index}
                />
              </tr>
            );
          })
        ) : (
          <></>
        )}
      </div>
    </>
  );
}

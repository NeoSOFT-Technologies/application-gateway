import React from "react";
import { useAppSelector } from "../../../../../../../store/hooks";
import PathBased from "../../../../../common-settings/path-based-permission/PathBased";

export default function ApiAccess() {
  const state = useAppSelector((RootState) => RootState.createKeyState);
  // console.log("states", state.data.form);
  return (
    <>
      <br /> <br />
      <h5>ApiAccess</h5> <br />
      <div className="card col-lg-12 grid-margin stretch-card">
        {state.data.form.AccessRights !== null &&
        state.data.form.AccessRights?.length! > 0 &&
        Array.isArray(state.data.form.AccessRights) ? (
          (state.data.form.AccessRights as any[]).map(
            (data: any, index: number) => {
              // console.log("apiacessIndex", index, data);
              return (
                <div key={index}>
                  <PathBased state={state} indexdata={index} />
                </div>
              );
            }
          )
        ) : (
          <></>
        )}
      </div>
    </>
  );
}

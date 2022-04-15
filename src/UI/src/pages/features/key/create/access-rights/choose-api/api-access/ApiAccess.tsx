import React from "react";
import { useAppSelector } from "../../../../../../../store/hooks";
import PathBased from "../../../../../common-settings/path-based-permission/PathBased";

export default function ApiAccess() {
  const state = useAppSelector((RootState) => RootState.createKeyState);
  console.log("states", state.data.form);
  return (
    <>
      <br /> <br /> <br />
      <div className="card col-lg-12 grid-margin stretch-card">
        {state.data.form.accessRights !== null &&
        state.data.form.accessRights?.length! > 0 &&
        Array.isArray(state.data.form.accessRights) ? (
          (state.data.form.accessRights as any[]).map(
            (data: any, index: number) => {
              console.log("Apiacess", data);
              return (
                <tr key={index}>
                  <PathBased
                    state={state}
                    apidata={data[index]}
                    indexdata={index}
                  />
                </tr>
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

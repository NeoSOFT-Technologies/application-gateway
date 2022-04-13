import React from "react";
import { useAppSelector } from "../../../../../../../store/hooks";
import PathBased from "../../../../../common-settings/path-based-permission/PathBased";

export default function ApiAccess() {
  const state = useAppSelector((RootState) => RootState.createKeyState);
  console.log("states", state.data.form);
  return (
    <div>
      <>
        {state.data.form.accessRights !== null &&
        state.data.form.accessRights?.length! > 0 &&
        Array.isArray(state.data.form.accessRights) ? (
          (state.data.form.accessRights as any[]).map(
            (data: any, index: number) => {
              const { policies } = data;
              console.log(policies);
              return (
                <tr key={index}>
                  <PathBased />
                </tr>
              );
            }
          )
        ) : (
          <></>
        )}
      </>
    </div>
  );
}

import React from "react";
import { useAppSelector } from "../../../../../../../store/hooks";
import GlobalLimit from "../../../../../common-settings/global-limit/GlobalLimit";

export default function Policies() {
  const state = useAppSelector((RootState) => RootState.createKeyState);
  console.log("states", state.data.form);
  return (
    <div>
      <>
        {state.data.form.policies !== null &&
        state.data.form.policies.length > 0 &&
        Array.isArray(state.data.form.policies) ? (
          (state.data.form.policies as any[]).map(
            (data: any, index: number) => {
              const { apiName } = data;
              console.log(apiName);
              return (
                <tr key={index}>
                  <GlobalLimit />
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

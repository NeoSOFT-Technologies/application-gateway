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
                  <GlobalLimit isDisabled={true} />
                </tr>
              );
            }
          )
        ) : (
          <></>
        )}
      </>
      {/* <GlobalLimit isDisabled={true} /> */}
      <GlobalLimit
        isDisabled={true}
        policyId="9f07e3a1-7c9c-4173-a172-a7c37668f9f6"
      />
    </div>
  );
}

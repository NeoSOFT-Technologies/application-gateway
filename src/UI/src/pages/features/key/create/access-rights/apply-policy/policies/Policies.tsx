import React from "react";
import { useAppSelector } from "../../../../../../../store/hooks";
import GlobalLimit from "../../../../../common-settings/global-limit/GlobalLimit";

export default function Policies() {
  const state = useAppSelector((RootState) => RootState.createKeyState);
  return (
    <>
      <br />
      <br /> <br /> <h5>Policies</h5> <br />
      {state.data.form.Policies !== null &&
      state.data.form.Policies.length > 0 &&
      Array.isArray(state.data.form.Policies) ? (
        (state.data.form.Policies as any[]).map((data: any, index: number) => {
          // const { policies } = data;
          return (
            <div key={index}>
              <GlobalLimit
                isDisabled={true}
                msg={""}
                policyId={data}
                index={index}
              />
            </div>
          );
        })
      ) : (
        <></>
      )}{" "}
      <br /> <br />{" "}
    </>
  );
}

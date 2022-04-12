import React from "react";
import { useAppSelector } from "../../../../../../store/hooks";
import GlobalLimit from "../../../../common-settings/global-limit/GlobalLimit";
import AccessList from "./api-access-rights/AccessList";
import ApiAccess from "./api-access/ApiAccess";

export default function ChooseApi() {
  const state = useAppSelector((RootState) => RootState.createKeyState);
  console.log(state.data.form.accessRights?.length);
  return (
    <div>
      <h4>Choose Api</h4>
      <AccessList />
      <GlobalLimit />
      {/* <ApiAccess /> */}

      {state.data.form.accessRights?.length! > 1 ? <ApiAccess /> : <></>}
    </div>
  );
}

import { FormEvent } from "react";
import { ToastAlert } from "../../../../components/ToasterAlert/ToastAlert";
import { updateApi } from "../../../../store/features/api/update/slice";
import { IApiGetByIdState } from "../../../../types/api";

async function handleSubmitApiUpdate(
  event: FormEvent,
  state: IApiGetByIdState,
  dispatch: any
) {
  event.preventDefault();
  if (
    state.data.form.Name !== "" &&
    state.data.form.ListenPath !== "" &&
    state.data.form.TargetUrl !== ""
  ) {
    await dispatch(updateApi(state.data.form));
    ToastAlert("Api Updated Successfully!!", "success");
  } else {
    ToastAlert("Please correct the error", "error");
  }
}
export default handleSubmitApiUpdate;

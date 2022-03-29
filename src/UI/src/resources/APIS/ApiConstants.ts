import {
  setFormError,
  setForm,
} from "../../store/features/api/update-state/slice";
export const regexForName = /^[A-Z a-z]{1,29}$/;
export const regexForListenPath = /^[/][a-zA-Z0-9]*[/]$/; // "//" + "[a-z]*" + "//"; // /^[a-z]*ing$/i; (\/)(A-Z a-z)?(\/)
export const regexForTagetUrl =
  /[(http(s)?):\\\\(www\\.)?a-zA-Z0-9@:%._\\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)/;
// /[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)?/gi;
export const regexForNumber = /^[0-9]*$/;

export function setFormData(e: any, dispatch: any, state: any) {
  // console.log("ye object hai - ", e);
  dispatch(setForm({ ...state.form, [e.target.name]: e.target.value }));
}
export function setFormErrors(e: any, dispatch: any) {
  // console.log("ye apna err - ", e);
  // {listenPath: "Enter Valid Listen Path", apiName: "Enter Valid Api Name"}
  dispatch(setFormError(e));
}

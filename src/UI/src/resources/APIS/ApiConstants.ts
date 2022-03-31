import { setFormError, setForm } from "../../store/features/api/getById/slice";
export const regexForName = /^[A-Z a-z][A-Z a-z 0-9+&@#\\/%=_|$?!:,.-]{3,29}$/;
export const regexForListenPath = /^[/][a-zA-Z0-9]*[/]$/;
export const regexForTagetUrl =
  /^(?:(?:https?|ftp|file):\/\/|www\.|ftp\.)(?:\([a-zA-Z0-9+&@#\\/%=_|$?!:,.]*\)|[-A-Z0-9+&@#\\/%=_|$?!:,.])*(?:\([-A-Z0-9+&@#\\/%=_|$?!:,.]*\)|[A-Z0-9+&@#\\/%=_|$])*/;

export const regexForNumber = /^[0-9]*$/;

function setNestedState(e: any, state: any) {
  const path = e.target.name.split(".");
  const value =
    e.target.type === "checkbox" ? e.target.checked : e.target.value;
  const depth = path.length;
  const oldstate = state.data.form;
  const newstate = { ...oldstate };
  let newStateLevel = newstate;
  let oldStateLevel = oldstate;

  for (let i = 0; i < depth; i += 1) {
    if (i === depth - 1) {
      newStateLevel[path[i]] = value;
    } else {
      newStateLevel[path[i]] = { ...oldStateLevel[path[i]] };
      oldStateLevel = oldStateLevel[path[i]];
      newStateLevel = newStateLevel[path[i]];
    }
  }
  return newstate;
}
export function setFormData(e: any, dispatch: any, state: any) {
  const newState = setNestedState(e, state);
  dispatch(setForm(newState));
}

// export function setFormData(
//   e: any,
//   dispatch: any,
//   state: any,
//   complex?: any,
//   objName?: string
// ) {
//   console.log("ye object hai - ", complex);
//   if (complex !== null && complex !== undefined) {
//     if (objName === "RateLimit") {
//       dispatch(setForm({ ...state.data.form, RateLimit: complex }));
//     }
//   } else if (e.target.type === "checkbox") {
//     dispatch(
//       setForm({ ...state.data.form, [e.target.name]: e.target.checked })
//     );
//   } else {
//     dispatch(setForm({ ...state.data.form, [e.target.name]: e.target.value }));
//   }
//   // dispatch(setForm({ ...state.data.form, [e.target.name]: e.target.value }));
// }

export function setFormErrors(e: any, dispatch: any) {
  // console.log("ye apna err - ", e);
  // {listenPath: "Enter Valid Listen Path", apiName: "Enter Valid Api Name"}
  dispatch(setFormError(e));
}

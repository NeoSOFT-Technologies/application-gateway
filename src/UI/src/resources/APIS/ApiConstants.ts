import { setFormError, setForm } from "../../store/features/api/getById/slice";
import { camelCase } from "lodash";

export const regexForName = /^[A-Z a-z]{1,29}$/;
export const regexForListenPath = /^[/][a-zA-Z0-9]*[/]$/; // "//" + "[a-z]*" + "//"; // /^[a-z]*ing$/i; (\/)(A-Z a-z)?(\/)
export const regexForTagetUrl =
  // /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$/;
  /[(http(s)?):\\\\(www\\.)?a-zA-Z0-9@:%._\\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)/;
// /[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)?/gi;
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

export function setFormErrors(e: any, dispatch: any) {
  dispatch(setFormError(e));
}

export const camelizeKeys: any = (obj: any) => {
  if (Array.isArray(obj)) {
    return obj.map((v) => camelizeKeys(v));
  } else if (obj != null && obj.constructor === Object) {
    return Object.keys(obj).reduce(
      (result, key) => ({
        ...result,
        [camelCase(key)]: camelizeKeys(obj[key]),
      }),
      {}
    );
  }
  return obj;
};

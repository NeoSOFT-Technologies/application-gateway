export const regexForName = /^[A-Z a-z]{4,29}$/;
export const regexForListenPath = /^[/][a-zA-Z0-9]*[/]$/; // "//" + "[a-z]*" + "//"; // /^[a-z]*ing$/i; (\/)(A-Z a-z)?(\/)
export const regexForTagetUrl =
  /[(http(s)?):\\\\(www\\.)?a-zA-Z0-9@:%._\\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)/;
// /[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)?/gi;

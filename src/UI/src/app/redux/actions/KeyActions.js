import { KeyListService } from "../../Services/Keys/KeyService";

export const getKeyList = async () => {
  let res = await KeyListService();
  return {
    type: "getKeys",
    payload: { ...res.data },
  };
};

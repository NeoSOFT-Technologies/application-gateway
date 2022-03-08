import { KeyListService } from "../../config/Keys/KeyService";

export const getKeyList = async () => {
  let res = await KeyListService();
  console.log(res);
  return {
    type: "getKeys",
    payload: { ...res.data },
  };
};

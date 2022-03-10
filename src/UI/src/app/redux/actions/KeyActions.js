import { KeyListService } from "../../Services/Keys/KeyService";

export const getKeyList = async (currentPage) => {
  let res = await KeyListService();
  let start = (currentPage - 1) * 5;
  let count = Math.ceil(res.data.Data.KeyDto.length / 5);
  let actionData = res.data.Data.KeyDto.splice(start, 5);
  return {
    type: "getKeys",
    //payload: { ...res.data },
    payload: {
      listData: [...actionData],
      countList: count,
    },
  };
};

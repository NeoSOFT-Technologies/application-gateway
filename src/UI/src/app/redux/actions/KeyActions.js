import { KeyListService } from "../../Services/Keys/KeyService";

export const getKeyList = async (currentPage) => {
  let res = await KeyListService(currentPage);
  console.log("Action", res);
  // let totalData = res.data.Data.Keys.length;
  // let start = (currentPage - 1) * 5;
  // let count = Math.ceil(res.data.Data.Keys.length / 5);
  // let actionData = res.data.Data.Keys.splice(start, 5);
  return {
    type: "getKeys",
    payload: { ...res.data },
    // payload: {
    //   listData: [...actionData],
    //   countList: count,
    //   total: totalData,
    // },
  };
};

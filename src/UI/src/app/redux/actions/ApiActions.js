import { ApiListService } from "../../Services/APIs/APIService";

export const getAPIList = async (currentPage) => {
  let res = await ApiListService(currentPage);
  //let totalData = res.data.Data.Apis.length;
  console.log("action", res);
  // let start = (currentPage - 1) * 5;
  // let count = Math.ceil(res.data.Data.Apis.length / 5);
  // let actionData = res.data.Data.Apis.splice(start, 5);
  return {
    type: "getAPIs",
    payload: { ...res.data },
    // payload: {
    //   // listData: [...actionData],
    //   // countList: count,
    //   // total: totalData,
    //   listData: { ...res.data },
    //   total: totalData,
    // },
  };
};

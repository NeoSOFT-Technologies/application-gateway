import { PolicyListService } from "../../Services/Policies/PolicyService";

export const getPolicyList = async (currentPage) => {
  let res = await PolicyListService(currentPage);
  // let totalData = res.data.Data.Policies.length;
  // let start = (currentPage - 1) * 5;
  // let count = Math.ceil(res.data.Data.Policies.length / 5);
  // let actionData = res.data.Data.Policies.splice(start, 5);
  return {
    type: "getPolicies",
    payload: { ...res.data },
    // payload: {
    //   listData: [...actionData],
    //   countList: count,
    //   total: totalData,
    // },
  };
};

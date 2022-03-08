import { PolicyListService } from "../../config/Policies/PolicyService";

export const getPolicyList = async () => {
  let res = await PolicyListService();
  console.log(res);
  return {
    type: "getPolicies",
    payload: { ...res.data },
  };
};

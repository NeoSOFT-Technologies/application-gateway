import { PolicyListService } from "../../Services/Policies/PolicyService";

export const getPolicyList = async () => {
  let res = await PolicyListService();
  return {
    type: "getPolicies",
    payload: { ...res.data },
  };
};

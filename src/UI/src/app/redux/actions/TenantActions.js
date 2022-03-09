import { tenantListService } from "../../Services/Myservices";

export const getTenantList = async (currentPage, search) => {
  let res = await tenantListService(currentPage, search);
  // console.log(res);
  return {
    type: "getTenants",
    payload: { ...res.data },
  };
};

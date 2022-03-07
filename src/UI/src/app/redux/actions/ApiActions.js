import { ApiListService } from "../../config/APIs/APIService";

export const getAPIList = async () => {
  let res = await ApiListService();
  console.log(res);
  return {
    type: "getAPIs",
    payload: { ...res.data },
  };
};

import { ApiListService } from "../../Services/APIs/APIService";

export const getAPIList = async () => {
  let res = await ApiListService();
  return {
    type: "getAPIs",
    payload: { ...res.data },
  };
};

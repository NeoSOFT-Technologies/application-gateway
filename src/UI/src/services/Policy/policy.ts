// import { IApiData } from "../../types/api/index";
import apiFactory from "../../utils/api";

export function policyListService(currentPage: number) {
  return apiFactory().get(`Policy?pageNum=${currentPage}&pageSize=3`);
}

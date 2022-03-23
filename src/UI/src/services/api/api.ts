// import { IApiData } from "../../types/api/index";
import { IApiData } from "../../types/api";
import apiFactory from "../../utils/api";

export function apiListService(currentPage: number) {
  return apiFactory().get(
    `ApplicationGateway?pageNum=${currentPage}&pageSize=3`
  );
}
export function addApiDataService(data: IApiData) {
  return apiFactory().post(`ApplicationGateway/CreateApi`, data);
}

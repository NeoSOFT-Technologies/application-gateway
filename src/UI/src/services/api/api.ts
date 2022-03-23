// import { IApiData } from "../../types/api/index";
import { IApiFormData } from "../../types/api";
import apiFactory from "../../utils/api";

export function apiListService(currentPage: number) {
  return apiFactory().get(
    `ApplicationGateway?pageNum=${currentPage}&pageSize=3`
  );
}
export function addApiDataService(data: IApiFormData) {
  return apiFactory().post(`ApplicationGateway/CreateApi`, data);
}

export function deleteApiDataService(Id: string) {
  return apiFactory().delete(`ApplicationGateway/` + Id);
}

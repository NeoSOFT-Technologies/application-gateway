// import { IApiData } from "../../types/api/index";
import { IApiFormData, IApiUpdateForm } from "../../types/api";
import apiFactory from "../../utils/api";

export function apiListService(currentPage: number) {
  return apiFactory().get(
    `ApplicationGateway?pageNum=${currentPage}&pageSize=3`
  );
}
export function addApiDataService(data: IApiFormData) {
  return apiFactory().post(`ApplicationGateway/CreateApi`, data);
}

export function getApiByIdService(Id: string) {
  return apiFactory().get(`ApplicationGateway/` + Id);
}

export function updateApiService(data: IApiUpdateForm) {
  return apiFactory().put(`ApplicationGateway`, data);
}

export function deleteApiDataService(Id: string) {
  return apiFactory().delete(`ApplicationGateway/` + Id);
}

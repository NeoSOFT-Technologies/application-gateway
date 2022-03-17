// import { IApiData } from "../../types/api/index";
import apiFactory from "../../utils/api";

export function apiListService(currentPage: number) {
  return apiFactory().get(
    `ApplicationGateway?pageNum=${currentPage}&pageSize=3`
  );
}

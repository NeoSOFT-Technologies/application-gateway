import apiFactory from "../../utils/api";

export function keyListService(currentPage: number) {
  return apiFactory().get(`Key/GetAllKeys?pageNum=${currentPage}&pageSize=3`);
}

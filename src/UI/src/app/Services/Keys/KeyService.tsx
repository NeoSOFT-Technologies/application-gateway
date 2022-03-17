import axios from "axios";
import { APIURL } from "../../config/URL";

export function KeyListService(currentPage) {
  return axios.get(
    `${APIURL}/Key/GetAllKeys?pageNum=${currentPage}&pageSize=3`
  );
}

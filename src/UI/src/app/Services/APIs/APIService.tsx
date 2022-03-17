import axios from "axios";
import { APIURL } from "../../config/URL";

export function ApiListService(currentPage) {
  return axios.get(
    `${APIURL}/ApplicationGateway?pageNum=${currentPage}&pageSize=3`
  );
}

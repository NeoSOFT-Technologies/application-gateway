import axios from "axios";
import { APIURL } from "../../config/URL";

export function PolicyListService(currentPage) {
  return axios.get(`${APIURL}/Policy?pageNum=${currentPage}&pageSize=3`);
  //.catch((error) => {console.log("error:", error)});
}

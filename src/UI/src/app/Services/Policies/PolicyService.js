import axios from "axios";
import { APIURL } from "../../config/URL";

export function PolicyListService() {
  return axios.get(`${APIURL}/Policy`);
  //.catch((error) => {console.log("error:", error)});
}

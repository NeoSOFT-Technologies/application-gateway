import axios from "axios";
import { APIURL } from "../../config/URL";

export function ApiListService() {
  return axios.get(`${APIURL}/ApplicationGateway`);
}

import axios from "axios";
import { APIURL } from "../../config/URL";

export function KeyListService() {
  return axios.get(`${APIURL}/Key/GetAllKeys`);
}

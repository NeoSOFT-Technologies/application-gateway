import axios from "axios";
import { URL1 } from "../URL";

export function KeyListService() {
  return axios.get(`${URL1}/Key/GetAllKeys`);
}

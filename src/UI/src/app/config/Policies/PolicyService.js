//import { URL } from "./URL";
import axios from "axios";
import { URL1 } from "../URL";

export function PolicyListService() {
  return axios.get(`${URL1}/Policy`);
}

import axios from "axios";

export function getUserData(email, password) {
  const data = { email, password };
  return axios.post(`http://localhost:3000/api/login`, data);
}

export function deleteTenantData(id) {
  return axios.delete(`http://localhost:3000/api/user/${id}`);
}

export function updateTenantData(id, data) {
  return axios.put(`http://localhost:3000/api/user/${id}`, data);
}

export function addTenantData(data) {
  return axios.post(`http://localhost:3000/api/user`, data);
}

export function tenantListService(currentPage, search) {
  return axios.get(
    `http://localhost:3000/api/user?type=tenant&_page=${currentPage}&name_like=${search}`
  );
}

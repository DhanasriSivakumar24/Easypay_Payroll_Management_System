import axios from "../interceptors/AuthInterceptor";
import {baseUrl} from '../enviroment.dev';

export function SearchEmployees(searchData){
    const url = baseUrl +'Employee/search';// for accessing the authorized url we need to provide the token.. while logining we store it in thesession object.. now we will go and pick it from there //to do that we need to create a interceptor
    return axios.post(url,searchData);
}

export function GetPersonalInfo(employeeId) {
    const url = baseUrl + `Employee/${employeeId}`;
    return axios.get(url);
}

export function UpdatePersonalInfo(employeeId, updatedData) {
    const url = baseUrl + `Employee/${employeeId}`;
    return axios.put(url, updatedData);
}

export function GetAllEmployees() {
  const url = baseUrl +'Employee/all'; 
  return axios.get(url);
}

export function GetEmployeeById(employeeId) {
  const url = baseUrl + `Employee/${employeeId}`;
  return axios.get(url);
}

export function AddEmployeeAPI(employeeData) {
  const url = baseUrl + "Employee/add";
  return axios.post(url, employeeData);
}

export function UpdateEmployee(employeeId, updatedData) {
  const url = baseUrl + `Employee/update/${employeeId}`;
  return axios.put(url, updatedData);
}

export function DeleteEmployee(employeeId) {
  const url = baseUrl + `Employee/delete/${employeeId}`;
  return axios.delete(url);
}

export function ChangeUserRoleAPI(changeRoleDTO) {
  const url = baseUrl + "Employee/change-userrole";
  return axios.put(url, changeRoleDTO);
}
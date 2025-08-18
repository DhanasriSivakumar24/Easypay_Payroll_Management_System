import axios from "../interceptors/AuthInterceptor";
import {baseUrl} from '../enviroment.dev';

export function SearchEmployees(searchData){
    const url = baseUrl +'Employee/search';// for accessing the authorized url we need to provide the token.. while logining we store it in thesession object.. now we will go and pick it from there //to do that we need to create a interceptor
    return axios.post(url,searchData);
}

// Get logged-in employee personal info
export function GetPersonalInfo(employeeId) {
    const url = baseUrl + `Employee/${employeeId}`;
    return axios.get(url);
}

// Update logged-in employee personal info
export function UpdatePersonalInfo(employeeId, updatedData) {
    const url = baseUrl + `Employee/${employeeId}`;
    return axios.put(url, updatedData);
}

export function GetAllEmployees() {
  const url = baseUrl +'Employee/all'; 
  return axios.get(url);
}
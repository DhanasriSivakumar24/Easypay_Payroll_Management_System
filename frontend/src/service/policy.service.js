import axios from "../interceptors/AuthInterceptor";
import {baseUrl} from '../enviroment.dev';

export function GetAllPolicies() {
  const url = baseUrl +'PayrollPolicy/all'; 
  return axios.get(url);
}
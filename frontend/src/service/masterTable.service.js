import axios from "axios";
import {baseUrl} from '../enviroment.dev';

export function GetDepartments() {
  const url = baseUrl +'MasterData/departments'; 
  return axios.get(url);
}

export function GetRoles() {
  const url = baseUrl +'MasterData/roles'; 
  return axios.get(url);
}

export function GetLeaveType() {
  const url = baseUrl +'MasterData/leave-type'; 
  return axios.get(url);
}
import axios from "../interceptors/AuthInterceptor";
import {baseUrl} from '../environment.dev';

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

export function GetTimesheetType() {
  const url = baseUrl +'MasterData/timesheet-type'; 
  return axios.get(url);
}

export function GetNotificationChannel() {
  const url = baseUrl +'MasterData/notification-channel'; 
  return axios.get(url);
}

export function GetNotificationStatus() {
  const url = baseUrl +'MasterData/notification-status'; 
  return axios.get(url);
}

export function GetUserRoles() {
  const url = baseUrl +'MasterData/user-roles'; 
  return axios.get(url);
}

export function GetBenefitsMaster() {
  const url = baseUrl +'MasterData/benefit-master'; 
  return axios.get(url);
}

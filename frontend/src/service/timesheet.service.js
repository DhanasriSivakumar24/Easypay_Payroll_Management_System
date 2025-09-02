import axios from "../interceptors/AuthInterceptor";
import { baseUrl } from "../environment.dev";

export function AddTimesheet(timesheetData) {
  const url = baseUrl + "Timesheet";
  return axios.post(url, timesheetData);
}

export function GetTimesheetsByEmployee(employeeId) {
  const url = baseUrl + `Timesheet/employee/${employeeId}`;
  return axios.get(url);
}

export function ApproveTimesheetAPI(timesheetId) {
  const url = baseUrl + `Timesheet/${timesheetId}/approve`;
  return axios.put(url);
}

export function RejectTimesheet(timesheetId) {
  const url = baseUrl + `Timesheet/${timesheetId}/reject`;
  return axios.put(url);
}

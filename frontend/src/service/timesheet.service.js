import axios from "../interceptors/AuthInterceptor";
import { baseUrl } from "../enviroment.dev";

export function AddTimesheet(timesheetData) {
  const url = baseUrl + "Timesheet";
  return axios.post(url, timesheetData);
}

export function GetTimesheetsByEmployee(employeeId) {
  const url = baseUrl + `Timesheet/employee/${employeeId}`;
  return axios.get(url);
}

export function ApproveTimesheet(timesheetId) {
  const url = baseUrl + `Timesheet/${timesheetId}/approve`;
  return axios.put(url);
}

export function RejectTimesheet(timesheetId) {
  const url = baseUrl + `Timesheet/${timesheetId}/reject`;
  return axios.put(url);
}

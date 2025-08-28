import axios from "../interceptors/AuthInterceptor";
import { baseUrl } from "../enviroment.dev";

// 🔹 Add a new timesheet (Employee)
export function AddTimesheet(timesheetData) {
  const url = baseUrl + "Timesheet";
  return axios.post(url, timesheetData);
}

// 🔹 Get timesheets by employee
export function GetTimesheetsByEmployee(employeeId) {
  const url = baseUrl + `Timesheet/employee/${employeeId}`;
  return axios.get(url);
}

// 🔹 Approve timesheet (HR / Admin)
export function ApproveTimesheet(timesheetId) {
  const url = baseUrl + `Timesheet/${timesheetId}/approve`;
  return axios.put(url);
}

// 🔹 Reject timesheet (HR / Admin)
export function RejectTimesheet(timesheetId) {
  const url = baseUrl + `Timesheet/${timesheetId}/reject`;
  return axios.put(url);
}

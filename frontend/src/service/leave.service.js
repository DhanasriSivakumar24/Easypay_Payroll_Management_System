import axios from "../interceptors/AuthInterceptor";
import { baseUrl } from '../enviroment.dev';

export function GetAllLeaveRequests() {
    const url = baseUrl + 'LeaveRequest/all';
    return axios.get(url);
}

export function SubmitLeaveRequest(leaveData) {
  const url = baseUrl + "LeaveRequest/submit";
  return axios.post(url, leaveData);
}

export function GetLeaveRequestById(leaveId) {
  const url = baseUrl + `LeaveRequest/${leaveId}`;
  return axios.get(url);
}

export function DeleteLeaveRequest(leaveId) {
  const url = baseUrl + `LeaveRequest/delete/${leaveId}`;
  return axios.delete(url);
}

//ManagerId-query param
export function ApproveLeaveRequest(leaveId, managerId) {
  const url = baseUrl + `LeaveRequest/approve/${leaveId}?managerId=${managerId}`;
  return axios.put(url);
}

export function RejectLeaveRequest(leaveId, managerId) {
  const url = baseUrl + `LeaveRequest/reject/${leaveId}?managerId=${managerId}`;
  return axios.put(url);
}

export function GetLeaveRequestsByEmployee(employeeId) {
  const url = baseUrl + `LeaveRequest/Employee/${employeeId}`;
  return axios.get(url);
}

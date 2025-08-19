import axios from "../interceptors/AuthInterceptor";
import { baseUrl } from '../enviroment.dev';

// Get all leave requests
export function GetAllLeaveRequests() {
    const url = baseUrl + 'LeaveRequest/all';
    return axios.get(url);
}

import axios from "../interceptors/AuthInterceptor";
import { baseUrl } from '../enviroment.dev';

// Get all payrolls
export function GetAllPayrolls() {
    const url = baseUrl + 'Payroll/all';
    return axios.get(url);
}

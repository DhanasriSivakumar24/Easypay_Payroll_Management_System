import axios from "../interceptors/AuthInterceptor";
import { baseUrl } from "../enviroment.dev";

export function GeneratePayroll(payload) {
  const url = baseUrl + "Payroll/generate";
  return axios.post(url, payload);
}

export function GetAllPayrolls() {
  const url = baseUrl + "Payroll/all";
  return axios.get(url);
}

export function GetPayrollByEmployeeId(empId) {
  const url = baseUrl + `Payroll/employee/${empId}`;
  return axios.get(url);
}

export function VerifyPayroll(payrollId) {
  const url = baseUrl + `Payroll/verify/${payrollId}`;
  return axios.put(url);
}

export function ApprovePayroll(payrollId) {
  const url = baseUrl + `Payroll/approve/${payrollId}`;
  return axios.put(url);
}

export function MarkPayrollAsPaid(payrollId, adminId) {
  const url = baseUrl + `Payroll/mark-paid/${payrollId}/${adminId}`;
  return axios.put(url);
}

export function GetApprovedPayrolls(start, end) {
  const url = baseUrl + `Payroll/get-approved-payroll?start=${start}&end=${end}`;
  return axios.get(url);
}

export function GetComplianceReport(start, end) {
  const url = baseUrl + `Payroll/compliance-report?start=${start}&end=${end}`;
  return axios.get(url);
}

export function GetPayrollById(payrollId) {
  const url = baseUrl + `Payroll/${payrollId}`;
  return axios.get(url);
}

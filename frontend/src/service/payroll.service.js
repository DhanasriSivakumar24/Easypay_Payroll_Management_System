import axios from "../interceptors/AuthInterceptor";
import { baseUrl } from '../enviroment.dev';

// 1. Generate Payroll (Admin, HR Manager)
export function GeneratePayroll(dto) {
  const url = `${baseUrl}Payroll/generate`;
  return axios.post(url, dto);
}

// 2. Get All Payrolls (Admin, HR Manager, Payroll Processor)
export function GetAllPayrolls() {
    const url = baseUrl + 'Payroll/all';
    return axios.get(url);
}

// 3. Get Payrolls by Employee Id (Employee only)
export function GetPayrollByEmployeeId(empId) {
  const url = `${baseUrl}Payroll/employee/${empId}`;
  return axios.get(url);
}

// 4. Verify Payroll (Payroll Processor)
export function VerifyPayroll(payrollId) {
  const url = `${baseUrl}Payroll/verify/${payrollId}`;
  return axios.put(url);
}

// 5. Approve Payroll (HR Manager)
export function ApprovePayroll(payrollId) {
  const url = `${baseUrl}Payroll/approve/${payrollId}`;
  return axios.put(url);
}

// 6. Mark Payroll as Paid (Admin)
export function MarkAsPaid(payrollId, adminId) {
  const url = `${baseUrl}Payroll/mark-paid/${payrollId}/${adminId}`;
  return axios.put(url);
}

// 7. Get Approved Payrolls in Date Range (Admin, HR Manager)
export function GetApprovedPayrolls(start, end) {
  const url = `${baseUrl}Payroll/get-approved-payroll?start=${start}&end=${end}`;
  return axios.get(url);
}

// 8. Compliance Report (Admin, HR Manager)
export function GetComplianceReport(start, end) {
  const url = `${baseUrl}Payroll/compliance-report?start=${start}&end=${end}`;
  return axios.get(url);
}
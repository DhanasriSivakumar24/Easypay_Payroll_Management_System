import axios from '../../interceptors/AuthInterceptor';
import {
  GeneratePayroll,
  GetAllPayrolls,
  GetPayrollByEmployeeId,
  VerifyPayroll,
  ApprovePayroll,
  MarkPayrollAsPaid,
  GetApprovedPayrolls,
  GetComplianceReport,
  GetPayrollById,
} from '../payroll.service.js';
import { baseUrl } from '../../environment.dev.js';

jest.mock('../../interceptors/AuthInterceptor', () => ({
  post: jest.fn(),
  get: jest.fn(),
  put: jest.fn(),
}));

describe('Payroll Service', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test('GeneratePayroll should call axios.post with correct URL and payload', async () => {
    const payload = { employeeId: 1, month: 'August', year: 2025 };
    const mockResponse = { data: { success: true } };
    axios.post.mockResolvedValue(mockResponse);

    const result = await GeneratePayroll(payload);

    expect(axios.post).toHaveBeenCalledWith(`${baseUrl}Payroll/generate`, payload);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetAllPayrolls should call axios.get with correct URL', async () => {
    const mockResponse = { data: [{ id: 1, employeeId: 1 }] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetAllPayrolls();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}Payroll/all`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetPayrollByEmployeeId should call axios.get with correct URL', async () => {
    const empId = 2;
    const mockResponse = { data: [{ id: 1, employeeId: empId }] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetPayrollByEmployeeId(empId);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}Payroll/employee/${empId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('VerifyPayroll should call axios.put with correct URL', async () => {
    const payrollId = 3;
    const mockResponse = { data: { verified: true } };
    axios.put.mockResolvedValue(mockResponse);

    const result = await VerifyPayroll(payrollId);

    expect(axios.put).toHaveBeenCalledWith(`${baseUrl}Payroll/verify/${payrollId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('ApprovePayroll should call axios.put with correct URL', async () => {
    const payrollId = 4;
    const mockResponse = { data: { approved: true } };
    axios.put.mockResolvedValue(mockResponse);

    const result = await ApprovePayroll(payrollId);

    expect(axios.put).toHaveBeenCalledWith(`${baseUrl}Payroll/approve/${payrollId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('MarkPayrollAsPaid should call axios.put with correct URL', async () => {
    const payrollId = 5;
    const adminId = 1;
    const mockResponse = { data: { paid: true } };
    axios.put.mockResolvedValue(mockResponse);

    const result = await MarkPayrollAsPaid(payrollId, adminId);

    expect(axios.put).toHaveBeenCalledWith(`${baseUrl}Payroll/mark-paid/${payrollId}/${adminId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetApprovedPayrolls should call axios.get with correct URL', async () => {
    const start = '2025-08-01';
    const end = '2025-08-31';
    const mockResponse = { data: [{ id: 1 }] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetApprovedPayrolls(start, end);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}Payroll/get-approved-payroll?start=${start}&end=${end}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetComplianceReport should call axios.get with correct URL', async () => {
    const start = '2025-08-01';
    const end = '2025-08-31';
    const mockResponse = { data: [{ employeeId: 1, compliance: true }] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetComplianceReport(start, end);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}Payroll/compliance-report?start=${start}&end=${end}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetPayrollById should call axios.get with correct URL', async () => {
    const payrollId = 6;
    const mockResponse = { data: { id: payrollId, employeeId: 2 } };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetPayrollById(payrollId);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}Payroll/${payrollId}`);
    expect(result.data).toEqual(mockResponse.data);
  });
});

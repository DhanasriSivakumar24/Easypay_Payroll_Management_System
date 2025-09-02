import axios from '../../interceptors/AuthInterceptor';
import {
  AddTimesheet,
  GetTimesheetsByEmployee,
  ApproveTimesheetAPI,
  RejectTimesheet,
} from '../timesheet.service.js';
import { baseUrl } from '../../environment.dev.js';

jest.mock('../../interceptors/AuthInterceptor', () => ({
  post: jest.fn(),
  get: jest.fn(),
  put: jest.fn(),
}));

describe('Timesheet Service', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test('AddTimesheet should call axios.post with correct URL and payload', async () => {
    const timesheetData = { employeeId: 1, hours: 8, date: '2025-09-02' };
    const mockResponse = { data: { success: true } };
    axios.post.mockResolvedValue(mockResponse);

    const result = await AddTimesheet(timesheetData);

    expect(axios.post).toHaveBeenCalledWith(`${baseUrl}Timesheet`, timesheetData);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetTimesheetsByEmployee should call axios.get with correct URL', async () => {
    const employeeId = 5;
    const mockResponse = { data: [{ id: 1, hours: 8 }] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetTimesheetsByEmployee(employeeId);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}Timesheet/employee/${employeeId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('ApproveTimesheetAPI should call axios.put with correct URL', async () => {
    const timesheetId = 3;
    const mockResponse = { data: { approved: true } };
    axios.put.mockResolvedValue(mockResponse);

    const result = await ApproveTimesheetAPI(timesheetId);

    expect(axios.put).toHaveBeenCalledWith(`${baseUrl}Timesheet/${timesheetId}/approve`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('RejectTimesheet should call axios.put with correct URL', async () => {
    const timesheetId = 4;
    const mockResponse = { data: { rejected: true } };
    axios.put.mockResolvedValue(mockResponse);

    const result = await RejectTimesheet(timesheetId);

    expect(axios.put).toHaveBeenCalledWith(`${baseUrl}Timesheet/${timesheetId}/reject`);
    expect(result.data).toEqual(mockResponse.data);
  });
});

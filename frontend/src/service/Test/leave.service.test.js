import axios from '../../interceptors/AuthInterceptor';
import {
  GetAllLeaveRequests,
  SubmitLeaveRequest,
  GetLeaveRequestById,
  DeleteLeaveRequest,
  ApproveLeaveRequest,
  RejectLeaveRequest,
  GetLeaveRequestsByEmployee,
} from '../leave.service.js';
import { baseUrl } from '../../environment.dev.js';

jest.mock('../../interceptors/AuthInterceptor', () => ({
  get: jest.fn(),
  post: jest.fn(),
  put: jest.fn(),
  delete: jest.fn(),
}));

describe('Leave Request Service', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test('GetAllLeaveRequests should call axios.get with correct URL', async () => {
    const mockResponse = { data: [{ id: 1, type: 'Sick' }] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetAllLeaveRequests();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}LeaveRequest/all`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('SubmitLeaveRequest should call axios.post with correct URL and leaveData', async () => {
    const mockResponse = { data: { success: true } };
    axios.post.mockResolvedValue(mockResponse);

    const leaveData = { employeeId: 2, type: 'Annual' };
    const result = await SubmitLeaveRequest(leaveData);

    expect(axios.post).toHaveBeenCalledWith(`${baseUrl}LeaveRequest/submit`, leaveData);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetLeaveRequestById should call axios.get with correct leaveId', async () => {
    const mockResponse = { data: { id: 5, type: 'Casual' } };
    axios.get.mockResolvedValue(mockResponse);

    const leaveId = 5;
    const result = await GetLeaveRequestById(leaveId);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}LeaveRequest/${leaveId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('DeleteLeaveRequest should call axios.delete with correct leaveId', async () => {
    const mockResponse = { data: { success: true } };
    axios.delete.mockResolvedValue(mockResponse);

    const leaveId = 10;
    const result = await DeleteLeaveRequest(leaveId);

    expect(axios.delete).toHaveBeenCalledWith(`${baseUrl}LeaveRequest/delete/${leaveId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('ApproveLeaveRequest should call axios.put with correct leaveId and managerId', async () => {
    const mockResponse = { data: { approved: true } };
    axios.put.mockResolvedValue(mockResponse);

    const leaveId = 3;
    const managerId = 100;
    const result = await ApproveLeaveRequest(leaveId, managerId);

    expect(axios.put).toHaveBeenCalledWith(`${baseUrl}LeaveRequest/approve/${leaveId}?managerId=${managerId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('RejectLeaveRequest should call axios.put with correct leaveId and managerId', async () => {
    const mockResponse = { data: { rejected: true } };
    axios.put.mockResolvedValue(mockResponse);

    const leaveId = 4;
    const managerId = 200;
    const result = await RejectLeaveRequest(leaveId, managerId);

    expect(axios.put).toHaveBeenCalledWith(`${baseUrl}LeaveRequest/reject/${leaveId}?managerId=${managerId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetLeaveRequestsByEmployee should call axios.get with correct employeeId', async () => {
    const mockResponse = { data: [{ id: 8, type: 'Sick' }] };
    axios.get.mockResolvedValue(mockResponse);

    const employeeId = 8;
    const result = await GetLeaveRequestsByEmployee(employeeId);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}LeaveRequest/Employee/${employeeId}`);
    expect(result.data).toEqual(mockResponse.data);
  });
});
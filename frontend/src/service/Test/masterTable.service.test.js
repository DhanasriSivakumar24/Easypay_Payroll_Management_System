import axios from '../../interceptors/AuthInterceptor';
import {
  GetDepartments,
  GetRoles,
  GetLeaveType,
  GetTimesheetType,
  GetNotificationChannel,
  GetNotificationStatus,
  GetUserRoles,
  GetBenefitsMaster,
} from '../masterTable.service.js';
import { baseUrl } from '../../environment.dev.js';

jest.mock('../../interceptors/AuthInterceptor', () => ({
  get: jest.fn(),
}));

describe('Master Data Service', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test('GetDepartments should call axios.get with correct URL', async () => {
    const mockResponse = { data: ['HR', 'Finance'] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetDepartments();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}MasterData/departments`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetRoles should call axios.get with correct URL', async () => {
    const mockResponse = { data: ['Admin', 'Employee'] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetRoles();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}MasterData/roles`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetLeaveType should call axios.get with correct URL', async () => {
    const mockResponse = { data: ['Sick', 'Casual'] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetLeaveType();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}MasterData/leave-type`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetTimesheetType should call axios.get with correct URL', async () => {
    const mockResponse = { data: ['Billable', 'Non-Billable'] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetTimesheetType();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}MasterData/timesheet-type`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetNotificationChannel should call axios.get with correct URL', async () => {
    const mockResponse = { data: ['Email', 'SMS'] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetNotificationChannel();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}MasterData/notification-channel`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetNotificationStatus should call axios.get with correct URL', async () => {
    const mockResponse = { data: ['Sent', 'Pending'] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetNotificationStatus();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}MasterData/notification-status`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetUserRoles should call axios.get with correct URL', async () => {
    const mockResponse = { data: ['Admin', 'Manager', 'Employee'] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetUserRoles();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}MasterData/user-roles`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetBenefitsMaster should call axios.get with correct URL', async () => {
    const mockResponse = { data: ['Health', 'Retirement'] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetBenefitsMaster();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}MasterData/benefit-master`);
    expect(result.data).toEqual(mockResponse.data);
  });
});

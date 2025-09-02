import axios from '../../interceptors/AuthInterceptor';
import {
  SearchEmployees,
  GetPersonalInfo,
  UpdatePersonalInfo,
  GetAllEmployees,
  GetEmployeeById,
  AddEmployeeAPI,
  UpdateEmployee,
  DeleteEmployee,
  ChangeUserRoleAPI,
} from '../employee.service.js';
import { baseUrl } from '../../environment.dev.js';

jest.mock('../../interceptors/AuthInterceptor', () => ({
  get: jest.fn(),
  post: jest.fn(),
  put: jest.fn(),
  delete: jest.fn(),
}));

describe('Employee Service', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test('SearchEmployees should call axios.post with correct URL and data', async () => {
    const mockResponse = { data: [{ id: 1, name: 'Alice' }] };
    axios.post.mockResolvedValue(mockResponse);

    const searchData = { name: 'Alice' };
    const result = await SearchEmployees(searchData);

    expect(axios.post).toHaveBeenCalledWith(`${baseUrl}Employee/search`, searchData);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetPersonalInfo should call axios.get with correct employeeId', async () => {
    const mockResponse = { data: { id: 5, name: 'John' } };
    axios.get.mockResolvedValue(mockResponse);

    const employeeId = 5;
    const result = await GetPersonalInfo(employeeId);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}Employee/${employeeId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('UpdatePersonalInfo should call axios.put with correct URL and updatedData', async () => {
    const mockResponse = { data: { success: true } };
    axios.put.mockResolvedValue(mockResponse);

    const employeeId = 3;
    const updatedData = { name: 'Updated Name' };
    const result = await UpdatePersonalInfo(employeeId, updatedData);

    expect(axios.put).toHaveBeenCalledWith(`${baseUrl}Employee/${employeeId}`, updatedData);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetAllEmployees should call axios.get with correct URL', async () => {
    const mockResponse = { data: [{ id: 1, name: 'Alice' }] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetAllEmployees();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}Employee/all`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetEmployeeById should call axios.get with correct URL', async () => {
    const mockResponse = { data: { id: 7, name: 'Emma' } };
    axios.get.mockResolvedValue(mockResponse);

    const employeeId = 7;
    const result = await GetEmployeeById(employeeId);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}Employee/${employeeId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('AddEmployeeAPI should call axios.post with correct URL and employeeData', async () => {
    const mockResponse = { data: { success: true } };
    axios.post.mockResolvedValue(mockResponse);

    const employeeData = { name: 'New Employee' };
    const result = await AddEmployeeAPI(employeeData);

    expect(axios.post).toHaveBeenCalledWith(`${baseUrl}Employee/add`, employeeData);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('UpdateEmployee should call axios.put with correct URL and updatedData', async () => {
    const mockResponse = { data: { success: true } };
    axios.put.mockResolvedValue(mockResponse);

    const employeeId = 4;
    const updatedData = { name: 'Updated Employee' };
    const result = await UpdateEmployee(employeeId, updatedData);

    expect(axios.put).toHaveBeenCalledWith(`${baseUrl}Employee/update/${employeeId}`, updatedData);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('DeleteEmployee should call axios.delete with correct URL and employeeId', async () => {
    const mockResponse = { data: { success: true } };
    axios.delete.mockResolvedValue(mockResponse);

    const employeeId = 10;
    const result = await DeleteEmployee(employeeId);

    expect(axios.delete).toHaveBeenCalledWith(`${baseUrl}Employee/delete/${employeeId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('ChangeUserRoleAPI should call axios.put with correct URL and changeRoleDTO', async () => {
    const mockResponse = { data: { success: true } };
    axios.put.mockResolvedValue(mockResponse);

    const changeRoleDTO = { userId: 1, role: 'Admin' };
    const result = await ChangeUserRoleAPI(changeRoleDTO);

    expect(axios.put).toHaveBeenCalledWith(`${baseUrl}Employee/change-userrole`, changeRoleDTO);
    expect(result.data).toEqual(mockResponse.data);
  });
});
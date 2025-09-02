import axios from '../../interceptors/AuthInterceptor.js';
import {
  GetAllAuditTrail,
  GetAuditTrailByUser,
  GetAuditTrailByAction,
  GetAuditTrailById,
} from '../audit.service.js';
import { baseUrl } from '../../environment.dev.js';

jest.mock('../../interceptors/AuthInterceptor', () => ({
  get: jest.fn(),
  post: jest.fn(),
  put: jest.fn(),
  delete: jest.fn(),
}));

describe('AuditTrail Service', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test('GetAllAuditTrail should call axios.get with correct URL', async () => {
    const mockResponse = { data: [{ id: 1, action: 'LOGIN' }] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetAllAuditTrail();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}AuditTrail/all`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetAuditTrailByUser should call axios.get with correct URL and username', async () => {
    const mockResponse = { data: [{ id: 2, user: 'testuser' }] };
    axios.get.mockResolvedValue(mockResponse);

    const username = 'testuser';
    const result = await GetAuditTrailByUser(username);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}AuditTrail/user/${username}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetAuditTrailByAction should call axios.get with correct URL and actionId', async () => {
    const mockResponse = { data: [{ id: 3, action: 'CREATE' }] };
    axios.get.mockResolvedValue(mockResponse);

    const actionId = 5;
    const result = await GetAuditTrailByAction(actionId);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}AuditTrail/action/${actionId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetAuditTrailById should call axios.get with correct URL and actionId', async () => {
    const mockResponse = { data: { id: 10, action: 'DELETE' } };
    axios.get.mockResolvedValue(mockResponse);

    const actionId = 10;
    const result = await GetAuditTrailById(actionId);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}AuditTrail/${actionId}`);
    expect(result.data).toEqual(mockResponse.data);
  });
});

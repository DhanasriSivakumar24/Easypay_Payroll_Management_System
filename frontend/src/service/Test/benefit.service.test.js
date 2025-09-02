import axios from '../../interceptors/AuthInterceptor';
import {
  GetAllBenefits,
  AddBenefitEnrollment,
  DeleteBenefitEnrollment,
  GetBenefitEnrollmentById,
  GetBenefitEnrollmentsByEmployee,
  UpdateBenefitEnrollment,
} from '../benefit.service.js';
import { baseUrl } from '../../environment.dev.js';

jest.mock('../../interceptors/AuthInterceptor', () => ({
  get: jest.fn(),
  post: jest.fn(),
  put: jest.fn(),
  delete: jest.fn(),
}));

describe('BenefitEnrollment Service', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test('GetAllBenefits should call axios.get with correct URL', async () => {
    const mockResponse = { data: [{ id: 1, name: 'Health Insurance' }] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetAllBenefits();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}BenefitEnrollment/all`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('AddBenefitEnrollment should call axios.post with correct URL and payload', async () => {
    const mockResponse = { data: { success: true } };
    axios.post.mockResolvedValue(mockResponse);

    const payload = { employeeId: 1, benefitType: 'Dental' };
    const result = await AddBenefitEnrollment(payload);

    expect(axios.post).toHaveBeenCalledWith(`${baseUrl}BenefitEnrollment/add`, payload);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('DeleteBenefitEnrollment should call axios.delete with correct URL and BenefitId', async () => {
    const mockResponse = { data: { success: true } };
    axios.delete.mockResolvedValue(mockResponse);

    const BenefitId = 5;
    const result = await DeleteBenefitEnrollment(BenefitId);

    expect(axios.delete).toHaveBeenCalledWith(`${baseUrl}BenefitEnrollment/${BenefitId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetBenefitEnrollmentById should call axios.get with correct URL and BenefitId', async () => {
    const mockResponse = { data: { id: 10, name: 'Retirement Plan' } };
    axios.get.mockResolvedValue(mockResponse);

    const BenefitId = 10;
    const result = await GetBenefitEnrollmentById(BenefitId);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}BenefitEnrollment/${BenefitId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetBenefitEnrollmentsByEmployee should call axios.get with correct URL and employeeId', async () => {
    const mockResponse = { data: [{ id: 1, employeeId: 2, benefit: 'Vision' }] };
    axios.get.mockResolvedValue(mockResponse);

    const employeeId = 2;
    const result = await GetBenefitEnrollmentsByEmployee(employeeId);

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}BenefitEnrollment/employee/${employeeId}`);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('UpdateBenefitEnrollment should call axios.put with correct URL, BenefitId, and payload', async () => {
    const mockResponse = { data: { success: true } };
    axios.put.mockResolvedValue(mockResponse);

    const BenefitId = 7;
    const payload = { benefitType: 'Health Insurance Plus' };
    const result = await UpdateBenefitEnrollment(BenefitId, payload);

    expect(axios.put).toHaveBeenCalledWith(`${baseUrl}BenefitEnrollment/update/${BenefitId}`, payload);
    expect(result.data).toEqual(mockResponse.data);
  });
});
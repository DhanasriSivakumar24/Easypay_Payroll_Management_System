import axios from '../../interceptors/AuthInterceptor';
import { GetAllPolicies } from '../policy.service.js';
import { baseUrl } from '../../environment.dev.js';

jest.mock('../../interceptors/AuthInterceptor', () => ({
  get: jest.fn(),
}));

describe('Policy Service', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test('GetAllPolicies should call axios.get with correct URL', async () => {
    const mockResponse = { data: [{ id: 1, name: 'Leave Policy' }] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetAllPolicies();

    expect(axios.get).toHaveBeenCalledWith(`${baseUrl}PayrollPolicy/all`);
    expect(result.data).toEqual(mockResponse.data);
  });
});

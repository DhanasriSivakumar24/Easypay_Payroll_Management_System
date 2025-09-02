import axios from '../../interceptors/AuthInterceptor';
import {
  SendNotificationAPI,
  GetNotificationsByUser,
} from '../notification.service.js';
import { baseUrl } from '../../environment.dev.js';

jest.mock('../../interceptors/AuthInterceptor', () => ({
  post: jest.fn(),
  get: jest.fn(),
}));

describe('Notification Service', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test('SendNotificationAPI should call axios.post with correct URL and payload', async () => {
    const mockNotification = { message: 'Test message', employeeId: 1 };
    const mockResponse = { data: { success: true } };
    axios.post.mockResolvedValue(mockResponse);

    const result = await SendNotificationAPI(mockNotification);

    expect(axios.post).toHaveBeenCalledWith(
      `${baseUrl}NotificationLog/send`,
      mockNotification
    );
    expect(result.data).toEqual(mockResponse.data);
  });

  test('GetNotificationsByUser should call axios.get with correct URL', async () => {
    const employeeId = 5;
    const mockResponse = { data: [{ id: 1, message: 'Test' }] };
    axios.get.mockResolvedValue(mockResponse);

    const result = await GetNotificationsByUser(employeeId);

    expect(axios.get).toHaveBeenCalledWith(
      `${baseUrl}NotificationLog/user/${employeeId}`
    );
    expect(result.data).toEqual(mockResponse.data);
  });
});

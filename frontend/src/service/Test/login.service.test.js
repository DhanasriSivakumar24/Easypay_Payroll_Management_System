import axios from "axios";
import { loginAPICall, RegisterUser } from "../login.service.js";
import { baseUrl } from "../../environment.dev.js";

jest.mock("axios", () => ({
  post: jest.fn(),
}));

describe('Login Service', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  test('loginAPICall should call axios.post with correct URL and data', async () => {
    const mockResponse = { data: { token: 'abc123' } };
    axios.post.mockResolvedValue(mockResponse);

    const loginData = { username: 'test', password: '1234' };
    const result = await loginAPICall(loginData);

    expect(axios.post).toHaveBeenCalledWith(`${baseUrl}Authentication/Login`, loginData);
    expect(result.data).toEqual(mockResponse.data);
  });

  test('RegisterUser should call axios.post with correct URL and data', async () => {
    const mockResponse = { data: { success: true } };
    axios.post.mockResolvedValue(mockResponse);

    const registerData = { username: 'newuser', password: 'pass' };
    const result = await RegisterUser(registerData);

    expect(axios.post).toHaveBeenCalledWith(`${baseUrl}Authentication/Register`, registerData);
    expect(result.data).toEqual(mockResponse.data);
  });
});

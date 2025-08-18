import axios from "axios";
import {baseUrl} from '../enviroment.dev';
import { LoginModel } from "../models/login.model";

export function loginAPICall(LoginModel)
{
    const url = baseUrl +'Authentication/Login';
    return axios.post(url,LoginModel)
}
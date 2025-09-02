import axios from "axios";
import {baseUrl} from '../environment.dev';
import { LoginModel } from "../models/login.model";

export function loginAPICall(LoginModel)
{
    const url = baseUrl +'Authentication/Login';
    return axios.post(url,LoginModel)
}

export function RegisterUser(register){
    const url = baseUrl+'Authentication/Register';
    return axios.post(url,register);
}
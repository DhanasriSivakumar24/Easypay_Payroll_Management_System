import axios from "../interceptors/AuthInterceptor";
import {baseUrl} from '../enviroment.dev';

export function SendNotificationAPI(notification){
    const url = baseUrl+'NotificationLog/send';
    return axios.post(url,notification);
}

export function GetNotificationsByUser(employeeId){
    const url =baseUrl+`NotificationLog/user/${employeeId}`;
    return axios.get(url);
}
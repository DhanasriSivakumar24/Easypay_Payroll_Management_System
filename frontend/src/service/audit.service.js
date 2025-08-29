import axios from "../interceptors/AuthInterceptor";
import {baseUrl} from '../enviroment.dev';

export function GetAllAuditTrail(){
    const url = baseUrl + 'AuditTrail/all';
    return axios.get(url);
}

export function GetAuditTrailByUser(username){
    const url = baseUrl + `AuditTrail/user/${username}`;
    return axios.get(url);
}

export function GetAuditTrailByAction(actionId){
    const url = baseUrl + `AuditTrail/action/${actionId}`;
    return axios.get(url);
}

export function GetAuditTrailById(actionId){
    const url = baseUrl + `AuditTrail/${actionId}`;
    return axios.get(url);
}
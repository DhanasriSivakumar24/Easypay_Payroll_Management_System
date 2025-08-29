import axios from "../interceptors/AuthInterceptor";
import { baseUrl } from "../enviroment.dev";

export function GetAllBenefits() {
    const url = baseUrl + "BenefitEnrollment/all"
    return axios.get(url);
}

export function AddBenefitEnrollment(payload) {
    const url = baseUrl + "BenefitEnrollment/add";
    return axios.post(url, payload);
}

export function DeleteBenefitEnrollment(BenefitId) {
  const url = baseUrl + `BenefitEnrollment/${BenefitId}`;
  return axios.delete(url);
}

export function GetBenefitEnrollmentById(BenefitId) {
  const url = baseUrl + `BenefitEnrollment/${BenefitId}`;
  return axios.get(url);
}
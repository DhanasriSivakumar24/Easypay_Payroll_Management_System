import axios from "axios";

const requestInterceptor = (config)=>{
    const token = sessionStorage.getItem("token");
    if(token){
        config.headers["Authorization"] =`Bearer ${token}`;
        return config;
    }
    return config;
}

const responseInterceptor = (response)=>{
    return response;
}

axios.interceptors.request.use(requestInterceptor,error=>Promise.reject(error));
axios.interceptors.response.use(responseInterceptor);

export default axios;
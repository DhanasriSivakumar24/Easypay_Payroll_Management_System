// using axios to create a service
//axios have methods to put, get, delete and post 
// before we get the data and then we convert it into the json format
// but in axios, implictly it will return the data in json format

import axios from "axios";

// it is to handle the api calls and businness logics
export function getProducts(){
    return axios.get('https://dummyjson.com/products')
}
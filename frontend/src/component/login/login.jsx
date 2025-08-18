'use client';

import { useState } from "react";
import './login.css'
import {loginAPICall} from '../../service/login.service';
import {LoginModel} from '../../models/login.model';
import {LoginErrorModel} from '../../models/loginerror.model';
import { useNavigate } from "react-router-dom";

const Login=()=>{
    const [user, setuser] = useState(new LoginModel());
    const [error, setError] = useState(new LoginErrorModel());
    const navigate = useNavigate();

    const changeUser =(eventArgs)=>{
        const fieldName =eventArgs.target.name;
        switch(fieldName){
            case 'username':  
                if(eventArgs.target.value==="")
                    setError(e=>({...error,username:"Username cannot be Empty"}));
                else{
                setuser(u=>({...u,username:eventArgs.target.value}))
                setError(e=>({...error,username:""}));
                break;
                }
            case 'password':
                setuser(u=>({...u,password:eventArgs.target.value}))
                break;
            default:
                break;
        }
    }
    const login = ()=>{
        if(error.username || error.password)
            return;
        loginAPICall(user)
        .then(result =>{
            console.log(result.data)
            sessionStorage.setItem("token",result.data.token);
            sessionStorage.setItem("username",result.data.userName);
            sessionStorage.setItem("employeeId", result.data.employeeId);
            alert("login Success")
            navigate("/employee/AllEmployees");
            //navigate('/employee/SearchEmployee');
            //navigate("/employee/personal-info");

        })
        .catch( err=>{
            console.log(err);
            alert(err.response.data.error);
        })
    }
    const Cancel =()=>{
        
    }
    return(
        <section className="loginDiv">
            <h1>Login</h1>
            <label className="form-control">Username</label>
            <input type="text" name="username" value={user.username} onChange={(e)=>changeUser(e)}className="form-control"/>
            {
                error.username?.length>0 && (<span className="alert alert-danger">{error.username}</span>)
            }
            <label className="form-control">Pasword</label>
            <input type="password" name = "password" value={user.password} onChange={(e)=>changeUser(e)}className="form-control"/>
            <button className="button btn btn-success" onClick={login}>Login</button>
            <button className="button btn btn-danger" onClick={Cancel}>Cancel</button>
        </section>
    );   
}
export default Login;
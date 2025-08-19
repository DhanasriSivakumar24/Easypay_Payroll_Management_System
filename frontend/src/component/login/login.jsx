// 'use client';

// import { useState } from "react";
// import './login.css'
// import {loginAPICall} from '../../service/login.service';
// import {LoginModel} from '../../models/login.model';
// import {LoginErrorModel} from '../../models/loginerror.model';
// import { useNavigate } from "react-router-dom";

// const Login=()=>{
//     const [user, setuser] = useState(new LoginModel());
//     const [error, setError] = useState(new LoginErrorModel());
//     const navigate = useNavigate();

//     const changeUser =(eventArgs)=>{
//         const fieldName =eventArgs.target.name;
//         switch(fieldName){
//             case 'username':  
//                 if(eventArgs.target.value==="")
//                     setError(e=>({...error,username:"Username cannot be Empty"}));
//                 else{
//                 setuser(u=>({...u,username:eventArgs.target.value}))
//                 setError(e=>({...error,username:""}));
//                 break;
//                 }
//             case 'password':
//                 setuser(u=>({...u,password:eventArgs.target.value}))
//                 break;
//             default:
//                 break;
//         }
//     }
//     const login = ()=>{
//         if(error.username || error.password)
//             return;
//         loginAPICall(user)
//         .then(result =>{
//             console.log(result.data)
//             sessionStorage.setItem("token",result.data.token);
//             sessionStorage.setItem("username",result.data.userName);
//             sessionStorage.setItem("employeeId", result.data.employeeId);
//             sessionStorage.setItem("role", result.data.role);
//             alert("login Success")
//             navigate("/home");
//             //navigate("/employee/AllEmployees");
//             //navigate('/employee/SearchEmployee');
//             //navigate("/employee/personal-info");

//         })
//         .catch( err=>{
//             console.log(err);
//             alert(err.response.data.error);
//         })
//     }
//     const Cancel =()=>{
//         setuser(new LoginModel());
//         setError(new LoginErrorModel());
//     }
//     return(
//         <section className="loginDiv">
//             <h1>Login</h1>
//             <label className="form-control">Username</label>
//             <input type="text" name="username" value={user.username} onChange={(e)=>changeUser(e)}className="form-control"/>
//             {
//                 error.username?.length>0 && (<span className="alert alert-danger">{error.username}</span>)
//             }
//             <label className="form-control">Pasword</label>
//             <input type="password" name = "password" value={user.password} onChange={(e)=>changeUser(e)}className="form-control"/>
//             <button className="button btn btn-success" onClick={login}>Login</button>
//             <button className="button btn btn-danger" onClick={Cancel}>Cancel</button>
//         </section>
//     );   
// }
// export default Login;

import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { loginAPICall } from "../../service/login.service";
import { LoginModel } from "../../models/login.model";
import { LoginErrorModel } from "../../models/loginerror.model";
import "./login.css";

const Login = () => {
  const [user, setUser] = useState(new LoginModel());
  const [error, setError] = useState(new LoginErrorModel());
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUser((u) => ({ ...u, [name]: value }));
    setError((err) => ({
      ...err,
      [name]: value ? "" : `${name.charAt(0).toUpperCase() + name.slice(1)} is required`,
    }));
  };

  const handleLogin = () => {
    if (error.username || error.password) return;
    loginAPICall(user)
      .then((res) => {
        const data = res.data;
        sessionStorage.setItem("token", data.token);
        sessionStorage.setItem("username", data.userName);
        sessionStorage.setItem("employeeId", data.employeeId);
        sessionStorage.setItem("role", data.role);
        navigate("/dashboard");
        //navigate("/home");
      })
      .catch((err) => alert(err.response?.data?.error || "Login failed"));
  };

  const handleCancel = () => {
    setUser(new LoginModel());
    setError(new LoginErrorModel());
  };

  return (
    <div className="login-container">
      {/* Left Section (Form) */}
      <div className="login-form-section">
        <div className="login-box">
          <div className="login-logo">Payroll System</div>

          <h1 className="login-title">Welcome Back 👋</h1>
          <p className="login-subtitle">Get into your account to check Updates</p>

          <input
            type="text"
            name="username"
            placeholder="Username"
            value={user.username}
            onChange={handleChange}
            className="login-input"
          />
          {error.username && <div className="login-error">{error.username}</div>}

          <input
            type="password"
            name="password"
            placeholder="Password"
            value={user.password}
            onChange={handleChange}
            className="login-input"
          />
          {error.password && <div className="login-error">{error.password}</div>}

            <div className="login-actions">
                <button className="login-button" onClick={handleLogin}>
                    Login
                </button>
                <button className="cancel-button" onClick={handleCancel}>
                    Cancel
                </button>
            </div>
        </div>
      </div>

      {/* Right Section (Blue Background with Illustration) */}
      <div className="login-illustration-section">
        <img
          src="/login.png"
          alt="Payroll Illustration"
          className="login-illustration"
        />
      </div>
    </div>
  );
};

export default Login;

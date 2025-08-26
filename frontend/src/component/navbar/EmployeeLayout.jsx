// src/component/navbar/EmployeeLayout.jsx
import React from "react";
import { useSelector, useDispatch } from "react-redux";
import { logout } from "../../authSlicer"; // adjust path if different
import Sidebar from "./Sidebar";
import "./Sidebar.css";

const EmployeeLayout = ({ children }) => {
  const dispatch = useDispatch();
  const { username } = useSelector((state) => state.auth); // assuming auth slice stores user info

  const handleLogout = () => {
    dispatch(logout());
    window.location.href = "/login"; // redirect
  };

  return (
    <div className="employee-dashboard">
      <Sidebar
        role="employee"
        userName={username || "Employee"}
        onLogout={handleLogout}
      />
      <main className="main-content">{children}</main>
    </div>
  );
};

export default EmployeeLayout;

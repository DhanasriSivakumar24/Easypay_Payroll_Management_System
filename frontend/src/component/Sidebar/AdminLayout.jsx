import React from "react";
import { useSelector, useDispatch } from "react-redux";
import { logout } from "../../authSlicer"; 
import Sidebar from "./Sidebar";
import "./Sidebar.css";

const AdminLayout = ({ children }) => {
  const dispatch = useDispatch();
  const { username } = useSelector((state) => state.auth);
  const handleLogout = () => {
    dispatch(logout()); 
    window.location.href = "/login";
  };

  return (
    <div className="admin-dashboard">
      <Sidebar role="Admin" userName={username || "Admin"} onLogout={handleLogout} />
      <main className="main-content">{children}</main>
    </div>
  );
};

export default AdminLayout;
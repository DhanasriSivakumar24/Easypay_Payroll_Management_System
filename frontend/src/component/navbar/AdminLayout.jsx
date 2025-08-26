import React from "react";
import { useSelector, useDispatch } from "react-redux";
import { logout } from "../../authSlicer"; // Adjust path to your authSlice
import Sidebar from "./Sidebar";
import "./Sidebar.css";

const AdminLayout = ({ children }) => {
  const dispatch = useDispatch();
  const { username } = useSelector((state) => state.auth); // Get username from Redux

  const handleLogout = () => {
    dispatch(logout()); // Dispatch Redux logout action
    window.location.href = "/login"; // Redirect to login
  };

  return (
    <div className="admin-dashboard">
      <Sidebar role="Admin" userName={username || "Admin"} onLogout={handleLogout} />
      <main className="main-content">{children}</main>
    </div>
  );
};

export default AdminLayout;
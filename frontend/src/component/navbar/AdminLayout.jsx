import React from "react";
import Sidebar from "./Sidebar";
import "./Sidebar.css";

const AdminLayout = ({ children }) => {
  const username = sessionStorage.getItem("username") || "Admin";

  const handleLogout = () => {
    sessionStorage.clear();
    window.location.href = "/login";
  };

  return (
    <div className="admin-dashboard">
      <Sidebar role="Admin" userName={username} onLogout={handleLogout} />
      <main className="main-content">{children}</main>
    </div>
  );
};

export default AdminLayout;

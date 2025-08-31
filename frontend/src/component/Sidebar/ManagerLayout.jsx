import React from "react";
import { useSelector, useDispatch } from "react-redux";
import { logout } from "../../authSlicer"; 
import Sidebar from "./Sidebar";
import "./Sidebar.css";

const ManagerLayout = ({ children }) => {
  const dispatch = useDispatch();
  const { username } = useSelector((state) => state.auth);

  const handleLogout = () => {
    dispatch(logout());
    window.location.href = "/login";
  };

  return (
    <div className="manager-dashboard-layout">
      <Sidebar
        role="manager"
        userName={username || "Manager"}
        onLogout={handleLogout}
      />
      <main className="main-content">{children}</main>
    </div>
  );
};

export default ManagerLayout;

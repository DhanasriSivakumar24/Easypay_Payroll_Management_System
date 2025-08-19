'use client';

import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

const Home = () => {
  const navigate = useNavigate();
  const [role, setRole] = useState("");

  useEffect(() => {
    const userRole = sessionStorage.getItem("role");
    if (!sessionStorage.getItem("token")) {
      // no login → redirect to login
      navigate("/login");
    }
    setRole(userRole);
  }, [navigate]);

  return (
    <div className="container mt-5">
      <h1 className="mb-4">Welcome, {sessionStorage.getItem("username")} 👋</h1>

      {/* Role Based Dashboard */}
      {role === "Admin" && (
        <div className="row">
          <div className="col-md-4">
            <div className="card p-3 shadow">
              <h3>Employees</h3>
              <p>Manage employees and their details.</p>
              <button className="btn btn-primary"
                onClick={() => navigate("/employee/AllEmployees")}>
                Go to Employees
              </button>
            </div>
          </div>

          <div className="col-md-4">
            <div className="card p-3 shadow">
              <h3>Payroll</h3>
              <p>View and manage payroll details.</p>
              <button className="btn btn-primary"
                onClick={() => navigate("/payroll")}>
                Go to Payroll
              </button>
            </div>
          </div>
        </div>
      )}

      {role === "Employee" && (
        <div className="row">
          <div className="col-md-4">
            <div className="card p-3 shadow">
              <h3>My Profile</h3>
              <p>View your personal information.</p>
              <button className="btn btn-primary"
                onClick={() => navigate("/employee/personal-info")}>
                View Profile
              </button>
            </div>
          </div>

          <div className="col-md-4">
            <div className="card p-3 shadow">
              <h3>Leave Requests</h3>
              <p>Apply for and track your leave requests.</p>
              <button className="btn btn-primary"
                onClick={() => navigate("/leave-request")}>
                Leave Request
              </button>
            </div>
          </div>
        </div>
      )}

      {role === "Manager" && (
        <div className="row">
          <div className="col-md-4">
            <div className="card p-3 shadow">
              <h3>Team</h3>
              <p>View and manage your team members.</p>
              <button className="btn btn-primary"
                onClick={() => navigate("/team")}>
                Manage Team
              </button>
            </div>
          </div>

          <div className="col-md-4">
            <div className="card p-3 shadow">
              <h3>Approvals</h3>
              <p>Approve or reject employee requests.</p>
              <button className="btn btn-primary"
                onClick={() => navigate("/approvals")}>
                Go to Approvals
              </button>
            </div>
          </div>
        </div>
      )}

      {!role && (
        <p>No role assigned. Please log in again.</p>
      )}
    </div>
  );
};

export default Home;

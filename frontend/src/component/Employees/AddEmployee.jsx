import { useEffect, useState } from "react";
import AdminLayout from "../navbar/AdminLayout";
import DepartmentRoleDropdown from "./DepartmentRoleDropdown";
import { AddEmployeeAPI, GetAllEmployees } from "../../service/employee.service";
import './AddEmployee.css';

const AddEmployee = () => {
  const [employee, setEmployee] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phoneNumber: "",
    dateOfBirth: "",
    joinDate: "",
    address: "",
    panNumber: "",
    salary: "",
    gender: "",
    departmentId: "",
    roleId: "",
    reportingManagerId: ""
  });

  const [managers, setManagers] = useState([]);

  // Load all employees for Reporting Manager dropdown
  useEffect(() => {
    GetAllEmployees()
      .then(res => setManagers(res.data))
      .catch(err => console.log(err));
  }, []);

  const handleChange = (field, value) => {
    setEmployee({ ...employee, [field]: value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const payload = {
      ...employee,
      dateOfBirth: new Date(employee.dateOfBirth).toISOString(),
      joinDate: new Date(employee.joinDate).toISOString(),
      departmentId: parseInt(employee.departmentId),
      roleId: parseInt(employee.roleId),
      reportingManagerId: employee.reportingManagerId ? parseInt(employee.reportingManagerId) : null,
      salary: parseFloat(employee.salary),
      statusId: 1, // default active
      userRoleId: 3 // Employee
    };

    AddEmployeeAPI(payload)
      .then(() => alert("Employee added successfully"))
      .catch((err) => alert("Failed: " + (err.response?.data || err.message)));
  };

  return (
    <AdminLayout>
      <div className="add-employee-container">
        <div className="add-employee-card">
          <h4>Add New Employee</h4>
          <form onSubmit={handleSubmit}>
            
            {/* Name */}
            <div className="row g-3 mb-3">
              <div className="col-md-6">
                <input
                  type="text"
                  placeholder="First Name"
                  className="form-control"
                  value={employee.firstName}
                  onChange={(e) => handleChange("firstName", e.target.value)}
                  required
                />
              </div>
              <div className="col-md-6">
                <input
                  type="text"
                  placeholder="Last Name"
                  className="form-control"
                  value={employee.lastName}
                  onChange={(e) => handleChange("lastName", e.target.value)}
                  required
                />
              </div>
            </div>

            {/* Email & Phone */}
            <div className="row g-3 mb-3">
              <div className="col-md-6">
                <input
                  type="email"
                  placeholder="Email"
                  className="form-control"
                  value={employee.email}
                  onChange={(e) => handleChange("email", e.target.value)}
                  required
                />
              </div>
              <div className="col-md-6">
                <input
                  type="text"
                  placeholder="Phone Number"
                  className="form-control"
                  value={employee.phoneNumber}
                  onChange={(e) => handleChange("phoneNumber", e.target.value)}
                />
              </div>
            </div>

            {/* DOB & Join Date */}
            <div className="row g-3 mb-3">
              <div className="col-md-6">
                <input
                  type="date"
                  className="form-control"
                  value={employee.dateOfBirth}
                  onChange={(e) => handleChange("dateOfBirth", e.target.value)}
                  required
                />
              </div>
              <div className="col-md-6">
                <input
                  type="date"
                  className="form-control"
                  value={employee.joinDate}
                  onChange={(e) => handleChange("joinDate", e.target.value)}
                  required
                />
              </div>
            </div>

            {/* Address & PAN */}
            <div className="row g-3 mb-3">
              <div className="col-md-6">
                <input
                  type="text"
                  placeholder="Address"
                  className="form-control"
                  value={employee.address}
                  onChange={(e) => handleChange("address", e.target.value)}
                />
              </div>
              <div className="col-md-6">
                <input
                  type="text"
                  placeholder="PAN Number"
                  className="form-control"
                  value={employee.panNumber}
                  onChange={(e) => handleChange("panNumber", e.target.value)}
                />
              </div>
            </div>

            {/* Salary & Gender */}
            <div className="row g-3 mb-3">
              <div className="col-md-6">
                <input
                  type="number"
                  placeholder="Salary"
                  className="form-control"
                  value={employee.salary}
                  onChange={(e) => handleChange("salary", e.target.value)}
                />
              </div>
              <div className="col-md-6">
                <select
                  className="form-select"
                  value={employee.gender}
                  onChange={(e) => handleChange("gender", e.target.value)}
                  required
                >
                  <option value="">Select Gender</option>
                  <option value="Male">Male</option>
                  <option value="Female">Female</option>
                  <option value="Other">Other</option>
                </select>
              </div>
            </div>

            {/* Department & Role */}
            <DepartmentRoleDropdown
              department={employee.departmentId}
              setDepartment={(val) => handleChange("departmentId", val)}
              role={employee.roleId}
              setRole={(val) => handleChange("roleId", val)}
            />

            {/* Reporting Manager */}
            <div className="mb-3">
              <select
                className="form-select"
                value={employee.reportingManagerId || ""}
                onChange={(e) => handleChange("reportingManagerId", e.target.value)}
              >
                <option value="">Select Reporting Manager</option>
                {managers.map((mgr) => (
                  <option key={mgr.id} value={mgr.id}>
                    {mgr.firstName} {mgr.lastName}
                  </option>
                ))}
              </select>
            </div>

            <button type="submit" className="btn btn-primary w-100">
              Add Employee
            </button>
          </form>
        </div>
      </div>
    </AdminLayout>
  );
};

export default AddEmployee;

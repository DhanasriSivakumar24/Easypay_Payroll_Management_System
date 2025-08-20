import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import AdminLayout from "../navbar/AdminLayout";
import DepartmentRoleDropdown from "./DepartmentRoleDropdown";
import {
  GetEmployeeById,
  UpdateEmployee,
} from "../../service/employee.service";
import "./AddEmployee.css";

const UpdateEmployeeDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [employee, setEmployee] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phoneNumber: "",
    address: "",
    panNumber: "",
    salary: "",
    dateOfBirth: "",
    joinDate: "",
    gender: "",
    departmentId: "",
    roleId: "",
    reportingManagerId: null,
  });

  // Format date into YYYY-MM-DD
  const formatDate = (dateStr) => {
    if (!dateStr) return "";
    const date = new Date(dateStr);
    return date.toISOString().split("T")[0];
  };

  // Fetch employee by ID
  useEffect(() => {
    if (id) {
      GetEmployeeById(id)
        .then((res) => {
          const data = res.data;
          setEmployee({
            ...data,
            dateOfBirth: formatDate(data.dateOfBirth),
            joinDate: formatDate(data.joinDate),
            departmentId: data.departmentId ?? "",
            roleId: data.roleId ?? "",
            salary: data.salary ?? "",
          });
        })
        .catch((err) => console.error("Fetch failed", err));
    }
  }, [id]);

  // Handle input change
  const handleChange = (field, value) => {
    setEmployee((prev) => ({ ...prev, [field]: value }));
  };

  // Submit updated employee
  const handleSubmit = async (e) => {
    e.preventDefault();

    let payload = {
      ...employee,
      departmentId: employee.departmentId ? Number(employee.departmentId) : null,
      roleId: employee.roleId ? Number(employee.roleId) : null,
      salary: employee.salary ? Number(employee.salary) : null,
      reportingManagerId: employee.reportingManagerId
        ? Number(employee.reportingManagerId)
        : null,
    };

    // Remove empty string/null fields so backend doesn't overwrite
    Object.keys(payload).forEach(
      (key) =>
        (payload[key] === "" || payload[key] === null) && delete payload[key]
    );

    try {
      await UpdateEmployee(id, payload);
      alert("Employee updated successfully!");
      navigate("/employees");
    } catch (err) {
      console.error("Update failed:", err.response?.data || err.message);
      alert("Update failed! Check console for details.");
    }
  };

  return (
    <AdminLayout>
      <div className="add-employee-container">
        <div className="add-employee-card">
          <h4 className="mb-4">Update Employee</h4>

          <form onSubmit={handleSubmit}>
            {/* First & Last Name */}
            <div className="row g-3 mb-3">
              <div className="col-md-6">
                <label>First Name</label>
                <input
                  type="text"
                  className="form-control"
                  value={employee.firstName}
                  onChange={(e) => handleChange("firstName", e.target.value)}
                  required
                />
              </div>
              <div className="col-md-6">
                <label>Last Name</label>
                <input
                  type="text"
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
                <label>Email</label>
                <input
                  type="email"
                  className="form-control"
                  value={employee.email}
                  onChange={(e) => handleChange("email", e.target.value)}
                  required
                />
              </div>
              <div className="col-md-6">
                <label>Phone Number</label>
                <input
                  type="text"
                  className="form-control"
                  value={employee.phoneNumber}
                  onChange={(e) => handleChange("phoneNumber", e.target.value)}
                  required
                />
              </div>
            </div>

            {/* DOB & Join Date */}
            <div className="row g-3 mb-3">
              <div className="col-md-6">
                <label>Date of Birth</label>
                <input
                  type="date"
                  className="form-control"
                  value={employee.dateOfBirth}
                  onChange={(e) => handleChange("dateOfBirth", e.target.value)}
                  required
                />
              </div>
              <div className="col-md-6">
                <label>Join Date</label>
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
                <label>Address</label>
                <input
                  type="text"
                  className="form-control"
                  value={employee.address}
                  onChange={(e) => handleChange("address", e.target.value)}
                />
              </div>
              <div className="col-md-6">
                <label>PAN Number</label>
                <input
                  type="text"
                  className="form-control"
                  value={employee.panNumber}
                  onChange={(e) => handleChange("panNumber", e.target.value)}
                />
              </div>
            </div>

            {/* Salary & Gender */}
            <div className="row g-3 mb-3">
              <div className="col-md-6">
                <label>Salary</label>
                <input
                  type="number"
                  className="form-control"
                  value={employee.salary}
                  onChange={(e) => handleChange("salary", e.target.value)}
                />
              </div>
              <div className="col-md-6">
                <label>Gender</label>
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

            {/* Submit */}
            <div className="d-flex gap-2 mt-3">
              <button
                type="submit"
                className="btn btn-success w-50"
              >
                Update Employee
              </button>
              <button
                type="button"
                className="btn btn-secondary w-50"
                onClick={() => navigate("/employees")}
              >
                Cancel
              </button>
            </div>

          </form>
        </div>
      </div>
    </AdminLayout>
  );
};

export default UpdateEmployeeDetail;

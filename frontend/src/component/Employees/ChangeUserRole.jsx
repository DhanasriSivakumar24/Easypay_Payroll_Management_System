import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import {
  GetAllEmployees,
  ChangeUserRoleAPI,
} from "../../service/employee.service";
import { GetUserRoles } from "../../service/masterTable.service";
import AdminLayout from "../Sidebar/AdminLayout";
import { useNavigate } from "react-router-dom";
import "./userManagement.css";

const ChangeUserRole = () => {
  const { role } = useSelector((state) => state.auth);
  const navigate = useNavigate();

  const [employees, setEmployees] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [employeeId, setEmployeeId] = useState("");
  const [userRoles, setUserRoles] = useState([]);
  const [newUserRoleId, setNewUserRoleId] = useState("");

  const [showConfirm, setShowConfirm] = useState(false);
  const [updatedRoleName, setUpdatedRoleName] = useState("");

  useEffect(() => {
    if (role === "Admin" || role === "HR Manager") {
      GetAllEmployees()
        .then((res) => setEmployees(res.data))
        .catch((err) => console.error("Failed to load employees", err));
    }
  }, [role]);

  useEffect(() => {
    GetUserRoles()
      .then((res) => setUserRoles(res.data))
      .catch((err) => console.error("Failed to fetch user roles", err));
  }, []);

  const handleUpdateRole = () => {
    if (!employeeId || !newUserRoleId) {
      alert("Please select employee and role");
      return;
    }

    const payload = {
      employeeId: parseInt(employeeId),
      newUserRoleId: parseInt(newUserRoleId),
    };

    ChangeUserRoleAPI(payload)
      .then(() => {
        const updatedRole = userRoles.find(
          (r) => r.id === parseInt(newUserRoleId)
        );
        setUpdatedRoleName(updatedRole?.userRoleName || "Unknown");
        setShowConfirm(true);

        setEmployeeId("");
        setNewUserRoleId("");
        setSearchTerm("");
      })
      .catch((err) => {
        console.error(err);
        alert(
          err?.response?.data?.errorMessage ||
            "Failed to update role. Please try again."
        );
      });
  };

  if (role !== "Admin" && role !== "HR Manager") {
    return (
      <div className="user-management-container">
        <div className="not-allowed">
          You are not allowed to change user roles.
        </div>
      </div>
    );
  }

  const filteredEmployees = searchTerm
    ? employees.filter(
        (emp) =>
          (emp.name &&
            emp.name.toLowerCase().includes(searchTerm.toLowerCase())) ||
          (emp.email &&
            emp.email.toLowerCase().includes(searchTerm.toLowerCase()))
      )
    : employees;

  return (
    <AdminLayout>
      <div className="user-management-container">
        <div className="header-row">
          <h2>Change User Role</h2>
        </div>

        <div className="form-section">
          <div className="employee-row">
            <div className="employee-dropdown">
              <label>Choose Employee</label>
              <select
                value={employeeId}
                onChange={(e) => setEmployeeId(e.target.value)}
                className="dropdown large-dropdown"
              >
                <option value="">-- Select Employee --</option>
                {filteredEmployees.map((emp) => (
                  <option key={emp.id} value={emp.id}>
                    {emp.name} ({emp.email})
                  </option>
                ))}
              </select>
            </div>

            <div className="employee-search">
              <label>Search Employee</label>
              <input
                type="text"
                className="input-box small-search"
                placeholder="Type name or email..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
            </div>
          </div>

          <label>Choose Role</label>
          <select
            value={newUserRoleId}
            onChange={(e) => setNewUserRoleId(e.target.value)}
            className="dropdown"
          >
            <option value="">-- Select Role --</option>
            {userRoles.map((r) => (
              <option key={r.id} value={r.id}>
                {r.userRoleName}
              </option>
            ))}
          </select>

          <div className="btn-row">
            <button className="btn-primary" onClick={handleUpdateRole}>
              Update Role
            </button>
            <button
              className="btn-secondary"
              onClick={() => {
                navigate("/user-management");
                setEmployeeId("");
                setNewUserRoleId("");
                setSearchTerm("");
              }}
            >
              Cancel
            </button>
          </div>
        </div>

        {showConfirm && (
          <div className="confirm-modal">
            <div className="confirm-box">
              <h3>Role Updated</h3>
              <p>
                User role has been successfully updated to{" "}
                <strong>{updatedRoleName}</strong>.
              </p>
              <div className="confirm-actions">
                <button
                  className="btn-confirm"
                  onClick={() => setShowConfirm(false)}
                >
                  OK
                </button>
              </div>
            </div>
          </div>
        )}
      </div>
    </AdminLayout>
  );
};

export default ChangeUserRole;

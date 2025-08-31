import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { GetAllEmployees } from "../../service/employee.service";
import { RegisterUser } from "../../service/login.service";
import AdminLayout from "../Sidebar/AdminLayout";
import "./userManagement.css";

const UserManagement = () => {
  const { role } = useSelector((state) => state.auth);
  const [employees, setEmployees] = useState([]);
  const [searchTerm, setSearchTerm] = useState("");
  const [employeeId, setEmployeeId] = useState("");
  const [userName, setUserName] = useState("");
  const [registeredUsers, setRegisteredUsers] = useState([]);

  useEffect(() => {
    if (role === "Admin" || role === "HR Manager") {
      GetAllEmployees()
        .then((res) => setEmployees(res.data))
        .catch((err) => console.error("Failed to load employees", err));
    }
  }, [role]);

  const handleRegister = () => {
    if (!employeeId || !userName) {
      alert("Please select employee and enter username");
      return;
    }

    const newUser = {
      employeeId: parseInt(employeeId),
      userName,
    };

    RegisterUser(newUser)
      .then((res) => {
        alert(`User Registered: ${res.data.userName} (${res.data.role})`);
        setRegisteredUsers([...registeredUsers, res.data]);
        setEmployeeId("");
        setUserName("");
      })
      .catch((err) => {
        console.error(err);
        alert("Failed to register user");
      });
  };

  const handleCancel = () => {
    setEmployeeId("");
    setUserName("");
    setSearchTerm("");
  };

  if (role !== "Admin" && role !== "HR Manager") {
    return (
      <div className="user-management-container">
        <div className="not-allowed">You are not allowed to manage users.</div>
      </div>
    );
  }

  const filteredEmployees = searchTerm
    ? employees.filter(
        (emp) =>
          (emp.name && emp.name.toLowerCase().includes(searchTerm.toLowerCase())) ||
          (emp.email && emp.email.toLowerCase().includes(searchTerm.toLowerCase()))
      )
    : employees;

  return (
    <AdminLayout>
      <div className="user-management-container">
        <h2>User Management</h2>

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

          <label>Username</label>
          <input
            type="text"
            className="input-box"
            placeholder="Enter username"
            value={userName}
            onChange={(e) => setUserName(e.target.value)}
          />

          <div className="btn-row">
            <button className="btn-primary" onClick={handleRegister}>
              Register User
            </button>
            <button className="btn-secondary" onClick={handleCancel}>
              Cancel
            </button>
          </div>
        </div>

        {registeredUsers.length > 0 && (
          <div className="table-section">
            <h3>Registered Users</h3>
            <table className="user-table">
              <thead>
                <tr>
                  <th>S.No</th>
                  <th>Username</th>
                  <th>Employee</th>
                  <th>Email</th>
                  <th>Role</th>
                </tr>
              </thead>
              <tbody>
                {registeredUsers.map((user, index) => {
                  const emp = employees.find((e) => e.id === user.employeeId);
                  return (
                    <tr key={index}>
                      <td>{index + 1}</td>
                      <td>{user.userName}</td>
                      <td>{emp?.name || "N/A"}</td>
                      <td>{emp?.email || "N/A"}</td>
                      <td>{user.role}</td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </AdminLayout>
  );
};

export default UserManagement;

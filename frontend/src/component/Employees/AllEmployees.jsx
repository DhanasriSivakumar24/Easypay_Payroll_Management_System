import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import { GetAllEmployees } from "../../service/employee.service";
import AdminLayout from "../Sidebar/AdminLayout";
import PayrollProcessorLayout from "../Sidebar/PayrollProcessorLayout";
import ManagerLayout from "../Sidebar/ManagerLayout";
import "./AllEmployees.css";

const AllEmployees = () => {
  const { role } = useSelector((state) => state.auth);
  const [employees, setEmployees] = useState([]);
  const [search, setSearch] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    GetAllEmployees()
      .then((res) => setEmployees(res.data || []))
      .catch((err) => console.log(err));
  }, []);

  const filteredEmployees = employees.filter((emp) => {
    const id = emp?.id ? String(emp.id).toLowerCase() : "";
    const name = emp?.firstName ? emp.firstName.toLowerCase() : "";
    const email = emp?.email ? emp.email.toLowerCase() : "";
    const dept = emp?.departmentName ? emp.departmentName.toLowerCase() : "";

    return (
      id.includes(search.toLowerCase()) ||
      name.includes(search.toLowerCase()) ||
      email.includes(search.toLowerCase()) ||
      dept.includes(search.toLowerCase())
    );
  });

  const Layout =
    role === "Payroll Processor"
      ? PayrollProcessorLayout
      : role === "Manager"
      ? ManagerLayout
      : AdminLayout;

  return (
    <Layout>
      <div className="employee-container">

        <div className="header-row">
          <h2>Employees</h2>
          <div className="actions">
            <input
              type="text"
              className="search-input"
              placeholder="Search by ID, Name, Email, or Department..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />

            {(role === "Admin" || role === "HR Manager") && (
              <button
                className="add-btn"
                onClick={() => navigate("/employees/add-employee")}
              >
                + Add Employee
              </button>
            )}
          </div>
        </div>

        <div className="employee-card">
          <table className="employee-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Name</th>
                <th>Email</th>
                <th>Phone</th>
                <th>Department</th>
                <th>Role</th>
                <th>Status</th>
                <th className="text-center">Action</th>
              </tr>
            </thead>
            <tbody>
              {filteredEmployees.length > 0 ? (
                filteredEmployees.map((emp, index) => (
                  <tr key={index}>
                    <td>{emp?.id || "-"}</td>
                    <td>{emp?.firstName} {emp?.lastName}</td>
                    <td>{emp?.email || "-"}</td>
                    <td>{emp?.phoneNumber || "-"}</td>
                    <td>{emp?.departmentName || "-"}</td>
                    <td>{emp?.roleName || "-"}</td>
                    <td>
                      <span
                        className={`status-badge ${emp?.statusName?.toLowerCase()}`}
                      >
                        {emp?.statusName || "Unknown"}
                      </span>
                    </td>
                    <td className="text-center">
                      <button
                        className="btn-view"
                        onClick={() => alert(`View profile of ${emp?.firstName}`)}
                      >
                        View
                      </button>

                      {(role === "Admin" || role === "HR Manager") && (
                        <button
                          className="btn-edit"
                          onClick={() =>
                            navigate(`/employees/update-employee/${emp.id}`)
                          }
                        >
                          Edit
                        </button>
                      )}

                      {role === "Admin" && (
                        <button
                          className="btn-delete"
                          onClick={() => alert("Delete functionality")}
                        >
                          Delete
                        </button>
                      )}
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="8" className="no-data">
                    No employees found
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </Layout>
  );
};

export default AllEmployees;

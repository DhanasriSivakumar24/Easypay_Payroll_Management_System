import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import { GetEmployeeById } from "../../service/employee.service";
import AdminLayout from "../Sidebar/AdminLayout";
import ManagerLayout from "../Sidebar/ManagerLayout";
import "./ViewEmployee.css";

const ViewEmployee = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [employee, setEmployee] = useState(null);
  const { role } = useSelector((state) => state.auth);

  const Layout = role === "Manager" ? ManagerLayout : AdminLayout;

  const formatDate = (dateStr) => {
    if (!dateStr) return "N/A";
    const date = new Date(dateStr);
    return date.toLocaleDateString("en-GB", {
      day: "2-digit",
      month: "short",
      year: "numeric",
    });
  };

  useEffect(() => {
    if (id) {
      GetEmployeeById(id)
        .then((res) => setEmployee(res.data))
        .catch((err) => console.error("Fetch failed", err));
    }
  }, [id]);

  if (!employee) {
    return (
      <Layout>
        <div className="view-employee-container">
          <div className="view-employee-card text-center">
            <p>Loading employee details...</p>
          </div>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="view-employee-container">
        <div className="view-employee-card">
          <h4 className="mb-4">Employee Details</h4>

          <div className="employee-info-grid">
            <div>
              <label>First Name</label>
              <p>{employee.firstName}</p>
            </div>
            <div>
              <label>Last Name</label>
              <p>{employee.lastName}</p>
            </div>
            <div>
              <label>Email</label>
              <p>{employee.email}</p>
            </div>
            <div>
              <label>Phone Number</label>
              <p>{employee.phoneNumber}</p>
            </div>
            <div>
              <label>Date of Birth</label>
              <p>{formatDate(employee.dateOfBirth)}</p>
            </div>
            <div>
              <label>Join Date</label>
              <p>{formatDate(employee.joinDate)}</p>
            </div>
            <div>
              <label>Address</label>
              <p>{employee.address || "N/A"}</p>
            </div>
            <div>
              <label>PAN Number</label>
              <p>{employee.panNumber || "N/A"}</p>
            </div>
            <div>
              <label>Salary</label>
              <p>₹ {employee.salary?.toLocaleString() || "N/A"}</p>
            </div>
            <div>
              <label>Gender</label>
              <p>{employee.gender}</p>
            </div>
            <div>
              <label>Department</label>
              <p>{employee.departmentName || "N/A"}</p>
            </div>
            <div>
              <label>Role</label>
              <p>{employee.roleName || "N/A"}</p>
            </div>
          </div>

          {/* Action Buttons */}
          {role === "Admin" || role === "HR Manager" ? (
            <div className="d-flex gap-2 mt-4">
              <button
                className="btn btn-primary w-50"
                onClick={() =>
                  navigate(`/employees/update-employee/${employee.id}`)
                }
              >
                Edit Employee
              </button>
              <button
                className="btn btn-secondary w-50"
                onClick={() => navigate("/employees")}
              >
                Back
              </button>
            </div>
          ) : role === "Manager" ? (
            <div className="mt-4 text-end">
              <button
                className="btn btn-secondary"
                onClick={() => navigate("/employees")}
              >
                Back
              </button>
            </div>
          ) : null}
        </div>
      </div>
    </Layout>
  );
};

export default ViewEmployee;

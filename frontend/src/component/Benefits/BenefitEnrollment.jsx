import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import AdminLayout from "../navbar/AdminLayout";
import { GetAllEmployees } from "../../service/employee.service";
import { AddBenefitEnrollment } from "../../service/benefit.service";
import {GetBenefitsMaster } from "../../service/masterTable.service"
import './benefitEnrollment.css';

const BenefitEnrollment = () => {
  const navigate = useNavigate();
  const { role } = useSelector(state => state.auth);

  const [employees, setEmployees] = useState([]);
  const [benefits, setBenefits] = useState([]);
  const [enrollment, setEnrollment] = useState({
    employeeId: "",
    benefitId: "",
    startDate: "",
    endDate: "",
    statusId: 1
  });

  useEffect(() => {
    const allowedRoles = ["Admin", "HR Manager"];
    if (!allowedRoles.includes(role)) return;

    // Load Employees
    GetAllEmployees()
      .then(res => setEmployees(res.data || []))
      .catch(err => console.error(err));

    // Load Benefits from Master Table and filter active ones
    GetBenefitsMaster()
      .then(res => {
        const activeBenefits = (res.data || []).filter(b => b.isActive);
        setBenefits(activeBenefits);
      })
      .catch(err => console.error(err));
  }, [role]);

  const handleChange = (field, value) => setEnrollment({ ...enrollment, [field]: value });

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!enrollment.employeeId || !enrollment.benefitId || !enrollment.startDate || !enrollment.endDate) {
      alert("Please fill all fields");
      return;
    }

    AddBenefitEnrollment(enrollment)
      .then(() => {
        alert("Benefit enrolled successfully!");
        navigate("/benefits");
      })
      .catch(err => alert("Error: " + (err.response?.data?.errorMessage || err.message)));
  };

  const allowedRoles = ["Admin", "HR Manager"];
  if (!allowedRoles.includes(role)) return <p className="access-denied">Access Denied</p>;

  return (
    <AdminLayout>
      <div className="benefit-enroll-form-container">
        <div className="benefit-enroll-card">
          <h2>Enroll Employee Benefit</h2>
          <form onSubmit={handleSubmit}>

            {/* Employee */}
            <div className="form-group">
              <label>Employee</label>
              <select
                className="form-select"
                value={enrollment.employeeId}
                onChange={(e) => handleChange("employeeId", parseInt(e.target.value))}
                required
              >
                <option value="">Select Employee</option>
                {employees.map(emp => (
                  <option key={emp.id} value={emp.id}>
                    {emp.firstName} {emp.lastName}
                  </option>
                ))}
              </select>
            </div>

            {/* Benefit */}
            <div className="form-group">
              <label>Benefit</label>
              <select
                className="form-select"
                value={enrollment.benefitId}
                onChange={(e) => handleChange("benefitId", parseInt(e.target.value))}
                required
              >
                <option value="">Select Benefit</option>
                {benefits.map(b => (
                  <option key={b.id} value={b.id}>{b.benefitName}</option>
                ))}
              </select>
            </div>

            {/* Start Date */}
            <div className="form-group">
              <label>Start Date</label>
              <input
                type="date"
                className="form-control"
                value={enrollment.startDate}
                onChange={(e) => handleChange("startDate", e.target.value)}
                required
              />
            </div>

            {/* End Date */}
            <div className="form-group">
              <label>End Date</label>
              <input
                type="date"
                className="form-control"
                value={enrollment.endDate}
                onChange={(e) => handleChange("endDate", e.target.value)}
                required
              />
            </div>

            {/* Form Actions */}
            <div className="form-actions">
              <button type="submit" className="btn btn-primary w-50">Enroll Benefit</button>
              <button type="button" className="btn btn-secondary w-50" onClick={() => navigate("/benefits-management")}>Cancel</button>
            </div>
          </form>
        </div>
      </div>
    </AdminLayout>
  );
};

export default BenefitEnrollment;

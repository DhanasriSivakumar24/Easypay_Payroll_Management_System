import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import AdminLayout from "../navbar/AdminLayout";
import { GeneratePayroll } from "../../service/payroll.service";
import { GetAllEmployees } from "../../service/employee.service";
import { GetAllPolicies } from "../../service/policy.service";
import "./generatePayroll.css";

const GeneratePayrollPage = () => {
  const [employeeId, setEmployeeId] = useState("");
  const [policyId, setPolicyId] = useState("");
  const [periodStart, setPeriodStart] = useState("");
  const [periodEnd, setPeriodEnd] = useState("");
  const [employees, setEmployees] = useState([]);
  const [policies, setPolicies] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    // Load Employees
    GetAllEmployees()
      .then(res => setEmployees(res.data || []))
      .catch(err => console.error("Failed to fetch employees:", err));

    // Load Policies
    GetAllPolicies()
      .then(res => {
        const data = res.data?.policies || res.data || [];
        setPolicies(data);
      })
      .catch(err => console.error("Failed to fetch policies:", err));
  }, []);

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!employeeId || !policyId || !periodStart || !periodEnd) {
      alert("Please fill all fields!");
      return;
    }

    // Correct payload matching C# DTO (PascalCase)
    const dto = {
      EmployeeId: Number(employeeId),
      PolicyId: Number(policyId),
      PeriodStart: periodStart, // YYYY-MM-DD
      PeriodEnd: periodEnd      // YYYY-MM-DD
    };

    // Debug: see what is being sent
    console.log("Payroll DTO to send:", dto);

    GeneratePayroll(dto)
      .then(() => {
        alert("Payroll generated successfully!");
        navigate("/payrolls");
      })
      .catch(err => {
        console.error("Generate payroll error:", err.response || err);
        alert("Failed to generate payroll: " + JSON.stringify(err.response?.data || err.message));
      });
  };

  return (
    <AdminLayout>
      <div className="generate-payroll-container">
        <h2>Generate Payroll</h2>
        <form onSubmit={handleSubmit} className="generate-form">

          {/* Employee Dropdown */}
          <div className="form-group">
            <label>Employee</label>
            <select value={employeeId} onChange={(e) => setEmployeeId(e.target.value)}>
              <option value="">-- Select Employee --</option>
              {employees.map(emp => (
                <option key={emp.id} value={emp.id}>
                  {emp.firstName} {emp.lastName}
                </option>
              ))}
            </select>
          </div>

          {/* Policy Dropdown */}
          <div className="form-group">
            <label>Policy</label>
            <select value={policyId} onChange={(e) => setPolicyId(e.target.value)}>
              <option value="">-- Select Policy --</option>
              {policies.map(pol => (
                <option key={pol.id} value={pol.id}>
                  {pol.policyName}
                </option>
              ))}
            </select>
          </div>

          {/* Period Start */}
          <div className="form-group">
            <label>Period Start</label>
            <input type="date" value={periodStart} onChange={(e) => setPeriodStart(e.target.value)} />
          </div>

          {/* Period End */}
          <div className="form-group">
            <label>Period End</label>
            <input type="date" value={periodEnd} onChange={(e) => setPeriodEnd(e.target.value)} />
          </div>

          {/* Buttons */}
          <div className="d-flex gap-2 mt-3">
            <button type="submit" className="btn btn-primary w-50">Generate</button>
            <button type="button" className="btn btn-secondary w-50" onClick={() => navigate("/payrolls")}>
              Cancel
            </button>
          </div>

        </form>
      </div>
    </AdminLayout>
  );
};

export default GeneratePayrollPage;

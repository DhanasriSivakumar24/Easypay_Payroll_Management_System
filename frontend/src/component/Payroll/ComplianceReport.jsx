import { useEffect, useState } from "react";
import AdminLayout from "../navbar/AdminLayout";
import { GetComplianceReport } from "../../service/payroll.service";
import "./complianceReport.css";

const ComplianceReport = () => {
  const [report, setReport] = useState(null);
  const [loading, setLoading] = useState(false);
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  // Store default previous month range and default report
  const [defaultStart, setDefaultStart] = useState("");
  const [defaultEnd, setDefaultEnd] = useState("");
  const [defaultReport, setDefaultReport] = useState(null);

  // On mount: get previous month range & initial report
  useEffect(() => {
    const now = new Date();
    const firstDayPrevMonth = new Date(now.getFullYear(), now.getMonth() - 1, 1);
    const lastDayPrevMonth = new Date(now.getFullYear(), now.getMonth(), 0);

    const start = firstDayPrevMonth.toISOString().split("T")[0];
    const end = lastDayPrevMonth.toISOString().split("T")[0];

    setStartDate(start);
    setEndDate(end);
    setDefaultStart(start);
    setDefaultEnd(end);

    fetchReport(start, end, true);
  }, []);

  const fetchReport = (start, end, isInitial = false) => {
    setLoading(true);
    GetComplianceReport(start, end)
      .then((res) => {
        setReport(res.data);
        if (isInitial) setDefaultReport(res.data); // store initial report
      })
      .catch((err) => console.error(err))
      .finally(() => setLoading(false));
  };

  const handleFilter = () => {
    if (!startDate || !endDate) return alert("Please select both dates");
    if (startDate > endDate) return alert("Start date cannot be after End date");
    fetchReport(startDate, endDate);
  };

  const handleCancel = () => {
    // Reset dates and table to initial previous month
    setStartDate(defaultStart);
    setEndDate(defaultEnd);
    setReport(defaultReport);
  };

  return (
    <AdminLayout>
      <div className="employee-container p-4">
        <div className="header-row">
          <h2>Compliance Report</h2>
          <div className="actions">
            <div className="date-filters">
              <div className="date-group">
                <label htmlFor="startDate">Start Date</label>
                <input
                  type="date"
                  id="startDate"
                  className="search-input"
                  value={startDate}
                  onChange={(e) => setStartDate(e.target.value)}
                />
              </div>
              <div className="date-group">
                <label htmlFor="endDate">End Date</label>
                <input
                  type="date"
                  id="endDate"
                  className="search-input"
                  value={endDate}
                  onChange={(e) => setEndDate(e.target.value)}
                />
              </div>
            </div>
            <div className="buttons">
              <button className="add-btn" onClick={handleFilter}>Filter</button>
              <button className="cancel-btn" onClick={handleCancel}>Cancel</button>
            </div>
          </div>
        </div>

        <div className="employee-card">
          <table className="employee-table">
            <thead>
              <tr>
                <th>S.No</th>
                <th>Employee ID</th>
                <th>Employee Name</th>
                <th>Gross Salary</th>
                <th>PF Contribution</th>
              </tr>
            </thead>
            <tbody>
              {report && report.employeeDetails.length > 0 ? (
                report.employeeDetails.map((emp, idx) => (
                  <tr key={idx}>
                    <td>{idx + 1}</td>
                    <td>{emp.employeeId}</td>
                    <td>{emp.employeeName}</td>
                    <td>{emp.grossSalary}</td>
                    <td>{emp.pfContribution}</td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="5" className="no-data">
                    No data found
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>

        {report && (
          <div className="summary mt-3">
            <p><strong>Total Gross Salary:</strong> {report.totalGrossSalary}</p>
            <p><strong>Total PF Contribution:</strong> {report.totalPFContribution}</p>
          </div>
        )}
      </div>
    </AdminLayout>
  );
};

export default ComplianceReport;

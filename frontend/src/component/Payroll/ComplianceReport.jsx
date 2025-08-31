import { useEffect, useState } from "react";
import AdminLayout from "../Sidebar/AdminLayout";
import { GetComplianceReport } from "../../service/payroll.service";
import jsPDF from "jspdf";
import autoTable from "jspdf-autotable";
import "./complianceReport.css";

const ComplianceReport = () => {
  const [report, setReport] = useState(null);
  const [loading, setLoading] = useState(false);
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  const [defaultStart, setDefaultStart] = useState("");
  const [defaultEnd, setDefaultEnd] = useState("");
  const [defaultReport, setDefaultReport] = useState(null);

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
        if (isInitial) setDefaultReport(res.data);
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
    setStartDate(defaultStart);
    setEndDate(defaultEnd);
    setReport(defaultReport);
  };

  const downloadReport = () => {
    const doc = new jsPDF();
    
    doc.setFontSize(16);
    doc.text("Compliance Report", 14, 20);
    doc.setFontSize(12);
    doc.text(`Period: ${startDate} to ${endDate}`, 14, 28);
    
    const tableColumn = ["S.No", "Employee ID", "Employee Name", "Gross Salary", "PF Contribution"];
    const tableRows = report.employeeDetails.map((emp, idx) => [
      idx + 1,
      emp.employeeId,
      emp.employeeName,
      emp.grossSalary,
      emp.pfContribution,
    ]);
    
    autoTable(doc, {
      head: [tableColumn],
      body: tableRows,
      startY: 35,
      styles: { fontSize: 10 },
      headStyles: { fillColor: [135, 206, 235] },
      margin: { top: 30, left: 14, right: 14 },
    });
    
    const finalY = doc.lastAutoTable.finalY;
    doc.setFontSize(12);
    doc.text(`Total Gross Salary: ${report.totalGrossSalary}`, 14, finalY + 10);
    doc.text(`Total PF Contribution: ${report.totalPFContribution}`, 14, finalY + 18);
    
    doc.save(`Compliance_Report_${startDate}_to_${endDate}.pdf`);
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
              {report && (
                <button className="add-btn" onClick={downloadReport}>
                  Download
                </button>
              )}
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
              {loading ? (
                <tr>
                  <td colSpan="5" className="no-data">
                    Loading report...
                  </td>
                </tr>
              ) : report && report.employeeDetails.length > 0 ? (
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
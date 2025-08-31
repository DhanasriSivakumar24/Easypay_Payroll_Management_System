import { useEffect, useState } from "react";
import PayrollProcessorLayout from "../Sidebar/PayrollProcessorLayout";
import { GetAllPayrolls } from "../../service/payroll.service";
import jsPDF from "jspdf";
import autoTable from "jspdf-autotable";
import "./payrollReport.css";

const PayrollReport = () => {
  const [payrolls, setPayrolls] = useState([]);
  const [loading, setLoading] = useState(false);
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  const [defaultStart, setDefaultStart] = useState("");
  const [defaultEnd, setDefaultEnd] = useState("");

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

    fetchPayrolls(start, end);
  }, []);

  const fetchPayrolls = (start, end) => {
    setLoading(true);
    GetAllPayrolls()
      .then((res) => {
        let all = res.data || [];
        const filtered = all.filter((p) => {
          if (!p.paidDate) return false;
          const payDate = new Date(p.paidDate);
          return payDate >= new Date(start) && payDate <= new Date(end);
        });
        setPayrolls(filtered);
      })
      .catch(console.error)
      .finally(() => setLoading(false));
  };

  const handleFilter = () => {
    if (!startDate || !endDate) return alert("Please select both dates");
    if (startDate > endDate) return alert("Start date cannot be after End date");
    fetchPayrolls(startDate, endDate);
  };

  const handleCancel = () => {
    setStartDate(defaultStart);
    setEndDate(defaultEnd);
    fetchPayrolls(defaultStart, defaultEnd);
  };

  const handleDownload = () => {
    const doc = new jsPDF();

    doc.setFontSize(16);
    doc.text("Payroll Report", 14, 20);
    doc.setFontSize(12);
    doc.text(`Period: ${startDate} to ${endDate}`, 14, 28);

    const tableColumn = [
      "S.No",
      "Employee",
      "Basic Pay",
      "Allowances",
      "Deductions",
      "Net Pay",
      "Status",
      "Pay Date",
    ];

    const tableRows = payrolls.map((p, idx) => [
      idx + 1,
      p.employeeName,
      p.basicPay,
      p.allowances,
      p.deductions,
      p.netPay,
      p.statusName,
      new Date(p.paidDate).toLocaleDateString("en-IN"),
    ]);

    autoTable(doc, {
      head: [tableColumn],
      body: tableRows,
      startY: 35,
      styles: { fontSize: 9 },
      headStyles: { fillColor: [135, 206, 235] },
      margin: { top: 30, left: 14, right: 14 },
    });

    doc.save(`Payroll_Report_${startDate}_to_${endDate}.pdf`);
  };

  return (
    <PayrollProcessorLayout>
      <div className="report-container">
        {/* Header */}
        <div className="report-header">
          <h2>Payroll Report</h2>
          <div className="report-actions">
            <div className="report-date-filters">
              <div className="report-date-group">
                <label>Start Date</label>
                <input
                  type="date"
                  className="report-input"
                  value={startDate}
                  onChange={(e) => setStartDate(e.target.value)}
                />
              </div>
              <div className="report-date-group">
                <label>End Date</label>
                <input
                  type="date"
                  className="report-input"
                  value={endDate}
                  onChange={(e) => setEndDate(e.target.value)}
                />
              </div>
            </div>
            <div className="report-buttons">
              <button className="report-btn-primary" onClick={handleFilter}>
                Filter
              </button>
              <button className="report-btn-cancel" onClick={handleCancel}>
                Cancel
              </button>
              {payrolls.length > 0 && (
                <button className="report-btn-primary" onClick={handleDownload}>
                  Download
                </button>
              )}
            </div>
          </div>
        </div>

        {/* Table */}
        <div className="report-card">
          <table className="report-table">
            <thead>
              <tr>
                <th>S.No</th>
                <th>Employee</th>
                <th>Basic Pay</th>
                <th>Allowances</th>
                <th>Deductions</th>
                <th>Net Pay</th>
                <th>Status</th>
                <th>Pay Date</th>
              </tr>
            </thead>
            <tbody>
              {loading ? (
                <tr>
                  <td colSpan="8" className="report-no-data">
                    Loading report...
                  </td>
                </tr>
              ) : payrolls.length > 0 ? (
                payrolls.map((p, idx) => (
                  <tr key={p.id}>
                    <td>{idx + 1}</td>
                    <td>{p.employeeName}</td>
                    <td>{p.basicPay}</td>
                    <td>{p.allowances}</td>
                    <td>{p.deductions}</td>
                    <td>{p.netPay}</td>
                    <td>{p.statusName}</td>
                    <td>{new Date(p.paidDate).toLocaleDateString("en-IN")}</td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="8" className="report-no-data">
                    No data found
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </PayrollProcessorLayout>
  );
};

export default PayrollReport;

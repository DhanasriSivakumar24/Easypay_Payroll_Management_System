import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { GetAllPayrolls, MarkPayrollAsPaid } from "../../service/payroll.service";
import PayrollProcessorLayout from "../Sidebar/PayrollProcessorLayout";
import "./pendingPayroll.css";

const PendingPayroll = () => {
  const [approvedPayrolls, setApprovedPayrolls] = useState([]);
  const [paidPayrolls, setPaidPayrolls] = useState([]);
  const [loading, setLoading] = useState(false);
  const [activeTab, setActiveTab] = useState("approved");
  const { employeeId: adminId } = useSelector((state) => state.auth);

  useEffect(() => {
    setLoading(true);
    GetAllPayrolls()
      .then((res) => {
        const all = res.data || [];
        setApprovedPayrolls(all.filter((p) => p.statusName?.toLowerCase() === "approved"));
        setPaidPayrolls(all.filter((p) => p.statusName?.toLowerCase() === "paid"));
      })
      .catch((err) => console.error("Fetch payrolls failed:", err))
      .finally(() => setLoading(false));
  }, []);

  const handlePay = (payrollId, empName) => {
    if (!window.confirm(`Are you sure you want to pay ${empName}?`)) return;
    MarkPayrollAsPaid(payrollId, adminId)
      .then(() => {
        alert(`Payroll for ${empName} marked as Paid `);
        setApprovedPayrolls((prev) => prev.filter((p) => p.id !== payrollId));
        const justPaid = approvedPayrolls.find((p) => p.id === payrollId);
        if (justPaid) {
          setPaidPayrolls((prev) => [
            ...prev,
            { ...justPaid, statusName: "Paid", paidDate: new Date().toISOString() },
          ]);
        }
      })
      .catch((err) => {
        console.error(err);
        alert("Failed to mark payroll as paid");
      });
  };

  const renderTable = (data, showAction) => (
    <div className="payroll-card">
      <table className="payroll-table">
        <thead>
          <tr>
            <th>S.No</th>
            <th>Employee</th>
            <th>Basic Pay</th>
            <th>Allowances</th>
            <th>Deductions</th>
            <th>Net Pay</th>
            <th>Pay Date</th>
            <th>Status</th>
            {showAction && <th className="text-center">Action</th>}
          </tr>
        </thead>
        <tbody>
          {loading ? (
            <tr>
              <td colSpan={showAction ? "9" : "8"} className="no-data">
                Loading...
              </td>
            </tr>
          ) : data.length > 0 ? (
            data.map((p, idx) => (
              <tr key={p.id}>
                <td>{idx + 1}</td>
                <td>{p.employeeName}</td>
                <td>{p.basicPay}</td>
                <td>{p.allowances}</td>
                <td>{p.deductions}</td>
                <td>{p.netPay}</td>
                <td>{p.paidDate ? new Date(p.paidDate).toLocaleDateString("en-IN") : "-"}</td>
                <td>
                  <span className={`status-badge ${p.statusName?.toLowerCase()}`}>
                    {p.statusName}
                  </span>
                </td>
                {showAction && (
                  <td className="text-center">
                    <button
                      className="pay-btn"
                      onClick={() => handlePay(p.id, p.employeeName)}
                    >
                      Pay Now
                    </button>
                  </td>
                )}
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={showAction ? "9" : "8"} className="no-data">
                No data found
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );

  return (
    <PayrollProcessorLayout>
      <div className="payroll-container">
        <div className="header-row">
          <h2>Payroll Payments</h2>
          <div className="tabs">
            <button
              className={activeTab === "approved" ? "tab active" : "tab"}
              onClick={() => setActiveTab("approved")}
            >
              Approved (To Pay)
            </button>
            <button
              className={activeTab === "paid" ? "tab active" : "tab"}
              onClick={() => setActiveTab("paid")}
            >
              Paid
            </button>
          </div>
        </div>

        {activeTab === "approved" && renderTable(approvedPayrolls, true)}
        {activeTab === "paid" && renderTable(paidPayrolls, false)}
      </div>
    </PayrollProcessorLayout>
  );
};

export default PendingPayroll;

import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import {  GetAllPayrolls,  VerifyPayroll,  ApprovePayroll,  MarkPayrollAsPaid,} from "../../service/payroll.service";
import AdminLayout from "../Sidebar/AdminLayout";
import PayrollProcessorLayout from "../Sidebar/PayrollProcessorLayout";
import ManagerLayout from "../Sidebar/ManagerLayout";
import "./allPayrolls.css";

const AllPayrolls = () => {
  const [payrolls, setPayrolls] = useState([]);
  const [search, setSearch] = useState("");
  const navigate = useNavigate();

  const { role, employeeId } = useSelector((state) => state.auth);
  const normalizedRole = role?.toLowerCase();

  const isAdmin = ["admin", "hrmanager", "hr manager"].includes(normalizedRole);
  const isProcessor = ["payrollprocessor", "payroll processor"].includes(normalizedRole);
  const isManager = normalizedRole === "manager";

  const Layout = isProcessor
    ? PayrollProcessorLayout
    : isManager
    ? ManagerLayout
    : AdminLayout;

  const fetchPayrolls = () => {
    GetAllPayrolls()
      .then((res) => setPayrolls(res.data || []))
      .catch(() =>
        alert("Failed to load payrolls. Please check your connection or contact support.")
      );
  };

  useEffect(() => {
    if (isAdmin || isProcessor || isManager) {
      fetchPayrolls();
    }
  }, [isAdmin, isProcessor, isManager]);

  const refreshPayrolls = () => {
    GetAllPayrolls()
      .then((res) => setPayrolls(res.data || []))
      .catch(() => alert("Failed to refresh payrolls. Please try again."));
  };

  const handleVerify = (id) => {
    if (window.confirm("Verify this payroll?")) {
      VerifyPayroll(id)
        .then(() => {
          alert("Payroll verified successfully!");
          refreshPayrolls();
        })
        .catch(() => alert("Failed to verify payroll. Please try again."));
    }
  };

  const handleApprove = (id) => {
    if (window.confirm("Approve this payroll?")) {
      ApprovePayroll(id)
        .then(() => {
          alert("Payroll approved successfully!");
          refreshPayrolls();
        })
        .catch(() => alert("Failed to approve payroll. Please try again."));
    }
  };

  const handleMarkPaid = (id) => {
    const adminId = sessionStorage.getItem("employeeId");
    if (!adminId) {
      alert("Admin ID not found. Please log in again.");
      return;
    }
    if (window.confirm("Mark this payroll as Paid?")) {
      MarkPayrollAsPaid(id, adminId)
        .then(() => {
          alert("Payroll marked as Paid!");
          refreshPayrolls();
        })
        .catch(() => alert("Failed to mark payroll as paid. Please try again."));
    }
  };

  const filteredPayrolls = payrolls.filter((p) => {
    const emp = p?.employeeName?.toLowerCase() || "";
    const policy = p?.policyName?.toLowerCase() || "";
    const status = p?.statusName?.toLowerCase() || "";
    return (
      emp.includes(search.toLowerCase()) ||
      policy.includes(search.toLowerCase()) ||
      status.includes(search.toLowerCase())
    );
  });

  if (!isAdmin && !isProcessor && !isManager) {
    return (
      <Layout>
        <div className="unauthorized">
          <h2>Unauthorized</h2>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="payroll-container">
        <div className="header-row">
          <h2>Payrolls</h2>
          <div className="actions">
            <input
              type="text"
              className="search-input"
              placeholder="Search by Employee, Policy, or Status..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
            {(isAdmin || isProcessor) && (
              <button
                className="add-btn"
                onClick={() => navigate("/payrolls/generate-payroll")}
              >
                + Generate Payroll
              </button>
            )}
          </div>
        </div>

        <div className="payroll-card">
          <table className="payroll-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Employee</th>
                <th>Policy</th>
                <th>Period</th>
                <th>Deduction</th>
                <th>Net Pay</th>
                <th>Status</th>
                <th className="text-center">Actions</th>
              </tr>
            </thead>
            <tbody>
              {filteredPayrolls.length > 0 ? (
                filteredPayrolls.map((p, i) => (
                  <tr key={i}>
                    <td>{p?.id || "-"}</td>
                    <td>{p?.employeeName || "-"}</td>
                    <td>{p?.policyName || "-"}</td>
                    <td>
                      {p?.periodStart?.slice(0, 10)} - {p?.periodEnd?.slice(0, 10)}
                    </td>
                    <td>₹{p?.deductions?.toFixed(2) || "0.00"}</td>
                    <td>₹{p?.netPay?.toFixed(2) || "0.00"}</td>
                    <td>
                      <span className={`status-badge ${p?.statusName?.toLowerCase()}`}>
                        {p?.statusName || "Unknown"}
                      </span>
                    </td>
                    <td className="text-center">
                      {(isAdmin || isProcessor) && (
                        <button className="btn-verify" onClick={() => handleVerify(p.id)}>
                          Verify
                        </button>
                      )}
                      {isAdmin && (
                        <>
                          <button className="btn-approve" onClick={() => handleApprove(p.id)}>
                            Approve
                          </button>
                          <button className="btn-paid" onClick={() => handleMarkPaid(p.id)}>
                            Mark Paid
                          </button>
                        </>
                      )}
                      {isManager && (
                        <button
                          className="btn-view"
                          onClick={() => navigate(`/payrolls/view/${p.id}`)}
                        >
                          View
                        </button>
                      )}
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="8" className="no-data">
                    No payrolls found
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

export default AllPayrolls;

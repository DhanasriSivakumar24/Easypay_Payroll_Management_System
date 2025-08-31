import { useEffect, useState } from "react";
import { useSelector } from "react-redux"; 
import { useNavigate } from "react-router-dom";
import {
  GetAllPayrolls,
  VerifyPayroll,
  ApprovePayroll,
  MarkPayrollAsPaid,
} from "../../service/payroll.service";
import AdminLayout from "../Sidebar/AdminLayout";
import PayrollProcessorLayout from "../Sidebar/PayrollProcessorLayout";
import "./allPayrolls.css";

const AllPayrolls = () => {
  const [payrolls, setPayrolls] = useState([]);
  const [search, setSearch] = useState("");
  const navigate = useNavigate();

  const { role } = useSelector((state) => state.auth);
  const normalizedRole = role?.toLowerCase();

  const isAdmin = ["admin", "hrmanager", "hr manager"].includes(normalizedRole);
  const isProcessor = ["payrollprocessor", "payroll processor"].includes(normalizedRole);

  const Layout = isProcessor ? PayrollProcessorLayout : AdminLayout;

  useEffect(() => {
    if (isAdmin || isProcessor) {
      GetAllPayrolls()
        .then((res) => setPayrolls(res.data || []))
        .catch((err) => console.log("Error fetching payrolls:", err));
    }
  }, [isAdmin, isProcessor]);

  const refreshPayrolls = () => {
    GetAllPayrolls().then((res) => setPayrolls(res.data || []));
  };

  const handleVerify = (id) => {
    if (window.confirm("Verify this payroll?")) {
      VerifyPayroll(id).then(() => {
        alert("Payroll verified successfully!");
        refreshPayrolls();
      });
    }
  };

  const handleApprove = (id) => {
    if (window.confirm("Approve this payroll?")) {
      ApprovePayroll(id).then(() => {
        alert("Payroll approved successfully!");
        refreshPayrolls();
      });
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
        .catch(err => console.error("Error marking payroll as paid:", err));
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

  if (!isAdmin && !isProcessor) {
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

            <button
              className="add-btn"
              onClick={() => navigate("/payrolls/generate-payroll")}
            >
              + Generate Payroll
            </button>
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
                    <td>{p?.periodStart?.slice(0, 10)} - {p?.periodEnd?.slice(0, 10)}</td>
                    <td>₹{p?.deductions?.toFixed(2) || "0.00"}</td>
                    <td>₹{p?.netPay?.toFixed(2) || "0.00"}</td>
                    <td>
                      <span className={`status-badge ${p?.statusName?.toLowerCase()}`}>
                        {p?.statusName || "Unknown"}
                      </span>
                    </td>
                    <td className="text-center">

                      {(isAdmin || isProcessor) && (
                        <button
                          className="btn-verify"
                          onClick={() => handleVerify(p.id)}
                        >
                          Verify
                        </button>
                      )}

                      {isAdmin && (
                        <button
                          className="btn-approve"
                          onClick={() => handleApprove(p.id)}
                        >
                          Approve
                        </button>
                      )}

                      {isAdmin && (
                        <button
                          className="btn-paid"
                          onClick={() => handleMarkPaid(p.id)}
                        >
                          Mark Paid
                        </button>
                      )}
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="7" className="no-data">No payrolls found</td>
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

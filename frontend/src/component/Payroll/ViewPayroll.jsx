import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useSelector } from "react-redux";
import { GetAllPayrolls } from "../../service/payroll.service";
import AdminLayout from "../Sidebar/AdminLayout";
import PayrollProcessorLayout from "../Sidebar/PayrollProcessorLayout";
import ManagerLayout from "../Sidebar/ManagerLayout";
import "./viewPayroll.css";

const ViewPayroll = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [payroll, setPayroll] = useState(null);
  const { role } = useSelector((state) => state.auth);

  const normalizedRole = role?.toLowerCase();
  const Layout =
    normalizedRole  === "payrollprocessor"
      ? PayrollProcessorLayout
      : normalizedRole  === "manager"
      ? ManagerLayout
      : AdminLayout;

  useEffect(() => {
    const fetchPayroll = async () => {
      try {
        const data = (await GetAllPayrolls()).data || [];
        const selected = data.find((p) => p.id === parseInt(id));
        setPayroll(selected);
      } catch (err) {
        console.error("Failed to load payroll details:", err);
      }
    };
    fetchPayroll();
  }, [id]);

  if (!payroll) {
    return (
      <Layout>
        <div className="view-payroll-container">
          <div className="view-payroll-card text-center">
            <p>Loading payroll details...</p>
          </div>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="view-payroll-container">
        <div className="view-payroll-card">
          <h4 className="mb-4">Payroll Details</h4>

          <div className="payroll-info-grid">
            <div>
              <label>Employee</label>
              <p>{payroll.employeeName || "N/A"}</p>
            </div>
            <div>
              <label>Policy</label>
              <p>{payroll.policyName || "N/A"}</p>
            </div>
            <div>
              <label>Period</label>
              <p>
                {payroll.periodStart?.slice(0, 10)} -{" "}
                {payroll.periodEnd?.slice(0, 10)}
              </p>
            </div>
            <div>
              <label>Pay Date</label>
              <p>
                {payroll.paidDate?.slice(0, 10)}
              </p>
            </div>
            <div>
              <label>Status</label>
              <p>
                <span
                  className={` ${payroll.statusName?.toLowerCase()}`}
                >
                  {payroll.statusName}
                </span>
              </p>
            </div>
            <div>
              <label>Basic Pay</label>
              <p>₹{payroll.basicPay?.toFixed(2) || "0.00"}</p>
            </div>
            <div>
              <label>Deductions</label>
              <p>₹{payroll.deductions?.toFixed(2) || "0.00"}</p>
            </div>
            <div>
              <label>Net Pay</label>
              <p>₹{payroll.netPay?.toFixed(2) || "0.00"}</p>
            </div>
          </div>

          <div className="mt-4 text-end">
            <button className="btn btn-secondary" onClick={() => navigate(-1)}>
              Back
            </button>
          </div>
        </div>
      </div>
    </Layout>
  );
};

export default ViewPayroll;
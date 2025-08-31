import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useSelector, useDispatch } from "react-redux";
import { logout } from "../../authSlicer";
import { GetAllPayrolls } from "../../service/payroll.service";
import { GetAllEmployees } from "../../service/employee.service";
import "./payrollProcessorDashboard.css";
import PayrollProcessorLayout from "../Sidebar/PayrollProcessorLayout";

const PayrollProcessorDashboard = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { username } = useSelector((state) => state.auth);

  const [recentPayrolls, setRecentPayrolls] = useState([]);
  const [pendingPayrolls, setPendingPayrolls] = useState([]);
  const [totalProcessed, setTotalProcessed] = useState(0);
  const [employeeCount, setEmployeeCount] = useState(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    Promise.all([GetAllPayrolls(), GetAllEmployees()])
      .then(([payrollRes, empRes]) => {
        const payrolls = payrollRes.data || [];
        const employees = empRes.data || [];

        setEmployeeCount(employees.length);

        setRecentPayrolls(payrolls.slice(-5).reverse());

        const approved = payrolls.filter(
          (p) => p.statusName?.toLowerCase() === "approved"
        );
        setPendingPayrolls(approved);

        const processedCount = payrolls.filter(
          (p) =>
            p.statusName?.toLowerCase() === "paid" ||
            p.statusName?.toLowerCase() === "completed"
        ).length;
        setTotalProcessed(processedCount);
      })
      .catch(console.error)
      .finally(() => setLoading(false));
  }, []);

  const handleLogout = () => {
    sessionStorage.clear();
    dispatch(logout());
    navigate("/login");
  };

  if (loading) return <p className="loading">Loading dashboard...</p>;

  const quickActions = [
    {
      title: "Pay Payrolls",
      desc: "Mark approved payrolls as paid",
      path: "/payrolls/pay",
    },
    {
      title: "Download Reports",
      desc: "Export payroll reports",
      path: "/payrolls/reports",
    },
    {
      title: "View Notifications",
      desc: "View notifications about payments",
      path: "/notifications/processor-notifications",
    },
    {
      title: "All Employees",
      desc: "View employee details",
      path: "/employees",
    },
  ];

  return (
    <PayrollProcessorLayout>
      <div className="payroll-dashboard">

        <div className="topbar">
          <h1>Welcome back, {username || "Processor"} 👋</h1>
          <button className="logout-btn" onClick={handleLogout}>
            Logout
          </button>
        </div>

        <div className="cards">
          <div className="card pending-payrolls">
            <h3>Pending Payrolls ⏳</h3>
            <p>{pendingPayrolls.length}</p>
          </div>
          <div className="card processed-payrolls">
            <h3>Total Processed ✅</h3>
            <p>{totalProcessed}</p>
          </div>
          <div className="card employees">
            <h3>Total Employees 👥</h3>
            <p>{employeeCount}</p>
          </div>
        </div>

        <h2 className="section-title">Quick Actions</h2>
        <div className="quick-actions-container">
          {quickActions.map((action, idx) => (
            <div
              className="quick-action-card"
              key={idx}
              onClick={() => navigate(action.path)}
            >
              <h5>{action.title}</h5>
              <p>{action.desc}</p>
            </div>
          ))}
        </div>

        <div className="tables">
          <div className="table-section">
            <h2>Recent Payrolls</h2>
            {recentPayrolls.length === 0 ? (
              <p className="no-data">No payrolls found</p>
            ) : (
              <table>
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
                  </tr>
                </thead>
                <tbody>
                  {recentPayrolls.map((payroll, index) => (
                    <tr key={payroll.id}>
                      <td>{index + 1}</td>
                      <td>{payroll.employeeName}</td>
                      <td>{payroll.basicPay}</td>
                      <td>{payroll.allowances}</td>
                      <td>{payroll.deductions}</td>
                      <td>{payroll.netPay}</td>
                      <td>
                        {payroll.paidDate
                          ? new Date(payroll.paidDate).toLocaleDateString("en-IN")
                          : "-"}
                      </td>
                      <td>
                        <span
                          className={`status-badge ${
                            payroll.statusName?.toLowerCase()
                          }`}
                        >
                          {payroll.statusName}
                        </span>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            )}
          </div>
        </div>
      </div>
    </PayrollProcessorLayout>
  );
};

export default PayrollProcessorDashboard;

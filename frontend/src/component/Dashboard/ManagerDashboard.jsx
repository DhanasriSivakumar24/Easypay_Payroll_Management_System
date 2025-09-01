import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useSelector, useDispatch } from "react-redux";
import { logout } from "../../authSlicer";
import { GetAllPayrolls } from "../../service/payroll.service";
import { GetAllEmployees } from "../../service/employee.service";
import "./managerDashboard.css";
import ManagerLayout from "../Sidebar/ManagerLayout";

const ManagerDashboard = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { username } = useSelector((state) => state.auth);

  const [recentPayrolls, setRecentPayrolls] = useState([]);
  const [teamPayrolls, setTeamPayrolls] = useState([]);
  const [totalTeamMembers, setTotalTeamMembers] = useState(0);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    Promise.all([GetAllPayrolls(), GetAllEmployees()])
      .then(([payrollRes, empRes]) => {
        const payrolls = payrollRes.data || [];
        const employees = empRes.data || [];

        setTotalTeamMembers(employees.length);

        setRecentPayrolls(payrolls.slice(-5).reverse());

        setTeamPayrolls(payrolls.filter(p => p.statusName?.toLowerCase() !== "draft"));
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
      title: "Team Payrolls",
      desc: "View payrolls for your team",
      path: "/payrolls",
    },
    {
      title: "Approve Leaves",
      desc: "Approve pending leave requests",
      path: "/leaves",
    },
    {
      title: "Download Reports",
      desc: "Export team payroll and leave reports",
      path: "/payrolls/reports",
    },
    {
      title: "All Employees",
      desc: "View details of your team",
      path: "/employees",
    },
  ];

  return (
    <ManagerLayout>
      <div className="manager-dashboard">

        <div className="topbar">
          <h1>Welcome back, {username || "Manager"} 👋</h1>
          <button className="logout-btn" onClick={handleLogout}>
            Logout
          </button>
        </div>

        <div className="cards">
          <div className="card team-members">
            <h3>Total Team Members 👥</h3>
            <p>{totalTeamMembers}</p>
          </div>
          <div className="card team-payrolls">
            <h3>Team Payrolls 📄</h3>
            <p>{teamPayrolls.length}</p>
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
    </ManagerLayout>
  );
};

export default ManagerDashboard;

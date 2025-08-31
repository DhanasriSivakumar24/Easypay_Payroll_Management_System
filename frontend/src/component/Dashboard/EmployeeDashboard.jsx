import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { GetLeaveRequestsByEmployee } from "../../service/leave.service"; 
import { GetPayrollByEmployeeId } from "../../service/payroll.service"; 
import Sidebar from "../navbar/Sidebar";
import "./employeeDashboard.css";

const EmployeeDashboard = () => {
  const navigate = useNavigate();
  const [employeeName, setEmployeeName] = useState("");
  const [employeeId, setEmployeeId] = useState(null);
  const [role, setRole] = useState("");
  const [totalLeaves, setTotalLeaves] = useState(0);
  const [pendingLeaves, setPendingLeaves] = useState(0);
  const [latestPayroll, setLatestPayroll] = useState(null);
  const [recentLeaves, setRecentLeaves] = useState([]);
  const [recentPayrolls, setRecentPayrolls] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const username = sessionStorage.getItem("username") || "Employee";
    const empId = sessionStorage.getItem("employeeId");
    const userRole = sessionStorage.getItem("role");

    setEmployeeName(username);
    setEmployeeId(empId);
    setRole(userRole);

    if (!empId || userRole !== "Employee") {
      navigate("/login");
      return;
    }

    const loadDashboardData = async () => {
      try {
        const leavesRes = await GetLeaveRequestsByEmployee(empId); 
        const leavesData = leavesRes?.data || [];
        setTotalLeaves(leavesData.length);
        setPendingLeaves(
          leavesData.filter((l) => l.statusName?.toLowerCase() === "pending").length
        );
        setRecentLeaves(leavesData.slice(-5).reverse());

        const payrollsRes = await GetPayrollByEmployeeId(empId); 
        const payrollsData = payrollsRes?.data || [];
        const latest = payrollsData.sort((a, b) => b.id - a.id)[0];
        setLatestPayroll(latest || null);
        setRecentPayrolls(payrollsData.slice(-5).reverse());
      } catch (err) {
        console.error("Error loading dashboard:", err);
      } finally {
        setLoading(false);
      }
    };

    loadDashboardData();
  }, [navigate]);

  const handleLogout = () => {
    sessionStorage.clear();
    window.location.href = "/login";
  };

  if (loading) return <p>Loading dashboard...</p>;

  const quickActions = [
    { title: "Apply Leave", desc: "Request for leave easily", path: "/leave-requests/leaves/apply" },
    { title: "View My Leaves", desc: "Check your leave history", path: "/leave-requests" },
    { title: "View Payrolls", desc: "Check your salary slips", path: "/paystubs" },
    { title: "View Benefits", desc: "See your enrolled benefits", path: "/myEnrolledBenefit" },
  ];

  return (
    <div className="employee-dashboard">
      <Sidebar role="Employee" userName={employeeName} onLogout={handleLogout} />

      <main className="main-content">
        <div className="topbar">
          <h1>Welcome, {employeeName} 👋</h1>
          <button className="logout-btn" onClick={handleLogout}>Logout</button>
        </div>

        <div className="cards">
          <div className="card leaves">
            <h3>Total Leaves 🗂</h3>
            <p>{totalLeaves}</p>
          </div>
          <div className="card pending">
            <h3>Pending Leaves 📥</h3>
            <p>{pendingLeaves}</p>
          </div>
          <div className="card payroll">
            <h3>Latest Payroll 💵</h3>
            <p>
              {latestPayroll 
                ? `Net Pay: ${latestPayroll.netPay}` 
                : "No payroll yet"}
            </p>
          </div>
        </div>

        <h2 className="mb-3 mt-4">Quick Actions</h2>
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
            <h2>Recent Leave Requests</h2>
            <table>
              <thead>
                <tr>
                  <th>S.No</th>
                  <th>Type</th>
                  <th>Start Date</th>
                  <th>End Date</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                {recentLeaves.length > 0 ? (
                  recentLeaves.map((leave, index) => (
                    <tr key={leave.id}>
                      <td>{index + 1}</td>
                      <td>{leave.leaveTypeName}</td>
                      <td>{new Date(leave.startDate).toLocaleDateString()}</td>
                      <td>{new Date(leave.endDate).toLocaleDateString()}</td>
                      <td>{leave.statusName}</td>
                    </tr>
                  ))
                ) : (
                  <tr><td colSpan="5">No leave requests found</td></tr>
                )}
              </tbody>
            </table>
          </div>

          <div className="table-section">
            <h2>Recent Payrolls</h2>
            <table>
              <thead>
                <tr>
                  <th>S.No</th>
                  <th>Status</th>
                  <th>Basic Pay</th>
                  <th>Allowances</th>
                  <th>Deductions</th>
                  <th>Net Pay</th>
                </tr>
              </thead>
              <tbody>
                {recentPayrolls.length > 0 ? (
                  recentPayrolls.map((payroll, index) => (
                    <tr key={payroll.id}>
                      <td>{index + 1}</td>
                      <td>{payroll.statusName}</td>
                      <td>{payroll.basicPay}</td>
                      <td>{payroll.allowances}</td>
                      <td>{payroll.deductions}</td>
                      <td>{payroll.netPay}</td>
                    </tr>
                  ))
                ) : (
                  <tr><td colSpan="6">No payroll records found</td></tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      </main>
    </div>
  );
};

export default EmployeeDashboard;

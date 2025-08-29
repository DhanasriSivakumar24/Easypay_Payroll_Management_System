import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useSelector, useDispatch } from "react-redux";
import { logout } from "../../authSlicer";   //  redux logout
import { GetAllEmployees } from "../../service/employee.service";
import { GetAllLeaveRequests } from "../../service/leave.service";
import { GetAllPayrolls } from "../../service/payroll.service";
import Sidebar from "../navbar/Sidebar"; 
import './adminDashboard.css';

const AdminDashboard = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const { username, role } = useSelector((state) => state.auth);

  const [totalEmployees, setTotalEmployees] = useState(0);
  const [pendingLeaves, setPendingLeaves] = useState(0);
  const [latestPayroll, setLatestPayroll] = useState(null);
  const [totalDeductions, setTotalDeductions] = useState(0);
  const [recentLeaves, setRecentLeaves] = useState([]);
  const [recentPayrolls, setRecentPayrolls] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    GetAllEmployees()
      .then((res) => {
        if (res.data) {
          setTotalEmployees(res.data.length);
        }
      })
      .catch((err) => console.error("Error fetching employees:", err));

    GetAllLeaveRequests()
      .then((res) => {
        if (res.data) {
          const leavesData = res.data;
          setPendingLeaves(leavesData.filter(l => l.statusName.toLowerCase() === "pending").length);
          setRecentLeaves(leavesData.slice(-5).reverse());
        }
      })
      .catch((err) => console.error("Error fetching leaves:", err));

    GetAllPayrolls()
      .then((res) => {
        if (res.data) {
          const payrollsData = res.data;
          const latest = payrollsData.sort((a, b) => b.id - a.id)[0];
          setLatestPayroll(latest);
          if (latest && latest.deductions !== undefined) setTotalDeductions(latest.deductions);
          setRecentPayrolls(payrollsData.slice(-5).reverse());
        }
      })
      .catch((err) => console.error("Error fetching payrolls:", err))
      .finally(() => setLoading(false));
  }, []);

  const handleLogout = () => {
    sessionStorage.clear(); 
    dispatch(logout());     
    navigate("/login");
  };

  if (loading) return <p>Loading dashboard...</p>;

  const quickActions = [
    { title: "Add Employee", desc: "Quickly add a new employee", path: "/employees/add-employee" },
    { title: "Update Employee", desc: "Edit existing employee details", path: "/employees" },
    { title: "Approve Leave", desc: "Approve pending leave requests", path: "/leaves" },
    { title: "Generate Payroll", desc: "Process payroll for employees", path: "/payrolls/generate-payroll" },
    { title: "Enroll Benefits", desc: "Add employee benefits", path: "/benefits-management/enroll" },
  ];

  return (
    <div className="admin-dashboard">
      <Sidebar role={role || "Admin"} userName={username || "Admin"} onLogout={handleLogout} />

      <main className="main-content">
        <div className="topbar">
          <h1>Welcome back, {username || "Admin"} 👋</h1>
          <button className="logout-btn" onClick={handleLogout}>Logout</button>
        </div>

        <div className="cards">
          <div className="card employees">
            <h3>Total Employees 👥</h3>
            <p>{totalEmployees}</p>
          </div>
          <div className="card leaves">
            <h3>Pending Leaves 📥</h3>
            <p>{pendingLeaves}</p>
          </div>
          <div className="card payroll">
            <h3>Latest Payroll 💵</h3>
            <p>{latestPayroll ? `#${latestPayroll.id} (${latestPayroll.statusName})` : "No payroll"}</p>
          </div>
          <div className="card deductions">
            <h3>Total Deductions 📈</h3>
            <p>{totalDeductions}</p>
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
                  <th>Employee</th>
                  <th>Type</th>
                  <th>Status</th>
                  <th>Start Date</th>
                  <th>End Date</th>
                </tr>
              </thead>
              <tbody>
                {recentLeaves.map((leave, index) => (
                  <tr key={leave.id}>
                    <td>{index + 1}</td>
                    <td>{leave.employeeName}</td>
                    <td>{leave.leaveTypeName}</td>
                    <td>{leave.statusName}</td>
                    <td>{new Date(leave.startDate).toLocaleDateString()}</td>
                    <td>{new Date(leave.endDate).toLocaleDateString()}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <div className="table-section">
            <h2>Recent Payrolls</h2>
            <table>
              <thead>
                <tr>
                  <th>S.No</th>
                  <th>Employee</th>
                  <th>Status</th>
                  <th>Basic Pay</th>
                  <th>Allowances</th>
                  <th>Deductions</th>
                  <th>Net Pay</th>
                </tr>
              </thead>
              <tbody>
                {recentPayrolls.map((payroll, index) => (
                  <tr key={payroll.id}>
                    <td>{index + 1}</td>
                    <td>{payroll.employeeName}</td>
                    <td>{payroll.statusName}</td>
                    <td>{payroll.basicPay}</td>
                    <td>{payroll.allowances}</td>
                    <td>{payroll.deductions}</td>
                    <td>{payroll.netPay}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </main>
    </div>
  );
};

export default AdminDashboard;

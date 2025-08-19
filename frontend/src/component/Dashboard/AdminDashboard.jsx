// import { useEffect, useState } from "react";
// import { GetAllEmployees } from "../../service/employee.service";
// import { GetAllLeaveRequests } from "../../service/leave.service";
// import { GetAllPayrolls } from "../../service/payroll.service";
// import Sidebar from "../navbar/Sidebar"; 
// import './adminDashboard.css';

// const AdminDashboard = () => {
//   const [adminName, setAdminName] = useState("");
//   const [totalEmployees, setTotalEmployees] = useState(0);
//   const [pendingLeaves, setPendingLeaves] = useState(0);
//   const [latestPayroll, setLatestPayroll] = useState(null);
//   const [totalDeductions, setTotalDeductions] = useState(0);
//   const [recentLeaves, setRecentLeaves] = useState([]);
//   const [recentPayrolls, setRecentPayrolls] = useState([]);
//   const [loading, setLoading] = useState(true);

//   useEffect(() => {
//     const username = sessionStorage.getItem("username") || "Admin";
//     setAdminName(username);

//     const loadDashboardData = async () => {
//       try {
//         const employeesRes = await GetAllEmployees();
//         setTotalEmployees(employeesRes.data.length);

//         const leavesRes = await GetAllLeaveRequests();
//         const leavesData = leavesRes.data;
//         setPendingLeaves(leavesData.filter(l => l.statusName.toLowerCase() === "pending").length);
//         setRecentLeaves(leavesData.slice(-5).reverse());

//         const payrollsRes = await GetAllPayrolls();
//         const payrollsData = payrollsRes.data;
//         const latest = payrollsData.sort((a, b) => b.id - a.id)[0];
//         setLatestPayroll(latest);
//         if (latest && latest.deductions !== undefined) setTotalDeductions(latest.deductions);
//         setRecentPayrolls(payrollsData.slice(-5).reverse());

//       } catch (err) {
//         console.error(err);
//       } finally {
//         setLoading(false);
//       }
//     };

//     loadDashboardData();
//   }, []);

//   const handleLogout = () => {
//     sessionStorage.clear();
//     window.location.href = "/login";
//   };

//   if (loading) return <p>Loading dashboard...</p>;

//   return (
//     <div className="admin-dashboard">
//       <Sidebar role="Admin" userName={adminName} onLogout={handleLogout} />

//       <main className="main-content">
//         <div className="topbar">
//           <h1>Welcome back, {adminName} 👋</h1>
//           <button className="logout-btn" onClick={handleLogout}>Logout</button>
//         </div>

//         {/* Cards */}
//         <div className="cards">
//           <div className="card employees">
//             <h3>Total Employees</h3>
//             <p>{totalEmployees}</p>
//           </div>
//           <div className="card leaves">
//             <h3>Pending Leaves</h3>
//             <p>{pendingLeaves}</p>
//           </div>
//           <div className="card payroll">
//             <h3>Latest Payroll</h3>
//             <p>{latestPayroll ? `#${latestPayroll.id} (${latestPayroll.statusName})` : "No payroll"}</p>
//           </div>
//           <div className="card deductions">
//             <h3>Total Deductions</h3>
//             <p>{totalDeductions}</p>
//           </div>
//         </div>

//         {/* Tables */}
//         <div className="tables">
//           <div className="table-section">
//             <h2>Recent Leave Requests</h2>
//             <table>
//               <thead>
//                 <tr>
//                     <th>S.No</th>
//                     <th>Employee</th>
//                     <th>Type</th>
//                     <th>Status</th>
//                     <th>Start Date</th>
//                     <th>End Date</th>
//                     </tr>
//                 </thead>
//                     <tbody>
//                         {recentLeaves.map((leave, index) => (
//                             <tr key={leave.id}>
//                             <td>{index + 1}</td>  {/* Serial Number */}
//                             <td>{leave.employeeName}</td>
//                             <td>{leave.leaveTypeName}</td>
//                             <td>{leave.statusName}</td>
//                             <td>{new Date(leave.startDate).toLocaleDateString()}</td>
//                             <td>{new Date(leave.endDate).toLocaleDateString()}</td>
//                             </tr>
//                         ))}
//                 </tbody>
//             </table>
//           </div>

//           <div className="table-section">
//             <h2>Recent Payrolls</h2>
//             <table>
//               <thead>
//                 <tr>
//                     <th>S.No</th>
//                   <th>Employee</th>
//                   <th>Status</th>
//                   <th>Basic Pay</th>
//                   <th>Allowances</th>
//                   <th>Deductions</th>
//                   <th>Net Pay</th>
//                 </tr>
//               </thead>
//               <tbody>
//                 {recentPayrolls.map((payroll, index) => (
//                     <tr key={payroll.id}>
//                     <td>{index + 1}</td>  {/* Serial Number */}
//                     <td>{payroll.employeeName}</td>
//                     <td>{payroll.statusName}</td>
//                     <td>{payroll.basicPay}</td>
//                     <td>{payroll.allowances}</td>
//                     <td>{payroll.deductions}</td>
//                     <td>{payroll.netPay}</td>
//                     </tr>
//                 ))}
//                 </tbody>
//             </table>
//           </div>
//         </div>
//       </main>
//     </div>
//   );
// };

// export default AdminDashboard;

import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { GetAllEmployees } from "../../service/employee.service";
import { GetAllLeaveRequests } from "../../service/leave.service";
import { GetAllPayrolls } from "../../service/payroll.service";
import Sidebar from "../navbar/Sidebar"; 
import './adminDashboard.css';

const AdminDashboard = () => {
  const navigate = useNavigate();
  const [adminName, setAdminName] = useState("");
  const [totalEmployees, setTotalEmployees] = useState(0);
  const [pendingLeaves, setPendingLeaves] = useState(0);
  const [latestPayroll, setLatestPayroll] = useState(null);
  const [totalDeductions, setTotalDeductions] = useState(0);
  const [recentLeaves, setRecentLeaves] = useState([]);
  const [recentPayrolls, setRecentPayrolls] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const username = sessionStorage.getItem("username") || "Admin";
    setAdminName(username);

    const loadDashboardData = async () => {
      try {
        const employeesRes = await GetAllEmployees();
        setTotalEmployees(employeesRes.data.length);

        const leavesRes = await GetAllLeaveRequests();
        const leavesData = leavesRes.data;
        setPendingLeaves(leavesData.filter(l => l.statusName.toLowerCase() === "pending").length);
        setRecentLeaves(leavesData.slice(-5).reverse());

        const payrollsRes = await GetAllPayrolls();
        const payrollsData = payrollsRes.data;
        const latest = payrollsData.sort((a, b) => b.id - a.id)[0];
        setLatestPayroll(latest);
        if (latest && latest.deductions !== undefined) setTotalDeductions(latest.deductions);
        setRecentPayrolls(payrollsData.slice(-5).reverse());

      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    loadDashboardData();
  }, []);

  const handleLogout = () => {
    sessionStorage.clear();
    window.location.href = "/login";
  };

  if (loading) return <p>Loading dashboard...</p>;

  // Quick action cards data
  const quickActions = [
    { title: "Add Employee", desc: "Quickly add a new employee", path: "/add-employee" },
    { title: "Update Employee", desc: "Edit existing employee details", path: "/employees" },
    { title: "Approve Leave", desc: "Approve pending leave requests", path: "/leaves" },
    { title: "Generate Payroll", desc: "Process payroll for employees", path: "/payroll" },
    { title: "Enroll Benefits", desc: "Add employee benefits", path: "/policies" },
  ];

  return (
    <div className="admin-dashboard">
      <Sidebar role="Admin" userName={adminName} onLogout={handleLogout} />

      <main className="main-content">
        <div className="topbar">
          <h1>Welcome back, {adminName} 👋</h1>
          <button className="logout-btn" onClick={handleLogout}>Logout</button>
        </div>

        {/* Cards */}
        <div className="cards">
          <div className="card employees">
            <h3>Total Employees</h3>
            <p>{totalEmployees}</p>
          </div>
          <div className="card leaves">
            <h3>Pending Leaves</h3>
            <p>{pendingLeaves}</p>
          </div>
          <div className="card payroll">
            <h3>Latest Payroll</h3>
            <p>{latestPayroll ? `#${latestPayroll.id} (${latestPayroll.statusName})` : "No payroll"}</p>
          </div>
          <div className="card deductions">
            <h3>Total Deductions</h3>
            <p>{totalDeductions}</p>
          </div>
        </div>

        {/* Quick Actions */}
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

        {/* Tables */}
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

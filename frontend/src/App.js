import logo from './logo.svg';
import './App.css';
import Login from './component/login/login';
import Employees from './component/Employees/employees';
import 'bootstrap/dist/css/bootstrap.css';
import { BrowserRouter, Route, Routes} from 'react-router-dom';
import PersonalInfo from './component/Employees/PersonalInfo';
import AllEmployees from './component/Employees/AllEmployees';
import AdminDashboard from './component/Dashboard/AdminDashboard';
import AddEmployee from './component/Employees/AddEmployee';
import UpdateEmployeeDetail from './component/Employees/UpdateEmployee';
import ProtectedRoute from './component/ProtectedRoute';
import EmployeeDashboard from './component/Dashboard/EmployeeDashboard';
import ApplyLeave from './component/LeaveRequest/ApplyLeave';
import LeaveRequests from './component/LeaveRequest/LeaveRequests';
import ApplyTimesheet from './component/Timesheet/ApplyTimesheet';
import TimesheetHistory from './component/Timesheet/TimesheetHistory';
import ViewNotification from './component/Notification/ViewNotification';
import UserManagement from './component/Employees/UserManagement';
import AuditTrail from './component/AuditTrail/AuditTrail';
import BenefitEnrollment from './component/Benefits/BenefitEnrollment';
import AllBenefitEnrollments from './component/Benefits/AllBenefitEnrollments';
import ViewBenefitEnrollment from './component/Benefits/ViewBenefitEnrollment';
import SendNotification from './component/Notification/SendNotification';
import AllPayrolls from './component/Payroll/AllPayrolls';
import GeneratePayrollPage from './component/Payroll/GeneratePayroll';
import ComplianceReport from './component/Payroll/ComplianceReport';
import ApproveLeaveRequests from './component/LeaveRequest/ApproveLeaveRequests';
import MyBenefitEnrollments from './component/Benefits/MyBenefitEnrollments';

function App() {
  return (
    <div className="App">
      <BrowserRouter>
            <Routes >
                <Route path= '/' element= {<Login/>}/>
                <Route path= '/login' element = {<Login/>}/>
                
                <Route path="/admin-dashboard" element={<ProtectedRoute><AdminDashboard/></ProtectedRoute>} />
                <Route path="/employees/add-employee" element={<ProtectedRoute><AddEmployee /></ProtectedRoute>} />
                <Route path="/employees/update-employee/:id" element={<ProtectedRoute><UpdateEmployeeDetail /></ProtectedRoute>} />
                <Route path='/employees' element={<ProtectedRoute><AllEmployees/></ProtectedRoute>} />
                <Route path='/user-management' element={<ProtectedRoute><UserManagement/></ProtectedRoute>} />
                <Route path='/audit' element={<ProtectedRoute><AuditTrail/></ProtectedRoute>} />
                <Route path='/benefits-management' element={<ProtectedRoute><AllBenefitEnrollments/></ProtectedRoute>} />
                <Route path='/benefits-management/enroll' element={<ProtectedRoute><BenefitEnrollment/></ProtectedRoute>} />
                <Route path="/benefits/view/:id" element ={<ProtectedRoute><ViewBenefitEnrollment/></ProtectedRoute>}/>
                <Route path='/notifications/admin-notifications' element={<ProtectedRoute><SendNotification/></ProtectedRoute>}/>
                <Route path='/payrolls' element={<ProtectedRoute><AllPayrolls/></ProtectedRoute>}/>
                <Route path='/payrolls/generate-payroll' element={<ProtectedRoute><GeneratePayrollPage/></ProtectedRoute>}/>
                <Route path='/compliance' element={<ProtectedRoute><ComplianceReport/></ProtectedRoute>}/>
                <Route path='/leaves' element={<ProtectedRoute><ApproveLeaveRequests/></ProtectedRoute>}/>
                
                <Route path="/employee-dashboard" element={<ProtectedRoute><EmployeeDashboard /></ProtectedRoute>}/>
                <Route path="/leave-requests/leaves/apply" element={<ApplyLeave />} />
                <Route path="/leave-requests" element={<LeaveRequests />} />
                <Route path='/personal-info' element={<ProtectedRoute><PersonalInfo/></ProtectedRoute>}/>
                <Route path='/timesheets' element={<ProtectedRoute><TimesheetHistory/></ProtectedRoute>}/>
                <Route path='/timesheets/submit-timesheet' element={<ProtectedRoute><ApplyTimesheet/></ProtectedRoute>}/>
                <Route path='/notifications/view-notifications' element={<ProtectedRoute><ViewNotification/></ProtectedRoute>}/>
                <Route path= '/employees/SearchEmployee' element ={<Employees/>}/>
                <Route path='/myEnrolledBenefit' element={<ProtectedRoute><MyBenefitEnrollments/></ProtectedRoute>}/>
            </Routes>
      </BrowserRouter>{/* //routing related things should be here in the broweseroute */}
    </div>
  );
}

export default App;

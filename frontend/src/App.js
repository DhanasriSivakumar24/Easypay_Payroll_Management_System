import Login from './component/login/login';
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
import MyPayStubs from './component/Payroll/MyPayStubs';
import PayrollProcessorDashboard from './component/Dashboard/PayrollProcessorDashboard';
import PayrollReport from './component/Payroll/PayrollReport';
import PendingPayroll from './component/Payroll/PendingPayroll';
import { GeneratePayroll } from './service/payroll.service';
import ManagerDashboard from './component/Dashboard/ManagerDashboard';
import ViewEmployee from './component/Employees/ViewEmployee';
import EditBenefitEnrollment from './component/Benefits/EditBenefitEnrollment';
import ViewPayroll from './component/Payroll/ViewPayroll';
import ChangeUserRole from './component/Employees/ChangeUserRole';
import LandingPage from './component/Landing Page/LandingPage';

function App() {
  return (
    <div className="App">
      <BrowserRouter>
            <Routes >
                <Route path= '/' element= {<LandingPage/>}/>
                <Route path= '/login' element = {<Login/>}/>
                
                <Route path="/admin-dashboard" element={<ProtectedRoute><AdminDashboard/></ProtectedRoute>} />
                <Route path="/employees/add-employee" element={<ProtectedRoute><AddEmployee /></ProtectedRoute>} />
                <Route path="/employees/update-employee/:id" element={<ProtectedRoute><UpdateEmployeeDetail /></ProtectedRoute>} />
                <Route path='/employees' element={<ProtectedRoute><AllEmployees/></ProtectedRoute>} />
                <Route path='/user-management' element={<ProtectedRoute><UserManagement/></ProtectedRoute>} />                
                <Route path='/user-management/change-user-role' element={<ProtectedRoute><ChangeUserRole/></ProtectedRoute>} />
                <Route path='/audit' element={<ProtectedRoute><AuditTrail/></ProtectedRoute>} />
                <Route path='/benefits-management' element={<ProtectedRoute><AllBenefitEnrollments/></ProtectedRoute>} />
                <Route path='/benefits-management/enroll' element={<ProtectedRoute><BenefitEnrollment/></ProtectedRoute>} />
                <Route path="/benefits-management/view/:id" element ={<ProtectedRoute><ViewBenefitEnrollment/></ProtectedRoute>}/>
                <Route path='/benefits-management/edit/:id' element={<ProtectedRoute><EditBenefitEnrollment/></ProtectedRoute>}/>
                <Route path='/notifications/admin-notifications' element={<ProtectedRoute><SendNotification/></ProtectedRoute>}/>
                <Route path='/payrolls' element={<ProtectedRoute><AllPayrolls/></ProtectedRoute>}/>
                <Route path='/payrolls/view/:id' element={<ProtectedRoute><ViewPayroll/></ProtectedRoute>}/>
                <Route path='/payrolls/generate-payroll' element={<ProtectedRoute><GeneratePayrollPage/></ProtectedRoute>}/>
                <Route path='/compliance' element={<ProtectedRoute><ComplianceReport/></ProtectedRoute>}/>
                <Route path='/leaves' element={<ProtectedRoute><ApproveLeaveRequests/></ProtectedRoute>}/>
                <Route path='/employees/view-employee/:id' element={<ProtectedRoute><ViewEmployee/></ProtectedRoute>}/>
                
                <Route path="/employee-dashboard" element={<ProtectedRoute><EmployeeDashboard /></ProtectedRoute>}/>
                <Route path="/leave-requests/leaves/apply" element={<ApplyLeave />} />
                <Route path="/leave-requests" element={<LeaveRequests />} />
                <Route path='/personal-info' element={<ProtectedRoute><PersonalInfo/></ProtectedRoute>}/>
                <Route path='/timesheets' element={<ProtectedRoute><TimesheetHistory/></ProtectedRoute>}/>
                <Route path='/timesheets/submit-timesheet' element={<ProtectedRoute><ApplyTimesheet/></ProtectedRoute>}/>
                <Route path='/notifications/view-notifications' element={<ProtectedRoute><ViewNotification/></ProtectedRoute>}/>
                <Route path='/myEnrolledBenefit' element={<ProtectedRoute><MyBenefitEnrollments/></ProtectedRoute>}/>
                <Route path='/paystubs' element={<ProtectedRoute><MyPayStubs/></ProtectedRoute>}/>

                <Route path='/processor-dashboard' element={<ProtectedRoute><PayrollProcessorDashboard/></ProtectedRoute>}/>
                <Route path='/notifications/processor-notifications' element={<ProtectedRoute><ViewNotification/></ProtectedRoute>}/>
                <Route path='/payrolls/reports' element={<ProtectedRoute><PayrollReport/></ProtectedRoute>}/>
                <Route path='/payrolls/pay' element={<ProtectedRoute><PendingPayroll/></ProtectedRoute>}/>
                <Route path='/payrolls/generate-payroll' element={<ProtectedRoute><GeneratePayroll/></ProtectedRoute>}/>
                <Route path='/payrolls-verification' element={<ProtectedRoute><AllPayrolls/></ProtectedRoute>}/>
                
                <Route path='/manager-dashboard' element={<ProtectedRoute><ManagerDashboard/></ProtectedRoute>}/>
                <Route path='/notifications/manager-notifications' element={<ProtectedRoute><ViewNotification/></ProtectedRoute>}/>
            </Routes>
      </BrowserRouter>{/* //routing related things should be here in the broweseroute */}
    </div>
  );
}

export default App;

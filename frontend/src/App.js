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
                
                <Route path="/employee-dashboard" element={<ProtectedRoute><EmployeeDashboard /></ProtectedRoute>}/>
                <Route path="/leave-requests/leaves/apply" element={<ApplyLeave />} />
                <Route path="/leave-requests" element={<LeaveRequests />} />
                <Route path='/personal-info' element={<ProtectedRoute><PersonalInfo/></ProtectedRoute>}/>
                <Route path='/timesheets' element={<ProtectedRoute><TimesheetHistory/></ProtectedRoute>}/>
                <Route path='/timesheets/submit-timesheet' element={<ProtectedRoute><ApplyTimesheet/></ProtectedRoute>}/>
                <Route path='/notifications/view-notifications' element={<ProtectedRoute><ViewNotification/></ProtectedRoute>}/>
                <Route path= '/employees/SearchEmployee' element ={<Employees/>}/>
            </Routes>
      </BrowserRouter>{/* //routing related things should be here in the broweseroute */}
    </div>
  );
}

export default App;

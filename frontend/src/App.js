import logo from './logo.svg';
import './App.css';
import Login from './component/login/login';
import Employees from './component/Employees/employees';
import 'bootstrap/dist/css/bootstrap.css';
import Navbar from './component/navbar/navbar';
import { BrowserRouter, Route, Routes} from 'react-router-dom';
import Home from './component/home/home';
import PersonalInfo from './component/Employees/PersonalInfo';
import AllEmployees from './component/Employees/AllEmployees';

function App() {
  return (
    <div className="App">
      <BrowserRouter>
            <Navbar/>
            <Routes>
                <Route path= '/' element= {<Home/>}/>
                <Route path= '/login' element = {<Login/>}/>
                <Route path= '/employee/SearchEmployee' element ={<Employees/>}/>
                <Route path='/employee/personal-info' element={<PersonalInfo/>}/>
                <Route path='/employee/AllEmployees' element={<AllEmployees/>} />
            </Routes>
      </BrowserRouter>{/* //routing related things should be here in the broweseroute */}
    </div>
  );
}

export default App;

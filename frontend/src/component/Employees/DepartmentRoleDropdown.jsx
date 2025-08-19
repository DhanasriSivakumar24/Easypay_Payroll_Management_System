import { useEffect, useState } from "react";
import { GetDepartments,GetRoles } from "../../service/masterTable.service";

const DepartmentRoleDropdown = ({ department, setDepartment, role, setRole }) => {
  const [departments, setDepartments] = useState([]);
  const [roles, setRoles] = useState([]);

  useEffect(() => {
    GetDepartments().then(res => setDepartments(res.data)).catch(err => console.log(err));
    GetRoles().then(res => setRoles(res.data)).catch(err => console.log(err));
  }, []);

  return (
    <div className="row g-3 mb-3">
      <div className="col-md-6">
        <label htmlFor="department" className="form-label">Department</label>
        <select 
          className="form-select" 
          id="department"
          value={department}
          onChange={(e) => setDepartment(e.target.value)}
        >
          <option value="">Select Department</option>
          {departments.map(dep => <option key={dep.id} value={dep.id}>{dep.departmentName}</option>)}
        </select>
      </div>
      <div className="col-md-6">
        <label htmlFor="role" className="form-label">Role</label>
        <select 
          className="form-select" 
          id="role"
          value={role}
          onChange={(e) => setRole(e.target.value)}
        >
          <option value="">Select Role</option>
          {roles.map(r => <option key={r.id} value={r.id}>{r.roleName}</option>)}
        </select>
      </div>
    </div>
  );
};

export default DepartmentRoleDropdown;

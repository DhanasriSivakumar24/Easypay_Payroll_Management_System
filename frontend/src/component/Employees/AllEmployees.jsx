import { useEffect, useState } from "react";
import { GetAllEmployees } from "../../service/employee.service";

const AllEmployees = () => {
  const [employees, setEmployees] = useState([]);
  const [error, setError] = useState("");

  useEffect(() => {
    GetAllEmployees()
      .then((result) => {
        //console.log(result.data); // check backend response
        setEmployees(result.data ?? []);
      })
      .catch((err) => {
        console.error(err);
        setError("Failed to fetch employees");
      });
  }, []);

  return (
    <>
      {employees.length === 0 && !error && <div>Loading employees...</div>}

      {employees.length > 0 && (
        <div className="container mt-4">
          <h2 className="text-center mb-4">All Employees</h2>
          {employees.map((info) => (
            <div key={info.id} className="card mb-3 p-3">
              <p><strong>Employee ID:</strong> {info.id}</p>
              <p><strong>Name:</strong> {info.firstName} {info.lastName}</p>
              <p><strong>Email:</strong> {info.email}</p>
              <p><strong>Phone:</strong> {info.phoneNumber}</p>
              <p><strong>Department:</strong> {info.departmentName}</p>
              <p><strong>Role:</strong> {info.roleName}</p>
              <p><strong>User Role:</strong> {info.userRoleName}</p>
              <p><strong>Status:</strong> {info.statusName}</p>
              <p><strong>Reporting Manager:</strong> {info.reportingManagerName ?? "N/A"}</p>
              <p><strong>Salary:</strong> {info.salary}</p>
            </div>
          ))}
        </div>
      )}
    </>
  );
};

export default AllEmployees;

import { useEffect, useState } from "react";
import { GetPersonalInfo } from "../../service/employee.service";

const PersonalInfo = () => {
    const [info, setInfo] = useState("");

    useEffect(() => {
        const employeeId = sessionStorage.getItem("employeeId"); // saved at login
        if (!employeeId) {
            console.error("No employeeId found");
            return;
        }

        GetPersonalInfo(employeeId)
            .then((result) => {
                console.log(result.data);
                setInfo(result.data);
            })
            .catch((err) => {
                console.error(err);
            });
    }, []);

  return (
    <>
    {
      info.length===0 && (<div>Loading...</div>)
    }
    {
      <div className="container mt-4">
      <h2 className="text-center mb-4">Personal Information</h2>
        <div>
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
      </div>
    }
    </>);
};

export default PersonalInfo;
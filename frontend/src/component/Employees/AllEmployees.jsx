// import { useEffect, useState } from "react";
// import { GetAllEmployees } from "../../service/employee.service";

// import "./AllEmployees.css";
// import AdminLayout from "../navbar/AdminLayout";

// const AllEmployees = () => {
//   const [employees, setEmployees] = useState([]);
//   const [search, setSearch] = useState("");

//   useEffect(() => {
//     GetAllEmployees()
//       .then((res) => setEmployees(res.data))
//       .catch((err) => console.log(err));
//   }, []);

//   const filteredEmployees = employees.filter((emp) => {
//     const name = emp?.firstName ? emp.firstName.toLowerCase() : "";
//     const email = emp?.email ? emp.email.toLowerCase() : "";
//     const dept = emp?.departmentName ? emp.departmentName.toLowerCase() : "";

//     return (
//       name.includes(search.toLowerCase()) ||
//       email.includes(search.toLowerCase()) ||
//       dept.includes(search.toLowerCase())
//     );
//   });

//   return (
//         <AdminLayout>
//           <div className="container mt-5">
//             <div className="card shadow-lg border-0 rounded-3">
//               <div className="card-header bg-white d-flex justify-content-between align-items-center p-3">
//                 <h4 className="fw-bold mb-0">Employee's List</h4>
//                 <div style={{ width: "250px" }}>
//                   <input
//                     type="text"
//                     className="form-control"
//                     placeholder="Search..."
//                     value={search}
//                     onChange={(e) => setSearch(e.target.value)}
//                   />
//                 </div>
//               </div>

//               <div className="table-responsive">
//                 <table className="table align-middle mb-0">
//                   <thead className="bg-light">
//                     <tr>
//                       <th>ID</th>
//                       <th>Name</th>
//                       <th>Email</th>
//                       <th>Phone</th>
//                       <th>Department</th>
//                       <th>Role</th>
//                       <th>Status</th>
//                       <th className="text-center">Action</th>
//                     </tr>
//                   </thead>
//                   <tbody>
//                     {filteredEmployees.map((emp, index) => (
//                       <tr key={index} className="employee-row">
//                         <td>{emp?.id || "-"}</td>
//                         <td>{emp?.firstName || "-"}</td>
//                         <td>{emp?.email || "-"}</td>
//                         <td>{emp?.phoneNumber || "-"}</td>
//                         <td>{emp?.departmentName || "-"}</td>
//                         <td>{emp?.roleName || "-"}</td>
//                         <td>
//                           <span
//                             className={`badge rounded-pill ${
//                               emp?.statusName === "Active"
//                                 ? "bg-success-subtle text-success"
//                                 : "bg-danger-subtle text-danger"
//                             }`}
//                           >
//                             {emp?.statusName || "Unknown"}
//                           </span>
//                         </td>
//                         <td className="text-center">
//                           <button
//                             className="btn btn-sm btn-outline-primary"
//                             onClick={() => alert(`View profile of ${emp?.firstName}`)}
//                           >
//                             View
//                           </button>
//                         </td>
//                       </tr>
//                     ))}
//                     {filteredEmployees.length === 0 && (
//                       <tr>
//                         <td colSpan="8" className="text-center text-muted py-4">
//                           No employees found
//                         </td>
//                       </tr>
//                     )}
//                   </tbody>
//                 </table>
//               </div>
//             </div>
//           </div>
//         </AdminLayout>
//   );
// };

// export default AllEmployees;

import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { GetAllEmployees } from "../../service/employee.service";
import "./AllEmployees.css";
import AdminLayout from "../navbar/AdminLayout";

const AllEmployees = () => {
  const [employees, setEmployees] = useState([]);
  const [search, setSearch] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    GetAllEmployees()
      .then((res) => setEmployees(res.data))
      .catch((err) => console.log(err));
  }, []);

  const filteredEmployees = employees.filter((emp) => {
    const name = emp?.firstName ? emp.firstName.toLowerCase() : "";
    const email = emp?.email ? emp.email.toLowerCase() : "";
    const dept = emp?.departmentName ? emp.departmentName.toLowerCase() : "";

    return (
      name.includes(search.toLowerCase()) ||
      email.includes(search.toLowerCase()) ||
      dept.includes(search.toLowerCase())
    );
  });

  return (
    <AdminLayout>
      <div className="container mt-4">
        {/* Add Employee Card */}
        <div className="row mb-4">
          <div className="col-md-3">
            <div
              className="card add-employee-card shadow-sm p-3 text-center"
              onClick={() => navigate("/add-employee")}
              style={{ cursor: "pointer", backgroundColor: "#40739e", color: "#fff" }}
            >
              <h5>Add Employee</h5>
              <p className="mb-0">Quickly add a new employee</p>
            </div>
          </div>
        </div>

        {/* Employee Table */}
        <div className="card shadow-lg border-0 rounded-3">
          <div className="card-header bg-white d-flex justify-content-between align-items-center p-3">
            <h4 className="fw-bold mb-0">Employee's List</h4>
            <div style={{ width: "250px" }}>
              <input
                type="text"
                className="form-control"
                placeholder="Search..."
                value={search}
                onChange={(e) => setSearch(e.target.value)}
              />
            </div>
          </div>

          <div className="table-responsive">
            <table className="table align-middle mb-0">
              <thead className="bg-light">
                <tr>
                  <th>ID</th>
                  <th>Name</th>
                  <th>Email</th>
                  <th>Phone</th>
                  <th>Department</th>
                  <th>Role</th>
                  <th>Status</th>
                  <th className="text-center">Action</th>
                </tr>
              </thead>
              <tbody>
                {filteredEmployees.map((emp, index) => (
                  <tr key={index} className="employee-row">
                    <td>{emp?.id || "-"}</td>
                    <td>{emp?.firstName || "-"}</td>
                    <td>{emp?.email || "-"}</td>
                    <td>{emp?.phoneNumber || "-"}</td>
                    <td>{emp?.departmentName || "-"}</td>
                    <td>{emp?.roleName || "-"}</td>
                    <td>
                      <span
                        className={`badge rounded-pill ${
                          emp?.statusName === "Active"
                            ? "bg-success-subtle text-success"
                            : "bg-danger-subtle text-danger"
                        }`}
                      >
                        {emp?.statusName || "Unknown"}
                      </span>
                    </td>
                    <td className="text-center">
                      <button
                        className="btn btn-sm btn-outline-primary"
                        onClick={() => alert(`View profile of ${emp?.firstName}`)}
                      >
                        View
                      </button>
                    </td>
                  </tr>
                ))}
                {filteredEmployees.length === 0 && (
                  <tr>
                    <td colSpan="8" className="text-center text-muted py-4">
                      No employees found
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </AdminLayout>
  );
};

export default AllEmployees;

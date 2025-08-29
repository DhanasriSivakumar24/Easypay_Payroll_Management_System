// import { useEffect, useState } from "react";
// import { useNavigate } from "react-router-dom";
// import { GetAllEmployees, DeleteEmployee } from "../../service/employee.service";
// import "./AllEmployees.css";
// import AdminLayout from "../navbar/AdminLayout";

// const AllEmployees = () => {
//   const [employees, setEmployees] = useState([]);
//   const [search, setSearch] = useState("");
//   const navigate = useNavigate();

//   useEffect(() => {
//     GetAllEmployees()
//       .then((res) => setEmployees(res.data))
//       .catch((err) => console.log(err));
//   }, []);

//   const handleDelete = (id, name) => {
//   if (window.confirm(`Are you sure you want to delete ${name}?`)) {
//     // Call backend API
//     DeleteEmployee(id)
//       .then(() => {
//         alert(`${name} deleted successfully!`);
//         // Update frontend state
//         setEmployees(employees.filter(emp => emp.id !== id));
//       })
//       .catch(err => {
//         console.error("Delete failed", err);
//         alert("Failed to delete employee. Check console.");
//       });
//   }
// };

//   const filteredEmployees = employees.filter((emp) => {
//     const id = emp?.id ? String(emp.id).toLowerCase() : "";
//     const name = emp?.firstName ? emp.firstName.toLowerCase() : "";
//     const email = emp?.email ? emp.email.toLowerCase() : "";
//     const dept = emp?.departmentName ? emp.departmentName.toLowerCase() : "";

//     return (
//       id.includes(search.toLowerCase()) ||
//       name.includes(search.toLowerCase()) ||
//       email.includes(search.toLowerCase()) ||
//       dept.includes(search.toLowerCase())
//     );
//   });

//   return (
//     <AdminLayout>
//     <div className="container mt-4">
//         {/* Only Add Employee Card */}
//         <div className="row mb-4">
//           <div className="col-md-3">
//             <div
//               className="card add-employee-card shadow-sm p-3 text-center"
//               onClick={() => navigate("/employees/add-employee")}
//               style={{ cursor: "pointer", backgroundColor: "#40739e", color: "#fff" }}
//             >
//               <h5>Add Employee</h5>
//               <p className="mb-0">Quickly add a new employee</p>
//             </div>
//           </div>
//         </div>

//         {/* Employee Table */}
//         <div className="card shadow-lg border-0 rounded-3">
//           <div className="card-header bg-white d-flex justify-content-between align-items-center p-3">
//             <h4 className="fw-bold mb-0">Employee's List</h4>
//             <div style={{ width: "250px" }}>
//               <input
//                 type="text"
//                 className="form-control"
//                 placeholder="Search by ID, Name, Email, or Department..."
//                 value={search}
//                 onChange={(e) => setSearch(e.target.value)}
//               />
//             </div>
//           </div>

//           <div className="table-responsive">
//             <div className="table-section">
//             <table className="table table-hover align-middle mb-0">
//               <thead className="bg-light">
//                 <tr>
//                   <th>ID</th>
//                   <th>Name</th>
//                   <th>Email</th>
//                   <th>Phone</th>
//                   <th>Department</th>
//                   <th>Role</th>
//                   <th>Status</th>
//                   <th className="text-center">Action</th>
//                 </tr>
//               </thead>
//               <tbody>
//                 {filteredEmployees.map((emp, index) => (
//                   <tr key={index} className="employee-row">
//                     <td>{emp?.id || "-"}</td>
//                     <td>
//                       {emp?.firstName || "-"} {emp?.lastName || "-"}
//                     </td>
//                     <td>{emp?.email || "-"}</td>
//                     <td>{emp?.phoneNumber || "-"}</td>
//                     <td>{emp?.departmentName || "-"}</td>
//                     <td>{emp?.roleName || "-"}</td>
//                     <td>
//                       <span
//                         className={`badge rounded-pill ${
//                           emp?.statusName === "Active"
//                             ? "bg-success-subtle text-success"
//                             : "bg-danger-subtle text-danger"
//                         }`}
//                       >
//                         {emp?.statusName || "Unknown"}
//                       </span>
//                     </td>
//                     <td className="text-center">
//                       {/* Edit button */}
//                       <button
//                         className="btn btn-sm btn-outline-warning me-2"
//                         onClick={() => navigate(`/employees/update-employee/${emp.id}`)}
//                       >
//                         Edit
//                       </button>

//                       {/* View button */}
//                       <button
//                         className="btn btn-sm btn-outline-primary me-2"
//                         onClick={() =>
//                           alert(`View profile of ${emp?.firstName}`)
//                         }
//                       >
//                         View
//                       </button>
//                       <button
//                         className="btn btn-sm btn-outline-danger me-2"
//                         onClick={() => handleDelete(emp.id, emp.firstName)}
//                       >
//                         Delete
//                       </button>
//                     </td>
//                   </tr>
//                 ))}
//                 {filteredEmployees.length === 0 && (
//                   <tr>
//                     <td colSpan="8" className="text-center text-muted py-4">
//                       No employees found
//                     </td>
//                   </tr>
//                 )}
//               </tbody>
//             </table>
//             </div>
//           </div>
//         </div>
//       </div>
//     </AdminLayout>
//   );
// };

// export default AllEmployees;



import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { GetAllEmployees, DeleteEmployee } from "../../service/employee.service";
import AdminLayout from "../navbar/AdminLayout";
import "./AllEmployees.css";

const AllEmployees = () => {
  const [employees, setEmployees] = useState([]);
  const [search, setSearch] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    GetAllEmployees()
      .then((res) => setEmployees(res.data || []))
      .catch((err) => console.log(err));
  }, []);

  const handleDelete = (id, name) => {
    if (window.confirm(`Are you sure you want to delete ${name}?`)) {
      DeleteEmployee(id)
        .then(() => {
          alert(`${name} deleted successfully!`);
          setEmployees(employees.filter(emp => emp.id !== id));
        })
        .catch(err => {
          console.error("Delete failed", err);
          alert("Failed to delete employee.");
        });
    }
  };

  const filteredEmployees = employees.filter((emp) => {
    const id = emp?.id ? String(emp.id).toLowerCase() : "";
    const name = emp?.firstName ? emp.firstName.toLowerCase() : "";
    const email = emp?.email ? emp.email.toLowerCase() : "";
    const dept = emp?.departmentName ? emp.departmentName.toLowerCase() : "";

    return (
      id.includes(search.toLowerCase()) ||
      name.includes(search.toLowerCase()) ||
      email.includes(search.toLowerCase()) ||
      dept.includes(search.toLowerCase())
    );
  });

  return (
    <AdminLayout>
      <div className="employee-container">
        {/* Header row with search + add */}
        <div className="header-row">
          <h2>Employees</h2>
          <div className="actions">
            <input
              type="text"
              className="search-input"
              placeholder="Search by ID, Name, Email, or Department..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
            <button
              className="add-btn"
              onClick={() => navigate("/employees/add-employee")}
            >
              + Add Employee
            </button>
          </div>
        </div>

        {/* Table card */}
        <div className="employee-card">
          <table className="employee-table">
            <thead>
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
              {filteredEmployees.length > 0 ? (
                filteredEmployees.map((emp, index) => (
                  <tr key={index}>
                    <td>{emp?.id || "-"}</td>
                    <td>{emp?.firstName} {emp?.lastName}</td>
                    <td>{emp?.email || "-"}</td>
                    <td>{emp?.phoneNumber || "-"}</td>
                    <td>{emp?.departmentName || "-"}</td>
                    <td>{emp?.roleName || "-"}</td>
                    <td>
                      <span
                        className={`status-badge ${emp?.statusName?.toLowerCase()}`}
                      >
                        {emp?.statusName || "Unknown"}
                      </span>
                    </td>
                    <td className="text-center">
                      <button
                        className="btn-view"
                        onClick={() => alert(`View profile of ${emp?.firstName}`)}
                      >
                        View
                      </button>
                      <button
                        className="btn-edit"
                        onClick={() =>
                          navigate(`/employees/update-employee/${emp.id}`)
                        }
                      >
                        Edit
                      </button>
                      <button
                        className="btn-delete"
                        onClick={() => handleDelete(emp.id, emp.firstName)}
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="8" className="no-data">
                    No employees found
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </AdminLayout>
  );
};

export default AllEmployees;

import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import AdminLayout from "../Sidebar/AdminLayout";
import {
  GetAllBenefits,
  DeleteBenefitEnrollment,
} from "../../service/benefit.service";
import "./allBenefitEnrollments.css";

const AllBenefitEnrollments = () => {
  const [enrollments, setEnrollments] = useState([]);
  const [search, setSearch] = useState("");
  const [showConfirm, setShowConfirm] = useState(false);
  const [enrollmentToDelete, setEnrollmentToDelete] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    fetchEnrollments();
  }, []);

  const fetchEnrollments = () => {
    GetAllBenefits()
      .then((res) => setEnrollments(res.data || []))
      .catch((err) => console.error(err));
  };

  const handleDeleteClick = (enrollment) => {
    setEnrollmentToDelete(enrollment);
    setShowConfirm(true);
  };

  const confirmDelete = () => {
    if (!enrollmentToDelete) return;
    DeleteBenefitEnrollment(enrollmentToDelete.id)
      .then(() => {
        setEnrollments((prev) =>
          prev.filter((e) => e.id !== enrollmentToDelete.id)
        );
        setShowConfirm(false);
        setEnrollmentToDelete(null);
        alert("Enrollment deleted successfully!");
      })
      .catch((err) => alert("Delete failed: " + err.message));
  };

  const cancelDelete = () => {
    setShowConfirm(false);
    setEnrollmentToDelete(null);
  };

  const filteredEnrollments = enrollments.filter((e) => {
    const empName = e.employeeName?.toLowerCase() || "";
    const benefit = e.benefitName?.toLowerCase() || "";
    const id = e.id ? String(e.id) : "";
    return (
      empName.includes(search.toLowerCase()) ||
      benefit.includes(search.toLowerCase()) ||
      id.includes(search.toLowerCase())
    );
  });

  return (
    <AdminLayout>
      <div className="benefit-enroll-container">
        <div className="header-row">
          <h2>Benefit Enrollments</h2>
          <div className="actions">
            <input
              type="text"
              className="search-input"
              placeholder="Search by Employee or Benefit..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
            <button
              className="add-btn"
              onClick={() => navigate("/benefits-management/enroll")}
            >
              + Add Enrollment
            </button>
          </div>
        </div>

        <div className="enroll-card">
          <table className="enroll-table">
            <thead>
              <tr>
                <th>S.No</th>
                <th>Employee</th>
                <th>Benefit</th>
                <th>Start Date</th>
                <th>End Date</th>
                <th>Status</th>
                <th className="text-center">Action</th>
              </tr>
            </thead>
            <tbody>
              {filteredEnrollments.length > 0 ? (
                filteredEnrollments.map((e, idx) => (
                  <tr key={e.id}>
                    <td>{idx + 1}</td>
                    <td>{e.employeeName}</td>
                    <td>{e.benefitName}</td>
                    <td>{new Date(e.startDate).toISOString().split("T")[0]}</td>
                    <td>{new Date(e.endDate).toISOString().split("T")[0]}</td>
                    <td>
                      <span
                        className={`status-badge ${e.statusName?.toLowerCase()}`}
                      >
                        {e.statusName || "Pending"}
                      </span>
                    </td>
                    <td className="text-center">
                      <button
                        className="btn-view"
                        onClick={() =>
                          navigate(`/benefits-management/view/${e.id}`)
                        }
                      >
                        View
                      </button>
                      <button
                        className="btn-edit"
                        onClick={() =>
                          navigate(`/benefits-management/edit/${e.id}`)
                        }
                      >
                        Edit
                      </button>
                      <button
                        className="btn-delete"
                        onClick={() => handleDeleteClick(e)}
                      >
                        Delete
                      </button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="7" className="no-data">
                    No enrollments found
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>

        {showConfirm && (
          <div className="confirm-modal">
            <div className="confirm-box">
              <h3>Confirm Delete</h3>
              <p>
                Are you sure you want to delete{" "}
                <strong>
                  {enrollmentToDelete?.employeeName}'s enrollment for{" "}
                  {enrollmentToDelete?.benefitName}
                </strong>
                ?
              </p>
              <div className="confirm-actions">
                <button className="btn-confirm" onClick={confirmDelete}>
                  Yes, Delete
                </button>
                <button className="btn-cancel" onClick={cancelDelete}>
                  Cancel
                </button>
              </div>
            </div>
          </div>
        )}
      </div>
    </AdminLayout>
  );
};

export default AllBenefitEnrollments;

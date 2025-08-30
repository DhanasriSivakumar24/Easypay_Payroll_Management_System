import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  GetAllLeaveRequests,
  ApproveLeaveRequest,
  RejectLeaveRequest,
} from "../../service/leave.service";
import AdminLayout from "../navbar/AdminLayout";
import "./approveLeaveRequests.css";

const AllLeaveRequests = () => {
  const [leaveRequests, setLeaveRequests] = useState([]);
  const [search, setSearch] = useState("");
  const [showAll, setShowAll] = useState(false);
  const navigate = useNavigate();

  const managerId = sessionStorage.getItem("employeeId");

  useEffect(() => {
    GetAllLeaveRequests()
      .then((res) => setLeaveRequests(res.data || []))
      .catch((err) => console.error(err));
  }, []);

  const handleApprove = (id, name) => {
    if (!managerId) {
      alert("Manager ID not found. Please log in again.");
      return;
    }

    if (window.confirm(`Approve leave request for ${name}?`)) {
      ApproveLeaveRequest(id, managerId)
        .then(() => {
          alert("Leave request approved!");
          setLeaveRequests(
            leaveRequests.map((lr) =>
              lr.id === id ? { ...lr, statusName: "Approved" } : lr
            )
          );
        })
        .catch(() => alert("Failed to approve request."));
    }
  };

  const handleReject = (id, name) => {
    if (!managerId) {
      alert("Manager ID not found. Please log in again.");
      return;
    }

    if (window.confirm(`Reject leave request for ${name}?`)) {
      RejectLeaveRequest(id, managerId)
        .then(() => {
          alert("Leave request rejected!");
          setLeaveRequests(
            leaveRequests.map((lr) =>
              lr.id === id ? { ...lr, statusName: "Rejected" } : lr
            )
          );
        })
        .catch(() => alert("Failed to reject request."));
    }
  };

  // 🔹 Filter requests
  const filteredRequests = leaveRequests.filter((lr) => {
    const id = lr?.id ? String(lr.id).toLowerCase() : "";
    const name = lr?.employeeName ? lr.employeeName.toLowerCase() : "";
    const status = lr?.statusName ? lr.statusName.toLowerCase() : "";

    const matchesSearch =
      id.includes(search.toLowerCase()) ||
      name.includes(search.toLowerCase()) ||
      status.includes(search.toLowerCase());

    if (!showAll) {
      return matchesSearch && status === "pending";
    }
    return matchesSearch;
  });

  return (
    <AdminLayout>
      <div className="employee-container">
        {/* Header row */}
        <div className="header-row">
          <h2>{showAll ? "All Leave Requests" : "Pending Leave Requests"}</h2>
          <div className="actions">
            <input
              type="text"
              className="search-input"
              placeholder="Search by ID, Employee Name, or Status..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
            <button
              className="toggle-btn"
              onClick={() => setShowAll(!showAll)}
            >
              {showAll ? "Show Pending" : "Show Leave Requests"}
            </button>
          </div>
        </div>

        {/* Table */}
        <div className="employee-card">
          <table className="employee-table">
            <thead>
              <tr>
                <th>S.No</th>
                <th>Employee</th>
                <th>Leave Type</th>
                <th>Start</th>
                <th>End</th>
                <th>Reason</th>
                <th>Status</th>
                <th className="text-center">Action</th>
              </tr>
            </thead>
            <tbody>
              {filteredRequests.length > 0 ? (
                filteredRequests.map((lr, index) => (
                  <tr key={index}>
                    <td>{index + 1}</td>
                    <td>{lr?.employeeName || "-"}</td>
                    <td>{lr?.leaveTypeName || "-"}</td>
                    <td>{lr?.startDate?.split("T")[0] || "-"}</td>
                    <td>{lr?.endDate?.split("T")[0] || "-"}</td>
                    <td>{lr?.reason || "-"}</td>
                    <td>
                      <span
                        className={`status-badge ${lr?.statusName?.toLowerCase()}`}
                      >
                        {lr?.statusName || "Unknown"}
                      </span>
                    </td>
                    <td className="text-center">
                      {lr?.statusName?.toLowerCase() === "pending" && (
                        <>
                          <button
                            className="btn-approve"
                            onClick={() =>
                              handleApprove(lr.id, lr.employeeName)
                            }
                          >
                            Approve
                          </button>
                          <button
                            className="btn-reject"
                            onClick={() =>
                              handleReject(lr.id, lr.employeeName)
                            }
                          >
                            Reject
                          </button>
                        </>
                      )}
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="8" className="no-data">
                    No leave requests found
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

export default AllLeaveRequests;

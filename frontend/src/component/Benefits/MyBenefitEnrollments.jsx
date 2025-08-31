import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import Sidebar from "../Sidebar/Sidebar";
import { GetBenefitEnrollmentsByEmployee } from "../../service/benefit.service";
import "./myBenefitEnrollments.css";
import EmployeeLayout from "../Sidebar/EmployeeLayout";

const MyBenefitEnrollments = () => {
  const [enrollments, setEnrollments] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  const employeeId = sessionStorage.getItem("employeeId");
  const employeeName = sessionStorage.getItem("username") || "Employee";

  useEffect(() => {
    if (!employeeId) {
      navigate("/login");
      return;
    }

    GetBenefitEnrollmentsByEmployee(employeeId)
      .then((res) => setEnrollments(res.data || []))
      .catch((err) => console.error("Error fetching enrollments:", err))
      .finally(() => setLoading(false));
  }, [employeeId, navigate]);

  if (loading) return <p>Loading your benefits...</p>;

  return (
    <EmployeeLayout>
        <main className="main-content">
            <h2>My Enrolled Benefits</h2>
            <div className="enroll-card">
            <table className="enroll-table">
                <thead>
                <tr>
                    <th>S.No</th>
                    <th>Benefit</th>
                    <th>Start Date</th>
                    <th>End Date</th>
                    <th>Status</th>
                </tr>
                </thead>
                <tbody>
                {enrollments.length > 0 ? (
                    enrollments.map((e, idx) => (
                    <tr key={e.id}>
                        <td>{idx + 1}</td>
                        <td>{e.benefitName}</td>
                        <td>{new Date(e.startDate).toISOString().split("T")[0]}</td>
                        <td>{new Date(e.endDate).toISOString().split("T")[0]}</td>
                        <td>
                        <span className={`status-badge ${e.statusName?.toLowerCase()}`}>
                            {e.statusName || "Pending"}
                        </span>
                        </td>
                    </tr>
                    ))
                ) : (
                    <tr>
                    <td colSpan="5" className="no-data">
                        You have not enrolled in any benefits.
                    </td>
                    </tr>
                )}
                </tbody>
            </table>
            </div>
        </main>
    </EmployeeLayout>
  );
};

export default MyBenefitEnrollments;

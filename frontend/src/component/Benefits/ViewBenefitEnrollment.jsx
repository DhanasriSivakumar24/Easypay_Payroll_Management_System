import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useSelector } from "react-redux";
import AdminLayout from "../Sidebar/AdminLayout";
import ManagerLayout from "../Sidebar/ManagerLayout";
import { GetBenefitEnrollmentById } from "../../service/benefit.service";
import { GetEmployeeById } from "../../service/employee.service";
import "./viewBenefitEnrollment.css";

const ViewBenefitEnrollment = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [enrollment, setEnrollment] = useState(null);
  const { role } = useSelector((state) => state.auth);

  const Layout = role === "Manager" ? ManagerLayout : AdminLayout;

  const formatDate = (dateStr) => {
    if (!dateStr) return "N/A";
    const date = new Date(dateStr);
    return date.toLocaleDateString("en-GB", {
      day: "2-digit",
      month: "short",
      year: "numeric",
    });
  };

  useEffect(() => {
    if (id) {
      GetBenefitEnrollmentById(id)
        .then((res) => {
          const enrollmentData = res.data;

          GetEmployeeById(enrollmentData.employeeId)
            .then((empRes) => {
              const employeeData = empRes.data;

              setEnrollment({
                ...enrollmentData,
                roleName: employeeData.roleName,
                departmentName: employeeData.departmentName,
                email: employeeData.email,
                phoneNumber: employeeData.phoneNumber,
                address: employeeData.address,
              });
            })
            .catch((err) =>
              console.error("Error fetching employee details:", err)
            );
        })
        .catch((err) =>
          console.error("Error fetching benefit enrollment:", err)
        );
    }
  }, [id]);

  if (!enrollment) {
    return (
      <Layout>
        <div className="view-benefit-container">
          <div className="view-benefit-card text-center">
            <p>Loading benefit enrollment details...</p>
          </div>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="view-benefit-container">
        <div className="view-benefit-card">
          <h4 className="mb-4">Benefit Enrollment Details</h4>

          <div className="benefit-info-grid">
            <div>
              <label>Employee Name</label>
              <p>{enrollment.employeeName || "N/A"}</p>
            </div>
            <div>
              <label>Role</label>
              <p>{enrollment.roleName || "N/A"}</p>
            </div>
            <div>
              <label>Department</label>
              <p>{enrollment.departmentName || "N/A"}</p>
            </div>
            <div>
              <label>Benefit</label>
              <p>{enrollment.benefitName || "N/A"}</p>
            </div>
            <div>
              <label>Start Date</label>
              <p>{formatDate(enrollment.startDate)}</p>
            </div>
            <div>
              <label>End Date</label>
              <p>{formatDate(enrollment.endDate)}</p>
            </div>
            <div>
              <label>Status</label>
              <p>{enrollment.statusName || "N/A"}</p>
            </div>
            <div>
              <label>Email</label>
              <p>{enrollment.email || "N/A"}</p>
            </div>
            <div>
              <label>Phone</label>
              <p>{enrollment.phoneNumber || "N/A"}</p>
            </div>
            <div>
              <label>Address</label>
              <p>{enrollment.address || "N/A"}</p>
            </div>
            <div>
              <label>Employee Contribution</label>
              <p>{enrollment.employeeContribution ?? "N/A"}%</p>
            </div>
            <div>
              <label>Employer Contribution</label>
              <p>{enrollment.employerContribution ?? "N/A"}%</p>
            </div>
          </div>

          {role === "Admin" || role === "HR Manager" ? (
            <div className="d-flex gap-2 mt-4">
              <button
                className="btn btn-primary w-50"
                onClick={() => navigate(`/benefits-management/edit/${id}`)}
              >
                Edit Enrollment
              </button>
              <button
                className="btn btn-secondary w-50"
                onClick={() => navigate("/benefits-management")}
              >
                Back
              </button>
            </div>
          ) : role === "Manager" ? (
            <div className="mt-4 text-end">
              <button
                className="btn btn-secondary"
                onClick={() => navigate("/benefits-management")}
              >
                Back
              </button>
            </div>
          ) : null}
        </div>
      </div>
    </Layout>
  );
};

export default ViewBenefitEnrollment;

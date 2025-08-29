import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import AdminLayout from "../navbar/AdminLayout";
import { GetBenefitEnrollmentById } from "../../service/benefit.service";
import '../Employees/personalInfo.css'; // Use the same card theme

const ViewBenefitEnrollment = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [enrollment, setEnrollment] = useState(null);

  useEffect(() => {
    GetBenefitEnrollmentById(id)
      .then(res => setEnrollment(res.data))
      .catch(err => console.error(err));
  }, [id]);

  if (!enrollment) return <div>Loading...</div>;

  // Avatar fallback
  const avatarUrl = enrollment.profileImage ||
    `https://ui-avatars.com/api/?name=${enrollment.employeeFirstName}+${enrollment.employeeLastName}&background=0D8ABC&color=fff&size=150`;

  const formattedStart = new Date(enrollment.startDate).toLocaleDateString('en-GB');
  const formattedEnd = new Date(enrollment.endDate).toLocaleDateString('en-GB');

  return (
    <AdminLayout>
      <div className="personal-info-container">
        {/* Profile Header */}
        <div className="profile-header">
          <img src={avatarUrl} alt="Employee Avatar" className="profile-avatar" />
          <div className="profile-basic">
            <h2>{enrollment.employeeFirstName} {enrollment.employeeLastName}</h2>
            <p className="role">{enrollment.roleName} • {enrollment.departmentName}</p>
            <span className={`status-badge ${enrollment.statusName.toLowerCase()}`}>
              {enrollment.statusName}
            </span>
          </div>
          <button className="edit-btn" onClick={() => navigate(`/benefits/edit/${id}`)}>Edit</button>
        </div>

        {/* Enrollment Details */}
        <div className="info-card">
          <h3>Benefit Enrollment Details</h3>
          <div className="info-grid">
            <p><strong>Benefit:</strong> {enrollment.benefitName}</p>
            <p><strong>Start Date:</strong> {formattedStart}</p>
            <p><strong>End Date:</strong> {formattedEnd}</p>
            <p><strong>Status:</strong> {enrollment.statusName}</p>
          </div>
        </div>

        {/* Contact Info */}
        <div className="info-card">
          <h3>Contact Information</h3>
          <div className="info-grid">
            <p><strong>Email:</strong> {enrollment.email}</p>
            <p><strong>Phone:</strong> {enrollment.phoneNumber}</p>
            <p><strong>Address:</strong> {enrollment.address || "N/A"}</p>
          </div>
        </div>

        <button className="btn btn-secondary" onClick={() => navigate("/benefits-management")}>Back</button>
      </div>
    </AdminLayout>
  );
};

export default ViewBenefitEnrollment;

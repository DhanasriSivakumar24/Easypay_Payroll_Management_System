import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import EmployeeLayout from '../navbar/EmployeeLayout';
import { GetPersonalInfo } from '../../service/employee.service';
import './personalInfo.css';

const PersonalInfo = () => {
  const { employeeId } = useSelector((state) => state.auth);
  const [info, setInfo] = useState(null);

  useEffect(() => {
    if (!employeeId) return;

    GetPersonalInfo(employeeId)
      .then(res => setInfo(res.data))
      .catch(err => console.error('Failed to fetch personal info', err));
  }, [employeeId]);

  if (!info) return <div>Loading...</div>;

  // Avatar from ui-avatars (if no profileImage in DB)
  const avatarUrl = info.profileImage ||
    `https://ui-avatars.com/api/?name=${info.firstName}+${info.lastName}&background=0D8ABC&color=fff&size=150`;

  // Format DOB as only date
  const formattedDOB = info.dateOfBirth
    ? new Date(info.dateOfBirth).toLocaleDateString('en-GB') // dd/mm/yyyy
    : "N/A";

  return (
    <EmployeeLayout>
      <div className="personal-info-container">
        {/* Profile Header */}
        <div className="profile-header">
          <img src={avatarUrl} alt="Employee Avatar" className="profile-avatar" />
          <div className="profile-basic">
            <h2>{info.firstName} {info.lastName}</h2>
            <p className="role">{info.roleName} • {info.departmentName}</p>
            <span className={`status-badge ${info.statusName.toLowerCase()}`}>
              {info.statusName}
            </span>
          </div>
          <button className="edit-btn">Edit</button>
        </div>

        {/* Contact Info + Job Info side by side */}
        <div className="info-row">
          <div className="info-card">
            <h3>Contact Information</h3>
            <div className="info-grid">
              <p><strong>Email:</strong> {info.email}</p>
              <p><strong>Phone:</strong> {info.phoneNumber}</p>
              <p><strong>Address:</strong> {info.address ?? "N/A"}</p>
            </div>
          </div>

          <div className="info-card">
            <h3>Job Information</h3>
            <div className="info-grid">
              <p><strong>Employee ID:</strong> {info.id}</p>
              <p><strong>Department:</strong> {info.departmentName}</p>
              <p><strong>Role:</strong> {info.roleName}</p>
              <p><strong>User Role:</strong> {info.userRoleName}</p>
              <p><strong>Reporting Manager:</strong> {info.reportingManagerName ?? "N/A"}</p>
              <p><strong>Salary:</strong> ₹{info.salary}</p>
            </div>
          </div>
        </div>

        {/* Personal Details */}
        <div className="info-card">
          <h3>Personal Details</h3>
          <div className="info-grid">
            <p><strong>Date of Birth:</strong> {formattedDOB}</p>
            <p><strong>Gender:</strong> {info.gender ?? "N/A"}</p>
            <p><strong>PAN Number:</strong> {info.panNumber ?? "N/A"}</p>
            <p><strong>Aadhaar Number:</strong> {info.aadhaarNumber ?? "N/A"}</p>
          </div>
        </div>
      </div>
    </EmployeeLayout>
  );
};

export default PersonalInfo;

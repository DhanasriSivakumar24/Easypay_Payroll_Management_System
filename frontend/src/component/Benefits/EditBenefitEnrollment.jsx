import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import AdminLayout from "../Sidebar/AdminLayout";
import {
  GetBenefitEnrollmentById,
  UpdateBenefitEnrollment,
} from "../../service/benefit.service";
import "./editBenefitEnrollment.css";

const EditBenefitEnrollment = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [enrollment, setEnrollment] = useState({
    employeeName: "",
    benefitName: "",
    startDate: "",
    endDate: "",
    statusName: "",
  });

  const [loading, setLoading] = useState(true);

  const formatDate = (dateStr) => {
    if (!dateStr) return "";
    return new Date(dateStr).toISOString().split("T")[0];
  };

  useEffect(() => {
    if (id) {
      GetBenefitEnrollmentById(id)
        .then((res) => {
          const data = res.data;
          setEnrollment({
            employeeName: data.employeeName || "",
            benefitName: data.benefitName || "",
            startDate: formatDate(data.startDate),
            endDate: formatDate(data.endDate),
            statusName: data.statusName || "Active",
          });
        })
        .catch((err) => console.error("Error fetching enrollment:", err))
        .finally(() => setLoading(false));
    }
  }, [id]);

  const handleChange = (field, value) => {
    setEnrollment((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    let payload = { ...enrollment };

    try {
      await UpdateBenefitEnrollment(id, payload);
      alert("Enrollment updated successfully!");
      navigate("/benefits-management/enrollments");
    } catch (err) {
      console.error("Update failed:", err.response?.data || err.message);
      alert("Update failed! Check console for details.");
    }
  };

  if (loading) return <p className="loading">Loading...</p>;

  return (
    <AdminLayout>
      <div className="edit-benefit-container">
        <div className="edit-benefit-card">
          <h4 className="mb-4">Update Benefit Enrollment</h4>

          <form onSubmit={handleSubmit}>

            <div className="row g-3 mb-3">
              <div className="col-md-6">
                <label>Employee</label>
                <input
                  type="text"
                  className="form-control"
                  value={enrollment.employeeName}
                  disabled
                />
              </div>
              <div className="col-md-6">
                <label>Benefit</label>
                <input
                  type="text"
                  className="form-control"
                  value={enrollment.benefitName}
                  disabled
                />
              </div>
            </div>

            <div className="row g-3 mb-3">
              <div className="col-md-6">
                <label>Start Date</label>
                <input
                  type="date"
                  className="form-control"
                  value={enrollment.startDate}
                  onChange={(e) => handleChange("startDate", e.target.value)}
                  required
                />
              </div>
              <div className="col-md-6">
                <label>End Date</label>
                <input
                  type="date"
                  className="form-control"
                  value={enrollment.endDate}
                  onChange={(e) => handleChange("endDate", e.target.value)}
                  required
                />
              </div>
            </div>

            <div className="row g-3 mb-3">
              <div className="col-md-6">
                <label>Status</label>
                <select
                  className="form-select"
                  value={enrollment.statusName}
                  onChange={(e) => handleChange("statusName", e.target.value)}
                >
                  <option value="Active">Active</option>
                  <option value="Pending">Pending</option>
                  <option value="Expired">Expired</option>
                </select>
              </div>
            </div>

            <div className="d-flex gap-2 mt-3">
              <button type="submit" className="btn btn-success w-50">
                Update Enrollment
              </button>
              <button
                type="button"
                className="btn btn-secondary w-50"
                onClick={() => navigate("/benefits-management")}
              >
                Cancel
              </button>
            </div>
          </form>
        </div>
      </div>
    </AdminLayout>
  );
};

export default EditBenefitEnrollment;

import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { GetLeaveType } from "../../service/masterTable.service";
import { SubmitLeaveRequest } from "../../service/leave.service";
import "./applyLeave.css";
import EmployeeLayout from "../navbar/EmployeeLayout";
import { useNavigate } from "react-router-dom";

const ApplyLeave = () => {
  const { employeeId } = useSelector((state) => state.auth);
  const [leaveTypes, setLeaveTypes] = useState([]);
  const [form, setForm] = useState({
    leaveTypeId: "",
    startDate: "",
    endDate: "",
    reason: "",
    attachment: null,
  });
  const [days, setDays] = useState(0);
  const [statusMsg, setStatusMsg] = useState("");
  const navigate = useNavigate();

  // fetch leave types
  useEffect(() => {
    GetLeaveType()
      .then((res) => setLeaveTypes(res.data))
      .catch((err) => console.error("Failed to load leave types", err));
  }, []);

  // auto-calc days
  useEffect(() => {
    if (form.startDate && form.endDate) {
      const s = new Date(form.startDate);
      const e = new Date(form.endDate);
      const diff = (e - s) / (1000 * 60 * 60 * 24) + 1;
      setDays(diff > 0 ? diff : 0);
    }
  }, [form.startDate, form.endDate]);

  const handleChange = (field, value) => {
    setForm({ ...form, [field]: value });
  };

  const handleFile = (e) => {
    setForm({ ...form, attachment: e.target.files[0] });
  };
  
  const handleCancel = () => {
    setForm({
      leaveTypes: "",
      startDate: "",
      endDate: "",
      reason: "",
    });
    navigate("/leave-requests");
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const payload = {
        employeeId: employeeId || sessionStorage.getItem("employeeId"),
        leaveTypeId: Number(form.leaveTypeId),
        startDate: form.startDate,
        endDate: form.endDate,
        reason: form.reason,
      };

      let dataToSend = payload;

      // if attachment exists, switch to FormData
      if (form.attachment) {
        const fd = new FormData();
        Object.entries(payload).forEach(([k, v]) => fd.append(k, v));
        fd.append("attachment", form.attachment);
        dataToSend = fd;
      }

      await SubmitLeaveRequest(dataToSend);
      setStatusMsg(" Leave applied successfully!");
      setForm({
        leaveTypeId: "",
        startDate: "",
        endDate: "",
        reason: "",
        attachment: null,
      });
      setDays(0);
    } catch (err) {
      console.error("Apply leave failed:", err.response?.data || err.message);
      setStatusMsg(" Failed to apply leave");
    }
  };

  return (
    <EmployeeLayout active="leaves">
      <div className="apply-leave-container">
        <h2>Apply Leave ✍️</h2>

        <form className="apply-leave-form" onSubmit={handleSubmit}>
          {/* Leave Type */}
          <label>Leave Type</label>
          <select
            value={form.leaveTypeId}
            onChange={(e) => handleChange("leaveTypeId", e.target.value)}
            required
          >
            <option value="">-- Select Leave Type --</option>
            {leaveTypes.map((lt) => (
              <option key={lt.id} value={lt.id}>
                {lt.leaveTypeName}
              </option>
            ))}
          </select>

          {/* Dates */}
          <div className="date-row">
            <div>
              <label>Start Date</label>
              <input
                type="date"
                value={form.startDate}
                onChange={(e) => handleChange("startDate", e.target.value)}
                required
              />
            </div>
            <div>
              <label>End Date</label>
              <input
                type="date"
                value={form.endDate}
                onChange={(e) => handleChange("endDate", e.target.value)}
                required
              />
            </div>
          </div>

          {days > 0 && <p className="days-info">📅 Total Days: {days}</p>}

          {/* Reason */}
          <label>Reason</label>
          <textarea
            placeholder="Enter reason"
            value={form.reason}
            onChange={(e) => handleChange("reason", e.target.value)}
            required
          />

          {/* Attachment */}
          <label>Attachment (Optional)</label>
          <input type="file" onChange={handleFile} />

          {/* Submit */}
          <div className="btn-row">
            <button type="submit" className="btn btn-primary w-50">
               Submit
            </button>
            <button
              type="button"
              onClick={handleCancel}
              className="btn btn-secondary w-50"
            >
               Cancel
            </button>
          </div>
        </form>

        {statusMsg && <p className="status-msg">{statusMsg}</p>}
      </div>
    </EmployeeLayout>
  );
};

export default ApplyLeave;

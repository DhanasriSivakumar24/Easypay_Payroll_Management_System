import { useEffect, useState } from "react";
import { GetLeaveType } from "../../service/masterTable.service";
import { SubmitLeaveRequest } from "../../service/leave.service";
import "./applyLeave.css";
import EmployeeLayout from "../navbar/EmployeeLayout";

const ApplyLeave = () => {
  const [leaveTypes, setLeaveTypes] = useState([]);
  const [form, setForm] = useState({
    leaveTypeId: "",
    startDate: "",
    endDate: "",
    reason: "",
    attachment: null
  });
  const [days, setDays] = useState(0);
  const [statusMsg, setStatusMsg] = useState("");

  useEffect(() => {
    GetLeaveType().then(res => setLeaveTypes(res.data));
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

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await SubmitLeaveRequest(form);
      setStatusMsg("✅ Leave applied successfully!");
    } catch (err) {
      setStatusMsg("❌ Failed to apply leave");
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
              <option key={lt.id} value={lt.id}>{lt.leaveTypeName}</option>
            ))}
          </select>

          {/* Dates */}
          <div className="date-row">
            <div>
              <label>Start Date</label>
              <input type="date" value={form.startDate} onChange={(e) => handleChange("startDate", e.target.value)} required />
            </div>
            <div>
              <label>End Date</label>
              <input type="date" value={form.endDate} onChange={(e) => handleChange("endDate", e.target.value)} required />
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
          <button type="submit" className="submit-btn">Submit Request</button>
        </form>

        {statusMsg && <p className="status-msg">{statusMsg}</p>}
      </div>
    </EmployeeLayout>
  );
};

export default ApplyLeave;

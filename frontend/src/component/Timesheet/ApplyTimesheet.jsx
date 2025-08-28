import { useState } from "react";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import { AddTimesheet } from "../../service/timesheet.service";
import EmployeeLayout from "../navbar/EmployeeLayout";
import "./applyTimesheet.css";

const ApplyTimesheet = () => {
  const { employeeId } = useSelector((state) => state.auth);
  const navigate = useNavigate();

  const [form, setForm] = useState({
    workDate: "",
    hoursWorked: "",
    taskDescription: "",
    isBillable: false,
  });

  const handleChange = (field, value) => {
    setForm({ ...form, [field]: value });
  };

  const handleCancel = () => {
    setForm({
      workDate: "",
      hoursWorked: "",
      taskDescription: "",
      isBillable: false,
    });
    navigate("/timesheets");
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const payload = {
      employeeId: employeeId || sessionStorage.getItem("employeeId"),
      workDate: form.workDate,
      hoursWorked: Number(form.hoursWorked),
      taskDescription: form.taskDescription,
      isBillable: form.isBillable,
    };

    AddTimesheet(payload)
      .then(() => {
        alert(" Timesheet submitted successfully!");
        handleCancel(); // reset after submit
      })
      .catch((err) => {
        console.error("Timesheet submit failed:", err.response?.data || err.message);
        alert(" Failed to submit timesheet");
      });
  };

  return (
    <EmployeeLayout active="timesheet">
      <div className="apply-timesheet-container">
        <div className="timesheet-header">
          <h2>Submit Timesheet 🕒</h2>
          <button
            className="history-btn"
            onClick={() => navigate("/timesheets/timesheet-history")}
          >
             View History
          </button>
        </div>

        <form className="apply-timesheet-form" onSubmit={handleSubmit}>
          {/* Work Date */}
          <label>Work Date</label>
          <input
            type="date"
            value={form.workDate}
            onChange={(e) => handleChange("workDate", e.target.value)}
            required
          />

          {/* Hours Worked */}
          <label>Hours Worked</label>
          <input
            type="number"
            step="0.5"
            min="0"
            max="24"
            value={form.hoursWorked}
            onChange={(e) => handleChange("hoursWorked", e.target.value)}
            required
          />

          {/* Task Description */}
          <label>Task Description</label>
          <textarea
            placeholder="Enter details of work done"
            value={form.taskDescription}
            onChange={(e) => handleChange("taskDescription", e.target.value)}
            required
          />

          {/* Is Billable */}
          <label className="billable-row">
            <input
              type="checkbox"
              checked={form.isBillable}
              onChange={(e) => handleChange("isBillable", e.target.checked)}
            />
            Is Billable?
          </label>

          {/* Buttons */}
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
      </div>
    </EmployeeLayout>
  );
};

export default ApplyTimesheet;
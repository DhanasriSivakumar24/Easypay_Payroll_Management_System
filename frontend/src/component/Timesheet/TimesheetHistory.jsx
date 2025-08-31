import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { useNavigate } from "react-router-dom";
import EmployeeLayout from "../Sidebar/EmployeeLayout";
import { GetTimesheetsByEmployee } from "../../service/timesheet.service";
import "./timesheetHistory.css";

const TimesheetHistory = () => {
  const { employeeId } = useSelector((state) => state.auth);
  const navigate = useNavigate();

  const [timesheets, setTimesheets] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedMonth, setSelectedMonth] = useState(new Date().toLocaleString('default', { month: 'long' }));
  const [sortOption, setSortOption] = useState('dateAsc');

  useEffect(() => {
    const fetchTimesheets = async () => {
      try {
        setLoading(true);
        const res = await GetTimesheetsByEmployee(employeeId);
        setTimesheets(res.data || []);
      } catch (err) {
        setError("Failed to fetch timesheets ❌");
      } finally {
        setLoading(false);
      }
    };
    fetchTimesheets();
  }, [employeeId]);

  const filteredTimesheets = timesheets.filter(t => 
    new Date(t.workDate).toLocaleString('default', { month: 'long' }) === selectedMonth
  );

  const sortedTimesheets = [...filteredTimesheets].sort((a, b) => {
    const dateA = new Date(a.workDate);
    const dateB = new Date(b.workDate);
    return sortOption === 'dateAsc' ? dateA - dateB : dateB - dateA;
  });

  const summary = {
    totalHours: sortedTimesheets.reduce((sum, t) => sum + (t.hoursWorked || 0), 0),
    approved: sortedTimesheets.filter((t) => t.statusName?.toLowerCase() === "approved").length,
    pending: sortedTimesheets.filter((t) => t.statusName?.toLowerCase() === "pending").length,
    rejected: sortedTimesheets.filter((t) => t.statusName?.toLowerCase() === "rejected").length,
  };

  const months = [
    "January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"
  ];

  const handleSubmitClick = () => {
    navigate("/timesheets/submit-timesheet");
  };

  return (
    <EmployeeLayout active="timesheet-history">
      <div className="timesheet-history-container">
        <div className="header-row">
          <h2> Timesheet History</h2>
          <div className="benefit-actions-left">
            <select
              className="filter-dropdown"
              value={selectedMonth}
              onChange={(e) => setSelectedMonth(e.target.value)}
            >
              {months.map(month => (
                <option key={month} value={month}>{month}</option>
              ))}
            </select>
            <button className="submit-btn" onClick={handleSubmitClick}> Submit Timesheet</button>
          </div>
        </div>

        {loading && <p>Loading timesheets...</p>}
        {error && <p style={{ color: "red" }}>{error}</p>}

        {!loading && !error && (
          <>
            <div className="summary-cards">
              <div className="summary-card total">
                <h3>Total Hours</h3>
                <p>{summary.totalHours} hrs</p>
              </div>
              <div className="summary-card approved">
                <h3>Approved</h3>
                <p>{summary.approved}</p>
              </div>
              <div className="summary-card pending">
                <h3>Pending</h3>
                <p>{summary.pending}</p>
              </div>
              <div className="summary-card rejected">
                <h3>Rejected</h3>
                <p>{summary.rejected}</p>
              </div>
            </div>

            <div className="timesheet-card">
              <div className="timesheet-header">
                <h4>Timesheet History</h4>
                <select
                  className="sort-button"
                  value={sortOption}
                  onChange={(e) => setSortOption(e.target.value)}
                >
                  <option value="dateAsc">Date Ascending</option>
                  <option value="dateDesc">Date Descending</option>
                </select>
              </div>
              <table className="timesheet-table">
                <thead>
                  <tr>
                    <th>S.No</th>
                    <th>Date</th>
                    <th>Hours Worked</th>
                    <th>Task</th>
                    <th>Billable</th>
                    <th>Status</th>
                  </tr>
                </thead>
                <tbody>
                  {sortedTimesheets.length > 0 ? (
                    sortedTimesheets.map((t, index) => (
                      <tr key={t.id}>
                        <td>{index + 1}</td>
                        <td>{new Date(t.workDate).toISOString().split("T")[0]}</td>
                        <td>{t.hoursWorked}</td>
                        <td>{t.taskDescription}</td>
                        <td>{t.isBillable ? "Yes" : "No"}</td>
                        <td>
                          <span className={`status-badge ${t.statusName?.toLowerCase() || 'pending'}`}>
                            {t.statusName || "Pending"}
                          </span>
                        </td>
                      </tr>
                    ))
                  ) : (
                    <tr>
                      <td colSpan="6" style={{ textAlign: "center" }}>
                        No timesheets found 🙅‍♂️
                      </td>
                    </tr>
                  )}
                </tbody>
              </table>
            </div>
          </>
        )}
      </div>
    </EmployeeLayout>
  );
};

export default TimesheetHistory;
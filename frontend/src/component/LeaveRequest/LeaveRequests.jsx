import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { GetLeaveRequestsByEmployee } from '../../service/leave.service';
import EmployeeLayout from '../Sidebar/EmployeeLayout';
import './leaveRequests.css';

const LeaveRequests = () => {
  const { employeeId } = useSelector((state) => state.auth);
  const [requests, setRequests] = useState([]);
  const [sortedRequests, setSortedRequests] = useState([]);
  const [sortOption, setSortOption] = useState('startDateAsc');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = () => {
      const id = employeeId || sessionStorage.getItem('employeeId');
      if (!id) return;
      GetLeaveRequestsByEmployee(id)
        .then((res) => {
          setRequests(res.data);
          setSortedRequests(res.data);
        })
        .catch((err) => console.error('Failed to load leave requests', err));
    };
    fetchData();
  }, [employeeId]);

  const calculateTotalDays = (startDate, endDate) => {
    const start = new Date(startDate);
    const end = new Date(endDate);
    return Math.ceil((end - start) / (1000 * 60 * 60 * 24)) + 1;
  };

  const getApprovedTime = (actionedAt, statusName) => {
    if (statusName?.toLowerCase() === 'approved' && actionedAt) {
      return new Date(actionedAt).toLocaleTimeString();
    }
    return '';
  };

  const sortMapping = {
    startDateAsc: { key: 'startDate', direction: 'asc' },
    startDateDesc: { key: 'startDate', direction: 'desc' },
    endDateAsc: { key: 'endDate', direction: 'asc' },
    endDateDesc: { key: 'endDate', direction: 'desc' },
    leaveTypeAsc: { key: 'leaveTypeName', direction: 'asc' },
    leaveTypeDesc: { key: 'leaveTypeName', direction: 'desc' },
    statusAsc: { key: 'statusName', direction: 'asc' },
    statusDesc: { key: 'statusName', direction: 'desc' },
    actionDateAsc: { key: 'actionedAt', direction: 'asc' },
    actionDateDesc: { key: 'actionedAt', direction: 'desc' },
    requestedDateAsc: { key: 'requestedAt', direction: 'asc' },
    requestedDateDesc: { key: 'requestedAt', direction: 'desc' },
  };

  const handleSortChange = (option) => {
    setSortOption(option);
    const { key, direction } = sortMapping[option];
    const sorted = [...requests].sort((a, b) => {
      let valA = a[key] || '';
      let valB = b[key] || '';
      if (['startDate','endDate','actionedAt','requestedAt'].includes(key)) {
        valA = valA ? new Date(valA) : new Date(0);
        valB = valB ? new Date(valB) : new Date(0);
        return direction === 'asc' ? valA - valB : valB - valA;
      }
      return direction === 'asc' ? valA.localeCompare(valB) : valB.localeCompare(valA);
    });
    setSortedRequests(sorted);
  };

  return (
    <EmployeeLayout>
      <div className="leave-requests-container">
        <div className="leave-header">
          <h2>Leave Requests</h2>
          <button
            className="apply-leave-btn"
            onClick={() => navigate('/leave-requests/leaves/apply')}
          >
            ➕ Apply Leave
          </button>
        </div>

        <div className="leave-history-card">
          <div className="leave-history-header">
            <h4>Leave History</h4>
            <select
              className="sort-button"
              value={sortOption}
              onChange={(e) => handleSortChange(e.target.value)}
            >
              <option value="startDateAsc">Start Date Ascending</option>
              <option value="startDateDesc">Start Date Descending</option>
              <option value="endDateAsc">End Date Ascending</option>
              <option value="endDateDesc">End Date Descending</option>
              <option value="leaveTypeAsc">Leave Type Ascending</option>
              <option value="leaveTypeDesc">Leave Type Descending</option>
              <option value="statusAsc">Status Ascending</option>
              <option value="statusDesc">Status Descending</option>
              <option value="actionDateAsc">Action Date Ascending</option>
              <option value="actionDateDesc">Action Date Descending</option>
              <option value="requestedDateAsc">Requested Date Ascending</option>
              <option value="requestedDateDesc">Requested Date Descending</option>
            </select>
          </div>

          {sortedRequests.length === 0 ? (
            <p>No leave requests found.</p>
          ) : (
            <table className="leave-history-table">
              <thead>
                <tr>
                  <th>S.No</th>
                  <th>Leave Type</th>
                  <th>Start Date</th>
                  <th>End Date</th>
                  <th>Reason</th>
                  <th>Status</th>
                  <th>Action Date</th>
                  <th>Approved Time</th>
                  <th>Requested Date</th>
                  <th>Total Days</th>
                </tr>
              </thead>
              <tbody>
                {sortedRequests.map((req, index) => {
                  const actionDate = req.actionedAt ? new Date(req.actionedAt).toLocaleDateString() : '';
                  const requestedDate = req.requestedAt ? new Date(req.requestedAt).toLocaleDateString() : '';
                  const approvedTime = getApprovedTime(req.actionedAt, req.statusName);
                  const totalDays = calculateTotalDays(req.startDate, req.endDate);

                  return (
                    <tr key={req.id}>
                      <td>{index + 1}</td>
                      <td>{req.leaveTypeName}</td>
                      <td>{new Date(req.startDate).toLocaleDateString()}</td>
                      <td>{new Date(req.endDate).toLocaleDateString()}</td>
                      <td>{req.reason}</td>
                      <td>
                        <span className={`status-badge ${req.statusName?.toLowerCase() || 'pending'}`}>
                          {req.statusName || 'Pending'}
                        </span>
                      </td>
                      <td>{actionDate}</td>
                      <td>{approvedTime}</td>
                      <td>{requestedDate}</td>
                      <td>{totalDays}</td>
                    </tr>
                  );
                })}
              </tbody>
            </table>
          )}
        </div>
      </div>
    </EmployeeLayout>
  );
};

export default LeaveRequests;

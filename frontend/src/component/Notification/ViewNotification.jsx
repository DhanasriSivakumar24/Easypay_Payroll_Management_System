import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux'; 
import { useNavigate } from 'react-router-dom';
import { GetNotificationsByUser } from '../../service/notification.service';
import ManagerLayout from '../Sidebar/ManagerLayout';
import PayrollProcessorLayout from '../Sidebar/PayrollProcessorLayout';
import EmployeeLayout from '../Sidebar/EmployeeLayout';
import './viewNotification.css';

const ViewNotification = () => {
  const { employeeId, role } = useSelector((state) => state.auth);
  const [requests, setRequests] = useState([]);
  const [filteredRequests, setFilteredRequests] = useState([]);
  const [selectedStatus, setSelectedStatus] = useState("All");
  const [selectedChannel, setSelectedChannel] = useState("All");
  const navigate = useNavigate();

  const normalizedRole = role?.toLowerCase();
  const isAdmin = ["manager"].includes(normalizedRole);
  const isProcessor = ["payrollprocessor", "payroll processor"].includes(normalizedRole);
  const isEmployee = ["employee"].includes(normalizedRole);

  const Layout = isProcessor
    ? PayrollProcessorLayout
    : isAdmin
    ? ManagerLayout
    : EmployeeLayout;

  useEffect(() => {
    const fetchData = async () => {
      const id = employeeId || sessionStorage.getItem('employeeId');
      if (!id) return;

      try {
        const res = await GetNotificationsByUser(id);
        setRequests(res.data);
        setFilteredRequests(res.data);
      } catch (err) {
        console.error('Failed to load notifications:', err);
      }
    };

    fetchData();
  }, [employeeId]);

  useEffect(() => {
    let filtered = [...requests];

    if (selectedStatus !== "All") {
      filtered = filtered.filter((n) => n.status === selectedStatus);
    }
    if (selectedChannel !== "All") {
      filtered = filtered.filter((n) => n.channelName === selectedChannel);
    }

    setFilteredRequests(filtered);
  }, [selectedStatus, selectedChannel, requests]);

  return (
    <Layout>
      <div className="notification-container">
        <h2 className="notification-title">My Notifications</h2>

        <div className="filter-section">
          <select
            value={selectedStatus}
            onChange={(e) => setSelectedStatus(e.target.value)}
          >
            <option value="All">All Status</option>
            <option value="Pending">Pending</option>
            <option value="Sent">Sent</option>
            <option value="Failed">Failed</option>
          </select>

          <select
            value={selectedChannel}
            onChange={(e) => setSelectedChannel(e.target.value)}
          >
            <option value="All">All Channels</option>
            <option value="Email">Email</option>
            <option value="SMS">SMS</option>
            <option value="In-App">In-App</option>
          </select>
        </div>

        {filteredRequests.length === 0 ? (
          <p>No notifications found.</p>
        ) : (
          filteredRequests.map((n) => (
            <div key={n.id} className="notification-card">
              <div className="notification-header">
                <span className="channel">{n.channelName}</span>
                <span className={`status status-${n.status?.toLowerCase()}`}>
                  {n.status}
                </span>
              </div>

              <p className="notification-message">{n.message}</p>

              <small className="notification-date">
                {new Date(n.sendDate).toLocaleString()}
              </small>
            </div>
          ))
        )}
      </div>
    </Layout>
  );
};

export default ViewNotification;

import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { GetAllAuditTrail } from "../../service/audit.service";
import AdminLayout from "../Sidebar/AdminLayout";
import PayrollProcessorLayout from "../Sidebar/PayrollProcessorLayout";
import './auditTrail.css';

const AuditTrail = () => {
  const { role } = useSelector((state) => state.auth);
  const [auditLogs, setAuditLogs] = useState([]);
  const [filteredLogs, setFilteredLogs] = useState([]);
  const [loading, setLoading] = useState(true);

  const [searchUser, setSearchUser] = useState("");
  const [searchAction, setSearchAction] = useState("");
  const [searchId, setSearchId] = useState("");
  const [actionsList, setActionsList] = useState([]);

  useEffect(() => {
    if (role === "Admin" || role === "Payroll Processor") {
      GetAllAuditTrail()
        .then((res) => {
          let logs = res.data || [];
          logs = logs.sort((a, b) => new Date(b.timeStamp) - new Date(a.timeStamp));
          setAuditLogs(logs);
          setFilteredLogs(logs);

          const actions = [...new Set(logs.map(l => l.actionName))];
          setActionsList(actions);
        })
        .catch((err) => console.error("Error fetching audit logs:", err))
        .finally(() => setLoading(false));
    } else {
      setLoading(false);
    }
  }, [role]);

  useEffect(() => {
    let temp = [...auditLogs];

    if (searchUser) {
      temp = temp.filter(l => l.userName.toLowerCase().includes(searchUser.toLowerCase()));
    }
    if (searchAction) {
      temp = temp.filter(l => l.actionName === searchAction);
    }
    if (searchId) {
      temp = temp.filter(l => l.id === parseInt(searchId));
    }

    setFilteredLogs(temp);
  }, [searchUser, searchAction, searchId, auditLogs]);

  if (loading) return <p>Loading audit trail...</p>;
  if (role !== "Admin" && role !== "Payroll Processor") 
    return <p>Access Denied: Only Admin or Payroll Processor can see audit logs.</p>;

  const Layout = role === "Payroll Processor" ? PayrollProcessorLayout : AdminLayout;

  return (
    <Layout>
      <div className="audit-trail-container audit-trail-scroll">
        <h2>Audit Trail Logs</h2>

        <div className="audit-trail-filters">
          <input
            type="text"
            placeholder="Search by Username"
            value={searchUser}
            onChange={(e) => setSearchUser(e.target.value)}
          />

          <select
            value={searchAction}
            onChange={(e) => setSearchAction(e.target.value)}
          >
            <option value="">All Actions</option>
            {actionsList.map((action, idx) => (
              <option key={idx} value={action}>{action}</option>
            ))}
          </select>

          <input
            type="number"
            placeholder="Search by Log ID"
            value={searchId}
            onChange={(e) => setSearchId(e.target.value)}
          />
        </div>

        <table className="audit-trail-table">
          <thead>
            <tr>
              <th>S.No</th>
              <th>User</th>
              <th>Action</th>
              <th>Entity</th>
              <th>Entity Id</th>
              <th>Old Value</th>
              <th>New Value</th>
              <th>Timestamp</th>
              <th>IP Address</th>
            </tr>
          </thead>
          <tbody>
            {filteredLogs.length === 0 ? (
              <tr>
                <td colSpan="9">No audit logs found</td>
              </tr>
            ) : (
              filteredLogs.map((log, index) => (
                <tr key={log.id}>
                  <td>{index + 1}</td>
                  <td>{log.userName}</td>
                  <td>{log.actionName}</td>
                  <td>{log.entityName}</td>
                  <td>{log.entityId}</td>
                  <td>{log.oldValue}</td>
                  <td>{log.newValue}</td>
                  <td>{new Date(log.timeStamp).toLocaleString()}</td>
                  <td>{log.ipAddress}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </Layout>
  );
};

export default AuditTrail;
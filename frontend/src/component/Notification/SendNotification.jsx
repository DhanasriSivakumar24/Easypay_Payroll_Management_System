import { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { GetNotificationChannel } from "../../service/masterTable.service";
import { GetAllEmployees } from "../../service/employee.service";
import { SendNotificationAPI } from "../../service/notification.service";
import AdminLayout from "../navbar/AdminLayout";
import "./sendNotification.css";

const SendNotification = () => {
  const { role } = useSelector((s) => s.auth);
  const [employees, setEmployees] = useState([]);
  const [channels, setChannels] = useState([]);

  const [userId, setUserId] = useState("");      // single user
  const [channelId, setChannelId] = useState(""); 
  const [message, setMessage] = useState("");
  const [sending, setSending] = useState(false);

  useEffect(() => {
    if (role !== "Admin" && role !== "HR Manager") return;

    GetAllEmployees()
      .then((r) => setEmployees(r.data || []))
      .catch((e) => console.error("Failed to load employees", e));

    GetNotificationChannel()
      .then((r) => setChannels(r.data || []))
      .catch((e) => console.error("Failed to load channels", e));
  }, [role]);

  const handleSend = async () => {
    if (!userId) return alert("Select a user");
    if (!channelId) return alert("Select a channel");
    if (!message.trim()) return alert("Enter a message");

    try {
      setSending(true);
      await SendNotificationAPI({
        userId: Number(userId),
        channelId: Number(channelId),
        message: message.trim(),
      });
      alert(" Notification sent!");
      setMessage("");
      setUserId("");
      setChannelId("");
    } catch (err) {
      // 401 usually means token missing/expired or wrong role
      if (err?.response?.status === 401) {
        alert("Unauthorized: please log in again or check your token/role.");
      } else if (err?.response?.status === 403) {
        alert("Forbidden: your role cannot send notifications.");
      } else {
        alert(" Failed to send notification.");
      }
      console.error(err);
    } finally {
      setSending(false);
    }
  };

  // gate the page by role
  if (role !== "Admin" && role !== "HR Manager") {
    return (
      <AdminLayout>
        <div className="not-allowed"> You are not allowed to send notifications.</div>
      </AdminLayout>
    );
  }

  return (
    <AdminLayout>
      <div className="send-notification-container">
        <h2 className="notification-title"> Send Notification</h2>

        <div className="form-grid">
          <div className="form-field">
            <label>User</label>
            <select
              className="notification-select"
              value={userId}
              onChange={(e) => setUserId(e.target.value)}
            >
              <option value="">Select user</option>
              {employees.map((emp) => (
                <option key={emp.id} value={emp.id}>
                  {emp.name} ({emp.email})
                </option>
              ))}
            </select>
          </div>

          <div className="form-field">
            <label>Channel</label>
            <select
              className="notification-select"
              value={channelId}
              onChange={(e) => setChannelId(e.target.value)}
            >
              <option value="">Select channel</option>
              {channels.map((c) => (
                <option key={c.id} value={c.id}>
                  {c.name}
                </option>
              ))}
            </select>
          </div>
        </div>

        <label>Message</label>
        <textarea
          className="message-box"
          placeholder="Write your notification..."
          value={message}
          onChange={(e) => setMessage(e.target.value)}
        />

        <button className="send-btn" onClick={handleSend} disabled={sending}>
          {sending ? "Sending..." : " Send Notification"}
        </button>
      </div>
    </AdminLayout>
  );
};

export default SendNotification;

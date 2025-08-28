import React from "react";
import { CheckCircleFill, XCircleFill, ClockFill } from "react-bootstrap-icons";

const TimesheetTable = ({ timesheets }) => {
  return (
    <div className="card">
      <div className="card-body">
        <table className="table table-hover">
          <thead className="table-light">
            <tr>
              <th>Date</th>
              <th>Hours</th>
              <th>Description</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            {timesheets.length === 0 ? (
              <tr>
                <td colSpan="4" className="text-center text-muted">No records found</td>
              </tr>
            ) : (
              timesheets.map((t, i) => (
                <tr key={i}>
                  <td>{t.date}</td>
                  <td>{t.hours}</td>
                  <td>{t.description}</td>
                  <td>
                    {t.status === "Approved" && (
                      <span className="text-success d-flex align-items-center gap-1">
                        <CheckCircleFill /> Approved
                      </span>
                    )}
                    {t.status === "Rejected" && (
                      <span className="text-danger d-flex align-items-center gap-1">
                        <XCircleFill /> Rejected
                      </span>
                    )}
                    {t.status === "Pending" && (
                      <span className="text-warning d-flex align-items-center gap-1">
                        <ClockFill /> Pending
                      </span>
                    )}
                    {t.status === "Submitted" && (
                      <span className="text-primary">Submitted</span>
                    )}
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default TimesheetTable;

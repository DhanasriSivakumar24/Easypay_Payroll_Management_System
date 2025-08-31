import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { GetPayrollByEmployeeId } from "../../service/payroll.service";
import { GetPersonalInfo } from "../../service/employee.service";
import jsPDF from "jspdf";
import autoTable from "jspdf-autotable";
import EmployeeLayout from "../navbar/EmployeeLayout";
import "./myPayStubs.css";

const MyPayStubs = () => {
  const { employeeId } = useSelector((state) => state.auth);
  const [paystubs, setPaystubs] = useState([]);
  const [employeeInfo, setEmployeeInfo] = useState(null);
  const [search, setSearch] = useState("");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (!employeeId) return;

    GetPayrollByEmployeeId(employeeId)
      .then((res) => setPaystubs(res.data || []))
      .catch((err) => console.error("Error fetching pay stubs:", err));

    GetPersonalInfo(employeeId)
      .then((res) => setEmployeeInfo(res.data))
      .catch((err) => console.error("Failed to fetch personal info", err))
      .finally(() => setLoading(false));
  }, [employeeId]);

  const formatDate = (dateStr) => {
    if (!dateStr) return "-";
    const options = { year: "numeric", month: "short", day: "numeric" };
    return new Date(dateStr).toLocaleDateString("en-IN", options);
  };

  const formatCurrency = (amount) => {
    if (amount === null || amount === undefined) return "₹0.00";
    return (
      "₹" +
      new Intl.NumberFormat("en-IN", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }).format(amount)
    );
  };

  const downloadPaystub = (paystub) => {
    if (!employeeInfo) return;

    const doc = new jsPDF("p", "mm", "a4");
    const pageWidth = doc.internal.pageSize.getWidth();
    let yPos = 20;

    // --- Title ---
    doc.setFont("helvetica", "bold");
    doc.setFontSize(16);
    doc.text("Pay Stub", pageWidth / 2, yPos, { align: "center" });

    yPos += 20;

    // --- Employee Info Box ---
    doc.setDrawColor(200, 200, 200);
    doc.rect(20, yPos, pageWidth - 40, 40);

    doc.setFontSize(10);
    doc.setFont("helvetica", "bold");

    // Left Column
    doc.text("Employee Name:", 25, yPos + 7);
    doc.text("Employee ID:", 25, yPos + 14);
    doc.text("Department:", 25, yPos + 21);
    doc.text("Pay Period:", 25, yPos + 28);

    doc.setFont("helvetica", "normal");
    doc.text(`${employeeInfo.firstName ?? ""} ${employeeInfo.lastName ?? ""}`, 60, yPos + 7);
    doc.text((employeeInfo.id ?? "").toString(), 60, yPos + 14);
    doc.text(employeeInfo.departmentName ?? "", 60, yPos + 21);
    doc.text(`${formatDate(paystub.periodStart)} - ${formatDate(paystub.periodEnd)}`, 60, yPos + 28);

    // Right Column
    doc.setFont("helvetica", "bold");
    doc.text("Role:", pageWidth / 2 + 5, yPos + 7);
    doc.text("Status:", pageWidth / 2 + 5, yPos + 14);
    doc.text("Phone Number:", pageWidth / 2 + 5, yPos + 21);

    doc.setFont("helvetica", "normal");
    doc.text(employeeInfo.roleName ?? "", pageWidth / 2 + 35, yPos + 7);
    doc.text(paystub.statusName ?? "Unknown", pageWidth / 2 + 35, yPos + 14);
    doc.text(employeeInfo.phoneNumber ?? "N/A", pageWidth / 2 + 35, yPos + 21);

    yPos += 50;

    // --- Particulars Table
    const particularsTableData = [
      ["Particulars", "Amount"],
      ["Basic Pay", formatCurrency(paystub.basicPay ?? 0)],
      ["Deductions", formatCurrency(paystub.deductions ?? 0)],
    ];

    autoTable(doc, {
      startY: yPos,
      head: [particularsTableData[0]],
      body: particularsTableData.slice(1),
      theme: "grid",
      headStyles: { fillColor: [240, 240, 240], textColor: [0, 0, 0], fontStyle: "bold" },
      styles: { fontSize: 10, cellPadding: 3, halign: "left" },
      columnStyles: { 1: { halign: "right", cellWidth: 50 } },
      margin: { left: 20, right: 20 },
    });

    const tableEnd = doc.lastAutoTable.finalY;
    yPos = tableEnd + 10;

    // --- Summary Section ---
    doc.setFont("helvetica", "bold");
    doc.setFontSize(10);
    doc.text("Summary", 20, yPos);

    yPos += 5;
    doc.setDrawColor(0);
    doc.setLineWidth(0.5);
    doc.line(20, yPos, pageWidth - 20, yPos);

    doc.setFontSize(10); 

    yPos += 5;
    doc.setFont("helvetica", "bold");
    doc.text("Gross Pay:", 25, yPos);
    doc.setFont("helvetica", "normal");
    doc.text(formatCurrency(paystub.grossPay ?? 0), pageWidth - 25, yPos, { align: "right" });

    yPos += 7;
    doc.setFont("helvetica", "bold");
    doc.text("Total Deductions:", 25, yPos);
    doc.setFont("helvetica", "normal");
    doc.text(formatCurrency(paystub.deductions ?? 0), pageWidth - 25, yPos, { align: "right" });

    yPos += 7;
    doc.line(20, yPos, pageWidth - 20, yPos);

    yPos += 5;
    doc.setFont("helvetica", "bold");
    doc.text("Net Pay:", 25, yPos);
    doc.setTextColor(0, 128, 0); 
    doc.text(formatCurrency(paystub.netPay ?? 0), pageWidth - 25, yPos, { align: "right" });
    doc.setTextColor(0, 0, 0); 

    doc.save(`Paystub_${formatDate(paystub.periodStart)}.pdf`);
  };

  const filteredPaystubs = paystubs.filter((p) => {
    const status = p?.statusName?.toLowerCase() || "";
    const period = `${p?.periodStart} - ${p?.periodEnd}`.toLowerCase();
    return status.includes(search.toLowerCase()) || period.includes(search.toLowerCase());
  });

  return (
    <EmployeeLayout>
      <div className="paystub-container">
        <div className="header-row">
          <h2>My Pay Stubs</h2>
          <div className="actions">
            <input
              type="text"
              className="search-input"
              placeholder="Search by Period or Status..."
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
          </div>
        </div>

        {loading ? (
          <div className="no-data">Loading pay stubs...</div>
        ) : (
          <div className="paystub-card">
            <table className="paystub-table">
              <thead>
                <tr>
                  <th>S.No</th>
                  <th>Pay Period</th>
                  <th>Gross Pay</th>
                  <th>Deductions</th>
                  <th>Net Pay</th>
                  <th>Status</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {filteredPaystubs.length > 0 ? (
                  filteredPaystubs.map((p, index) => (
                    <tr key={p.id}>
                      <td>{index + 1}</td>
                      <td>{formatDate(p.periodStart)} - {formatDate(p.periodEnd)}</td>
                      <td>{formatCurrency(p.basicPay)}</td>
                      <td>{formatCurrency(p.deductions)}</td>
                      <td className="netpay">{formatCurrency(p.netPay)}</td>
                      <td>
                        <span className={`status-badge ${p?.statusName?.toLowerCase()}`}>
                          {p?.statusName || "Unknown"}
                        </span>
                      </td>
                      <td>
                        <button className="download-btn" onClick={() => downloadPaystub(p)}>Download PDF</button>
                      </td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="7" className="no-data">No pay stubs found</td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </EmployeeLayout>
  );
};

export default MyPayStubs;
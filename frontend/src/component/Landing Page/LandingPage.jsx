import React from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import 'bootstrap-icons/font/bootstrap-icons.css';
import "./landingPage.css";

export default function LandingPage() {
  const features = [
    { icon: "bi-cash-stack", title: "Automated Payroll", desc: "Accurate salary, allowance, and deduction calculations with zero errors." },
    { icon: "bi-shield-check", title: "Compliance Management", desc: "Easily stay compliant with taxes, labor laws, and other legal requirements for your business." },
    { icon: "bi-person-circle", title: "Employee Self-Service", desc: "Employees can view pay stubs, submit leaves, and update info securely." },
    { icon: "bi-graph-up", title: "Analytics & Reports", desc: "Gain real time insights with dashboards, compliance reports, and audit trails." },
  ];

  return (
    <div>
      <nav className="navbar navbar-expand-lg navbar-light bg-white shadow-sm fixed-top">
        <div className="container">
          <a className="navbar-brand fw-bold text-primary" href="#home">EasyPay</a>
          <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="collapse navbar-collapse" id="navbarNav">
            <ul className="navbar-nav ms-auto">
              <li className="nav-item"><a className="nav-link active" href="#home">Home</a></li>
              <li className="nav-item"><a className="nav-link" href="#features">Features</a></li>
              <li className="nav-item"><a className="btn btn-primary ms-3 px-4" href="/login">Login</a></li>
            </ul>
          </div>
        </div>
      </nav>

      <section id="home" className="hero-section d-flex align-items-center">
        <div className="container text-center text-lg-start">
          <div className="row align-items-center">
            <div className="col-lg-6">
              <h1 className="display-4 fw-bold text-dark">EasyPay – The Payroll Management System</h1>
              <p className="lead my-4 text-muted">A robust and user-friendly solution designed to automate payroll, ensure compliance, and improve employee satisfaction.</p>
              <a href="/login" className="btn btn-primary btn-lg me-3">Get Started</a>
              <a href="#features" className="btn btn-outline-secondary btn-lg">Explore Features</a>
            </div>
            <div className="col-lg-6 text-center">
              <img src="../Illustrations.png" alt="Payroll Illustration" className="hero-img"/>
            </div>
          </div>
        </div>
      </section>

      <section id="features" className="py-5">
        <div className="container">
          <h2 className="fw-bold text-center mb-5">Key Features</h2>
          <div className="row g-4 justify-content-center">
            {features.map((feature, idx) => (
              <div key={idx} className="col-md-6 col-lg-3">
                <div className="card feature-card text-center p-4">
                  <div className="icon-circle mb-3"><i className={`bi ${feature.icon}`}></i></div>
                  <h5 className="fw-bold">{feature.title}</h5>
                  <p className="feature-desc">{feature.desc}</p>
                </div>
              </div>
            ))}
          </div>
        </div>
      </section>

      <footer className="py-5 bg-dark text-light">
        <div className="container text-center">
          <div className="footer-links mb-4">
            <a href="#home" className="me-3">Home</a>
            <a href="#features" className="me-3">Features</a>
            <a href="/login">Login</a>
          </div>
          <small>© {new Date().getFullYear()} EasyPay. All rights reserved.</small>
        </div>
      </footer>
    </div>
  );
}

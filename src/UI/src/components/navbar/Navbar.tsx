import React from "react";
// import { Dropdown } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { logo, logo_mini } from "../../resources/images";
export default function Navbar() {
  const naviagte = useNavigate();
  const toggleOffcanvas = () => {
    document?.querySelector(".sidebar-offcanvas")?.classList.toggle("active");
  };

  return (
    <nav className="navbar default-layout-navbar col-lg-12 col-12 p-0 fixed-top d-flex flex-row">
      <div className="text-center navbar-brand-wrapper d-flex align-items-center justify-content-center">
        <Link className="navbar-brand brand-logo" to="/">
          <img src={logo} alt="logo" />
        </Link>
        <Link className="navbar-brand brand-logo-mini" to="/">
          <img src={logo_mini} alt="logo" />
        </Link>
      </div>
      <div className="navbar-menu-wrapper d-flex align-items-stretch">
        <button
          className="navbar-toggler navbar-toggler align-self-center"
          type="button"
          onClick={() => document.body.classList.toggle("sidebar-icon-only")}
        >
          <span className="bi bi-list"></span>
        </button>

        <ul className="navbar-nav navbar-nav-right">
          <li className="nav-item nav-settings d-none d-lg-block">
            <button
              type="button"
              className="nav-link border-0"
              onClick={() => naviagte("/login-page")}
            >
              <i className="bi bi-power"></i>
            </button>
          </li>
        </ul>
        <button
          className="navbar-toggler navbar-toggler-right d-lg-none align-self-center"
          type="button"
          onClick={toggleOffcanvas}
        >
          <span className="bi bi-list-ul"></span>
        </button>
      </div>
    </nav>
  );
}

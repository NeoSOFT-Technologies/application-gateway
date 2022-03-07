import React, { Component } from "react";
import { Dropdown } from "react-bootstrap";
import { Link } from "react-router-dom";
// import {  } from "react-i18next";
import { host } from "../config/URL";

class Navbar extends Component {
  toggleOffcanvas() {
    document.querySelector(".sidebar-offcanvas").classList.toggle("active");
  }
  //toggleRightSidebar() {
  //  document.querySelector(".right-sidebar").classList.toggle("open");
  //}
  render() {
    return (
      <nav className="navbar default-layout-navbar col-lg-12 col-12 p-0 fixed-top d-flex flex-row">
        <div className="text-center navbar-brand-wrapper d-flex align-items-center justify-content-center">
          <Link className="navbar-brand brand-logo" to="/">
            <img src={`${host}/images/logo.svg`} alt="logo" />
          </Link>
          <Link className="navbar-brand brand-logo-mini" to="/">
            <img src={`${host}/images/logo-mini.png`} alt="logo" />
          </Link>
        </div>
        <div className="navbar-menu-wrapper d-flex align-items-stretch">
          <button
            className="navbar-toggler navbar-toggler align-self-center"
            type="button"
            onClick={() => document.body.classList.toggle("sidebar-icon-only")}
          >
            <span className="mdi mdi-menu"></span>
          </button>
          <ul className="navbar-nav navbar-nav-right">
            <li className="nav-item nav-profile">
              <Dropdown alignRight>
                <Dropdown.Toggle className="nav-link">
                  <div className="nav-profile-img">
                    <img src={`${host}/images/faces/face1.jpg`} alt="user" />
                    <span className="availability-status online"></span>
                  </div>
                  <div className="nav-profile-text">
                    <p className="mb-1 text-black">
                      <>David</>
                    </p>
                  </div>
                </Dropdown.Toggle>

                <Dropdown.Menu className="navbar-dropdown">
                  <Dropdown.Item
                    href="!#"
                    onClick={(evt) => evt.preventDefault()}
                  >
                    <i className="mdi mdi-cached mr-2 text-success"></i>
                    <>Activity Log</>
                  </Dropdown.Item>
                  <Dropdown.Item
                    href="!#"
                    onClick={(evt) => evt.preventDefault()}
                  >
                    <i className="mdi mdi-logout mr-2 text-primary"></i>
                    <>Signout</>
                  </Dropdown.Item>
                </Dropdown.Menu>
              </Dropdown>
            </li>
            <li className="nav-item nav-logout d-none d-lg-block">
              <a
                className="nav-link"
                href="!#"
                onClick={(event) => event.preventDefault()}
              >
                <i className="mdi mdi-power"></i>
              </a>
            </li>
          </ul>
          <button
            className="navbar-toggler navbar-toggler-right d-lg-none align-self-center"
            type="button"
            onClick={this.toggleOffcanvas}
          >
            <span className="mdi mdi-menu"></span>
          </button>
        </div>
      </nav>
    );
  }
}

export default Navbar;

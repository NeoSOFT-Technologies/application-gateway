import React from "react";
import { Link, useLocation } from "react-router-dom";
//import { host } from "../config/URL";
import withRouter from "../WithRouter";
function AdminSidebar() {
  const location = useLocation();
  const isPathActive = (path) => {
    return location.pathname.startsWith(path);
  };
  return (
    <>
      <nav className="sidebar sidebar-offcanvas" id="sidebar">
        <ul className="nav">
          <li
            className={isPathActive("/apis") ? "nav-item active" : "nav-item"}
          >
            <Link className="nav-link" to="/apis">
              <span className="menu-title">
                <>APIs</>
              </span>
              <i className="mdi mdi-format-list-bulleted menu-icon"></i>
            </Link>
          </li>
          <li
            className={isPathActive("/keys") ? "nav-item active" : "nav-item"}
          >
            <Link className="nav-link" to="/keys">
              <span className="menu-title">
                <>Keys</>
              </span>
              <i className="mdi mdi-key menu-icon"></i>
            </Link>
          </li>
          <li
            className={
              isPathActive("/policies") ? "nav-item active" : "nav-item"
            }
          >
            <Link className="nav-link" to="/policies">
              <span className="menu-title">
                <>Policies</>
              </span>
              <i className="mdi  mdi-file menu-icon"></i>
            </Link>
          </li>
        </ul>
      </nav>
    </>
  );
}
export default withRouter(AdminSidebar);

import React, { useEffect, useState } from "react";
// import { useSelector } from "react-redux";
// import "node_modules/bootstrap-icons/font/ootstrap-icons.css";
import "bootstrap/dist/css/bootstrap.min.css";
import { Link, useLocation } from "react-router-dom";
import adminRoutes from "../../routes/admin";
// import tenantRoutes from "../../routes/tenants";
// import { RootState } from "../../store";
// import { IUserDataState } from "../../types";
import withRouter from "../../WithRouter";

export const Sidebar = () => {
  const location = useLocation();
  const isPathActive = (path: string) => {
    return location.pathname.startsWith(path);
  };
  // const user: IUserDataState = useSelector(
  //   (state: RootState) => state.userData
  // );
  const [routes, setRoutes] = useState([{ path: "", title: "", icon: "" }]);
  useEffect(() => {
    setRoutes(adminRoutes);
  }, []);

  return (
    <>
      <nav className="sidebar sidebar-offcanvas" id="sidebar">
        <ul className="nav">
          <li className="nav-item nav-profile">
            <a
              href="!#"
              className="nav-link"
              onClick={(evt) => evt.preventDefault()}
            >
              <div className="nav-profile-image">
                <img
                  src={`${process.env.REACT_APP_HOST}/global/images/faces/face1.jpg`}
                  alt="profile"
                />
                <span className="login-status online"></span>{" "}
                {/* change to offline or busy as needed */}
              </div>
              <div className="nav-profile-text">
                <span className="font-weight-bold mb-2">
                  <>David</>
                </span>
                <span className="text-secondary text-small">
                  <></>
                </span>
              </div>
              <i className="bi bi-bookmark-star-fill text-success nav-profile-badge"></i>
            </a>
          </li>
          {routes.map((route, index) => (
            <li
              key={`route${index}`}
              className={
                isPathActive(route.path) ? "nav-item active" : "nav-item"
              }
            >
              <Link className="nav-link" to={route.path}>
                <span className="menu-title">
                  <>{route.title}</>
                </span>
                <i className={route.icon}></i>
              </Link>
            </li>
          ))}
        </ul>
      </nav>
    </>
  );
};
export default withRouter(Sidebar);

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
    let classholder = "nav-item";
    if (
      path === "api" &&
      (location.pathname === "/apilist" ||
        location.pathname === "/createapi" ||
        location.pathname === "/update")
    ) {
      classholder = "nav-item active";
    } else if (
      path === "key" &&
      (location.pathname === "/keylist" ||
        location.pathname === "/createkey" ||
        location.pathname === "/update")
    ) {
      classholder = "nav-item active";
    } else if (
      path === "policy" &&
      (location.pathname === "/policylist" ||
        location.pathname === "/createpolicy" ||
        location.pathname === "/update")
    ) {
      classholder = "nav-item active";
    }
    return classholder;
  };
  // const user: IUserDataState = useSelector(
  //   (state: RootState) => state.userData
  // );
  const [routes, setRoutes] = useState([
    { path: "", title: "", icon: "", id: "" },
  ]);
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
              id={`${route.id}`}
              key={`route${index}`}
              className={isPathActive(route.id)}
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

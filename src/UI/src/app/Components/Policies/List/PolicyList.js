import React, { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { getPolicyList } from "../../../redux/actions/PolicyActions";
import RenderList from "../../../shared/RenderList";
import Spinner from "../../../shared/Spinner";
import "react-toastify/dist/ReactToastify.css";
import { toast } from "react-toastify";
toast.configure();
function Policies() {
  const dispatch = useDispatch();
  const PolicyList = useSelector((state) => state.setPolicyList);
  const [selected, setSelected] = useState(1);
  const failure = (data) =>
    toast.error(data, { position: toast.POSITION.TOP_RIGHT, autoClose: 3000 });
  useEffect(() => {
    dispatch({ type: "POLICY_LOADING" });
    //console.log("dispatch of loading", PolicyList);
    mainCall(1);
  }, []);
  const handlePageClick = (selected) => {
    mainCall(selected);
    setSelected(selected);
  };

  const mainCall = (currentPage) => {
    try {
      getPolicyList(currentPage)
        .then((res) => {
          console.log("in Policy List", res.payload.Data);
          dispatch(res);
          console.log("main call", PolicyList);
        })
        .catch((err) => {
          console.log(err.message);
          dispatch({
            type: "POLICY_LOADING_FAILURE",
            payload: err.message,
          });
        });
    } catch (err) {
      console.log(err.message);
    }
  };
  const buttonClick = (e) => {
    e.preventDefault();
    setSelected(1);
    mainCall(1);
  };
  //Iterable function
  function isIterable(obj) {
    // checks for null and undefined
    if (obj == null) {
      return false;
    }
    return typeof obj[Symbol.iterator] === "function";
  }
  console.log("policylist", PolicyList);
  console.log(
    "policyList before datalist",
    isIterable(PolicyList.list) === true ? PolicyList[0] : {}
  ); //isIterable(PolicyList.list)
  const actions = [
    {
      className: "btn btn-sm btn-success",
      iconClassName: "mdi mdi-pencil",
    },
    {
      className: "btn btn-sm btn-danger",
      iconClassName: "mdi mdi-delete",
    },
  ];
  const datalist = {
    list:
      isIterable(PolicyList.list) === true && PolicyList.list.length > 0
        ? PolicyList.list[0]
        : [],
    fields: ["State", "Name", "Apis", "AuthType"],
  };
  const headings = [
    { title: "State" },
    { title: "Policy Name", className: "w-100" },
    { title: "Access Rights", className: "text-center" },
    { title: "Authentication Type", className: "text-center" },
    { title: "Action", className: "text-center" },
  ];
  // if (PolicyList.error != null && PolicyList.error.length > 0) {
  //   failure(PolicyList.error);
  //   return (
  //     <span>
  //       {" "}
  //       <Spinner />{" "}
  //     </span>
  //   );
  // } else {
  if (PolicyList.loading) {
    return (
      <span>
        <Spinner />
      </span>
    );
  } else if (PolicyList.error) {
    failure(PolicyList.error);
    return <></>;
  } else {
    return (
      <>
        <div className="col-lg-12 grid-margin stretch-card">
          <div className="card">
            <div className="card-body">
              <div className="align-items-center">
                <div className="search-field justify-content-around">
                  <form className="h-50" onSubmit={(e) => buttonClick(e)}>
                    <div className="input-group">
                      <input
                        type="text"
                        className="form-control bg-parent border-1"
                        placeholder="Search Policies"
                      />
                      <button
                        className=" btn  btn-success btn-sm"
                        onClick={(e) => buttonClick(e)}
                      >
                        <i className=" mdi mdi-magnify"></i>
                      </button>
                    </div>
                  </form>
                </div>
              </div>
              <br />
              <div>
                <button
                  className=" btn  btn-success btn-sm d-flex float-right mb-2"
                  onClick={(e) => buttonClick(e)}
                >
                  &nbsp;
                  <span className=" mdi mdi-plus"> </span>
                </button>
                &nbsp;
              </div>
              <div className="table-responsive">
                {PolicyList.loading ? (
                  <span>
                    <Spinner />
                  </span>
                ) : (
                  <RenderList
                    headings={headings}
                    data={datalist}
                    actions={actions}
                    handlePageClick={handlePageClick}
                    pageCount={PolicyList.count}
                    total={PolicyList.totalCount}
                    selected={selected}
                    // error={PolicyList.error}
                  />
                )}
              </div>
            </div>
          </div>
        </div>
      </>
    );
  }
}

export default Policies;

import React, { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { getAPIList } from "../../../redux/actions/ApiActions";
import RenderList from "../../../shared/RenderList";
import Spinner from "../../../shared/Spinner";
import "react-toastify/dist/ReactToastify.css";
import { toast } from "react-toastify";

toast.configure();
function APIList() {
  const dispatch = useDispatch();
  const ApiList = useSelector((state) => state.setAPIList);
  const [selected, setSelected] = useState(1);
  const failure = (data) =>
    toast.error(data, { position: toast.POSITION.TOP_RIGHT, autoClose: 3000 });

  //let currentPage = null;
  useEffect(() => {
    dispatch({ type: "API_LOADING" });
    //console.log("dispatch of loading", ApiList);
    mainCall(1);
  }, []);

  const handlePageClick = (selected) => {
    mainCall(selected);
    setSelected(selected);
  };

  const mainCall = (currentPage) => {
    try {
      //console.log(currentPage);
      getAPIList(currentPage)
        .then((res) => {
          //console.log("in Api List", res);
          dispatch(res);
          console.log("main call", ApiList);
        })
        .catch((err) => {
          console.log(err.message);
          //console.warn(err.message);
          dispatch({
            type: "API_LOADING_FAILURE",
            payload: err.message,
          });
        });
    } catch (err) {
      console.log(err);
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
  //console.log("apilist", ApiList);
  //console.log("ApiList before datalist", isIterable(ApiList.list));
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
  //console.log("apilist", isIterable(ApiList.list) === true ? ApiList : {});
  let datalist = {
    list:
      isIterable(ApiList.list) === true && ApiList.list.length > 0
        ? ApiList.list[0]
        : [],
    fields: ["Name", "TargetUrl", "IsActive", "CreatedDate"],
  };
  const headings = [
    { title: "Name" },
    { title: "Target Url" },
    { title: "Status" },
    { title: "Created Date" },
    { title: "Action", className: "text-center" },
  ];
  if (ApiList.loading) {
    return (
      <span>
        <Spinner />
      </span>
    );
  } else if (ApiList.error) {
    failure(ApiList.error);
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
                  <span className="mdi mdi-plus"></span> &nbsp;
                </button>
              </div>
              <br />
              <div className="table-responsive">
                {ApiList.loading ? (
                  <span>
                    <Spinner />
                  </span>
                ) : (
                  <RenderList
                    headings={headings}
                    data={datalist}
                    actions={actions}
                    handlePageClick={handlePageClick}
                    pageCount={ApiList.count}
                    total={ApiList.totalCount}
                    selected={selected}
                    // error={ApiList.error}
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

export default APIList;

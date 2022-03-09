import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { getAPIList } from "../../../redux/actions/ApiActions";
import RenderList from "../../../shared/RenderList";
import Spinner from "../../../shared/Spinner";

function APIList() {
  const dispatch = useDispatch();
  const ApiList = useSelector((state) => state.setAPIList);

  useEffect(() => {
    dispatch({ type: "API_LOADING" });
    console.log("dispatch of loading", ApiList);
    mainCall();
  }, []);

  const mainCall = () => {
    try {
      getAPIList().then((res) => {
        console.log("in Api List", res.payload.Data.Apis);
        dispatch(res);
        console.log("main call", ApiList);
      });
    } catch (err) {
      console.log(err);
    }
  };

  //Iterable function
  function isIterable(obj) {
    // checks for null and undefined
    if (obj == null) {
      return false;
    }
    return typeof obj[Symbol.iterator] === "function";
  }
  console.log("apilist", ApiList);
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
  console.log("apilist", isIterable(ApiList.list) === true ? ApiList : {});
  const datalist = {
    list:
      isIterable(ApiList.list) === true && ApiList.list.length > 0
        ? ApiList.list[0]
        : [],
    fields: ["Name", "TargetUrl", "Status", "Created"],
  };
  const headings = [
    { title: "Name" },
    { title: "Target Url" },
    { title: "Status" },
    { title: "Created Date" },
    { title: "Action", className: "text-center" },
  ];
  return (
    <>
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div className="card-body">
            <div className="d-flex align-items-center justify-content-around">
              <div className="search-field col-lg-12">
                <form className="h-50">
                  <div className="input-group">
                    <input
                      type="text"
                      className="form-control bg-parent border-1"
                      placeholder="Search Api"
                    />
                    <button className=" btn  btn-success btn-sm">
                      <i className=" mdi mdi-magnify"></i>
                    </button>
                  </div>
                </form>
              </div>
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
                />
              )}
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default APIList;

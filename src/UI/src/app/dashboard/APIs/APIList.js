import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { getAPIList } from "../../redux/actions/ApiActions";
import RenderList from "../../shared/RenderList";
import Spinner from "../../shared/Spinner";

function APIList() {
  const dispatch = useDispatch();
  const ApiList = useSelector((state) => state.setAPIList);
  // const [api, setApi] = useState({
  //   ApiId: null,
  //   Name: null,
  //   ListenPath: null,
  //   TargetUrl: null,
  // });

  useEffect(() => {
    dispatch({ type: "API_LOADING" });
    console.log("dispatch of loading", ApiList);
    mainCall();
  }, []);

  const mainCall = () => {
    // getAPIList().then((res) => {
    //   console.log("in api List", res.payload.Data.Apis);
    //   dispatch(res);
    //   console.log("main call", ApiList);
    // });
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

  // const renderTenant = (val) => {
  //   setTenant(val);
  // };
  //Iterable function
  function isIterable(obj) {
    // checks for null and undefined
    if (obj == null) {
      return false;
    }
    return typeof obj[Symbol.iterator] === "function";
  }
  console.log("apilist", ApiList);
  console.log("ApiList before datalist", isIterable(ApiList.list));
  const datalist = {
    //list: [...ApiList.list],
    list: [isIterable(ApiList.list) === true ? ApiList.list[0] : {}],
    fields: ["ApiId", "Name", "ListenPath", "TargetUrl"],
  };
  const headings = [
    { title: "API ID" },
    { title: "Name" },
    { title: "Listen Path" },
    { title: "Target Url" },
    //{ title: "Action", className: "text-center" },
  ];
  return (
    <>
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div className="card-body">
            <div className="d-flex align-items-center justify-content-around">
              <h2 className="card-title">Tenant List</h2>
            </div>
            <div className="table-responsive">
              {ApiList.loading ? (
                <span>
                  <Spinner />
                </span>
              ) : (
                <RenderList headings={headings} data={datalist} />
              )}
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default APIList;

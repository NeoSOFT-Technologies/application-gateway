import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { getKeyList } from "../../../redux/actions/KeyActions";
import RenderList from "../../../shared/RenderList";
import Spinner from "../../../shared/Spinner";

function KeyList() {
  const dispatch = useDispatch();
  const keyslist = useSelector((state) => state.setKeyList);

  useEffect(() => {
    dispatch({ type: "Key_LOADING" });
    //console.log("dispatch of loading", keyslist);
    mainCall();
  }, []);

  const mainCall = () => {
    try {
      getKeyList().then((res) => {
        //console.log("in Key List", res.payload.Data.KeyDto);
        dispatch(res);
        //console.log("main call", keyslist);
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
  console.log("ApiList before datalist", isIterable(keyslist.list));
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
  console.log("apilist", isIterable(keyslist.list) === true ? keyslist : {});
  const datalist = {
    list:
      isIterable(keyslist.list) === true && keyslist.list.length > 0
        ? keyslist.list[0]
        : [],
    fields: ["KeyId", "AuthType", "Status", "Created"],
  };
  const headings = [
    { title: "Key ID" },
    { title: "Authentication Type", className: "w-100" },
    { title: "Status" },
    { title: "Created" },
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
                      placeholder="Search Keys"
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
              {keyslist.loading ? (
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

export default KeyList;

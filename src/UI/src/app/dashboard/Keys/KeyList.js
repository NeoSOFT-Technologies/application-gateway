import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { getKeyList } from "../../redux/actions/KeyActions";
import RenderList from "../../shared/RenderList";
import Spinner from "../../shared/Spinner";

function KeyList() {
  const dispatch = useDispatch();
  const keyslist = useSelector((state) => state.setKeyList);

  useEffect(() => {
    dispatch({ type: "Key_LOADING" });
    console.log("dispatch of loading", keyslist);
    mainCall();
  }, []);

  const mainCall = () => {
    try {
      getKeyList().then((res) => {
        console.log("in Key List", res.payload.Data.KeyDto);
        dispatch(res);
        console.log("main call", keyslist);
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
  const datalist = {
    list: [isIterable(keyslist.list) === true ? keyslist.list[0] : {}],
    fields: ["KeyId", "AuthType", "Status", "Created"],
  };
  const headings = [
    { title: "Key ID" },
    { title: "Authentication Type", className: "w-100" },
    { title: "Status" },
    { title: "Created" },
    //{ title: "Action", className: "text-center" },
  ];
  return (
    <>
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div className="card-body">
            <div className="d-flex align-items-center justify-content-around">
              <h2 className="card-title">Key List</h2>
            </div>
            <div className="table-responsive">
              {keyslist.loading ? (
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

export default KeyList;

import React, { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { getKeyList } from "../../../redux/actions/KeyActions";
import RenderList from "../../../shared/RenderList";
import Spinner from "../../../shared/Spinner";
import "react-toastify/dist/ReactToastify.css";
import { toast } from "react-toastify";

toast.configure();
function KeyList() {
  const dispatch = useDispatch();
  const keyslist = useSelector((state) => state.setKeyList);
  const [selected, setSelected] = useState(1);
  const failure = (data) =>
    toast.error(data, { position: toast.POSITION.TOP_RIGHT, autoClose: 3000 });
  useEffect(() => {
    dispatch({ type: "Key_LOADING" });
    //console.log("dispatch of loading", keyslist);
    mainCall(selected);
  }, []);
  const handlePageClick = (selected) => {
    mainCall(selected);
    setSelected(selected);
  };

  const mainCall = (currentPage) => {
    try {
      getKeyList(currentPage)
        .then((res) => {
          //console.log("in Key List", res.payload.Data.KeyDto);
          dispatch(res);
          //console.log("main call", keyslist);
        })
        .catch((err) => {
          console.log(err.message);
          //console.warn(err.message);
          dispatch({
            type: "KEY_LOADING_FAILURE",
            payload: err.message,
          });
        });
    } catch (err) {
      console.log(err);
      //failure(err);
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
  console.log("Key before datalist", isIterable(keyslist.list));
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
  console.log("Keylist", isIterable(keyslist.list) === true ? keyslist : {});
  const datalist = {
    list:
      isIterable(keyslist.list) === true && keyslist.list.length > 0
        ? keyslist.list[0]
        : [],
    fields: ["Id", "KeyName", "IsActive", "CreatedDate"],
  };
  const headings = [
    { title: "Key ID" },
    { title: "Key Name" },
    { title: "Status" },
    { title: "Created Date" },
    { title: "Action", className: "text-center" },
  ];
  if (keyslist.error != null && keyslist.error.length > 0) {
    failure(keyslist.error);
    return <span>{/* <Spinner /> */}</span>;
  } else {
    return (
      <>
        <div className="col-lg-12 grid-margin stretch-card">
          <div className="card">
            <div className="card-body">
              <div className="d-flex align-items-center justify-content-around">
                <div className="search-field col-lg-12">
                  <form className="h-50" onClick={(e) => buttonClick(e)}>
                    <div className="input-group">
                      <input
                        type="text"
                        className="form-control bg-parent border-1"
                        placeholder="Search Keys"
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
                  className=" btn  btn-success btn-md d-flex float-right"
                  onClick={(e) => buttonClick(e)}
                >
                  &nbsp;
                  <span className=" mdi mdi-plus"> </span>&nbsp;
                </button>
              </div>
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
                    handlePageClick={handlePageClick}
                    pageCount={keyslist.count}
                    total={keyslist.totalCount}
                    selected={selected}
                    error={keyslist.error}
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

export default KeyList;

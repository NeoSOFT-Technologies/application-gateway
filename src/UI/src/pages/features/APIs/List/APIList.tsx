import React, { useState, useEffect } from "react";
import { Modal } from "react-bootstrap";
import RenderList from "../../../../components/list/RenderList";
import { useNavigate } from "react-router-dom";
import { RootState } from "../../../../store";
import { useAppDispatch, useAppSelector } from "../../../../store/hooks";
import { getApiList } from "../../../../store/features/api/list/slice";
import {
  IApiData,
  IApiDataList,
  IApiListState,
} from "../../../../types/api/index";
import Spinner from "../../../../components/loader/Loader";
import { deleteApi } from "../../../../store/features/api/delete/slice";
import { useErrorHandler } from "react-error-boundary";
// import moment from "moment";
import { ToastAlert } from "../../../../components/ToasterAlert/ToastAlert";
import helper from "../../../../utils/helper";
import { getApiById } from "../../../../store/features/api/getById/slice";
function Bomb() {
  console.log("");
  // throw new Error("Boom");
}

export default function APIList() {
  const handleError = useErrorHandler();
  try {
    Bomb();
  } catch (err) {
    handleError(err);
  }
  const navigate = useNavigate();
  const [selected, setSelected] = useState(1);
  const [search, setSearch] = useState(" ");
  const dispatch = useAppDispatch();
  const apiList: IApiListState = useAppSelector(
    (state: RootState) => state.apiList
  );
  const [deleteshow, setDeleteshow] = useState(false);
  // const [checkactive, setCheckactive] = useState({
  //   btn1: false,
  //   btn2: false,
  //   btn3: true,
  // });
  const [datalist, setDataList] = useState<IApiDataList>({
    list: [],
    fields: [],
  });

  const mainCall = async (currentPage: number) => {
    // const resp = await
    dispatch(getApiList({ currentPage }));
    // handleError(resp.payload);
  };
  useEffect(() => {
    if (apiList.data && apiList.data?.Apis?.length > 0) {
      const listAPI: IApiData[] = [];
      apiList.data?.Apis.forEach((item) => {
        const api = helper(item);
        listAPI.push(api);
      });
      setDataList({
        list: [...listAPI],
        fields: ["Name", "TargetUrl", "Status", "CreatedDateTxt"],
      });
    }
  }, [apiList.data]);

  useEffect(() => {
    mainCall(1);
  }, []);

  const handlePageClick = (pageSelected: number) => {
    mainCall(pageSelected);
    setSelected(pageSelected);
  };

  const searchFilter = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();
    const newSearchList = datalist.list.filter((item) =>
      item.Name.toLocaleLowerCase().includes(search)
    );

    setDataList({
      list: [...newSearchList],
      fields: ["Name", "TargetUrl", "Status", "CreatedDateTxt"],
    });
  };
  //   const handleUserDetails = (val: ITenantUserData) => {
  //     console.log(val);
  //     // navigate("/userdetails");
  //     navigate(`/userdetails/${val.id}`, { state: { ...val } });
  //   };

  const NavigateToCreateApi = (
    val: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    val.preventDefault();
    navigate("/createapi");
  };

  const NavigateUpdate = (val: IApiData) => {
    if (val.Id) {
      dispatch(getApiById(val.Id));
      navigate("/update", {});
    }
  };

  function deleteapifromState(id: string) {
    const newState = datalist.list.filter((item) => item.Id !== id);
    console.log(newState);
    const pageCount = apiList.data?.TotalCount;
    if (newState.length === 0 && pageCount !== 1) {
      mainCall(selected - 1);
      setSelected(selected - 1);
      // } else if (newState.length === 0 && pageCount! < selected) {
      //   mainCall(selected + 1);
      //   setSelected(selected + 1);
    } else mainCall(selected);

    setDataList({
      list: [...newState],
      fields: ["Name", "TargetUrl", "Status", "CreatedDateTxt"],
    });
  }
  const deleteApiFunction = async (val: IApiData) => {
    if (val.Id) {
      if (window.confirm("Are you sure you want to delete this Api ?")) {
        const result = await dispatch(deleteApi(val.Id));

        if (result.meta.requestStatus === "rejected") {
          await ToastAlert(" Request failed ", "error");
        } else {
          deleteapifromState(val.Id);
          await ToastAlert("Api Deleted Successfully", "success");
        }
      }
    }
  };
  const headings = [
    { title: "Name" },
    { title: "Target Url" },
    { title: "Status" },
    { title: "Created Date" },
    { title: "Action" },
  ];
  const actions = [
    {
      className: "btn btn-sm btn-light",
      iconClassName: "bi bi-pencil-square menu-icon",
      buttonFunction: NavigateUpdate,
    },
    {
      className: "btn btn-sm btn-light",
      iconClassName: "bi bi-trash-fill menu-icon",
      // buttonFunction: () => setDeleteshow(true),
      buttonFunction: deleteApiFunction,
    },
  ];
  return (
    <>
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div className="card-body">
            <div className="align-items-center">
              <div>
                <button
                  className=" btn  btn-success btn-sm d-flex float-right mb-4"
                  onClick={(e) => NavigateToCreateApi(e)}
                >
                  {" "}
                  Create API &nbsp;
                  <span className="bi bi-plus-lg"></span> &nbsp;
                </button>
              </div>
              <div className="search-field justify-content-around ">
                <form className="h-50">
                  <div className="input-group">
                    <input
                      type="text"
                      className="form-control bg-parent border-1"
                      placeholder="Search Api"
                      onChange={(e) => setSearch(e.target.value)}
                    />
                    <button
                      className=" btn  btn-success btn-sm"
                      onClick={(e) => searchFilter(e)}
                    >
                      <i className=" bi bi-search"></i>
                    </button>
                  </div>
                </form>
              </div>
            </div>
            <br />
            {apiList.loading && <Spinner />}
            <div className="table-responsive">
              {!apiList.loading && apiList.error === null && apiList.data && (
                <RenderList
                  headings={headings}
                  data={datalist}
                  actions={actions}
                  handlePageClick={handlePageClick}
                  pageCount={apiList.data.TotalCount}
                  selected={selected}
                />
              )}
            </div>
          </div>
        </div>
      </div>

      <Modal show={deleteshow} onHide={() => setDeleteshow(false)} centered>
        <Modal.Header closeButton>
          <Modal.Title>Delete Api</Modal.Title>
        </Modal.Header>
        <Modal.Body>Do You want To delete the Api</Modal.Body>
        <Modal.Footer>
          <button
            type="button"
            className="btn-danger"
            onClick={() => deleteApiFunction}
          >
            Remove
          </button>
        </Modal.Footer>
      </Modal>
    </>
  );
}

import React, { useState, useEffect } from "react";
// import { Button } from "react-bootstrap";
import RenderList from "../../../../components/list/RenderList";
// import { useNavigate } from "react-router-dom";
import { RootState } from "../../../../store";
import { useAppDispatch, useAppSelector } from "../../../../store/hooks";
import { getKeyList } from "../../../../store/features/key/list/slice";
import {
  IKeyData,
  IKeyDataList,
  IKeyListState,
} from "../../../../types/key/index";
import Spinner from "../../../../components/loader/Loader";
import helper from "../../../../utils/helper";

export default function KeyList() {
  // const navigate = useNavigate();
  const [selected, setSelected] = useState(1);
  // const [search, setSearch] = useState(" ");
  const dispatch = useAppDispatch();
  const keyList: IKeyListState = useAppSelector(
    (state: RootState) => state.keyList
  );
  // const [checkactive, setCheckactive] = useState({
  //   btn1: false,
  //   btn2: false,
  //   btn3: true,
  // });
  const [datalist, setDataList] = useState<IKeyDataList>({
    list: [],
    fields: [],
  });
  const mainCall = (currentPage: number) => {
    dispatch(getKeyList({ currentPage }));
  };
  useEffect(() => {
    console.log("UseEffect", keyList.data);
    if (keyList.data && keyList.data?.Keys?.length > 0) {
      const listKey: IKeyData[] = [];
      keyList.data?.Keys.forEach((item) => {
        const key = helper(item);
        listKey.push(key);
      });
      setDataList({
        list: [...listKey],
        fields: ["Id", "KeyName", "Status", "CreatedDateTxt"],
      });
    }
  }, [keyList.data]);

  useEffect(() => {
    mainCall(1);
  }, []);

  const handlePageClick = (pageSelected: number) => {
    mainCall(pageSelected);
    setSelected(pageSelected);
  };

  const searchFilter = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    e.preventDefault();
    setSelected(1);
    mainCall(1);
  };
  //   const handleUserDetails = (val: ITenantUserData) => {
  //     console.log(val);
  //     // navigate("/userdetails");
  //     navigate(`/userdetails/${val.id}`, { state: { ...val } });
  //   };

  // const NavigateTenant = (val: IApiDetails) => {
  //   console.log(val);
  //   navigate("/tenantdetails", {
  //     state: { val },
  //   });
  // };

  const headings = [
    { title: "Key ID" },
    { title: "Key Name" },
    { title: "Status" },
    { title: "Created Date" },
    { title: "Action", className: "text-center" },
  ];
  const actions = [
    {
      className: "btn btn-sm btn-light",
      iconClassName: "bi bi-pencil-square menu-icon",
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
                  onClick={(e) => searchFilter(e)}
                >
                  {" "}
                  Create Key &nbsp;
                  <span className="bi bi-plus-lg"></span> &nbsp;
                </button>
              </div>
              <div className="search-field justify-content-around">
                <form className="h-50">
                  <div className="input-group">
                    <input
                      type="text"
                      className="form-control bg-parent border-1"
                      placeholder="Search Key"
                      // onChange={(e) => setSearch(e.target.value)}
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
            {keyList.loading && <Spinner />}
            <div className="table-responsive">
              {!keyList.loading && keyList.data && (
                <RenderList
                  headings={headings}
                  data={datalist}
                  actions={actions}
                  handlePageClick={handlePageClick}
                  pageCount={keyList.data.TotalCount}
                  selected={selected}
                />
              )}
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

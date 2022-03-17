import React, { useState, useEffect } from "react";
// import { Button } from "react-bootstrap";
import RenderList from "../../../../components/list/RenderList";
// import { useNavigate } from "react-router-dom";
import { RootState } from "../../../../store";
import { useAppDispatch, useAppSelector } from "../../../../store/hooks";
import { getPolicyList } from "../../../../store/features/policy/list/slice";
import {
  IPolicyDataList,
  IPolicyListState,
} from "../../../../types/Policy/index";
import Spinner from "../../../../components/loader/Loader";

export default function PolicyList() {
  // const navigate = useNavigate();
  const [selected, setSelected] = useState(1);
  // const [search, setSearch] = useState(" ");
  const dispatch = useAppDispatch();
  const policyList: IPolicyListState = useAppSelector(
    (state: RootState) => state.policyList
  );
  // const [checkactive, setCheckactive] = useState({
  //   btn1: false,
  //   btn2: false,
  //   btn3: true,
  // });
  const [datalist, setDataList] = useState<IPolicyDataList>({
    list: [],
    fields: [],
  });
  const mainCall = (currentPage: number) => {
    dispatch(getPolicyList({ currentPage }));
  };
  useEffect(() => {
    console.log("UseEffect", policyList.data);
    if (policyList.data) {
      setDataList({
        list: [...policyList.data.Policies],
        fields: ["State", "Name", "Apis", "AuthType"],
      });
    }
  }, [policyList.data]);

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
    { title: "State" },
    { title: "Policy Name", className: "w-100" },
    { title: "Access Rights", className: "text-center" },
    { title: "Authentication Type", className: "text-center" },
    { title: "Action", className: "text-center" },
  ];
  const actions = [
    {
      className: "btn btn-sm btn-dark",
      iconClassName: "bi bi-gear-fill",
    },
  ];
  return (
    <>
      <span className="w-50 text-right  d-inline-block">Policy Record</span>

      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div className="card-body">
            <div className="d-flex align-items-center justify-content-around">
              <h2 className="card-title">Policy List</h2>
              <div className="search-field ">
                <form className="h-50">
                  <div className="input-group">
                    <input
                      type="text"
                      className="form-control bg-parent border-1"
                      placeholder="Search Tenant"
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
            {policyList.loading && <Spinner />}
            <div className="table-responsive">
              {!policyList.loading && policyList.data && (
                <RenderList
                  headings={headings}
                  data={datalist}
                  actions={actions}
                  handlePageClick={handlePageClick}
                  pageCount={policyList.data.TotalCount}
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

import { Grid } from "gridjs-react";
import React, { useEffect } from "react";
import { setForm } from "../../../../../../../store/features/key/create/slice";
import { IPolicyListState } from "../../../../../../../store/features/policy/list";
import { getPolicyList } from "../../../../../../../store/features/policy/list/slice";
import {
  useAppDispatch,
  useAppSelector,
} from "../../../../../../../store/hooks";

export default function PolicyList() {
  const accessPolicyList: IPolicyListState = useAppSelector(
    (state) => state.policyListState
  );
  const StateKey = useAppSelector((RootState) => RootState.createKeyState);
  const dispatch = useAppDispatch();
  const mainCall = async (currentPage: number) => {
    dispatch(getPolicyList({ currentPage }));
  };
  useEffect(() => {
    mainCall(1);
  }, []);
  const handleAddClick = (Id: string) => {
    const list = [...StateKey.data.form.policies, Id];
    dispatch(setForm({ ...StateKey.data.form, policies: list }));
  };
  // const handleAddClick = async (Id: string) => {
  //   const formobj = [...rowInput];
  //   formobj.push(Id);

  //   // formobj.id = Id;

  //   setRowInput(formobj);

  //   // listP.push(Id);

  //   // setRowInput((prevFormData: any) => [...prevFormData, Id]);

  //   const list = [...StateKey.data.form.policies, rowInput];
  //   console.log("newObj", list);
  //   // console.log("listP", rowInput);
  //   console.log("list", rowInput);
  //   dispatch(
  //     setForm({
  //       ...StateKey.data.form,
  //       policies: list,
  //     })
  //   );
  //   /// setRowInput({ ...rowInput, policies: "" });
  //   // console.log(StateKey.data);
  // };
  console.log(StateKey.data.form);
  const gridTable = new Grid({
    columns: [
      {
        name: "Id",
        attributes: (cell: string) => {
          if (cell) {
            return {
              "data-cell-content": cell,
              onclick: () => handleAddClick(cell),
              style: "cursor: pointer",
            };
          }
        },
      },
      {
        name: "Name",
        attributes: (cell: string) => {
          if (cell) {
            return {
              "data-cell-content": cell,
              onclick: () => alert(cell),
              style: "cursor: pointer",
            };
          }
        },
      },
      "State",
      "Access Rights",
      "Auth Type",
    ],
    data: () =>
      accessPolicyList.data?.Policies.map((data) => [
        data.Id,
        data.Name,
        data.State ? "active" : "Inactive",
        data.Apis,
        data.AuthType,
      ]),
    search: true,
    className: {
      container: "table table-responsive table-bordered table-stripped",
    },
    style: {
      table: {
        width: "100%",
        border: "2px solid #ccc",
        innerWidth: "100%",
      },
      th: {
        color: "#000",
      },
    },
  });
  return (
    <div>
      <div className="card mb-3">
        <div>
          <div className="align-items-center justify-content-around">
            <div className="accordion" id="accordionSetting">
              <div className="accordion-item">
                <h2 className="accordion-header" id="headingOne">
                  <button
                    className="accordion-button"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#collapseOne"
                    aria-expanded="true"
                    aria-controls="collapseOne"
                  >
                    Apply Policy
                  </button>
                </h2>
                <div
                  id="collapseOne"
                  className="accordion-collapse collapse show"
                  aria-labelledby="headingOne"
                  data-bs-parent="#accordionSetting"
                >
                  <div className="accordion-body">
                    <div>
                      <Grid {...gridTable.props} />
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

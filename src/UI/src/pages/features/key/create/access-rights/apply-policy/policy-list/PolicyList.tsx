import { Grid } from "gridjs-react";
import React, { useEffect, useState } from "react";
import { setForm } from "../../../../../../../store/features/policy/create/slice";
import { IPolicyListState } from "../../../../../../../store/features/policy/list";
import { getPolicyList } from "../../../../../../../store/features/policy/list/slice";
// import { getPolicyById } from "../../../../../../../store/features/policy/update/slice";
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
  const [rowInput, setRowInput] = useState<any>({ id: [] });

  const listP: any = [];
  console.log(accessPolicyList);
  const handleAddClick = async (Id: string) => {
    const formobj = { ...rowInput, id: Id };
    // formobj.id = Id;

    setRowInput(formobj);

    listP.push(Id);

    const list = [
      ...StateKey.data.form.policies,
      { policies: rowInput },
      // listP,
      // // Id,
      // // { policies: Id }
    ];
    // console.log("listP", State);
    console.log("newObj", list);
    console.log("listP", listP);
    // console.log("list", rowInput);

    dispatch(
      setForm({
        ...StateKey.data.form,
        policies: list[0].policies.id,
      })
    );
    /// setRowInput({ ...rowInput, policies: "" });
    console.log(StateKey.data);
  };
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
                    Policy List
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

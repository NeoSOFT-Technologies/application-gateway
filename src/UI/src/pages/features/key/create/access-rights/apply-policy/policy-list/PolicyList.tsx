import { Grid } from "gridjs-react";
import React, { useEffect } from "react";
import { RootState } from "../../../../../../../store";
import { IPolicyListState } from "../../../../../../../store/features/policy/list";
import { getPolicyList } from "../../../../../../../store/features/policy/list/slice";
import {
  useAppDispatch,
  useAppSelector,
} from "../../../../../../../store/hooks";

export default function PolicyList() {
  const acessPolicyList: IPolicyListState = useAppSelector(
    (state: RootState) => state.policyListState
  );
  const dispatch = useAppDispatch();
  const mainCall = async (currentPage: number) => {
    dispatch(getPolicyList({ currentPage }));
  };
  useEffect(() => {
    mainCall(1);
  }, []);

  console.log(acessPolicyList);
  const gridTable = new Grid({
    columns: [
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
      acessPolicyList.data?.Policies.map((data) => [
        data.Name,
        data.State ? "active" : "Inactive",
        data.Apis,
        data.AuthType,
      ]),
    className: {
      container:
        "table  table-wrapper table-bordered table-stripped align-items-center justify-content-around",
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

import { Grid } from "gridjs-react";
import React, { useEffect } from "react";
import { RootState } from "../../../../../../../store";
import { IApiListState } from "../../../../../../../store/features/api/list";
import { getApiList } from "../../../../../../../store/features/api/list/slice";
// import { IKeyCreateState } from "../../../../../../../store/features/key/create";
import {
  useAppDispatch,
  useAppSelector,
} from "../../../../../../../store/hooks";

export default function AccessList() {
  // const createState: IKeyCreateState = useAppSelector(
  //   (rootState: RootState) => rootState.addKeyState
  // );

  const apiList: IApiListState = useAppSelector(
    (state: RootState) => state.apiListState
  );
  const dispatch = useAppDispatch();
  const mainCall = async (currentPage: number) => {
    dispatch(getApiList({ currentPage }));
  };
  useEffect(() => {
    mainCall(1);
  }, []);

  // const es: IApiData[] = apiList.data?.Apis!;

  console.log(apiList);
  const grid = new Grid({
    columns: [
      {
        name: "Name",
        attributes: (cell: any) => {
          if (cell) {
            return {
              "data-cell-content": cell,
              onclick: () => alert(cell),
              style: "cursor: pointer",
            };
          }
        },
      },
      "Target Url",
      "CreatedDate",
    ],
    data: () =>
      apiList.data?.Apis.map((data) => [
        data.Name,
        data.TargetUrl,
        data.CreatedDate,
      ]),
    // apiList.data?.Apis.map((data) => [
    // data.Name,
    // data.TargetUrl,
    // data.CreatedDate,
    // ])
    className: {
      container: "table-wrapper",
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
                    API List
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
                      <Grid {...grid.props} />
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

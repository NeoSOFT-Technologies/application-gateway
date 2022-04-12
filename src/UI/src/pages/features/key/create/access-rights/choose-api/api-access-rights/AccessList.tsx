import { Grid } from "gridjs-react";
import moment from "moment";
import React, { useEffect } from "react";
// import { RootState } from "../../../../../../../store";
import { IApiListState } from "../../../../../../../store/features/api/list";
import { getApiList } from "../../../../../../../store/features/api/list/slice";
import { getApiById } from "../../../../../../../store/features/api/update/slice";
import { setForm } from "../../../../../../../store/features/key/create/slice";
// import { IKeyCreateState } from "../../../../../../../store/features/key/create";
// import { setForm } from "../../../../../../../store/features/key/create/slice";
import {
  useAppDispatch,
  useAppSelector,
} from "../../../../../../../store/hooks";
// import ApiAccess from "../api-access/ApiAccess";

export default function AccessList() {
  // const createState: IKeyCreateState = useAppSelector(
  //   (rootState: RootState) => rootState.createKeyState
  // );
  const state = useAppSelector((RootState) => RootState.createKeyState);
  const accessApiList: IApiListState = useAppSelector(
    (State) => State.apiListState
  );
  const dispatch = useAppDispatch();
  const mainCall = async (currentPage: number) => {
    dispatch(getApiList({ currentPage }));
  };
  useEffect(() => {
    mainCall(1);
  }, []);

  // console.log(accessApiList);
  console.log("datalength ", state.data.form.accessRights?.length);
  const handleAddClick = async (Id: string) => {
    const selectedApi = await dispatch(getApiById(Id));
    console.log(selectedApi);
    const listV: string[] = [];
    selectedApi.payload.Data.Versions.forEach((element: any) => {
      // element: { Name: string }
      listV.push(element.Name);
    });
    // console.log(listV);

    const list = [
      ...state.data.form.accessRights!,
      {
        apiId: "475b8639-6698-4a00-a395-bb80023a2915",
        apiName: "testApi",
        versions: ["Default"],
        allowedUrls: [
          {
            url: "",
            methods: [],
          },
        ],
        limit: {
          rate: 0,
          throttle_interval: 0,
          throttle_retry_limit: 0,
          max_query_depth: 0,
          quota_max: 0,
          quota_renews: 0,
          quota_remaining: 0,
          quota_renewal_rate: 0,
        },
      },
    ];
    console.log(state.data);
    dispatch(setForm({ ...state.data.form, accessRights: list }));
  };
  const grid = new Grid({
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
        // attributes: (cell: string) => {
        //   if (cell) {
        //     return {
        //       "data-cell-content": cell,
        //       onclick: () => alert(cell),
        //       style: "cursor: pointer",
        //     };
        //   }
        // },
      },
      "Status",
      "CreatedDate",
    ],
    search: true,
    // sort: true,
    data: () =>
      accessApiList.data?.Apis.map((data) => [
        data.Id,
        data.Name,
        data.IsActive ? "active" : "Inactive",
        data.CreatedDate !== null
          ? moment(data.CreatedDate).format("DD/MM/YYYY")
          : data.CreatedDate,
      ]),
    className: {
      container: "table table-bordered table-stripped",
    },
    style: {
      table: {
        width: "100%",
        // border: "2px solid #ccc",
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
                    {/* {state.data.form.accessRights?.map(
                      (data: any, index: number) => {
                        const { apiName } = data;
                        return (
                          <>
                            <tr key={index}>
                              <td>
                                <input
                                  type="text"
                                  value={apiName}
                                  // onChange={(evnt) => handleTableRowsInputChange(index, evnt)}
                                  name="Name"
                                  className="form-control"
                                />
                              </td>
                            </tr>
                          </>
                        );
                      }
                    )} */}
                  </div>
                  {/* <ApiAccess rawData={state.data.form} /> */}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

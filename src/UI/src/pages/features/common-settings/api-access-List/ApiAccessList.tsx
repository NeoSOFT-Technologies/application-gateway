import { Grid } from "gridjs-react";
import moment from "moment";
import React, { useEffect } from "react";
import { IApiListState } from "../../../../store/features/api/list";
import { getApiList } from "../../../../store/features/api/list/slice";
import { useAppSelector, useAppDispatch } from "../../../../store/hooks";
import { IKeyCreateState } from "../../../../store/features/key/create/index";
import { IPolicyCreateState } from "../../../../store/features/policy/create";

interface IProps {
  state: IKeyCreateState | IPolicyCreateState;
  handleAddClick: (val: any) => void;
}
export default function ApiAccessList(props: IProps) {
  const { state, handleAddClick } = props;

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
      container: "table table-responsive table-bordered table-stripped",
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
  console.log(state);
  return (
    <div>
      <Grid {...grid.props} />
    </div>

    // <div>
    //   <div className="card mb-3">
    //     <div>
    //       <div className="align-items-center justify-content-around">
    //         <div className="accordion" id="accordionSetting">
    //           <div className="accordion-item">
    //             <h2 className="accordion-header" id="headingOne">
    //               <button
    //                 className="accordion-button"
    //                 type="button"
    //                 data-bs-toggle="collapse"
    //                 data-bs-target="#collapseOne"
    //                 aria-expanded="true"
    //                 aria-controls="collapseOne"
    //               >
    //                 API List
    //               </button>
    //             </h2>
    //             <div
    //               id="collapseOne"
    //               className="accordion-collapse collapse show"
    //               aria-labelledby="headingOne"
    //               data-bs-parent="#accordionSetting"
    //             >
    //               <div className="accordion-body">
    //                 <Grid {...grid.props} />
    //               </div>
    //             </div>
    //           </div>
    //         </div>
    //       </div>
    //     </div>
    //   </div>
    // </div>
  );
}

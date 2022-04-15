import { Grid } from "gridjs-react";
import moment from "moment";
import React, { useEffect } from "react";
import { IApiListState } from "../../../../store/features/api/list";
import { getApiList } from "../../../../store/features/api/list/slice";
import { useAppSelector, useAppDispatch } from "../../../../store/hooks";
import { IKeyCreateState } from "../../../../store/features/key/create/index";
import { IPolicyCreateState } from "../../../../store/features/policy/create";
import { h } from "gridjs";

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
        hidden: true,
        // attributes: (cell: string) => {
        //   if (cell) {
        //     return {
        //       "data-cell-content": cell,
        //       onclick: () => handleAddClick(cell),
        //       style: "cursor: pointer",
        //     };
        //   }
        // },
      },
      {
        name: "Name",
        formatter: (cell: string, row: any) => {
          return h(
            "text",
            {
              // className: 'py-2 mb-4 px-4 border rounded-md text-white bg-blue-600',
              // onClick: () =>
              //   alert(`Editing "${row.cells[0].data}" "${row.cells[1].data}"`),
              onclick: () => handleAddClick(row.cells[0].data),
            },
            `${row.cells[1].data}`
          );
        },
        attributes: (cell: string) => {
          if (cell) {
            return {
              "data-cell-content": cell,
              //  onclick: () => handleAddClick(cell),
              style: "cursor: pointer",
            };
          }
        },
        // style: "cursor: pointer",
      },
      "Status",
      "CreatedDate",
    ],
    search: true,
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
  );
}

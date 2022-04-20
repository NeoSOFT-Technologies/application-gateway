import React from "react";
import { getApiById } from "../../../../../../../store/features/api/update/slice";
import { setForms } from "../../../../../../../store/features/key/create/slice";
import {
  useAppDispatch,
  useAppSelector,
} from "../../../../../../../store/hooks";
import ApiAccessList from "../../../../../common-settings/api-access-List/ApiAccessList";

export default function AccessList() {
  const state = useAppSelector((RootState) => RootState.createKeyState);
  const dispatch = useAppDispatch();

  // console.log("datalength ", state.data.form.accessRights?.length);
  const handleAddClick = async (Id: string) => {
    console.log(Id);
    // console.log(state.data.form.accessRights?.some((x) => x?.apiId !== Id));
    const data = state.data.form.accessRights?.some((x) => x?.apiId === Id);
    console.log("accessList check before", data);

    if (!data) {
      console.log(
        "accesslist check",
        state.data.form.accessRights?.some((x) => x?.apiId === Id)
      );

      const selectedApi = await dispatch(getApiById(Id));

      const listV: string[] = [];
      selectedApi.payload.Data.Versions.forEach((element: any) => {
        // element: { Name: string }
        listV.push(element.Name);
      });

      //   console.log(state.data.form.accessRights?.some((x) => x?.apiId === Id)); // const x = state.data.form.accessRights?.some((xx) => xx?.apiId !== Id);

      const list = [
        ...state.data.form.accessRights,
        {
          apiId: selectedApi.payload.Data.ApiId, // "475b8639-6698-4a00-a395-bb80023a2915"
          apiName: selectedApi.payload.Data.Name, // "testApi",
          versions: listV, // ["Default"],
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

      dispatch(setForms({ ...state.data.form, accessRights: list }));
    }
  };

  return (
    <>
      <div>
        <div className="card mb-3">
          <div>
            <div className="align-items-center justify-content-around">
              <div className="accordion" id="listAccordionSetting">
                <div className="accordion-item">
                  <h2 className="accordion-header" id="headingOne">
                    <button
                      className="accordion-button"
                      type="button"
                      data-bs-toggle="collapse"
                      data-bs-target="#AccessListcollapseOne"
                      aria-expanded="true"
                      aria-controls="AccessListcollapseOne"
                    >
                      Add API Access Rights
                    </button>
                  </h2>
                  <div
                    id="AccessListcollapseOne"
                    className="accordion-collapse collapse show"
                    aria-labelledby="headingOne"
                    data-bs-parent="#listAccordionSetting"
                  >
                    <div className="accordion-body">
                      <ApiAccessList
                        state={state}
                        handleAddClick={handleAddClick}
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

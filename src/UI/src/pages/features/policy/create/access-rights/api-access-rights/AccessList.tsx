import React from "react";
import { ToastAlert } from "../../../../../../components/ToasterAlert/ToastAlert";
import { getApiById } from "../../../../../../store/features/api/update/slice";
import { setForm } from "../../../../../../store/features/policy/create/slice";
import { useAppSelector, useAppDispatch } from "../../../../../../store/hooks";
import ApiAccessList from "../../../../common-settings/api-access-List/ApiAccessList";

export default function AccessList() {
  const state = useAppSelector((RootState) => RootState.createPolicyState);
  const dispatch = useAppDispatch();

  // console.log("datalength ", state.data.form.accessRights?.length);
  const handleAddClick = async (Id: string) => {
    const data = state.data.form.APIs?.some((x) => x?.Id === Id);
    // console.log("accessList check before", data);

    if (!data) {
      // console.log(
      //   "accesslist check",
      //   state.data.form.ApIs?.some((x) => x?.Id === Id)
      // );
      const selectedApi = await dispatch(getApiById(Id));
      console.log("dispath completed");
      const listV: string[] = [];
      selectedApi.payload.Data.Versions.forEach((element: any) => {
        // element: { Name: string }
        listV.push(element.Name);
      });
      console.log("array ended up here");
      const list = [
        ...state.data.form.APIs,
        {
          Id: selectedApi.payload.Data.ApiId,
          Name: selectedApi.payload.Data.Name,
          Versions: [],
          MasterVersions: listV,
          AllowedUrls: [
            // {
            //   url: "",
            //   methods: [],
            // },
          ],
          Limit: {
            rate: 0,
            per: 0,
            throttle_interval: 0,
            throttle_retry_limit: 0,
            max_query_depth: 0,
            quota_max: 0,
            quota_renews: 0,
            quota_remaining: 0,
            quota_renewal_rate: 0,
            set_by_policy: false,
          },
        },
      ];
      // console.log("list: ", list);
      dispatch(setForm({ ...state.data.form, APIs: list }));
    } else {
      ToastAlert("Already select...", "error");
    }
  };
  // console.log("ACCESSLIST", state.data);
  return (
    <>
      <div>
        <div className="card mb-3">
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
    </>
  );
}

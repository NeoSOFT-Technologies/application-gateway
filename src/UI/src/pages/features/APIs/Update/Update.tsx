import React from "react";
import Setting from "./Setting/Setting";
import Version from "./Version/Version";
import { Tab, Tabs, Form } from "react-bootstrap";
import { useAppDispatch, useAppSelector } from "../../../../store/hooks";
// import { IApiGetByIdState } from "../../../../types/api";

import Spinner from "../../../../components/loader/Loader";
import { ToastAlert } from "../../../../components/ToasterAlert/ToastAlert";
// import { updateApi } from "../../../../store/features/api/update/slice";
// import { IApiUpdateFormData } from "../../../../types/api";

import { updateApi } from "../../../../store/features/api/update/slice";
import { camelizeKeys } from "../../../../resources/APIS/ApiConstants";

export default function Update() {
  //   const apiData: IApiGetByIdState = useAppSelector(
  //     (state: RootState) => state.getApiById
  //   );
  //   console.log("update", apiData);

  const state: any = useAppSelector((RootState) => RootState.getApiById);
  const dispatch = useAppDispatch();

  const updateApiFormState: any = useAppSelector(
    (RootState) => RootState.updateApi
  );

  const oldState = updateApiFormState.data;
  const newstate = { ...oldState };
  const updateApiData = newstate;

  const formData = camelizeKeys(state.data.form);

  const handleSubmitApiUpdate = async (event: React.FormEvent) => {
    event.preventDefault();
    console.log("state form", state.data.form);
    console.log("error", state.data.errors);
    console.log("error state", !state.data.errors);

    if (state.data.errors === null) {
      if (
        state.data.form.Name !== "" &&
        state.data.form.ListenPath !== "" &&
        state.data.form.TargetUrl !== ""
      ) {
        console.log("original updateApiData", updateApiData);
        console.log("formData", formData);

        Object.keys(updateApiData).forEach(
          (key) => (updateApiData[key] = formData[key])
        );

        console.log("modified updateApiData", updateApiData);
        const result = await dispatch(updateApi(updateApiData));
        console.log("result", result);
        ToastAlert("Api Update", "success");
      } else {
        ToastAlert("Please correct the error", "error");
      }
    } else {
      ToastAlert("Please fill all the fields correctly! ", "error");
    }
  };

  return (
    <div>
      {state.loading && <Spinner />}
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div className="card-body">
            <Form onSubmit={handleSubmitApiUpdate} data-testid="form-input">
              <div className="align-items-center">
                <div>
                  <button className=" btn  btn-success btn-md d-flex float-right mb-4">
                    {" "}
                    Update
                  </button>
                </div>
                <Tabs
                  defaultActiveKey="setting"
                  id="uncontrolled-tab"
                  // transition={false}
                  className="mb-3 small"
                >
                  <Tab eventKey="setting" title="Setting">
                    <Setting />
                  </Tab>
                  <Tab eventKey="version" title="Version">
                    <Version />
                  </Tab>
                </Tabs>
              </div>
            </Form>
          </div>
        </div>
      </div>
    </div>
  );
}

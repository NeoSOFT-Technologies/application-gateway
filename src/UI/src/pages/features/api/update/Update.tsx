import React, { FormEvent } from "react";
import Setting from "./setting/Setting";
import Version from "./version/Version";
import { Tab, Tabs, Form } from "react-bootstrap";
import Spinner from "../../../../components/loader/Loader";
import { useAppDispatch, useAppSelector } from "../../../../store/hooks";
import { updateApi } from "../../../../store/features/api/update/slice";
import { ToastAlert } from "../../../../components/ToasterAlert/ToastAlert";
import { IApiGetByIdState } from "../../../../store/features/api/update";

export default function Update() {
  const state: IApiGetByIdState = useAppSelector(
    (RootState) => RootState.updateApiState
  );
  const dispatch = useAppDispatch();

  async function handleSubmitApiUpdate(event: FormEvent) {
    event.preventDefault();
    let validate: any;
    if (state.data.errors !== undefined) {
      validate = Object.values(state.data.errors).every(
        (x) => x === null || x === ""
      );
    }
    if (validate) {
      await dispatch(updateApi(state.data.form));
      ToastAlert("Api Updated Successfully!!", "success");
    } else {
      ToastAlert("Please fill all the fields correctly! ", "error");
    }
  }

  return (
    <div>
      {state.loading && <Spinner />}
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div className="card-body">
            <Form
              onSubmit={(e: FormEvent) => handleSubmitApiUpdate(e)}
              data-testid="form-input"
            >
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

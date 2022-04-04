import React, { FormEvent } from "react";
import Setting from "./setting/Setting";
import Version from "./version/Version";
import { Tab, Tabs, Form } from "react-bootstrap";
import Spinner from "../../../../components/loader/Loader";
import { useAppDispatch, useAppSelector } from "../../../../store/hooks";
import { updateApi } from "../../../../store/features/api/update/slice";
import { ToastAlert } from "../../../../components/ToasterAlert/ToastAlert";
import { IApiGetByIdState } from "../../../../store/features/api/update";
import { useNavigate } from "react-router-dom";

export default function Update() {
  const state: IApiGetByIdState = useAppSelector(
    (RootState) => RootState.updateApiState
  );
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
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
  const NavigateToApisList = (
    val: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    val.preventDefault();
    navigate("/api/list");
  };
  return (
    <div>
      {state.loading && <Spinner />}
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div>
            {/*  className="card-body" */}
            <Form
              onSubmit={(e: FormEvent) => handleSubmitApiUpdate(e)}
              data-testid="form-input"
            >
              <div className="align-items-center">
                <div
                  className="card-header bg-white mt-3 pt-2 pb-4"
                  style={{ padding: "0.5rem 2.5rem" }}
                >
                  <button className=" btn  btn-success btn-md d-flex float-right mb-4">
                    {" "}
                    Update
                  </button>
                  <button
                    className=" btn  btn-light btn-md d-flex float-right mb-4"
                    onClick={(e) => NavigateToApisList(e)}
                  >
                    {" "}
                    Cancel
                  </button>
                  <h5>
                    <b>UPDATE API</b>
                  </h5>
                </div>
                <div className="card-body pt-2">
                  <Tabs
                    defaultActiveKey="setting"
                    id="uncontrolled-tab"
                    // transition={false}
                    className="mb-2 small"
                  >
                    <Tab eventKey="setting" title="Setting">
                      <Setting />
                    </Tab>
                    <Tab eventKey="version" title="Version">
                      <Version />
                    </Tab>
                  </Tabs>
                </div>
              </div>
            </Form>
          </div>
        </div>
      </div>
    </div>
  );
}

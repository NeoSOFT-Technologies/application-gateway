import React, { FormEvent } from "react";
import Setting from "./Setting/Setting";
import Version from "./Version/Version";
import { Tab, Tabs, Form } from "react-bootstrap";
import Spinner from "../../../../components/loader/Loader";
import handleSubmitApiUpdate from "./_Update";
import { IApiGetByIdState } from "../../../../types/api";
import { useAppDispatch, useAppSelector } from "../../../../store/hooks";

export default function Update() {
  const state: IApiGetByIdState = useAppSelector(
    (RootState) => RootState.getApiById
  );
  const dispatch = useAppDispatch();

  return (
    <div>
      {state.loading && <Spinner />}
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div className="card-body">
            <Form
              onSubmit={(e: FormEvent) =>
                handleSubmitApiUpdate(e, state, dispatch)
              }
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

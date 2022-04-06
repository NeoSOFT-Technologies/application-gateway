import React from "react";
import { Form, Tab, Tabs } from "react-bootstrap";
import AccessRights from "./access-rights/AccessRights";
import Configurations from "./configurations/Configurations";
export default function CreatePolicy() {
  return (
    <div>
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div>
            {/*  className="card-body" */}
            <Form data-testid="form-input">
              <div className="align-items-center">
                <div
                  className="card-header bg-white mt-3 pt-2 pb-4"
                  style={{ padding: "0.5rem 2.5rem" }}
                >
                  <button className=" btn  btn-success btn-md d-flex float-right mb-4">
                    {" "}
                    Create
                  </button>
                  <button
                    className=" btn  btn-light btn-md d-flex float-right mb-4"
                    // onClick={(e) => NavigateToApisList(e)}
                  >
                    {" "}
                    Cancel
                  </button>
                  <h5>
                    <b>CREATE POLICY</b>
                  </h5>
                </div>
                <div className="card-body pt-2">
                  <Tabs
                    defaultActiveKey="accessRights"
                    id="uncontrolled-tab"
                    // transition={false}
                    className="mb-2 small"
                  >
                    <Tab eventKey="accessRights" title="Access Rights">
                      <AccessRights />
                    </Tab>
                    <Tab eventKey="configurations" title="Configurations">
                      <Configurations />
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

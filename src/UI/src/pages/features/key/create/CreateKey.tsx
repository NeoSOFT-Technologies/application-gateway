import React from "react";
import { Form, Tab, Tabs } from "react-bootstrap";
import AccessRights from "./access-rights/AccessRights";
import Configurations from "./configurations/Configurations";
export default function CreateKey() {
  return (
    <div>
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div>
            {/*  className="card-body" */}
            <Form data-testid="form-input">
              <div className="align-items-center">
                <div
                  className="card-header bg-white mt-3 pt-1 pb-4"
                  style={{ padding: "0.5rem 1.5rem" }}
                >
                  <button className=" btn btn-sm btn-success btn-md d-flex float-right mb-3">
                    {" "}
                    Create
                  </button>
                  <button
                    className=" btn btn-sm btn-light btn-md d-flex float-right mb-3"
                    // onClick={(e) => NavigateToApisList(e)}
                  >
                    {" "}
                    Cancel
                  </button>
                  <span>
                    <b>CREATE KEY</b>
                  </span>
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

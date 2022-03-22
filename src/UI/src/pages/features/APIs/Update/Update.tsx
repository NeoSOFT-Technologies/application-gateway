import React from "react";
import Setting from "./Setting/Setting";
import Version from "./Version/Version";
import { Tab, Tabs, Form } from "react-bootstrap";

export default function Update() {
  return (
    <div>
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div className="card-body">
            <Form>
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

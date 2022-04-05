import React from "react";
import { Tab, Tabs } from "react-bootstrap";
import ApplyPolicy from "./apply-policy/ApplyPolicy";
import ChooseApi from "./choose-api/ChooseApi";
export default function AccessRights() {
  return (
    <div>
      <div className="col-lg-12 grid-margin stretch-card">
        <div>
          <div className="align-items-center">
            <div className="pt-2">
              <Tabs
                defaultActiveKey="applyPolicy"
                id="uncontrolled-tab"
                // transition={false}
                className="mb-2 small"
              >
                <Tab eventKey="applyPolicy" title="Apply Policy">
                  <ApplyPolicy />
                </Tab>
                <Tab eventKey="chooseApi" title="Choose Api">
                  <ChooseApi />
                </Tab>
              </Tabs>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

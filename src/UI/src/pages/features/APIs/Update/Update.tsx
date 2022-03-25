import React from "react";
import Setting from "./Setting/Setting";
import Version from "./Version/Version";
import { Tab, Tabs, Form } from "react-bootstrap";
import {
  regexForListenPath,
  regexForName,
} from "../../../../resources/APIS/ApiConstants";
import { setForm, err } from "../../../../resources/common";

export default function Update() {
  // updateForm, setUpdateForm
  // form, setForm
  // errors, SetErrors
  const form = setForm();
  const error = err();
  function validateForm(event: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = event.target;
    switch (name) {
      case "apiName":
        error[1]({
          ...error[0],
          [name]: regexForName.test(value) ? "" : "Enter a valid Api Name ",
        });
        break;

      case "listenPath":
        error[1]({
          ...error[0],
          [name]: regexForListenPath.test(value)
            ? ""
            : "Enter a Valid Listen Path",
        });
        break;
      default:
        break;
    }
  }

  function changeApiUpdateForm(e: React.ChangeEvent<HTMLInputElement>) {
    validateForm(e);
    form[1]({ ...form[0], [e.target.name]: e.target.value });
  }
  console.log("Form - ", form[0]);
  console.log("Error -", error[0]);
  return (
    <div>
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div className="card-body">
            <Form data-testid="form-input">
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
                    <Setting onChange={changeApiUpdateForm} />
                  </Tab>
                  <Tab eventKey="version" title="Version">
                    <Version onChange={changeApiUpdateForm} />
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

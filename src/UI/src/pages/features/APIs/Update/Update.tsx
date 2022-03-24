import React, { useState } from "react";
import Setting from "./Setting/Setting";
import Version from "./Version/Version";
import { Tab, Tabs, Form } from "react-bootstrap";
import { IApiUpdateError } from "../../../../types/api";
import {
  regexForListenPath,
  regexForName,
  setForm,
} from "../../../../resources/APIS/ApiConstants";

export default function Update() {
  // updateForm, setUpdateForm
  // form, setForm
  // errors, SetErrors

  const [errors, setErrors] = useState<IApiUpdateError>({
    apiName: "",
    listenPath: "",
  });
  function validateForm(event: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = event.target;
    switch (name) {
      case "apiName":
        setErrors({
          ...errors,
          [name]: regexForName.test(value) ? "" : "Enter a valid Api Name ",
        });
        break;

      case "listenPath":
        setErrors({
          ...errors,
          [name]: regexForListenPath.test(value)
            ? ""
            : "Enter a Valid Listen Path",
        });
        break;
      default:
        break;
    }
  }
  const form = setForm();
  function changeApiUpdateForm(e: React.ChangeEvent<HTMLInputElement>) {
    validateForm(e);
    form[1]({ ...form[0], [e.target.name]: e.target.value });
  }
  console.log("Form - ", form[0]);
  console.log("Error -", errors);
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

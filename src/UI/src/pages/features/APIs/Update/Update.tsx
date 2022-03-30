import React from "react";
import Setting from "./Setting/Setting";
import Version from "./Version/Version";
import { Tab, Tabs, Form } from "react-bootstrap";
import { useAppSelector } from "../../../../store/hooks";
import { IApiGetByIdState } from "../../../../types/api";
import { RootState } from "../../../../store";
// import {
//   regexForListenPath,
//   regexForName,
//   configureForm,
// } from "../../../../resources/APIS/ApiConstants";

export default function Update() {
  const apiData: IApiGetByIdState = useAppSelector(
    (state: RootState) => state.getApiById
  );
  console.log(apiData);

  // const model = configureForm();

  // function validateForm(event: React.ChangeEvent<HTMLInputElement>) {
  //   const { name, value, type } = event.target;

  //   switch (name) {
  //     case "apiName":
  //       model.setErrors({
  //         ...model.errors,
  //         [name]: regexForName.test(value) ? "" : "Enter a valid Api Name ",
  //       });
  //       break;

  //     case "listenPath":
  //       model.setErrors({
  //         ...model.errors,
  //         [name]: regexForListenPath.test(value)
  //           ? ""
  //           : "Enter a Valid Listen Path",
  //       });
  //       break;
  //     default:
  //       break;
  //   }

  //   if (type === "checkbox") {
  //     const isChecked = event.target.checked;
  //     model.setForm({ ...model.form, [event.target.name]: isChecked });
  //   } else {
  //     model.setForm({ ...model.form, [event.target.name]: event.target.value });
  //   }
  // }

  // function changeForm(e: React.ChangeEvent<HTMLInputElement>) {
  //   validateForm(e);
  // }
  // console.log("Form - ", model.form);
  // console.log("Error -", model.errors);
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

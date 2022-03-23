import React, { useState } from "react";
import Setting from "./Setting/Setting";
import Version from "./Version/Version";
import { Tab, Tabs, Form } from "react-bootstrap";
import { IApiUpdateForm, IErrorApiUpdate } from "../../../../types/api";
// import {
//   IApiUpdateFormData,
//   IErrorApiUpdateInput,
// } from "../../../../types/api";

// import { regexForListenPath } from "../../../../resources/APIS/ApiConstants";
// import { ToastAlert } from "../../../../components/ToasterAlert/ToastAlert";
// import { useNavigate } from "react-router-dom";

export default function Update() {
  // const navigate = useNavigate();

  // const [apisUpdateForm, setApisUpdateForm] = useState<IApiUpdateFormData>({
  //   apiName: "",
  //   listenPath: "",
  //   targetUrl: "",
  //   stripListenPath: false,
  //   internal: false,
  //   roundRobin: false,
  //   service: false,
  //   rateLimit: false,
  //   rate: "",
  //   perSecond: "",
  //   quotas: false,
  // });
  // const [err, setErr] = useState<IErrorApiUpdateInput>({
  //   apiName: "",
  //   targetUrl: "",
  //   listenPath: "",
  //   rate: "",
  //   perSecond: "",
  // });
  const [apisUpdateForm, setApisUpdateForm] = useState<IApiUpdateForm>({
    apiName: "",
  });
  const [err, setErr] = useState<IErrorApiUpdate>({
    apiName: "",
  });
  // const handleValidate = () => {
  //   const validate = !!(err.apiName === "");
  //   return validate;
  // };
  // const handleSubmitApi = (event: React.FormEvent) => {
  //   event.preventDefault();
  //   if (handleValidate()) {
  //     console.log("apisUpdateForm", apisUpdateForm);
  //     if (apisUpdateForm.apiName !== "") {
  //       setApisUpdateForm({
  //         apiName: "",
  //       });
  //     }
  //   } else {
  //     setErr({
  //       apiName: "",
  //     });
  //   }
  // };
  console.log("update componenet", apisUpdateForm);
  console.log("update error", err);
  // const NavigateToApisList = (
  //   val: React.MouseEvent<HTMLButtonElement, MouseEvent>
  // ) => {
  //   val.preventDefault();
  //   // console.log(val);
  //   navigate("/apilist");
  // };
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
                    <Setting
                      setApisUpdateForm={setApisUpdateForm}
                      setErr={setErr}
                    />
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

import React, { useState } from "react";
import ListenPath from "./ListenPath/ListenPath";
// import { Accordion } from "react-bootstrap";
import RateLimit from "./RateLimit/RateLimit";
import TargetUrl from "./TargetUrl/TargetUrl";
import { Col, Form, Row } from "react-bootstrap";
import {
  IApiUpdateFormData,
  IErrorApiUpdateInput,
} from "../../../../../types/api";
import {
  regexForListenPath,
  regexForName,
} from "../../../../../resources/APIS/ApiConstants";

export default function Setting() {
  const [apisUpdateForm, setApisUpdateForm] = useState<IApiUpdateFormData>({
    apiName: "",
    listenPath: "",
    targetUrl: "",
    stripListenPath: false,
    internal: false,
    roundRobin: false,
    service: false,
    rateLimit: false,
    rate: "",
    perSecond: "",
    quotas: false,
  });
  const [err, setErr] = useState<IErrorApiUpdateInput>({
    apiName: "",
    targetUrl: "",
    listenPath: "",
    rate: "",
    perSecond: "",
  });
  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    // console.log(event.target.value);
    const { name, value } = event.target;
    switch (name) {
      case "apiName":
        setErr({
          ...err,
          [name]: regexForName.test(value)
            ? ""
            : "Name should only consist Alphabets",
        });
        break;
      case "listenPath":
        setErr({
          ...err,
          [name]: regexForListenPath.test(value)
            ? ""
            : "ListenPath should be in correct format eg: /abc/",
        });
        break;
      default:
        break;
    }
    setApisUpdateForm({ ...apisUpdateForm, [name]: value });
  };
  return (
    <div>
      <div className="card">
        <div>
          <div className="align-items-center justify-content-around">
            {/* <Accordion defaultActiveKey="0"> */}
            {/* <Accordion.Item eventKey="0"> */}
            <div className="card-header">
              API Settings
              {/* <Accordion.Header>API Settings</Accordion.Header> */}
            </div>

            <div className="card-body">
              {/* <Accordion.Body> */}
              <Row>
                <Col md={12} className="mb-3">
                  <div className="h-50">
                    <Form.Group className="mb-3">
                      <Form.Label> API Name</Form.Label>
                      <br />

                      <Form.Control
                        className="mt-2"
                        type="text"
                        id="apiName"
                        placeholder="Enter API Name"
                        name="apiName"
                        data-testid="name-input"
                        value={apisUpdateForm.apiName}
                        isInvalid={!!err.apiName}
                        isValid={!err.apiName && !!apisUpdateForm.apiName}
                        onChange={handleInputChange}
                        required
                      />
                      <Form.Control.Feedback type="invalid">
                        {err.apiName}
                      </Form.Control.Feedback>
                    </Form.Group>
                  </div>
                </Col>
              </Row>
              <br />
              <div>
                <ListenPath />
              </div>
              <div>
                <TargetUrl />
              </div>
              <div>
                <RateLimit />
              </div>
              {/* </Accordion.Body> */}
            </div>
            {/* </Accordion.Item> */}
            {/* </Accordion> */}
          </div>
        </div>
      </div>
      {/* <table className="table table-bordered">
        <thead>
          <tr>
            <th>API Settings</th>
          </tr>
        </thead>
        <tbody>
          <td>
            API Name <br />
            <br />
            <div className="h-50 input-group">
              <input
                type="text"
                className="bg-parent border-1"
                placeholder="Enter API Name"
              />
            </div>
          </td>
        </tbody>
      </table> */}
    </div>
  );
}

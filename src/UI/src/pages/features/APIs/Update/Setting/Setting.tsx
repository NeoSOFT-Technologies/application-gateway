import React from "react";
import ListenPath from "./ListenPath/ListenPath";
// import { Accordion } from "react-bootstrap";
import RateLimit from "./RateLimit/RateLimit";
import TargetUrl from "./TargetUrl/TargetUrl";
import { Col, Form, Row } from "react-bootstrap";

export default function Setting() {
  return (
    <div>
      {/* <h1>
        <ListenPath />
        <TargetUrl />
        <RateLimit />
      </h1> */}
      <div className="card">
        <div>
          <div className="align-items-center justify-content-around">
            {/* <Accordion defaultActiveKey="0"> */}
            {/* <Accordion.Item eventKey="0"> */}
            <div className="card-header">
              API Settings
              {/* <Accordion.Header>API Settings</Accordion.Header> */}
            </div>
            {/* <h2 className="accordion-header">
                    <button
                      type="button"
                      aria-expanded="true"
                      className="accordion-button"
                    >
                      API Settings
                    </button>
                  </h2>
                </div> */}

            <div className="card-body">
              {/* <Accordion.Body> */}
              <Row>
                <Col md={12} className="mb-3">
                  <div className="h-50">
                    {/* <div>
                  <label>API Name :</label>
                </div>
                <div className="input-group">
                  <input
                    type="text"
                    className="form-control bg-parent border-1"
                    placeholder="Enter API Name"
                  />
                </div> */}
                    <Form.Group className="mb-3">
                      <Form.Label> API Name</Form.Label>
                      <br />

                      <Form.Control
                        className="mt-2"
                        type="text"
                        id="apiname"
                        placeholder="Enter API Name"
                        name="apiname"
                        required
                      />
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

import React from "react";
import { Col, Form, Row } from "react-bootstrap";

export default function Authentication() {
  return (
    <div>
      <div className="card">
        <div>
          <div className="align-items-center justify-content-around">
            <div className="accordion" id="accordionListenPath">
              <div className="accordion-item">
                <h2 className="accordion-header" id="headingTwo">
                  <button
                    className="accordion-button"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#collapseTwo"
                    aria-expanded="true"
                    aria-controls="collapseTwo"
                  >
                    Authentication
                  </button>
                </h2>
                <div
                  id="collapseTwo"
                  className="accordion-collapse collapse show"
                  aria-labelledby="headingTwo"
                  data-bs-parent="#accordionListenPath"
                >
                  <div className="accordion-body">
                    <div>
                      <Row>
                        <Col md="12">
                          <Form.Group className="mb-3">
                            <Form.Label> Authentication mode:</Form.Label>
                            <br />
                            <Form.Select
                              aria-label="Default select example"
                              name="AuthType"
                            >
                              <option id="authToken" value="1">
                                Authentication Token
                              </option>
                              <option id="basicAuth" value="2">
                                Basic Authentication
                              </option>
                              <option id="jwt" value="3">
                                Json Web Token
                              </option>
                              <option id="mutualTls" value="4">
                                Mutual TLS
                              </option>
                              <option id="oidc" value="5">
                                OpenId Connect
                              </option>
                              <option id="keyless" value="6">
                                Open (KeyLess)
                              </option>
                            </Form.Select>
                          </Form.Group>
                        </Col>
                      </Row>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

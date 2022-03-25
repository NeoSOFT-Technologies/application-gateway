import React from "react";
import { Col, Form, Row } from "react-bootstrap";
import { changeApiUpdateForm } from "../../../../../../resources/common";
import { IProps } from "../../../../../../types/api";

export default function VersionSettings(props: IProps) {
  return (
    <div>
      <div className="card">
        <div>
          <div className="align-items-center justify-content-around">
            <div className="accordion" id="accordionVersionSettings">
              <div className="accordion-item">
                <h2 className="accordion-header" id="headingFive">
                  <button
                    className="accordion-button"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#collapseFive"
                    aria-expanded="true"
                    aria-controls="collapseFive"
                  >
                    Version Settings
                  </button>
                </h2>
                <div
                  id="collapseFive"
                  className="accordion-collapse collapse show"
                  aria-labelledby="headingFive"
                  data-bs-parent="#accordionVersionSettings"
                >
                  <div className="accordion-body">
                    <div>
                      <Row>
                        <Col md="12">
                          <Form.Group className="mb-3">
                            <Form.Check
                              type="switch"
                              id="enableVersioning"
                              name="enableVersioning"
                              label="Enable Versioning"
                              onChangeCapture={(e: any) =>
                                changeApiUpdateForm(e, props)
                              }
                            />
                          </Form.Group>
                        </Col>
                        <Col md="12">
                          <Form.Group className="mb-3">
                            <Form.Label> Version Data Location</Form.Label>
                            <br />
                            <Form.Select
                              aria-label="Default select example"
                              name="versionDataLocation"
                              onClick={(e: any) =>
                                changeApiUpdateForm(e, props)
                              }
                            >
                              <option id="1" value="header">
                                Header
                              </option>
                              <option id="2" value="url">
                                Url
                              </option>
                              <option id="3" value="queryurl">
                                Query Url
                              </option>
                            </Form.Select>
                          </Form.Group>
                        </Col>
                        <Col md="12">
                          <Form.Group className="mb-3">
                            <Form.Label>
                              {" "}
                              Version Identifier Key Name
                            </Form.Label>
                            <br />

                            <Form.Control
                              className="mt-2"
                              type="text"
                              id="keyName"
                              placeholder="Enter key Name"
                              name="keyName"
                              required
                              onChange={(e: any) =>
                                changeApiUpdateForm(e, props)
                              }
                            />
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

import React from "react";
import { Col, Form, Row } from "react-bootstrap";
// import { useAppSelector, useAppDispatch } from "../../../../../../store/hooks";
// import { setFormData } from "../../../../../../resources/APIS/ApiConstants";

export default function VersionSettings() {
  // const state = useAppSelector((RootState) => RootState.getApiById);
  // const dispatch = useAppDispatch();
  // console.log("setting", state);
  // function validateForm(event: React.ChangeEvent<HTMLInputElement>) {
  //   setFormData(event, dispatch, state);
  // }
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
                              id="enable"
                              name="enable"
                              label="Enable Versioning"
                            />
                          </Form.Group>
                        </Col>
                        <Col md="12">
                          <Form.Group className="mb-3">
                            <Form.Label> Version Data Location</Form.Label>
                            <br />
                            <Form.Select aria-label="Default select example">
                              <option value="1">Header</option>
                              <option value="2">Url</option>
                              <option value="3">Query Url</option>
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
                              id="versionIdentifier"
                              placeholder="Enter key Name"
                              name="versionIdentifier"
                              // value={state.data.form?.VersioningInfo.Key}
                              // onChange={(e: any) => validateForm(e)}
                              required
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

import React from "react";
import { Col, Form, Row } from "react-bootstrap";
import { setFormData } from "../../../../../../resources/APIS/ApiConstants";
import { useAppDispatch, useAppSelector } from "../../../../../../store/hooks";
// import { useAppSelector, useAppDispatch } from "../../../../../../store/hooks";
// import { setFormData } from "../../../../../../resources/APIS/ApiConstants";

export default function VersionSettings() {
  const dispatch = useAppDispatch();
  const state = useAppSelector((RootState) => RootState.getApiById);

  function validateForm(event: React.ChangeEvent<HTMLInputElement>) {
    // const { name, value } = event.target;
    // switch (name) {
    //   case "VersionKey":
    //     setFormErrors(
    //       {
    //         ...state.data.errors,
    //         [name]: regexForName.test(value)
    //           ? ""
    //           : "Enter a Valid Version Key Name",
    //       },
    //       dispatch
    //     );
    //     break;
    //   default:
    //     break;
    // }
    setFormData(event, dispatch, state);
  }

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
                              id="EnableVersioning"
                              name="EnableVersioning"
                              label="Enable Versioning"
                            />
                          </Form.Group>
                        </Col>
                        <Col md="12">
                          <Form.Group className="mb-3">
                            <Form.Label> Version Data Location</Form.Label>
                            <br />
                            <Form.Select
                              aria-label="Default select example"
                              name="VersioningInfo.Location"
                              onClick={(e: any) => validateForm(e)}
                            >
                              <option id="1" value="1">
                                1
                              </option>
                              <option id="2" value="2">
                                2
                              </option>
                              <option id="3" value="3">
                                3
                              </option>
                            </Form.Select>
                          </Form.Group>
                        </Col>
                        <Col md="12">
                          <Form.Group className="mb-3">
                            <Form.Label>Version Identifier Key Name</Form.Label>
                            <br />

                            <Form.Control
                              className="mt-2"
                              type="text"
                              id="versionIdentifier"
                              placeholder="Enter Version key Name"
                              name="VersioningInfo.Key"
                              value={state.data.form?.VersioningInfo.Key}
                              // isInvalid={!!state.data.errors?.VersionKey}
                              // isValid={!state.data.errors?.VersionKey}
                              onChange={(e: any) => validateForm(e)}
                              required
                            />
                            {/* <Form.Control.Feedback type="invalid">
                              {state.data.errors?.VersionKey}
                            </Form.Control.Feedback> */}
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

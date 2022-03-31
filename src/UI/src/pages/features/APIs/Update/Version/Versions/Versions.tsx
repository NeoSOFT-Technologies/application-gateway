import React from "react";
import { Button, Col, Form, Row } from "react-bootstrap";
import { setFormData } from "../../../../../../resources/APIS/ApiConstants";

import { useAppDispatch, useAppSelector } from "../../../../../../store/hooks";

export default function Versions() {
  const dispatch = useAppDispatch();
  const state = useAppSelector((RootState) => RootState.getApiById);

  function validateForm(event: React.ChangeEvent<HTMLInputElement>) {
    // const { name, value } = event.target;
    // switch (name) {
    //   case "VersionKey":
    //     setFormErrors(
    //       {
    //         ...state.data.errors,
    //         [name]: regexForListenPath.test(value)
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
    <>
      <div className="accordion" id="accordionVersions">
        <div className="accordion-item">
          <h2 className="accordion-header" id="headingSix">
            <button
              className="accordion-button"
              type="button"
              data-bs-toggle="collapse"
              data-bs-target="#collapseSix"
              aria-expanded="true"
              aria-controls="collapseSix"
            >
              Versions
            </button>
          </h2>
          <div
            id="collapseSix"
            className="accordion-collapse collapse show"
            aria-labelledby="headingSix"
            data-bs-parent="#accordionVersions"
          >
            <div className="accordion-body">
              <Row>
                <Col md={12} className="mb-3">
                  <i>
                    Add versions using the fields below. Leave the expiry field
                    empty for the version to never expire. Your local time will
                    be automatically converted to UTC time.
                  </i>
                  <Form.Group className="mt-3">
                    <Form.Label>
                      <b>Choose a version:</b>
                    </Form.Label>
                    <br></br>
                    <Form.Select name="DefaultVersion">
                      <option disabled>Choose a version</option>
                      <option id="1" value="default">
                        Default
                      </option>
                    </Form.Select>
                  </Form.Group>

                  <i>
                    If you do not set this and no specific version is requested,
                    Your API request will fail with an error.
                  </i>
                </Col>
                <Col md={3}>
                  <Form.Group className="mb-3">
                    <Form.Control
                      type="text"
                      placeholder="Version Name"
                      id="versionName"
                      name="Versions.Name"
                      value={state.data.form?.Versions[0].Name}
                      // isInvalid={!!state.data.errors?.VersionName}
                      // isValid={!state.data.errors?.VersionName}
                      onChange={(e: any) => validateForm(e)}
                    />
                    {/* <Form.Control.Feedback type="invalid">
                      {state.data.errors?.VersionName}
                    </Form.Control.Feedback> */}
                  </Form.Group>
                </Col>
                <Col md={4}>
                  <Form.Group className="mb-3">
                    <Form.Control
                      type="text"
                      placeholder="Override Target Host"
                      id="overrideTarget"
                      name="Versions.OverrideTarget"
                      value={state.data.form?.Versions[0].OverrideTarget}
                      // isInvalid={!!state.data.errors?.OverrideTarget}
                      // isValid={!state.data.errors?.OverrideTarget}
                      onChange={(e: any) => validateForm(e)}
                    />
                    {/* <Form.Control.Feedback type="invalid">
                      {state.data.errors?.OverrideTarget}
                    </Form.Control.Feedback> */}
                  </Form.Group>
                </Col>
                <Col md={3}>
                  <Form.Group className="mb-3">
                    <Form.Control
                      type="text"
                      placeholder="Expires"
                      id="expires"
                      name="Versions.Expires"
                      value={state.data.form?.Versions[0].Expires}
                      // isInvalid={!!state.data.errors?.Expires}
                      // isValid={!state.data.errors?.Expires}
                      onChange={(e: any) => validateForm(e)}
                    />
                    {/* <Form.Control.Feedback type="invalid">
                      {state.data.errors?.Expires}
                    </Form.Control.Feedback> */}
                  </Form.Group>
                </Col>
                <Col md={2}>
                  <Form.Group className="mb-3">
                    <Button variant="dark">Add</Button>{" "}
                  </Form.Group>
                </Col>
              </Row>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

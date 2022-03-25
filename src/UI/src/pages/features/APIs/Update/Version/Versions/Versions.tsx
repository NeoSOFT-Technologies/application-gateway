import React from "react";
import { Button, Col, Form, Row } from "react-bootstrap";
import { changeApiUpdateForm } from "../../../../../../resources/common";
import { IProps } from "../../../../../../types/api";

export default function Versions(props: IProps) {
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
                    <Form.Select
                      name="version"
                      onClick={(e: any) => changeApiUpdateForm(e, props)}
                    >
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
                      id="versionname"
                      name="versionname"
                      placeholder="Version Name"
                      required
                      onChange={(e: any) => changeApiUpdateForm(e, props)}
                    />
                  </Form.Group>
                </Col>
                <Col md={4}>
                  <Form.Group className="mb-3">
                    <Form.Control
                      type="text"
                      id="targethost"
                      name="targethost"
                      placeholder="Override Target Host"
                      required
                      onChange={(e: any) => changeApiUpdateForm(e, props)}
                    />
                  </Form.Group>
                </Col>
                <Col md={3}>
                  <Form.Group className="mb-3">
                    <Form.Control
                      type="text"
                      id="expires"
                      name="expires"
                      placeholder="Expires"
                      required
                      onChange={(e: any) => changeApiUpdateForm(e, props)}
                    />
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

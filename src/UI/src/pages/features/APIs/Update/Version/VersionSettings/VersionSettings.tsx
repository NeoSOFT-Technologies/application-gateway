import React from "react";
import { Col, Form, Row } from "react-bootstrap";

export default function VersionSettings() {
  return (
    <div>
      <div className="card">
        <div>
          <div className="align-items-center justify-content-around">
            <div className="card-header !important">Version Settings</div>
            <div className="card-body">
              <div>
                <Row>
                  <Col md="12">
                    <Form.Group className="mb-3">
                      <Form.Check
                        type="switch"
                        id="custom-switch"
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
                      <Form.Label> Version Identifier Key Name</Form.Label>
                      <br />

                      <Form.Control
                        className="mt-2"
                        type="text"
                        id="versionIdentifier"
                        placeholder="Enter key Name"
                        name="versionIdentifier"
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
  );
}

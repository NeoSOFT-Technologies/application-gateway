import React from "react";
import { Col, Form, Row } from "react-bootstrap";

export default function TargetUrl() {
  return (
    <div>
      <div className="card">
        <div>
          <div className="align-items-center justify-content-around">
            <div className="card-header !important">Targets</div>
            <div className="card-body">
              <div>
                <Row>
                  <Col md="12">
                    <Form.Group className="mb-3">
                      <Form.Label> Target Url :</Form.Label>
                      <br />
                      <i className="mb-3">
                        Supported protocol schemes:
                        http,https,tcp,tls,h2c,tyk,ws,wss.If empty, fallback to
                        default protocolof current API.:
                      </i>
                      <Form.Control
                        className="mt-2"
                        type="text"
                        id="name"
                        placeholder="Enter Target Url"
                        name="name"
                        required
                      />
                      <i>
                        If you add a trailing &apos;/ &apos; to your listen
                        path, you can only make requests that include the
                        trailing &apos;/ &apos;
                      </i>
                    </Form.Group>
                  </Col>
                  <Col md="12">
                    <Form.Group className="mb-3">
                      <Form.Check
                        type="switch"
                        id="custom-switch"
                        label="Enable round-robin load balancing"
                      />
                    </Form.Group>
                  </Col>
                  <Col md="12">
                    <Form.Group className="mb-3">
                      <Form.Check
                        type="switch"
                        id="custom-switch"
                        label="Enable service discovery"
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

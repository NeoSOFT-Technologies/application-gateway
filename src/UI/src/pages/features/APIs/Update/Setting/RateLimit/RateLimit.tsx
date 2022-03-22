import React from "react";
import { Col, Form, Row } from "react-bootstrap";

export default function RateLimit() {
  return (
    <div>
      <div className="card">
        <div>
          <div className="align-items-center justify-content-around">
            <div className="card-header !important">
              Rate Limiting And Quotas
            </div>
            <div className="card-body">
              <div>
                <Row>
                  <Col md="12">
                    <Form.Group className="mb-3">
                      <Form.Check
                        type="switch"
                        id="custom-switch"
                        label="Disable rate limiting"
                      />
                    </Form.Group>
                  </Col>
                  <Col md="12">
                    <i> Global Rate Limiting</i>
                    <i className="mb-3">
                      Turn on global rate limit for the whole Api.Key specific
                      rate limit will still work, but separate Api global rate
                      limiter will have higher priority and will be aggregated
                      across all keys.
                    </i>
                    <Form.Group className="mb-3">
                      <Form.Label> Rate</Form.Label>
                      <br />

                      <Form.Control
                        className="mt-2"
                        type="text"
                        id="rate"
                        placeholder="Enter rate"
                        name="rate"
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
                      <Form.Label> Per (Seconds)</Form.Label>
                      <br />

                      <Form.Control
                        className="mt-2"
                        type="text"
                        id="per"
                        placeholder="Enter time"
                        name="per"
                        required
                      />
                    </Form.Group>
                  </Col>
                  <Col md="12">
                    <Form.Group className="mb-3">
                      <Form.Check
                        type="switch"
                        id="custom-switch"
                        label="Disable quotas"
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

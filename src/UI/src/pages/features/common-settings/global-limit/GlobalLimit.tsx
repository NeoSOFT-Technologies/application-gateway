import React from "react";
import { Col, Form, Row } from "react-bootstrap";
export default function GlobalLimit() {
  return (
    <>
      <div className="card">
        <div>
          <div className="accordion " id="accordionGlobalLimit">
            <div className="accordion-item ">
              <h2 className="accordion-header" id="headingGlobalLimit">
                <button
                  className="accordion-button"
                  type="button"
                  data-bs-toggle="collapse"
                  data-bs-target="#collapseGlobalLimit"
                  aria-expanded="true"
                  aria-controls="collapseGlobalLimit"
                >
                  Global Limits and Quota
                </button>
              </h2>
              <div
                id="collapseGlobalLimit"
                className="accordion-collapse collapse show "
                aria-labelledby="headingGlobalLimit"
                data-bs-parent="#accordionGlobalLimit"
              >
                <div className="accordion-body ">
                  <Row>
                    <Col md="4">
                      <Form.Group className="mb-3">
                        <Form.Label>
                          <b>Rate Limiting</b>
                        </Form.Label>
                        <Form.Check
                          type="switch"
                          id="disableGlobalRate"
                          name="GlobalLimit.IsDisabled"
                          label="Disable rate limiting"
                        />
                        <Form.Label>Rate</Form.Label>
                        <br />
                        <Form.Control
                          className="mt-2"
                          type="text"
                          id="rate"
                          placeholder="Enter Rate"
                          name="RateLimit.Rate"
                        />
                        <Form.Label>Per (Seconds)</Form.Label>
                        <br />
                        <Form.Control
                          className="mt-2"
                          type="text"
                          id="rate"
                          placeholder="Enter time"
                          name="RateLimit.Per"
                        />
                      </Form.Group>
                    </Col>
                    <Col md="4">
                      <Form.Group className="mb-3">
                        <Form.Label>
                          <b>Throttling</b>
                        </Form.Label>
                        <Form.Check
                          type="switch"
                          id="disableThrottling"
                          name="Throttling.IsDisabled"
                          label="Disable Throttling"
                        />
                        <Form.Label>Throttle retry limit</Form.Label>
                        <br />
                        <Form.Control
                          className="mt-2"
                          type="text"
                          id="rate"
                          placeholder="Enter retry limit"
                          name="Throttling.Retry"
                        />
                        <Form.Label>Throttle interval</Form.Label>
                        <br />
                        <Form.Control
                          className="mt-2"
                          type="text"
                          id="rate"
                          placeholder="Enter interval"
                          name="Throttling.Interval"
                        />
                      </Form.Group>
                    </Col>
                    <Col md="4">
                      <Form.Group className="mb-3">
                        <Form.Label>
                          <b>Usage Quota</b>
                        </Form.Label>
                        <Form.Check
                          type="switch"
                          id="unlimitedRequests"
                          name="unlimitedRequests.IsDisabled"
                          label="Unlimited requests"
                        />
                        <Form.Label>Max requests per period</Form.Label>
                        <br />
                        <Form.Control
                          className="mt-2"
                          type="text"
                          id="rate"
                          placeholder="Enter request per period"
                          name="Quota.Per"
                        />
                        <Form.Label> Quota resets every</Form.Label>
                        <Form.Select className="mt-2" style={{ height: 45 }}>
                          <option>never</option>
                          <option>1 hour</option>
                          <option>6 hour</option>
                          <option>12 hour</option>
                          <option>1 week</option>
                          <option>1 month</option>
                          <option>6 months</option>
                          <option>12 months</option>
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
    </>
  );
}

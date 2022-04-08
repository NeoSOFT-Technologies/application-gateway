import React, { useState } from "react";
import { Col, Form, Row } from "react-bootstrap";
export default function GlobalLimit() {
  const [rate, setRate] = useState(false);
  const [throttle, setThrottle] = useState(true);
  const [quota, setQuota] = useState(true);
  const [throttleRetry, setThrottleRetry] = useState("Disabled throttling");
  const [throttleInterval, setThrottleInterval] = useState(
    "Disabled throttling"
  );
  const [quotaPerPeriod, setQuotaPerPeriod] = useState("Unlimited");

  function handleThrottleChange(evt: any) {
    setThrottle(evt.target.checked);
    if (throttle === false) {
      setThrottleRetry("Disabled throttling");
      setThrottleInterval("Disabled throttling");
    } else {
      setThrottleRetry("Enter retry limit");
      setThrottleInterval("Enter interval");
    }
  }

  function handleQuotaChange(evt: any) {
    setQuota(evt.target.checked);
    if (quota === false) {
      setQuotaPerPeriod("Unlimited");
    } else {
      setQuotaPerPeriod("Enter request per period");
    }
  }
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
                    <Row>
                      <Col md="4">
                        <Form.Group className="mb-3">
                          <Form.Label className="mt-2">
                            <b>Rate Limiting</b>
                          </Form.Label>
                          <Form.Check
                            type="switch"
                            id="disableGlobalRate"
                            name="GlobalLimit.IsDisabled"
                            label="Disable rate limiting"
                            checked={rate}
                            onChange={(e: any) => setRate(e.target.checked)}
                          />
                          <Form.Label className="mt-3">Rate</Form.Label>
                          <br />
                          <Form.Control
                            className="mt-2"
                            type="text"
                            id="rate"
                            placeholder="Enter Rate"
                            name="RateLimit.Rate"
                            disabled={rate}
                          />
                          <Form.Control.Feedback type="invalid"></Form.Control.Feedback>
                          <Form.Label className="mt-3">
                            Per (Seconds)
                          </Form.Label>
                          <br />
                          <Form.Control
                            className="mt-2"
                            type="text"
                            id="rate"
                            placeholder="Enter time"
                            name="RateLimit.Per"
                            disabled={rate}
                          />
                          <Form.Control.Feedback type="invalid"></Form.Control.Feedback>
                        </Form.Group>
                      </Col>
                      <Col md="4">
                        <Form.Group className="mb-3">
                          <Form.Label className="mt-2">
                            <b>Throttling</b>
                          </Form.Label>
                          <Form.Check
                            type="switch"
                            id="disableThrottling"
                            name="Throttling.IsDisabled"
                            label="Disable Throttling"
                            checked={throttle}
                            onChange={(e: any) => handleThrottleChange(e)}
                          />
                          <Form.Label className="mt-3">
                            Throttle retry limit
                          </Form.Label>
                          <br />
                          <Form.Control
                            className="mt-2"
                            type="text"
                            id="rate"
                            placeholder={throttleRetry}
                            name="Throttling.Retry"
                            // value={throttleDefault}
                            disabled={throttle}
                          />

                          <Form.Control.Feedback type="invalid"></Form.Control.Feedback>
                          <Form.Label className="mt-3">
                            Throttle interval
                          </Form.Label>
                          <br />
                          <Form.Control
                            className="mt-2"
                            type="text"
                            id="rate"
                            placeholder={throttleInterval}
                            name="Throttling.Interval"
                            disabled={throttle}
                          />
                        </Form.Group>
                      </Col>
                      <Col md="4">
                        <Form.Group className="mb-3">
                          <Form.Label className="mt-2">
                            <b>Usage Quota</b>
                          </Form.Label>
                          <Form.Check
                            type="switch"
                            id="unlimitedRequests"
                            name="unlimitedRequests.IsDisabled"
                            label="Unlimited requests"
                            checked={quota}
                            onChange={(e: any) => handleQuotaChange(e)}
                          />
                          <Form.Label className="mt-3">
                            Max requests per period
                          </Form.Label>
                          <br />
                          <Form.Control
                            className="mt-2"
                            type="text"
                            id="rate"
                            placeholder={quotaPerPeriod}
                            name="Quota.Per"
                            disabled={quota}
                          />
                          <Form.Control.Feedback type="invalid"></Form.Control.Feedback>
                          <Form.Label className="mt-3">
                            Quota resets every
                          </Form.Label>
                          <Form.Select
                            className="mt-2"
                            style={{ height: 46 }}
                            disabled={quota}
                          >
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

import React, { useEffect, useState } from "react";
import { Col, Form, Row } from "react-bootstrap";
import { IPolicyUpdateState } from "../../../../store/features/policy/update";
import { getPolicybyId } from "../../../../store/features/policy/update/slice";
import { useAppDispatch, useAppSelector } from "../../../../store/hooks";
// import { setFormData } from "../../../../resources/api/api-constants";
import { setForm } from "../../../../store/features/policy/create/slice";
import { IPolicyCreateState } from "../../../../store/features/policy/create";
// import statusAndDateHelper from "../../../../utils/helper";
interface IProps {
  isDisabled: boolean;
  // state: any;
  policyId?: any;
}

export default function GlobalLimit(props: IProps) {
  const Policy: IPolicyUpdateState = useAppSelector(
    (state) => state.updatePolicyState
  );
  const dispatch = useAppDispatch();
  const state: IPolicyCreateState = useAppSelector(
    (RootStates) => RootStates.createPolicyState
  );

  const mainCall = async (id: string) => {
    dispatch(getPolicybyId(id));
  };
  useEffect(() => {
    mainCall(props.policyId);
  }, []);

  console.log("mypolicies", Policy);

  const [rate, setRate] = useState(props.isDisabled);
  const [throttle, setThrottle] = useState(true);
  const [quota, setQuota] = useState(true);
  const [throttleRetry, setThrottleRetry] = useState("Disabled throttling");
  const [throttleInterval, setThrottleInterval] = useState(
    "Disabled throttling"
  );
  const [quotaPerPeriod, setQuotaPerPeriod] = useState("Unlimited");

  const [rateValue, setRateValue] = useState("");
  const [perValue, setPerValue] = useState("");
  const [retryValue, setRetryValue] = useState("");
  const [intervalValue, setIntervalValue] = useState("");
  const [maxQuotaValue, setMaxQuotaValue] = useState("");
  const [quotaResetValue, setQuotaResetValue] = useState("");

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

  function validateForm(event: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = event.target;
    setRateValue(event.target.value);
    dispatch(setForm({ ...state.data.form, [name]: value }));
    // setFormData(event, dispatch, state);
    console.log("key", name, value);
  }
  console.log("newvalue", state.data.form);
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
                            disabled={props.isDisabled}
                            // checked={rate}
                            onChange={(e: any) => setRate(e.target.checked)}
                          />
                          <Form.Label className="mt-3">Rate</Form.Label>
                          <br />
                          <Form.Control
                            className="mt-2"
                            type="text"
                            id="rate"
                            placeholder="Enter Rate"
                            value={
                              props.isDisabled
                                ? Policy.data.form.rate
                                : rateValue
                            }
                            onChange={(e: any) => validateForm(e)}
                            name="Rate"
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
                            id="per"
                            placeholder="Enter time"
                            value={
                              props.isDisabled ? Policy.data.form.per : perValue
                            }
                            onChange={(e: any) => setPerValue(e.target.value)}
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
                            disabled={props.isDisabled}
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
                            id="retry"
                            placeholder={throttleRetry}
                            name="Throttling.Retry"
                            value={
                              props.isDisabled
                                ? Policy.data.form.throttleRetries
                                : retryValue
                            }
                            onChange={(e: any) => setRetryValue(e.target.value)}
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
                            id="interval"
                            placeholder={throttleInterval}
                            value={
                              props.isDisabled
                                ? Policy.data.form.throttleInterval
                                : intervalValue
                            }
                            onChange={(e: any) =>
                              setIntervalValue(e.target.value)
                            }
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
                            disabled={props.isDisabled}
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
                            id="quotaPer"
                            placeholder={quotaPerPeriod}
                            value={
                              props.isDisabled
                                ? Policy.data.form.maxQuota
                                : maxQuotaValue
                            }
                            onChange={(e: any) =>
                              setMaxQuotaValue(e.target.value)
                            }
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
                            value={
                              props.isDisabled
                                ? Policy.data.form.quotaRate
                                : quotaResetValue
                            }
                            onChange={(e: any) =>
                              setQuotaResetValue(e.target.value)
                            }
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

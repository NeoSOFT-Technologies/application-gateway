import React, { useState } from "react";
import { Accordion, Col, Form, Row } from "react-bootstrap";
import {
  setForm,
  setFormError,
} from "../../../../store/features/policy/create/slice";
import { useAppDispatch, useAppSelector } from "../../../../store/hooks";
import { IPolicyCreateState } from "../../../../store/features/policy/create";
import Spinner from "../../../../components/loader/Loader";
import { IKeyCreateState } from "../../../../store/features/key/create";
import {
  setFormErrors,
  setForms,
} from "../../../../store/features/key/create/slice";
import {
  regexForNumber,
  // setFormErrors,
} from "../../../../resources/api/api-constants";

interface IProps {
  state?: IPolicyCreateState;
  keystate?: IKeyCreateState;
  current: string;
}

export default function GlobalRateLimit(props: IProps) {
  const dispatch = useAppDispatch();
  const states = useAppSelector((RootState) => RootState.createKeyState);
  const state: IPolicyCreateState = useAppSelector(
    (RootStates) => RootStates.createPolicyState
  );

  function validateForm(event: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = event.target;
    switch (name) {
      case "rate":
        props.current === "policy"
          ? dispatch(
              setFormError({
                ...state.data.errors,
                Rate: regexForNumber.test(value) ? "" : "Enter only Numbers",
              })
            )
          : dispatch(
              setFormErrors({
                ...states.data.errors,
                Rate: regexForNumber.test(value) ? "" : "Enter only Numbers",
              })
            );
        break;
      case "per":
        props.current === "policy"
          ? dispatch(
              setFormError({
                ...state.data.errors,
                Per: regexForNumber.test(value) ? "" : "Enter only Numbers",
              })
            )
          : dispatch(
              setFormErrors({
                ...states.data.errors,
                Per: regexForNumber.test(value) ? "" : "Enter only Numbers",
              })
            );
        break;
      case "throttle_retry_limit":
        props.current === "policy"
          ? dispatch(
              setFormError({
                ...state.data.errors,
                ThrottleRetries: regexForNumber.test(value)
                  ? ""
                  : "Enter only Numbers",
              })
            )
          : dispatch(
              setFormErrors({
                ...states.data.errors,
                ThrottleRetries: regexForNumber.test(value)
                  ? ""
                  : "Enter only Numbers",
              })
            );
        break;
      case "throttle_interval":
        props.current === "policy"
          ? dispatch(
              setFormError({
                ...state.data.errors,
                ThrottleInterval: regexForNumber.test(value)
                  ? ""
                  : "Enter only Numbers",
              })
            )
          : dispatch(
              setFormErrors({
                ...states.data.errors,
                ThrottleInterval: regexForNumber.test(value)
                  ? ""
                  : "Enter only Numbers",
              })
            );
        break;
      case "quota_max":
        props.current === "policy"
          ? dispatch(
              setFormError({
                ...state.data.errors,
                Quota: regexForNumber.test(value) ? "" : "Enter only Numbers",
              })
            )
          : dispatch(
              setFormErrors({
                ...states.data.errors,
                Quota: regexForNumber.test(value) ? "" : "Enter only Numbers",
              })
            );
        break;
      default:
        break;
    }
  }

  const [Limits, setLimits] = useState<any>({
    rate: 0,
    per: 0,
    throttle_interval: 0,
    throttle_retry_limit: 0,
    max_query_depth: 0,
    quota_max: 0,
    quota_renews: 0,
    quota_remaining: 0,
    quota_renewal_rate: 0,
    set_by_policy: false,
  });
  const [rate, setRate] = useState(false);
  const [throttle, setThrottle] = useState(true);
  const [quota, setQuota] = useState(true);
  const [throttleRetry, setThrottleRetry] = useState("Disabled throttling");
  const [throttleInterval, setThrottleInterval] = useState(
    "Disabled throttling"
  );
  const [quotaPerPeriod, setQuotaPerPeriod] = useState("Unlimited");

  const handlerateclick = (event: any) => {
    event.preventDefault();
    validateForm(event);
    let fieldValue;

    const fieldName = event.target.getAttribute("name");
    if (fieldName === "quota_renews") {
      switch (event.target.value) {
        case "1 hour":
          fieldValue = 3600;
          console.log(fieldValue);
          break;
        case "6 hour":
          fieldValue = 21600;
          break;
        case "12 hour":
          fieldValue = 43200;
          break;
        case "1 week":
          fieldValue = 604800;
          break;
        case "1 months":
          fieldValue = 2.628e6;
          break;
        case "6 months":
          fieldValue = 1.577e7;
          break;
        case "12 months":
          fieldValue = 3.154e7;
          break;
      }
    } else {
      fieldValue = event.target.value;
    }
    console.log("ye field values - ", fieldValue);
    const newFormData: any = { ...Limits };
    newFormData[fieldName] = fieldValue;
    setLimits(newFormData);

    props.current === "policy"
      ? dispatch(
          setForm({
            ...state.data.form,
            Rate: newFormData.rate,
            Per: newFormData.per,
            MaxQuota: newFormData.quota_max,
            QuotaRate: newFormData.quota_renews,
            ThrottleInterval: newFormData.throttle_interval,
            ThrottleRetries: newFormData.throttle_retry_limit,
          })
        )
      : dispatch(
          setForms({
            ...states.data.form,
            Rate: newFormData.rate,
            Per: newFormData.per,
            Quota: newFormData.quota_max,
            QuotaRenewalRate: newFormData.quota_renews,
            ThrottleInterval: newFormData.throttle_interval,
            ThrottleRetries: newFormData.throttle_retry_limit,
          })
        );
  };
  console.log("checklimit", state.data.form);
  console.log("checklimit2", states.data.form);
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
      {state.loading === false ? (
        <div className="card">
          <Accordion defaultActiveKey="0">
            <Accordion.Item eventKey="0">
              <Accordion.Header>Global Limits and Quota</Accordion.Header>

              <Accordion.Body>
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
                          // checked={rate}
                          onChange={(e: any) => setRate(e.target.checked)}
                        />
                        <Form.Label className="mt-3">Rate</Form.Label>
                        <br />

                        <Form.Control
                          className="mt-2"
                          type="text"
                          id="rate"
                          placeholder="Enter Request per period"
                          value={
                            props.current === "policy"
                              ? state.data.form.Rate
                              : states.data.form.Rate
                          }
                          // onChange={(e: any) => validateForm(e)}
                          onChange={(e: any) => handlerateclick(e)}
                          name="rate"
                          isInvalid={
                            props.current === "policy"
                              ? !!state.data.errors?.Rate
                              : !!states.data.errors?.Rate
                          }
                          isValid={
                            props.current === "policy"
                              ? !state.data.errors?.Rate
                              : !states.data.errors?.Rate
                          }
                          disabled={rate}
                        />
                        <Form.Control.Feedback type="invalid">
                          {" "}
                          {props.current === "policy"
                            ? state.data.errors?.Rate
                            : states.data.errors?.Rate}
                        </Form.Control.Feedback>
                        <Form.Label className="mt-3">Per (Seconds)</Form.Label>
                        <br />
                        <Form.Control
                          className="mt-2"
                          type="text"
                          id="per"
                          value={
                            props.current === "policy"
                              ? state.data.form.Per
                              : states.data.form.Per
                          }
                          placeholder="Enter time"
                          onChange={(e: any) => handlerateclick(e)}
                          name="per"
                          isInvalid={
                            props.current === "policy"
                              ? !!state.data.errors?.Per
                              : !!states.data.errors?.Per
                          }
                          isValid={
                            props.current === "policy"
                              ? !state.data.errors?.Per
                              : !states.data.errors?.Per
                          }
                          disabled={rate}
                        />
                        <Form.Control.Feedback type="invalid">
                          {props.current === "policy"
                            ? state.data.errors?.Per
                            : states.data.errors?.Per}
                        </Form.Control.Feedback>
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
                          id="retry"
                          value={
                            props.current === "policy"
                              ? state.data.form.ThrottleRetries
                              : states.data.form.ThrottleRetries
                          }
                          placeholder={throttleRetry}
                          name="throttle_retry_limit"
                          onChange={(e: any) => handlerateclick(e)}
                          // value={throttleDefault}
                          isInvalid={
                            props.current === "policy"
                              ? !!state.data.errors?.ThrottleRetries
                              : !!states.data.errors?.ThrottleRetries
                          }
                          isValid={
                            props.current === "policy"
                              ? !state.data.errors?.ThrottleRetries
                              : !states.data.errors?.ThrottleRetries
                          }
                          disabled={throttle}
                        />
                        <Form.Control.Feedback type="invalid">
                          {props.current === "policy"
                            ? state.data.errors?.ThrottleRetries
                            : states.data.errors?.ThrottleRetries}
                        </Form.Control.Feedback>
                        <Form.Label className="mt-3">
                          Throttle interval
                        </Form.Label>
                        <br />
                        <Form.Control
                          className="mt-2"
                          type="text"
                          id="interval"
                          name="throttle_interval"
                          value={
                            props.current === "policy"
                              ? state.data.form.ThrottleInterval
                              : states.data.form.ThrottleInterval
                          }
                          placeholder={throttleInterval}
                          onChange={(e: any) => handlerateclick(e)}
                          isInvalid={
                            props.current === "policy"
                              ? !!state.data.errors?.ThrottleInterval
                              : !!states.data.errors?.ThrottleInterval
                          }
                          isValid={
                            props.current === "policy"
                              ? !state.data.errors?.ThrottleInterval
                              : !states.data.errors?.ThrottleInterval
                          }
                          disabled={throttle}
                        />
                        <Form.Control.Feedback type="invalid">
                          {props.current === "policy"
                            ? state.data.errors?.ThrottleInterval
                            : states.data.errors?.ThrottleInterval}
                        </Form.Control.Feedback>
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
                          id="quotaPer"
                          value={
                            props.current === "policy"
                              ? state.data.form.QuotaRate
                              : states.data.form.Quota
                          }
                          placeholder={quotaPerPeriod}
                          onChange={(e: any) => handlerateclick(e)}
                          name="quota_max"
                          isInvalid={
                            props.current === "policy"
                              ? !!state.data.errors?.Quota
                              : !!states.data.errors?.Quota
                          }
                          isValid={
                            props.current === "policy"
                              ? !state.data.errors?.Quota
                              : !states.data.errors?.Quota
                          }
                          disabled={quota}
                        />
                        <Form.Control.Feedback type="invalid">
                          {" "}
                          {props.current === "policy"
                            ? state.data.errors?.Quota
                            : states.data.errors?.Quota}
                        </Form.Control.Feedback>
                        <Form.Label className="mt-3">
                          Quota resets every
                        </Form.Label>
                        <Form.Select
                          className="mt-2"
                          style={{ height: 46 }}
                          disabled={quota}
                          name="quota_renews"
                          onChange={(e: any) => handlerateclick(e)}
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
              </Accordion.Body>
            </Accordion.Item>
          </Accordion>
        </div>
      ) : (
        <Spinner />
      )}
    </>
  );
}

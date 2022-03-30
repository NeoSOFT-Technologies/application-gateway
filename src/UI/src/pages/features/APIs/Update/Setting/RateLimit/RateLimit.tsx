import React from "react";
import { Col, Form, Row } from "react-bootstrap";
// import {
//   setFormErrors,
//   setFormData,
//   regexForNumber,
// } from "../../../../../../resources/APIS/ApiConstants";
// import { useAppDispatch, useAppSelector } from "../../../../../../store/hooks";

export default function RateLimit() {
  // const dispatch = useAppDispatch();
  // const state = useAppSelector((RootState) => RootState.getApiById);
  // console.log("Rate", state.data.form?.RateLimit.Rate);
  // function validateForm(event: React.ChangeEvent<HTMLInputElement>) {
  //   const { name, value } = event.target;

  //   switch (name) {
  //     case "rate":
  //       setFormErrors(
  //         {
  //           ...state.data.errors,
  //           [name]: regexForNumber.test(value) ? "" : "Enter only Numbers",
  //         },
  //         dispatch
  //       );
  //       break;
  //     case "perSecond":
  //       setFormErrors(
  //         {
  //           ...state.data.errors,
  //           [name]: regexForNumber.test(value) ? "" : "Enter only Numbers",
  //         },
  //         dispatch
  //       );
  //       break;
  //     default:
  //       break;
  //   }
  //   setFormData(event, dispatch, state);
  // }
  return (
    <div>
      <div className="card">
        <div>
          <div className="align-items-center justify-content-around">
            <div className="accordion" id="accordionRateLimit">
              <div className="accordion-item">
                <h2 className="accordion-header" id="headingFour">
                  <button
                    className="accordion-button"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#collapseFour"
                    aria-expanded="true"
                    aria-controls="collapseFour"
                  >
                    Rate Limiting And Quotas
                  </button>
                </h2>
                <div
                  id="collapseFour"
                  className="accordion-collapse collapse show"
                  aria-labelledby="headingFour"
                  data-bs-parent="#accordionRateLimit"
                >
                  <div className="accordion-body">
                    <div>
                      <Row>
                        <Col md="12">
                          <Form.Group className="mb-3">
                            <Form.Check
                              type="switch"
                              id="disableRate"
                              name="disableRate"
                              label="Disable rate limiting"
                            />
                          </Form.Group>
                        </Col>
                        <Col md="12">
                          <i> Global Rate Limiting</i>
                          <i className="mb-3">
                            Turn on global rate limit for the whole Api.Key
                            specific rate limit will still work, but separate
                            Api global rate limiter will have higher priority
                            and will be aggregated across all keys.
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
                              // value={state.data.form?.RateLimit.Rate}
                              // isInvalid={!!state.data.errors?.rate}
                              // isValid={!state.data.errors?.rate}
                              // onChange={(e: any) => validateForm(e)}
                              required
                            />
                            {/* <Form.Control.Feedback type="invalid">
                              {state.data.errors?.rate}
                            </Form.Control.Feedback> */}
                            <i>
                              If you add a trailing &apos;/ &apos; to your
                              listen path, you can only make requests that
                              include the trailing &apos;/ &apos;
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
                              id="perSecond"
                              placeholder="Enter time"
                              name="perSecond"
                              // value={state.data.form?.RateLimit.Per}
                              // isInvalid={!!state.data.errors?.perSecond}
                              // isValid={!state.data.errors?.perSecond}
                              // onChange={(e: any) => validateForm(e)}
                              required
                            />
                            {/* <Form.Control.Feedback type="invalid">
                              {state.data.errors?.perSecond}
                            </Form.Control.Feedback> */}
                          </Form.Group>
                        </Col>
                        <Col md="12">
                          <Form.Group className="mb-3">
                            <Form.Check
                              type="switch"
                              id="disableQuotas"
                              name="disableQuotas"
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
        </div>
      </div>
    </div>
  );
}

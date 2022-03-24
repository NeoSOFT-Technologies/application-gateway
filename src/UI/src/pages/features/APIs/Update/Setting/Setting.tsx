import React from "react";
import ListenPath from "./ListenPath/ListenPath";
// import { Accordion } from "react-bootstrap";
import RateLimit from "./RateLimit/RateLimit";
import TargetUrl from "./TargetUrl/TargetUrl";
import { Col, Form, Row } from "react-bootstrap";
import { IProps } from "../../../../../types/api";
import { changeApiUpdateForm } from "../../../../../resources/common";

export default function Setting(props: IProps) {
  // function changeApiUpdateForm(e: React.ChangeEvent<HTMLInputElement>) {
  //   props.onChange(e);
  // }
  return (
    <div>
      <div className="card">
        <div>
          <div className="align-items-center justify-content-around">
            <div className="accordion" id="accordionSetting">
              <div className="accordion-item">
                <h2 className="accordion-header" id="headingOne">
                  <button
                    className="accordion-button"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#collapseOne"
                    aria-expanded="true"
                    aria-controls="collapseOne"
                  >
                    API Settings
                  </button>
                </h2>
                <div
                  id="collapseOne"
                  className="accordion-collapse collapse show"
                  aria-labelledby="headingOne"
                  data-bs-parent="#accordionSetting"
                >
                  <div className="accordion-body">
                    <Row>
                      <Col md={12} className="mb-3">
                        <div className="h-50">
                          <Form.Group className="mb-3">
                            <Form.Label> API Name</Form.Label>
                            <br />

                            <Form.Control
                              className="mt-2"
                              type="text"
                              id="apiName"
                              placeholder="Enter API Name"
                              name="apiName"
                              // data-testid="name-input"
                              // value={api.apiName}
                              // isInvalid={!!props.err}
                              // isValid={!props.err && !!props.apisUpdateForm}
                              onChange={(e: any) =>
                                changeApiUpdateForm(e, props)
                              }
                              // required
                            />
                            {/* <Form.Control.Feedback type="invalid">
                      {props.err}
                    </Form.Control.Feedback> */}
                          </Form.Group>
                        </div>
                      </Col>
                    </Row>
                    <br />
                    <div>
                      <ListenPath
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) =>
                          changeApiUpdateForm(e, props)
                        }
                      />
                    </div>
                    <div>
                      <TargetUrl />
                    </div>
                    <div>
                      <RateLimit />
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

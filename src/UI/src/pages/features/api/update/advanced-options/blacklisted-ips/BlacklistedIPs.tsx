import React, { useState } from "react";
import { Form, Col, Row, Button } from "react-bootstrap";

export default function BlacklistedIPs() {
  const [enableBlacklist, setBlacklist] = useState(false);

  function validateForm(event: React.ChangeEvent<HTMLInputElement>) {
    console.log("check value: ", event.target.checked);
    setBlacklist(event.target.checked);

    // setFormData(event, dispatch, state);
  }

  return (
    <div>
      <div className="card">
        <div>
          <div className="align-items-center justify-content-around">
            <div className="accordion" id="accordionBlacklistedIPs">
              <div className="accordion-item">
                <h2 className="accordion-header" id="headingFive">
                  <button
                    className="accordion-button"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#collapseBlacklisted"
                    aria-expanded="true"
                    aria-controls="collapseBlacklisted"
                  >
                    Blacklisted IPs
                  </button>
                </h2>
                <div
                  id="collapseBlacklisted"
                  className="accordion-collapse collapse show"
                  aria-labelledby="headingFive"
                  data-bs-parent="#accordionBlacklistedIPs"
                >
                  <div className="accordion-body">
                    <div>
                      <Row>
                        <Col md="12">
                          <b>Enable Blacklisted IPs</b>
                          <p>
                            Blacklisted IPs limit the originating address of a
                            request to disallow group of addresses.
                          </p>
                        </Col>
                        <Col md="12">
                          <Form.Group className="mb-3">
                            <Form.Check
                              type="checkbox"
                              label="Enable Blacklisted IPs"
                              name="blacklisted"
                              checked={enableBlacklist}
                              onChange={(e: any) => validateForm(e)}
                            />
                          </Form.Group>
                        </Col>
                      </Row>
                      {enableBlacklist ? (
                        <div>
                          <b>Blacklisted IPs</b>
                          <p>No IPs selected, please add one below.</p>
                          <Row>
                            <Form.Label>
                              <b>IP Address:</b>
                            </Form.Label>
                            <Col md={10}>
                              <Form.Group className="mt-0">
                                <Form.Control
                                  type="text"
                                  placeholder="accounts.google.com"
                                  id="issuer"
                                  name="issuer"
                                />
                              </Form.Group>
                            </Col>
                            <Col md={2}>
                              <Form.Group className="mb-5">
                                <Form.Label></Form.Label>
                                <Button
                                  variant="dark"
                                  // disabled={!addFormData.issuer}
                                  // onClick={handleIssuerAddClick}
                                >
                                  Add
                                </Button>{" "}
                              </Form.Group>
                            </Col>
                          </Row>
                        </div>
                      ) : (
                        <></>
                      )}
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

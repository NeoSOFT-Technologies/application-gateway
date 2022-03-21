import React from "react";
import { Col, Form, Row } from "react-bootstrap";
// import { Button, Col, Container, Dropdown, Form, Row } from "react-bootstrap";

export default function ListenPath() {
  return (
    <>
      <div id="accordion">
        <div className="card">
          <div className="card-header" id="headingOne">
            Listen Path
          </div>
          <div className="card-body">
            <Form className="p-4">
              <Row>
                <Col md={12} className="mb-3">
                  <p>
                    The listen path dictates what path Tyk will listen on, if
                    you leave this blank, it will be automatically populated by
                    the ID of the API.
                  </p>
                  <Form.Group className="mb-3">
                    <Form.Label>
                      <b>Listen Path:</b>
                    </Form.Label>

                    <Form.Control
                      type="text"
                      placeholder="Enter listen path"
                      name="listenpath"
                      value=""
                    />
                  </Form.Group>
                  <i>
                    If you add a trailing &apos;/ &apos; to your listen path,
                    you can only make requests that include the trailing &apos;/
                    &apos;.
                  </i>
                </Col>
                <Col md={12}>
                  <Form.Group className="mb-3">
                    <Form.Label>
                      <b>Strip the Listen path</b>
                    </Form.Label>
                    <p>
                      If this setting is checked, then Tyk will remove the above
                      listen path from the inbound URL so that it does not
                      interfere with routing upstream.
                    </p>
                    {/* <Form.Control
                            className="text-left"
                            type="checkbox"
                            name="striplistenpath"
                            value=""
                          />
                          <span>Strip the Listen path</span> */}
                    <Form.Check
                      type="switch"
                      id="custom-switch"
                      label="Strip the Listen path"
                    />
                  </Form.Group>
                </Col>
                <Col md={12}>
                  <Form.Group className="mb-3">
                    <Form.Label>
                      <b>Internal</b>
                    </Form.Label>
                    <p>
                      If set, API can&apos;t be accessed except when using
                      Internal
                    </p>
                    {/* <Form.Control
                            className="text-left"
                            type="switch"
                            name="internal"
                            value=""
                          />
                          <span>Activated</span> */}

                    <Form.Check
                      type="switch"
                      id="custom-switch"
                      label="Activated"
                    />
                  </Form.Group>
                  {/* <ToggleButtonGroup
                          id="1"
                          disabled={true}
                        ></ToggleButtonGroup> */}
                </Col>
              </Row>
            </Form>
          </div>
        </div>
      </div>
    </>
  );
}

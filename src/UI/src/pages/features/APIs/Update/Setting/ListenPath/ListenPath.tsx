import React from "react";
import { Col, Form, Row } from "react-bootstrap";
interface IProps {
  onChange: Function;
}
export default function ListenPath(props: IProps) {
  function changeApiUpdateForm(e: React.ChangeEvent<HTMLInputElement>) {
    props.onChange(e);
  }
  return (
    <>
      <div className="accordion" id="accordionExample">
        <div className="accordion-item">
          <h2 className="accordion-header" id="headingTwo">
            <button
              className="accordion-button"
              type="button"
              data-bs-toggle="collapse"
              data-bs-target="#collapseTwo"
              aria-expanded="true"
              aria-controls="collapseTwo"
            >
              Listen Path
            </button>
          </h2>
          <div
            id="collapseTwo"
            className="accordion-collapse collapse show"
            aria-labelledby="headingTwo"
            data-bs-parent="#accordionExample"
          >
            <div className="accordion-body">
              <p>This is test</p>

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
                      id="listenPath"
                      name="listenPath"
                      data-testid="name-input"
                      onChange={changeApiUpdateForm}
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
                      id="stripListenPath"
                      name="stripListenPath"
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

                    <Form.Check
                      type="switch"
                      id="custom-switch"
                      label="Activated"
                    />
                  </Form.Group>
                </Col>
              </Row>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

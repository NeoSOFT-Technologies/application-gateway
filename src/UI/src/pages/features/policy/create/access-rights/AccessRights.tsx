import React from "react";
import { Row, Col, Form } from "react-bootstrap";
import { useAppSelector } from "../../../../../store/hooks";
// import { useAppSelector } from "../../../../../store/hooks";
import GlobalLimit from "../../../common-settings/global-limit/GlobalLimit";
import AccessList from "./api-access-rights/AccessList";
import ApiAccess from "./api-access/ApiAccess";
export default function AccessRights() {
  const state = useAppSelector((RootState) => RootState.createPolicyState);
  return (
    <div>
      <div>
        <div>
          <div className="align-items-center">
            <div className="pt-2">
              <AccessList />
              <GlobalLimit isDisabled={false} msg={""} />

              <div className="card col-lg-12 grid-margin stretch-card mt-3 p-3">
                <Row>
                  <Row>
                    <Col md="4">
                      <Form.Group className="mb-3">
                        <Form.Check
                          type="switch"
                          id="accessrights"
                          name="GlobalLimit.IsDisabled"
                          label="Enforce access rights"
                          disabled={false}
                          // onChange={(e: any) => setRate(e.target.checked)}
                        />

                        <Form.Control.Feedback type="invalid"></Form.Control.Feedback>
                      </Form.Group>
                    </Col>
                    <Col md="4">
                      <Form.Group className="mb-3">
                        <Form.Check
                          type="switch"
                          id="usagequota"
                          name="GlobalLimit.IsDisabled"
                          label="Enforce usage quota"
                          disabled={false}
                          // onChange={(e: any) => setRate(e.target.checked)}
                        />

                        <Form.Control.Feedback type="invalid"></Form.Control.Feedback>
                      </Form.Group>
                    </Col>{" "}
                    <Col md="4">
                      <Form.Group className="mb-3">
                        <Form.Check
                          type="switch"
                          id="GlobalRate"
                          name="GlobalLimit.IsDisabled"
                          label="Enforce rate limit"
                          disabled={false}
                          // onChange={(e: any) => setRate(e.target.checked)}
                        />

                        <Form.Control.Feedback type="invalid"></Form.Control.Feedback>
                      </Form.Group>
                    </Col>
                  </Row>
                </Row>
              </div>
              {state.data.form.ApIs?.length > 0 ? <ApiAccess /> : <></>}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

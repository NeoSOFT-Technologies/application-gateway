import React from "react";
import { Button, Col, Form, Row } from "react-bootstrap";

export default function LoadBalancing() {
  return (
    <div>
      <Row>
        <i className="mb-3">
          Tyk can perform round-robin load balancing on a series of upstream
          targets, you will need to add all of the targets using the fields
          below.
        </i>
        <br />
        <br />
        <Form.Label> Add Target URL&apos;s :</Form.Label>
        <Col md="10">
          <Form.Group className="mb-3">
            <Form.Control
              className="mt-2"
              type="text"
              id="targetUrl"
              placeholder="Please enter target(s) and hit enter key"
              name="TargetUrl"
            />
            {/* <Form.Control.Feedback type="invalid">
              {state.data.errors?.TargetUrl}
            </Form.Control.Feedback> */}
          </Form.Group>
        </Col>
        <Col md="2">
          <Form.Group className="mt-2 mb-3">
            <Button className="" variant="dark">
              Add
            </Button>{" "}
          </Form.Group>
        </Col>
        <i>
          If you add a trailing &apos;/ &apos; to your listen path, you can only
          make requests that include the trailing &apos;/ &apos;
        </i>
      </Row>
    </div>
  );
}

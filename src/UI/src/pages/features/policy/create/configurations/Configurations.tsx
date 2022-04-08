import React from "react";
import { Form, Accordion } from "react-bootstrap";

export default function Configurations() {
  return (
    <div>
      <Accordion defaultActiveKey="0">
        <Accordion.Item eventKey="0">
          <Accordion.Header>
            <h5>Configuration Page</h5>
          </Accordion.Header>
          <Accordion.Body>
            <Accordion defaultActiveKey="0">
              <Accordion.Item eventKey="0">
                <Accordion.Header>
                  <h5>Policy Name</h5>
                </Accordion.Header>
                <Accordion.Body>
                  <Form.Control
                    type="text"
                    placeholder="DemoPolicy "
                    required
                  />
                </Accordion.Body>
              </Accordion.Item>
            </Accordion>
            <Accordion defaultActiveKey="0">
              <Accordion.Item eventKey="0">
                <Accordion.Header>
                  <h5>Settings</h5>
                </Accordion.Header>
                <Accordion.Body>
                  <Form.Label>Policy status : </Form.Label>
                  <Form.Select aria-label="Default select example">
                    <option selected>Select an option</option>
                    <option value="1">Active</option>
                    <option value="2">Draft</option>
                    <option value="3">Access Deneid</option>
                  </Form.Select>
                  <Form.Label> Key expires after :</Form.Label>
                  <Form.Select aria-label="Default select example">
                    <option selected>Select an option</option>
                    <option value="1">Do not expire key</option>
                    <option value="2">1 hour</option>
                    <option value="3">6 hours</option>
                    <option value="4">12 hours</option>
                    <option value="5">24 hours</option>
                    <option value="6">1 week</option>
                    <option value="7">2 weeks</option>
                  </Form.Select>
                </Accordion.Body>
              </Accordion.Item>
            </Accordion>
          </Accordion.Body>
        </Accordion.Item>
      </Accordion>
    </div>
  );
}

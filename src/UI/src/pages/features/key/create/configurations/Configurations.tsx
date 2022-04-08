import React from "react";
import { Form, Accordion } from "react-bootstrap";

function Configurations() {
  return (
    <div>
      <Accordion defaultActiveKey="0">
        <Accordion.Item eventKey="0">
          <Accordion.Header>
            <h5>Settings</h5>
          </Accordion.Header>
          <Accordion.Body>
            <div className="align-items-center">
              <Form.Label>API Status :</Form.Label>
              <Form.Check type="switch" />
              <br />
              <br />
              <Form.Label> Alias :</Form.Label>
              <Form.Control
                type="text"
                placeholder="Give your key alias to remember it by "
                required
              />
              <br />
              <br />
              <Form.Label> Expires :</Form.Label>
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
            </div>
          </Accordion.Body>
        </Accordion.Item>
      </Accordion>
    </div>
  );
}
export default Configurations;

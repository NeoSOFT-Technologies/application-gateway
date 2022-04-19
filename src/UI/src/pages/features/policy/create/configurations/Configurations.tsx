import React from "react";
import { Form, Accordion } from "react-bootstrap";
import {
  setFormErrors,
  setFormData,
  regexForName,
} from "../../../../../resources/policy/policy-constants";
import { useAppDispatch, useAppSelector } from "../../../../../store/hooks";

export default function Configurations() {
  const dispatch = useAppDispatch();
  const state = useAppSelector((RootState) => RootState.createPolicyState);
  function validateForm(event: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = event.target;

    switch (name) {
      case "name":
        setFormErrors(
          {
            ...state.data.errors,
            [name]: regexForName.test(value) ? "" : "Enter a Valid Policy name",
          },
          dispatch
        );
        break;
      default:
        break;
    }

    setFormData(event, dispatch, state);
    console.log("policyId", name, value);
  }
  return (
    <div>
      <Accordion defaultActiveKey="0">
        <Accordion.Item eventKey="0">
          <Accordion.Header>
            <span>Policy Name</span>
          </Accordion.Header>
          <Accordion.Body>
            <Form.Control
              type="text"
              placeholder="DemoPolicy "
              name="name"
              id="policyId"
              data-testid="name-input"
              value={state.data.form?.name}
              isInvalid={!!state.data.errors?.name}
              isValid={!state.data.errors?.name}
              onChange={(e: any) => validateForm(e)}
              required
            />
            <Form.Control.Feedback type="invalid">
              {state.data.errors?.name}
            </Form.Control.Feedback>
          </Accordion.Body>
        </Accordion.Item>
      </Accordion>
      <br />
      <Accordion defaultActiveKey="0">
        <Accordion.Item eventKey="0">
          <Accordion.Header>
            <span>Settings</span>
          </Accordion.Header>
          <Accordion.Body>
            <Form.Label>Policy status : </Form.Label>
            <Form.Select aria-label="Default select example">
              <option>Select an option</option>
              <option value="1">Active</option>
              <option value="2">Draft</option>
              <option value="3">Access Deneid</option>
            </Form.Select>
            <br />
            <Form.Label> Key expires after :</Form.Label>
            <Form.Select aria-label="Default select example">
              <option>Select an option</option>
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
    </div>
  );
}

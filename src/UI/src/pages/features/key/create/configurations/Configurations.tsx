import React from "react";
import { Form, Accordion } from "react-bootstrap";
import {
  setFormErrors,
  setFormData,
  regexForName,
} from "../../../../../resources/key/key-constants";
import { useAppDispatch, useAppSelector } from "../../../../../store/hooks";

function Configurations() {
  const dispatch = useAppDispatch();
  const state = useAppSelector((RootState) => RootState.createKeyState);
  function validateForm(event: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = event.target;

    switch (name) {
      case "keyName":
        setFormErrors(
          {
            ...state.data.errors,
            [name]: regexForName.test(value) ? "" : "Enter a Valid Name",
          },
          dispatch
        );
        break;
      default:
        break;
    }

    setFormData(event, dispatch, state);
    console.log("keyId", name, value);
  }
  return (
    <div>
      <Accordion defaultActiveKey="0">
        <Accordion.Item eventKey="0">
          <Accordion.Header>
            <span>Settings</span>
          </Accordion.Header>
          <Accordion.Body>
            <div className="align-items-center">
              <Form.Label>Enabled detailed logging :</Form.Label>
              <Form.Check type="switch" />
              <br />
              <Form.Label> Alias :</Form.Label>
              <Form.Control
                type="text"
                placeholder="Give your key alias to remember it by "
                name="keyName"
                id="keyName"
                data-testid="name-input"
                value={state.data.form?.keyName}
                isInvalid={!!state.data.errors?.keyName}
                isValid={!state.data.errors?.keyName}
                onChange={(e: any) => validateForm(e)}
                required
              />
              <Form.Control.Feedback type="invalid">
                {state.data.errors?.keyName}
              </Form.Control.Feedback>
              <br />
              <Form.Label> Expires :</Form.Label>
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
            </div>
          </Accordion.Body>
        </Accordion.Item>
      </Accordion>
    </div>
  );
}
export default Configurations;

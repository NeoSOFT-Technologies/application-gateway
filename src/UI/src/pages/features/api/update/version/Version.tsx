import React, { useState } from "react";
import Versions from "./versions/Versions";
import VersionSettings from "./version-settings/VersionSettings";
import { Col, Form } from "react-bootstrap";

export default function Version() {
  const isVersioning: Boolean = true;
  let [check, setDisabledVersioning] = useState(true);

  if (isVersioning) {
    check = false;
  }

  return (
    <div>
      <Col md="12">
        <Form.Group className="mb-3">
          <Form.Check
            type="switch"
            id="isEnabledVersioning"
            name="isEnabledVersioning"
            label="Disable Versioning"
            checked={check}
            onChange={(e: any) => setDisabledVersioning(e.target.checked)}
          />
        </Form.Group>
      </Col>
      {check ? (
        <></>
      ) : (
        <div>
          <VersionSettings />
          <Versions />
        </div>
      )}
    </div>
  );
}

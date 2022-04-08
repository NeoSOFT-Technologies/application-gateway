import React, { useState } from "react";
import { Form, Row, Col } from "react-bootstrap";
import Ipathpermission from "../../../../pages/features/common-settings/path-based-permission/path-file";
export default function PathBased() {
  const [isActive, setisActive] = useState<boolean>(false);
  const [isActiveApi, setisActiveApi] = useState<boolean>(false);
  const setPathPermission = (event: React.ChangeEvent<HTMLInputElement>) => {
    const value =
      event.target.type === "checkbox"
        ? event.target.checked
        : event.target.value;
    if (event.target.getAttribute("name") === "isActiveApi") {
      setisActiveApi(Boolean(value));
    } else {
      setisActive(Boolean(value));
    }
  };
  const handleversion = (event: any) => {
    console.log(event.target.value);
  };

  return (
    <div className="accordion m-2" id="accordionExample">
      <div className="card-body pt-2">
        <h2 className="accordion-header " id="headingOne">
          <button
            className="accordion-button"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#collapseOne"
            aria-expanded="true"
            aria-controls="collapseOne"
          >
            Your API
          </button>
        </h2>

        <div
          id="collapseOne"
          className="accordion-item accordion-collapse collapse show"
          aria-labelledby="headingOne"
          data-bs-parent="#accordionExample"
        >
          <div className="accordion-body">
            <Row>
              <Col md="12">
                <button
                  className="btn btn-danger"
                  style={{ float: "right" }}
                  type="button"
                >
                  Remove Access
                </button>
              </Col>
            </Row>
            <Form>
              <Row>
                <Col md="12">
                  <Form.Group className="mb-3 mt-3">
                    <Form.Select
                      style={{ height: 46 }}
                      name="method"
                      onClick={(e: any) => handleversion(e)}
                    >
                      <option value="Default">Default</option>
                      <option value="V1">V1</option>
                      <option value="V2">V2</option>
                    </Form.Select>
                  </Form.Group>
                </Col>
              </Row>

              <div className="w-100 p-3 border rounded mt-3">
                <Row>
                  <Col md="12">
                    <Form.Group className="mt-6">
                      <Form.Label>
                        <b>Set Per Api Limits and Quota</b>
                      </Form.Label>
                      <Form.Check
                        className="float-lg-end"
                        type="switch"
                        name="isActiveApi"
                        onChange={setPathPermission}
                        checked={isActiveApi}
                        id="isActiveApi"
                      />
                    </Form.Group>
                  </Col>
                  <Col md="12">
                    <Form.Group className="mt-6">
                      <Form.Label>
                        {" "}
                        This Api with inherit the Global Limit settings above
                        unless per Api limits and quotas are set here.
                      </Form.Label>
                    </Form.Group>
                  </Col>
                </Row>
              </div>
              <div className="w-100 p-3 mt-3 border rounded">
                <Row>
                  <Col md="12">
                    <Form.Group className="mt-6">
                      <Form.Label>
                        <b>Path Based Permission</b>
                      </Form.Label>
                      <Form.Check
                        className="float-lg-end"
                        type="switch"
                        name="isActive"
                        onChange={setPathPermission}
                        checked={isActive}
                        id="isActive"
                      />
                    </Form.Group>
                  </Col>
                  <Col md="12">
                    <Form.Group className="mt-6">
                      <Form.Label>
                        {" "}
                        Restrict access on per-path and per method basis to only
                        allow access to specific portion of the API.
                      </Form.Label>
                    </Form.Group>
                  </Col>
                  {isActive ? <Ipathpermission /> : " "}
                </Row>
              </div>
            </Form>
          </div>
        </div>
      </div>
    </div>
  );
}

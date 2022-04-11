import React, { useState } from "react";
import { Form, Row, Col } from "react-bootstrap";
import Ipathpermission from "./path-file";
import GlobalLimit from "../global-limit/GlobalLimit";
export default function PathBased() {
  const [isActive, setisActive] = useState<boolean>(false);
  const [isActiveApi, setisActiveApi] = useState<boolean>(false);
  const [versions, setversion] = useState<string[]>([]);

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
    const value = event.target.value;
    const mapped = versions;
    const found = mapped.includes(value);
    if (!found) {
      setversion([...versions, value]);
    }
  };

  const deleteversion = (event: any, index: any) => {
    event.preventDefault();
    const rows = [...versions];
    rows.splice(index, 1);
    setversion(rows);
  };
  return (
    <div className="card mt-4">
      <div className="accordion " id="accordionExample">
        <div className="accordion-item">
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
                        onChange={(e: any) => handleversion(e)}
                      >
                        <option value="Default">Default</option>
                        <option value="V1">V1</option>
                        <option value="V2">V2</option>
                      </Form.Select>
                    </Form.Group>
                  </Col>
                  {isActiveApi ? <GlobalLimit /> : " "}
                </Row>
                <Row>
                  <Col md="12">
                    {versions.length > 0 ? (
                      <div
                        style={{ width: 960 }}
                        className="float-lg-left border rounded p-4"
                      >
                        {versions.map((data: any, index: any) => {
                          return (
                            <div key={index} className="border-0">
                              <i
                                className="bi bi-x-circle-fill float-left"
                                style={{ marginLeft: 40 }}
                                onClick={(e: any) => deleteversion(e, index)}
                              >
                                &nbsp;&nbsp;{data}
                              </i>
                            </div>
                          );
                        })}
                      </div>
                    ) : (
                      ""
                    )}
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
                          Restrict access on per-path and per method basis to
                          only allow access to specific portion of the API.
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
    </div>
  );
}

import React, { useState } from "react";
import { Button, Form, Row, Col } from "react-bootstrap";
import { addNewApi } from "../../../../store/features/api/create/slice";
import {
  regexForName,
  regexForListenPath,
  regexForTagetUrl,
} from "../../../../resources/api/api-constants";
import { useAppDispatch } from "../../../../store/hooks";
import { IErrorApiInput, IApiFormData } from "../../../../types/api/index";
import { ToastAlert } from "../../../../components/ToasterAlert/ToastAlert";
import { useNavigate } from "react-router-dom";
function CreateApi() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const [isToggled, setIsToggled] = useState(true);
  // const onToggled = () => setIsToggled(!isToggled);
  const [apisForm, setapisForm] = useState<IApiFormData>({
    name: "",
    listenPath: "",
    targetUrl: "",
    stripListenPath: true,
    isActive: true,
  });

  const [err, setErr] = useState<IErrorApiInput>({
    name: "",
    targetUrl: "",
    listenPath: "",
    status: true,
  });
  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type } = event.target;
    switch (name) {
      case "name":
        setErr({
          ...err,
          [name]: regexForName.test(value)
            ? ""
            : "Enter valid Api Name eg: abcd or Abcd1",
        });
        break;
      case "listenPath":
        setErr({
          ...err,
          [name]: regexForListenPath.test(value)
            ? ""
            : "Enter a Valid Listen Path eg: /abc/",
        });
        break;

      case "targetUrl":
        setErr({
          ...err,
          [name]: regexForTagetUrl.test(value) ? "" : "Enter a Valid url",
        });
        break;

      default:
        break;
    }
    if (type === "checkbox") {
      const isChecked = event.target.checked;
      if (event.target.name === "isActive") {
        setIsToggled(isChecked);
      }
      setapisForm({ ...apisForm, [event.target.name]: isChecked });
    } else
      setapisForm({ ...apisForm, [event.target.name]: event.target.value });

    // setapisForm({ ...apisForm, [name]: value });
  };

  const handleValidate = () => {
    const validate =
      err.name === "" && err.listenPath === "" && err.targetUrl === "";
    return validate;
  };
  const handleSubmitApi = async (event: React.FormEvent) => {
    event.preventDefault();

    if (handleValidate()) {
      if (
        apisForm.name !== "" &&
        apisForm.listenPath !== "" &&
        apisForm.targetUrl !== ""
      ) {
        // const listObj: IApiFormData = Object.create(apisForm);
        // listObj.stripListenPath = true;
        const newApi = {
          ...apisForm,
        };
        newApi.stripListenPath = true;
        const result = await dispatch(addNewApi(newApi));
        console.log("result", result);
        if (result.payload.Errors !== null) {
          ToastAlert(result.payload.Errors[0], "error");
          setapisForm({
            name: apisForm.name,
            listenPath: apisForm.listenPath,
            targetUrl: apisForm.targetUrl,
            isActive: apisForm.isActive,
          });
        } else {
          ToastAlert("Api created successfully", "success");
          navigate("/apilist");
          setapisForm({
            name: "",
            listenPath: "",
            targetUrl: "",
            isActive: true,
          });
        }
      } else {
        ToastAlert("Please Fill All Fields", "warning");
      }
    } else {
      ToastAlert("Please correct the error", "error");
      // setErr({
      //   name: "",
      //   listenPath: "",
      //   targetUrl: "",
      //   status: true,
      // });
    }
  };

  const NavigateToApisList = (
    val: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    val.preventDefault();
    navigate("/apilist");
  };
  return (
    <>
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div className="align-items-center">
            <Form onSubmit={handleSubmitApi} data-testid="form-input">
              <Button
                className="btn btn-success btn-md d-flex float-right mb-4 mr-5"
                type="submit"
                data-testid="submit-input"
              >
                Save
              </Button>
              <Button
                className="btn btn-light btn-md d-flex float-right mb-4"
                type="button"
                data-testid="cancel-input"
                onClick={(event: React.MouseEvent<HTMLButtonElement>) =>
                  NavigateToApisList(event)
                }
              >
                Cancel
              </Button>
              <div className="accordion m-5" id="accordionExample">
                <div>
                  <h2 className="accordion-header " id="headingOne">
                    <button
                      className="accordion-button"
                      type="button"
                      data-bs-toggle="collapse"
                      data-bs-target="#collapseOne"
                      aria-expanded="true"
                      aria-controls="collapseOne"
                    >
                      CREATE API
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
                          <Form.Group className="mt-6">
                            <Form.Label> API Name :</Form.Label>
                            <Form.Control
                              type="text"
                              id="name"
                              placeholder="Enter API Name"
                              name="name"
                              // data-testid="name-input"
                              value={apisForm.name}
                              isInvalid={!!err.name}
                              isValid={!err.name && !!apisForm.name}
                              onChange={handleInputChange}
                              required
                            />
                            <Form.Control.Feedback type="invalid">
                              {err.name}
                            </Form.Control.Feedback>
                          </Form.Group>
                        </Col>
                        <Col md="12">
                          <Form.Group className="mt-3">
                            <Form.Label>Listen Path :</Form.Label>
                            <Form.Control
                              type="text"
                              name="listenPath"
                              id="listenPath"
                              // data-testid="listenPath-input"
                              placeholder="Enter Listen Path"
                              isValid={!err.listenPath && !!apisForm.listenPath}
                              value={apisForm.listenPath}
                              isInvalid={!!err.listenPath}
                              onChange={handleInputChange}
                              required
                            />
                            <Form.Control.Feedback type="invalid">
                              {err.listenPath}
                            </Form.Control.Feedback>
                          </Form.Group>
                        </Col>
                        <Col md="12">
                          <Form.Group className="mt-2">
                            <Form.Label>Target Url :</Form.Label>
                            <Form.Control
                              type="text"
                              placeholder="Enter Target Url"
                              name="targetUrl"
                              id="targetUrl"
                              isValid={!err.targetUrl && !!apisForm.targetUrl}
                              value={apisForm.targetUrl}
                              isInvalid={!!err.targetUrl}
                              onChange={handleInputChange}
                              required
                            />
                            <Form.Control.Feedback type="invalid">
                              {err.targetUrl}
                            </Form.Control.Feedback>
                          </Form.Group>
                        </Col>

                        <Col md="12">
                          <Form.Group className="mb-3 mt-3">
                            <Form.Label>API Status :</Form.Label>
                            <Form.Check
                              type="switch"
                              // onChangeCapture={handleInputChange}
                              onChange={handleInputChange}
                              checked={isToggled}
                              name="isActive"
                              id="isActive"
                              // onChange={onToggled}
                              label={isToggled ? "  Active" : "  InActive"}
                            />
                          </Form.Group>
                        </Col>
                      </Row>
                    </div>
                  </div>
                </div>
              </div>
            </Form>
          </div>
        </div>
      </div>
    </>
  );
}

export default CreateApi;

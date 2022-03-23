import React, { useState } from "react";
import { Button, Form, Row, Col, Container } from "react-bootstrap";
import { addNewApi } from "../../../../store/features/api/create/slice";
import "./toggle-switch.css";
import {
  regexForName,
  regexForListenPath,
  regexForTagetUrl,
} from "../../../../resources/APIS/ApiConstants";
import { useAppDispatch } from "../../../../store/hooks";
import { IErrorApiInput, IApiFormData } from "../../../../types/api/index";
import { ToastAlert } from "../../../../components/ToasterAlert/ToastAlert";
import { useNavigate } from "react-router-dom";
function CreateApi() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const [isToggled, setIsToggled] = useState(true);
  const onToggled = () => setIsToggled(!isToggled);
  console.log(isToggled);
  const [apisForm, setapisForm] = useState<IApiFormData>({
    name: "",
    listenPath: "",
    targetUrl: "",
    isActive: true,
  });

  const [err, setErr] = useState<IErrorApiInput>({
    name: "",
    targetUrl: "",
    listenPath: "",
    status: true,
  });
  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    console.log("event", event.target.value);
    const { name, value } = event.target;
    switch (name) {
      case "listenPath":
        setErr({
          ...err,
          [name]: regexForListenPath.test(value)
            ? ""
            : "ListenPath should be in correct format eg: /abc/",
        });
        break;

      case "name":
        setErr({
          ...err,
          [name]: regexForName.test(value)
            ? ""
            : "Name should only consist Alphabets",
        });
        break;
      case "targetUrl":
        setErr({
          ...err,
          [name]: regexForTagetUrl.test(value)
            ? ""
            : "Please enter correct url",
        });
        break;

      default:
        break;
    }

    setapisForm({ ...apisForm, [name]: value });
    console.log(apisForm);
  };

  const handleValidate = () => {
    const validate = !!(
      err.name === "" &&
      err.listenPath === "" &&
      err.status === true &&
      err.targetUrl === ""
    );
    return validate;
  };
  const handleSubmitApi = (event: React.FormEvent) => {
    event.preventDefault();

    if (handleValidate()) {
      console.log(apisForm);
      if (
        apisForm.name !== "" &&
        apisForm.listenPath !== "" &&
        apisForm.targetUrl !== ""
      ) {
        const newApi = {
          ...apisForm,

          // lastlogin: "Mar 01 2022 11:51:39",
        };
        //  newApi.isActive = isToggled;
        dispatch(addNewApi(newApi));
        ToastAlert("Api created successfully", "success");

        setapisForm({
          name: "",
          listenPath: "",
          targetUrl: "",
          isActive: true,
        });
      }
      // (
      //   apisForm.name === "" ||
      //   apisForm.listenPath === "" ||
      //   apisForm.targetUrl === ""
      // )
      else {
        ToastAlert("Please Fill All Fields", "warning");
      }
    } else {
      setErr({
        name: "",
        listenPath: "",
        targetUrl: "",
        status: true,
      });
    }
  };
  // const clearData = (
  //   event: React.MouseEvent<HTMLButtonElement, MouseEvent>
  // ) => {
  //   event.preventDefault();
  //   setapisForm({
  //     name: "",
  //     listenPath: "",
  //     targetUrl: "",
  //     isActive: true,
  //   });
  // };

  const NavigateToApisList = (
    val: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    val.preventDefault();
    // console.log(val);
    navigate("/apilist");
  };
  return (
    <>
      {/* <div className=" bg-white"> */}
      <div className="col-lg-12 grid-margin stretch-card">
        <div className="card">
          <div className="card-body">
            <div className="align-items-center">
              <Container className="m-1 pt-1">
                <Form
                  onSubmit={handleSubmitApi}
                  data-testid="form-input"
                  className="p-3"
                >
                  <Button
                    className="btn btn-success btn-md d-flex float-right mb-4"
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
                      // clearData(event)
                      NavigateToApisList(event)
                    }
                  >
                    Cancel
                  </Button>
                  <div className="card col-lg-12 grid-margin stretch-card">
                    <div className="card card-header text-left pl-2 pb-2 pt-3 text-dark">
                      {/* bg-info */}
                      CREATE API
                    </div>
                    <div className="card-body">
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
                              // {console.log(inValid)}
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
                            <br />
                            <Form.Label className="toggle-switch">
                              {/* API Status : */} <br />
                              <Form.Control
                                className="switch"
                                type="checkbox"
                                checked={isToggled}
                                // value={apisForm.isActive ? 0 : 1}
                                name="apiStatus"
                                id="apiStatus"
                                onChange={onToggled}
                              />
                              <span className="slider">
                                {isToggled
                                  ? (apisForm.isActive = true)
                                  : (apisForm.isActive = false)}
                                {/* {console.log(isToggled)}; */}
                                {/* {isToggled ? "Active" : "InActive"} */}
                              </span>
                            </Form.Label>
                            <span>{isToggled ? "  Active" : "  InActive"}</span>
                          </Form.Group>
                        </Col>
                      </Row>
                    </div>
                  </div>
                </Form>
              </Container>
              {/* </div> */}
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default CreateApi;

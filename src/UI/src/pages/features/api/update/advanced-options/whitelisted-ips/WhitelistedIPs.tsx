import React, { useState, useEffect } from "react";
import { Form, Col, Row, Button } from "react-bootstrap";
import { useAppDispatch, useAppSelector } from "../../../../../../store/hooks";
import { setForm } from "../../../../../../store/features/api/update/slice";

export default function WhitelistedIPs() {
  const dispatch = useAppDispatch();
  const state = useAppSelector((RootState) => RootState.updateApiState);
  console.log("state: ", state.data.form);

  const [enableWhitelist, setWhitelist] = useState(false);

  const whitelistLength = state.data.form.Whitelist.length;

  useEffect(() => {
    if (whitelistLength > 0) {
      setWhitelist(true);
    }
  }, []);

  const [addFormData, setAddFormData] = useState<any>({
    Whitelist: "",
  });

  function setEnableWhitelist(event: React.ChangeEvent<HTMLInputElement>) {
    setWhitelist(event.target.checked);
    if (event.target.checked === false) {
      const whitelistObj: any = [];
      dispatch(setForm({ ...state.data.form, Whitelist: whitelistObj }));
    }
  }

  const handleFormInputChange = (event: any) => {
    const { name, value } = event.target;
    const formobj = { ...addFormData };
    formobj[name] = value;
    setAddFormData(formobj);
  };

  const handleAddClick = () => {
    const whitelistObj: any = [
      ...state.data.form.Whitelist,
      addFormData.Whitelist,
    ];
    dispatch(setForm({ ...state.data.form, Whitelist: whitelistObj }));
    setAddFormData({ ...addFormData, Whitelist: "" });
  };

  const deleteTableRows = (
    index: number,
    event: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    event.preventDefault();
    const list = [...state.data.form.Whitelist];
    list.splice(index, 1);
    dispatch(setForm({ ...state.data.form, Whitelist: list }));
  };

  return (
    <div>
      <div className="card">
        <div>
          <div className="align-items-center justify-content-around">
            <div className="accordion" id="accordionWhitelistedIPs">
              <div className="accordion-item">
                <h2 className="accordion-header" id="headingFive">
                  <button
                    className="accordion-button"
                    type="button"
                    data-bs-toggle="collapse"
                    data-bs-target="#collapseWhitelisted"
                    aria-expanded="true"
                    aria-controls="collapseWhitelisted"
                  >
                    Whitelisted IPs
                  </button>
                </h2>
                <div
                  id="collapseWhitelisted"
                  className="accordion-collapse collapse show"
                  aria-labelledby="headingFive"
                  data-bs-parent="#accordionWhitelistedIPs"
                >
                  <div className="accordion-body">
                    <div>
                      <Row>
                        <Col md="12">
                          <b>Enable Whitelisted IPs</b>
                          <p>
                            Whitelisted IPs limit the originating address of a
                            request to only come from a select group of
                            addresses.
                          </p>
                        </Col>
                        <Col md="12">
                          <Form.Group className="mb-3">
                            <Form.Check
                              type="checkbox"
                              label="Enable Whitelisted IPs"
                              name="whitelisted"
                              checked={enableWhitelist}
                              onChange={(e: any) => setEnableWhitelist(e)}
                            />
                          </Form.Group>
                        </Col>
                      </Row>
                      {enableWhitelist ? (
                        <div>
                          <b>Whitelisted IPs</b>
                          <p>No IPs selected, please add one below.</p>
                          <Row>
                            <Form.Label>
                              <b>IP Address:</b>
                            </Form.Label>
                            <Col md={10}>
                              <Form.Group className="mt-0">
                                <Form.Control
                                  type="text"
                                  placeholder="127.0.0.1"
                                  id="whitelist"
                                  name="Whitelist"
                                  value={addFormData.Whitelist}
                                  onChange={(event) =>
                                    handleFormInputChange(event)
                                  }
                                />
                              </Form.Group>
                            </Col>
                            <Col md={2}>
                              <Form.Group className="mb-5">
                                <Form.Label></Form.Label>
                                <Button
                                  variant="dark"
                                  disabled={!addFormData.Whitelist}
                                  // onClick={handleAddClick}
                                  onClick={() => handleAddClick()}
                                >
                                  Add
                                </Button>{" "}
                              </Form.Group>
                            </Col>
                          </Row>

                          <div className="container">
                            <div className="row">
                              <div className="col-sm-11">
                                <table className="table table-striped">
                                  <tbody>
                                    {state.data.form.Whitelist.map(
                                      (data: any, index: any) => {
                                        return (
                                          <tr key={index}>
                                            <td>
                                              <label>{data}</label>
                                            </td>

                                            <td
                                              style={{
                                                textAlign: "right",
                                              }}
                                            >
                                              <button
                                                className="btn btn-default bi bi-trash-fill"
                                                onClick={(event) =>
                                                  deleteTableRows(index, event)
                                                }
                                              ></button>
                                            </td>
                                          </tr>
                                        );
                                      }
                                    )}
                                  </tbody>
                                </table>
                              </div>
                              <div className="col-sm-4"></div>
                            </div>
                          </div>
                        </div>
                      ) : (
                        <></>
                      )}
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

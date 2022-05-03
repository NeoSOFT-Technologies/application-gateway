import { Form, Col, Row, Button, Table } from "react-bootstrap";
import React, { useEffect, useState } from "react";
import {
  useAppDispatch,
  useAppSelector,
} from "../../../../../../../store/hooks";
import { setForm } from "../../../../../../../store/features/api/update/slice";
import { IPolicyListState } from "../../../../../../../store/features/policy/list";
import { getPolicyList } from "../../../../../../../store/features/policy/list/slice";
import Spinner from "../../../../../../../components/loader/Loader";
// import {
//   regexForName,
//   setFormErrors,
// } from "../../../../../../../resources/api/api-constants";

export default function OpenIdConnectAuthentication() {
  const state = useAppSelector((RootState) => RootState.updateApiState);
  console.log("state", state);

  const policyList: IPolicyListState = useAppSelector(
    (RootState) => RootState.policyListState
  );
  console.log("policyList", policyList);
  const dispatch = useAppDispatch();

  const [loading, setLoading] = useState(true);
  const mainCall = async (currentPage: number) => {
    dispatch(getPolicyList({ currentPage }));
    setLoading(false);
  };
  useEffect(() => {
    mainCall(1);
  }, []);

  const [addFormData, setAddFormData] = useState({
    issuer: "",
    client_ids: [],
  });

  const [addClientFormData, setClientAddFormData] = useState<any>([]);
  // console.log("addClientFormData", addClientFormData);

  const handleIssuerInputChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const { name, value } = event.target;
    // switch (name) {
    //   case "issuer":
    //     setFormErrors(
    //       {
    //         ...state.data.errors,
    //         [name]: regexForName.test(value)
    //           ? ""
    //           : "Enter a valid issuer Name ",
    //       },
    //       dispatch
    //     );
    //     break;
    //   default:
    //     break;
    // }
    const newFormData: any = { ...addFormData };
    newFormData[name] = value;

    setAddFormData(newFormData);
  };

  const handleClientInputChange = (event: any, index: number) => {
    const { name, value } = event.target;
    const newFormData: any = [...addClientFormData];
    newFormData[index] = { ...newFormData[index], [name]: value };
    setClientAddFormData(newFormData);
  };

  const handleIssuerAddClick = () => {
    const clientObj: any = {
      clientId: "",
      policy: "",
    };
    setClientAddFormData([...addClientFormData, clientObj]);

    const providerList = [...state.data.form.OpenidOptions.Providers];
    const list = {
      Issuer: addFormData.issuer,
      Client_ids: [],
    };
    providerList.push(list);

    const OpenidOptionsData = {
      Providers: providerList,
    };
    dispatch(setForm({ ...state.data.form, OpenidOptions: OpenidOptionsData }));
    setAddFormData({ issuer: "", client_ids: [] });
  };

  const handleClientAddClick = (index: any) => {
    const providerList = [...state.data.form.OpenidOptions.Providers];

    const clientList = [
      ...state.data.form.OpenidOptions.Providers[index].Client_ids,
    ];

    const list = {
      ClientId: addClientFormData[index].clientId,
      Policy: addClientFormData[index].policy,
    };
    clientList.push(list);

    providerList[index] = {
      ...providerList[index],
      Client_ids: [...clientList],
    };

    const OpenidOptionsData = {
      Providers: providerList,
    };
    dispatch(setForm({ ...state.data.form, OpenidOptions: OpenidOptionsData }));

    const clientObj = {
      clientId: "",
      policy: "",
    };
    const newFormData: any = [...addClientFormData];
    newFormData[index] = clientObj;
    setClientAddFormData(newFormData);
  };

  const deleteIssuerTableRows = (
    index: number,
    event: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    event.preventDefault();
    const providerList = [...state.data.form.OpenidOptions.Providers];
    providerList.splice(index, 1);

    const OpenidOptionsData = {
      Providers: providerList,
    };
    dispatch(setForm({ ...state.data.form, OpenidOptions: OpenidOptionsData }));
  };

  const deleteClientTableRows = (
    issuerIndex: number,
    clientIndex: number,
    event: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    event.preventDefault();
    const providerList = [...state.data.form.OpenidOptions.Providers];
    const clientList = [
      ...state.data.form.OpenidOptions.Providers[issuerIndex].Client_ids,
    ];
    clientList.splice(clientIndex, 1);
    providerList[issuerIndex] = {
      ...providerList[issuerIndex],
      Client_ids: [...clientList],
    };
    const OpenidOptionsData1 = {
      Providers: providerList,
    };
    dispatch(
      setForm({ ...state.data.form, OpenidOptions: OpenidOptionsData1 })
    );
  };

  // console.log("form", state.data.form);

  return (
    <div>
      {policyList.loading ? (
        <Spinner />
      ) : (
        <div className="ml-3 mr-3">
          <Row>
            <Col
              md={12}
              className="border rounded mb-3 bg-warning bg-opacity-10 p-2"
            >
              Changing the Authentication mode on an Active API can have severe
              consequences for your users. Please be aware that this will stop
              the current keys working for this API.
            </Col>

            <Col md={8} className="border rounded">
              <div>
                <h5 className="mt-2">OpenID Connect</h5>
                {/* <Form.Group className="mb-3">
              <Form.Check
                type="checkbox"
                name="stripAuthenticationData"
                label="Enable rate limiting on a per-user / per-client basis"
              />
            </Form.Group> */}
              </div>

              {/* <div>
            Enabling this option will cause a user to be rate limited
            differently depending on which client they are using.{" "}
          </div> */}
              <br />
              <b>Add Issuers, clients and policies</b>
              <br />
              <div>
                OpenID connect only allows valid issuers and registered client
                IDs to be validated. Add your issuer below, and for each Client
                that is managed by an issuer, add a matching policy that will be
                applied to tokens registered to that client.
              </div>
              <br />
              <Row>
                <Form.Label>
                  <b> Add Issuers, clients and policies:</b>
                </Form.Label>
                <Col md={10}>
                  <Form.Group className="mt-0">
                    <Form.Control
                      type="text"
                      placeholder="accounts.google.com"
                      id="issuer"
                      name="issuer"
                      value={addFormData.issuer}
                      onChange={handleIssuerInputChange}
                    />
                  </Form.Group>
                </Col>
                <Col md={2}>
                  <Form.Group className="mb-5">
                    <Form.Label></Form.Label>
                    <Button
                      variant="dark"
                      disabled={!addFormData.issuer}
                      onClick={handleIssuerAddClick}
                    >
                      Add
                    </Button>{" "}
                  </Form.Group>
                </Col>
              </Row>

              <Row>
                <div className="container">
                  <div className="row">
                    <div className="mb-1"></div>
                    <div className="col-sm-11">
                      {loading ? (
                        <></>
                      ) : (
                        (state.data.form.OpenidOptions.Providers as any[]).map(
                          (data: any, index) => {
                            // console.log("data", data);
                            const { Issuer, Client_ids } = data;
                            // console.log(
                            //   "Issuer: ",
                            //   Issuer + " ClientIds: ",
                            //   Client_ids
                            // );
                            if (Client_ids?.length > 0) {
                              return state.data.form.OpenidOptions.Providers[
                                index
                              ].Client_ids.map(
                                (clientData: any, clientIndex: any) => {
                                  // console.log(clientData, clientIndex);
                                  const { ClientId, Policy } = clientData;
                                  // console.log("Policy from client : ", Policy);
                                  // const policyObj =
                                  //   policyList?.data?.Policies.filter(
                                  //     (p: any) => p.Id === Policy
                                  //   );
                                  // console.log("policyObj", policyObj);

                                  if (Policy !== "") {
                                    return policyList?.data?.Policies.filter(
                                      (p: any) => p.Id === Policy
                                    ).map((filteredPolicy) => {
                                      const { Name, Id } = filteredPolicy;
                                      console.log(
                                        "policy name and id : ",
                                        Name,
                                        Id
                                      );
                                      return (
                                        <div key={index}>
                                          {clientIndex === 0 ? (
                                            <table className="table">
                                              <thead>
                                                <tr>
                                                  <th></th>
                                                  <th>Issuer:</th>
                                                  <th>ClientID:</th>
                                                  <th>Policy:</th>
                                                  <th></th>
                                                </tr>
                                              </thead>
                                              <tbody>
                                                <tr key={index}>
                                                  <td>
                                                    <button
                                                      className="btn bi bi-trash-fill"
                                                      onClick={(event) =>
                                                        deleteIssuerTableRows(
                                                          index,
                                                          event
                                                        )
                                                      }
                                                    ></button>
                                                  </td>
                                                  <td>
                                                    <input
                                                      type="text"
                                                      className="form-control"
                                                      id="issuer"
                                                      name="issuer"
                                                      value={Issuer}
                                                      readOnly
                                                    />
                                                  </td>
                                                  <td>
                                                    <input
                                                      type="text"
                                                      className="form-control"
                                                      placeholder="Your-client-id"
                                                      id="clientId"
                                                      name="clientId"
                                                      value={
                                                        addClientFormData[index]
                                                          ?.clientId
                                                      }
                                                      onChange={(evnt) =>
                                                        handleClientInputChange(
                                                          evnt,
                                                          index
                                                        )
                                                      }
                                                    />{" "}
                                                  </td>
                                                  <td>
                                                    <select
                                                      className="p-2 rounded mb-0"
                                                      name="policy"
                                                      id="policy"
                                                      placeholder="select policy"
                                                      value={
                                                        addClientFormData.policy
                                                      }
                                                      onChange={(evnt) =>
                                                        handleClientInputChange(
                                                          evnt,
                                                          index
                                                        )
                                                      }
                                                    >
                                                      <option></option>
                                                      {policyList.data?.Policies.map(
                                                        (item: any) => {
                                                          return (
                                                            <option
                                                              key={item.Name}
                                                              value={item.Id}
                                                              id={item.Name}
                                                            >
                                                              {item.Name}
                                                            </option>
                                                          );
                                                        }
                                                      )}
                                                    </select>
                                                  </td>
                                                  <td>
                                                    <button
                                                      className="btn btn-outline-dark btn-dark"
                                                      onClick={() =>
                                                        handleClientAddClick(
                                                          index
                                                        )
                                                      }
                                                      disabled={
                                                        !(
                                                          addClientFormData[
                                                            index
                                                          ]?.clientId &&
                                                          addClientFormData[
                                                            index
                                                          ]?.policy
                                                        )
                                                      }
                                                    >
                                                      Add
                                                    </button>
                                                  </td>
                                                </tr>
                                              </tbody>
                                            </table>
                                          ) : (
                                            <></>
                                          )}

                                          <Table striped bordered hover>
                                            <tbody>
                                              <tr key={clientIndex}>
                                                <td>{ClientId}</td>
                                                <td
                                                  style={{
                                                    textAlign: "center",
                                                  }}
                                                >
                                                  {Name} : {Id}
                                                  {/* {policyObj![0].Name} : {policyObj![0].Id} */}
                                                </td>
                                                <td
                                                  style={{
                                                    textAlign: "center",
                                                  }}
                                                >
                                                  <button
                                                    className="btn bi bi-trash-fill"
                                                    onClick={(event) =>
                                                      deleteClientTableRows(
                                                        index,
                                                        clientIndex,
                                                        event
                                                      )
                                                    }
                                                  ></button>
                                                </td>
                                              </tr>
                                            </tbody>
                                          </Table>
                                        </div>
                                      );
                                    });
                                  } else {
                                    return <></>;
                                  }
                                }
                              );
                            } else {
                              return (
                                <div>
                                  <table className="table">
                                    <thead>
                                      <tr>
                                        <th></th>
                                        <th>Issuer:</th>
                                        <th>ClientID:</th>
                                        <th>Policy:</th>
                                        <th></th>
                                      </tr>
                                    </thead>
                                    <tbody>
                                      <tr key={index}>
                                        <td>
                                          <button
                                            className="btn bi bi-trash-fill"
                                            onClick={(event) =>
                                              deleteIssuerTableRows(
                                                index,
                                                event
                                              )
                                            }
                                          ></button>
                                        </td>
                                        <td>
                                          <input
                                            type="text"
                                            className="form-control"
                                            id="issuer"
                                            name="issuer"
                                            value={Issuer}
                                            readOnly
                                          />
                                        </td>
                                        <td>
                                          <input
                                            type="text"
                                            className="form-control"
                                            placeholder="Your-client-id"
                                            id="clientId"
                                            name="clientId"
                                            value={
                                              addClientFormData[index]?.clientId
                                            }
                                            onChange={(evnt) =>
                                              handleClientInputChange(
                                                evnt,
                                                index
                                              )
                                            }
                                          />{" "}
                                        </td>
                                        <td>
                                          <select
                                            className="p-2 rounded mb-0"
                                            name="policy"
                                            id="policy"
                                            placeholder="select policy"
                                            value={addClientFormData.policy}
                                            onChange={(evnt) =>
                                              handleClientInputChange(
                                                evnt,
                                                index
                                              )
                                            }
                                          >
                                            <option></option>
                                            {policyList.data?.Policies.map(
                                              (item: any) => {
                                                // console.log("item", item);
                                                return (
                                                  <option
                                                    key={item.Name}
                                                    value={item.Id}
                                                    id={item.Name}
                                                  >
                                                    {item.Name}
                                                  </option>
                                                );
                                              }
                                            )}
                                          </select>
                                        </td>
                                        <td>
                                          <button
                                            className="btn btn-outline-dark btn-dark"
                                            onClick={() =>
                                              handleClientAddClick(index)
                                            }
                                            disabled={
                                              !(
                                                addClientFormData[index]
                                                  ?.clientId &&
                                                addClientFormData[index]?.policy
                                              )
                                            }
                                          >
                                            Add
                                          </button>
                                        </td>
                                      </tr>
                                    </tbody>
                                  </table>
                                </div>
                              );
                            }
                          }
                        )
                      )}
                    </div>
                    <div className="col-sm-4"></div>
                  </div>
                </div>
              </Row>
            </Col>

            <Col md={3} className="ml-3">
              <div className="border rounded p-3 bg-primary bg-opacity-10">
                API Gateway can transparently handle OpenID connect JWT ID
                Token, in order to make these works, register the issue(iss) ,
                client(aud) to a policy ID for the API in order for dynamic
                per-token access limits to be applied.
              </div>
            </Col>
          </Row>
        </div>
      )}
    </div>
  );
}

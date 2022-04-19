import React, { useState } from "react";
import { Button, Form, Row, Col, Table } from "react-bootstrap";
import { IPolicyCreateState } from "../../../../store/features/policy/create";
import { setForm } from "../../../../store/features/policy/create/slice";
import { useAppSelector, useAppDispatch } from "../../../../store/hooks";

export default function Ipathpermission() {
  const dispatch = useAppDispatch();
  const state: IPolicyCreateState = useAppSelector(
    (RootState) => RootState.createPolicyState
  );

  const [rowsData, setRowsData] = useState<any>([]);
  const [inputData, setInputData] = useState<any>({
    path: "",
    method: "",
  });
  const handleAddclick = () => {
    console.log("apIs", state.data.form);
    const list = [
      ...state.data.form.ApIs[0].AllowedUrls!,
      {
        id: null,
        name: "",
        versions: [],
        allowedUrls: [
          {
            url: "welcome",
            methods: [],
          },
        ],
        limit: {
          rate: 0,
          per: 0,
          throttle_interval: 0,
          throttle_retry_limit: 0,
          max_query_depth: 0,
          quota_max: 0,
          quota_renews: 0,
          quota_remaining: 0,
          quota_renewal_rate: 0,
          set_by_policy: false,
        },
      },
    ];
    dispatch(setForm({ ...state.data.form.ApIs[0], allowedUrls: list }));
    setInputData({ path: "", method: "" });
  };
  console.log("checkhandle", state.data.form);
  const deleteTableRows = (event: any, index: any) => {
    event.preventDefault();
    const rows = [...rowsData];
    rows.splice(index, 1);
    setRowsData(rows);
  };

  const handleAddFormChange = (event: any) => {
    event.preventDefault();
    const fieldName = event.target.getAttribute("name");
    const fieldValue = event.target.value;
    console.log("fieldName: ", fieldName + " | fieldValue:", fieldValue);
    const newFormData: any = { ...inputData };
    newFormData[fieldName] = fieldValue;
    setInputData(newFormData);
  };

  return (
    <div>
      <Row>
        <Col md={5}>
          <Form.Group className="mb-3">
            <Form.Label>
              <b>Paths :</b>
            </Form.Label>
            <Form.Control
              type="text"
              id="path"
              placeholder="Enter custom regex"
              name="path"
              onChange={handleAddFormChange}
              // data-testid="name-input"
              required
            />
            <Form.Control.Feedback type="invalid"></Form.Control.Feedback>
          </Form.Group>
        </Col>
        <Col md={5}>
          <Form.Group className="mb-3">
            <Form.Label>
              <b> Allowed Methods :</b>
            </Form.Label>
            <Form.Select
              style={{ height: 45 }}
              name="method"
              onChange={handleAddFormChange}
            >
              <option disabled>Select Method(s)</option>
              <option>GET</option>
              <option>POST</option>
              <option>PUT</option>
              <option>DELETE</option>
              <option>PATCH</option>
              <option>OPTIONS</option>
              <option>HEAD</option>
            </Form.Select>
          </Form.Group>
        </Col>

        <Col md={2} className="pt-2">
          <Form.Label></Form.Label>
          <Form.Group className="mb-3">
            <Button variant="dark" onClick={handleAddclick}>
              Add
            </Button>{" "}
          </Form.Group>
        </Col>
      </Row>
      {
        <Row>
          <Col md={12}>
            <Table striped bordered hover size="lg">
              <thead>
                <tr>
                  <th>Paths</th>
                  <th>Methods</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {state.data.form.ApIs?.length > 0 ? (
                  (state.data.form.ApIs as any[]).map(
                    (data: any, index: any) => {
                      return (
                        state.data.form.ApIs[index].AllowedUrls as any[]
                      ).map((data1: any, index1: any) => {
                        return (
                          <tr key={index}>
                            <td>{data1.url}</td>
                            <td>{data1.methods}</td>
                            <td style={{ textAlign: "center" }}>
                              <i
                                className="bi bi-trash"
                                onClick={(e: any) => deleteTableRows(e, index)}
                              ></i>
                            </td>
                          </tr>
                        );
                      });
                    }
                  )
                ) : (
                  <></>
                )}
              </tbody>
            </Table>
          </Col>
        </Row>
      }
    </div>
  );
}

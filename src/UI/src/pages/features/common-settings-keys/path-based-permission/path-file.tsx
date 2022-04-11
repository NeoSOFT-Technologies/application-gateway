import React, { useState } from "react";
import { Button, Form, Row, Col, Table } from "react-bootstrap";
export default function Ipathpermission() {
  const [rowsData, setRowsData] = useState<any>([]);
  const [inputData, setInputData] = useState<any>({
    path: "",
    method: "",
  });

  const handleAddclick = () => {
    setRowsData([...rowsData, inputData]);
  };

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
  console.log(rowsData);
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
              <option>Select Method(s)</option>
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
                {rowsData.map(
                  (
                    data: {
                      path:
                        | boolean
                        | React.ReactChild
                        | React.ReactFragment
                        | React.ReactPortal
                        | null
                        | undefined;
                      method:
                        | boolean
                        | React.ReactChild
                        | React.ReactFragment
                        | React.ReactPortal
                        | null
                        | undefined;
                      checkbox: boolean | undefined;
                    },
                    index: React.Key | null | undefined
                  ) => {
                    return (
                      <tr key={index}>
                        <td>{data.path}</td>
                        <td>{data.method}</td>
                        <td style={{ textAlign: "center" }}>
                          <i
                            className="bi bi-trash"
                            onClick={(e: any) => deleteTableRows(e, index)}
                          ></i>
                        </td>
                      </tr>
                    );
                  }
                )}
              </tbody>
            </Table>
          </Col>
        </Row>
      }
    </div>
  );
}

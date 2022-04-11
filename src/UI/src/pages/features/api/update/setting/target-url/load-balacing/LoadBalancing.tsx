import React, { useState } from "react";
import { Button, Col, Form, Row } from "react-bootstrap";
// import { setFormData } from "../../../../../../../resources/api/api-constants";
import {
  useAppDispatch,
  useAppSelector,
} from "../../../../../../../store/hooks";
import { setForm } from "../../../../../../../store/features/api/update/slice";

export default function LoadBalancing() {
  const dispatch = useAppDispatch();
  const state = useAppSelector((RootState) => RootState.updateApiState);
  // const [rowsData, setRowsData] = useState<any>([]);
  const [rowInput, setRowInput] = useState<any>({
    LoadBalancingTargets: "",
    weighting: "",
    traffic: "",
  });
  const handleInputChange = (event: any) => {
    const { name, value } = event.target;
    const formobj = { ...rowInput };
    formobj[name] = value;
    setRowInput(formobj);
  };
  const handleAddChange = () => {
    console.log("load", rowInput.LoadBalancingTargets);
    // setRowsData([...rowsData, rowInput]);
    const newObj: any = [
      ...state.data.form.LoadBalancingTargets,
      rowInput.LoadBalancingTargets,
    ];
    console.log("newObj", newObj);
    dispatch(setForm({ ...state.data.form, LoadBalancingTargets: newObj }));
    setRowInput({ ...rowInput, LoadBalancingTargets: "" });
  };
  const deleteTableRows = (index: number) => {
    const data = [...state.data.form.LoadBalancingTargets];
    data.splice(index, 1);
    dispatch(setForm({ ...state.data.form, LoadBalancingTargets: data }));
  };
  const handleChange = (evnt: any, index: number) => {
    const { name, value } = evnt.target;
    const list = [...rowInput];
    list[index][name] = value;
    setRowInput(list);
  };
  console.log("dara", rowInput);
  return (
    <div>
      <Row>
        <i className="mb-3">
          Tyk can perform round-robin load balancing on a series of upstream
          targets, you will need to add all of the targets using the fields
          below.
        </i>
        <br />
        <br />
        <Form.Label> Add Target URL&apos;s :</Form.Label>
        <Col md="10">
          <Form.Group className="mb-3">
            <Form.Control
              className="mt-2"
              type="text"
              id="LoadBalancingTargets"
              value={rowInput.LoadBalancingTargets}
              placeholder="Please enter target(s) and hit enter key"
              name="LoadBalancingTargets"
              onChange={handleInputChange}
            />
            {/* <Form.Control.Feedback type="invalid">
              {state.data.errors?.TargetUrl}
            </Form.Control.Feedback> */}
          </Form.Group>
        </Col>
        <Col md="2">
          <Form.Group className="mt-2 mb-3">
            <Button onClick={handleAddChange} variant="dark">
              Add
            </Button>{" "}
          </Form.Group>
        </Col>
        <i>
          If you add a trailing &apos;/ &apos; to your listen path, you can only
          make requests that include the trailing &apos;/ &apos;
        </i>
      </Row>
      <div className="container">
        <div className="row">
          <div className="col-sm-8">
            <table className="table">
              <thead>
                <tr>
                  <th>Upstream Target</th>
                  <th>Weighting</th>
                  <th>Traffic</th>
                </tr>
              </thead>
              <tbody>
                {state.data.form.LoadBalancingTargets.map(
                  (data: any, index: any) => {
                    return (
                      <tr key={index}>
                        <td>
                          <input
                            value={data}
                            name="LoadBalancingTargets"
                            onChange={(evnt) => handleChange(evnt, index)}
                            className="form-control"
                          />
                        </td>
                        <td>
                          <input
                            type="number"
                            min="1"
                            max="100"
                            value={data.weighting}
                            onChange={(evnt) => handleChange(evnt, index)}
                            name="weighting"
                            className="form-control"
                          />{" "}
                        </td>
                        <td>
                          <input
                            type="text"
                            value={data.traffic}
                            onChange={(evnt) => handleChange(evnt, index)}
                            name="traffic"
                            className="form-control"
                          />{" "}
                        </td>
                        <td>
                          <button
                            className="btn btn-outline-danger"
                            onClick={() => deleteTableRows(index)}
                          >
                            x
                          </button>
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
      )
    </div>
  );
}

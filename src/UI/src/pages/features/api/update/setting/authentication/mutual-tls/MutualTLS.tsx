import React from "react";
import { Row } from "react-bootstrap";
import { useAppSelector } from "../../../../../../../store/hooks";

export default function MutualTLS() {
  const state = useAppSelector((RootState) => RootState.updateApiState);
  // const handleAddCertificate = () => {};
  return (
    <div>
      <div className="border rounded p-2">
        Changing the Authentication mode on an active API can have severe
        consequences for your users. Please be aware that this will stop the
        current keys working for this API.
      </div>
      <br />
      <p>
        Only clients with whitelisted SSL certificates will be allowed to access
        your API.
      </p>
      <button className=" btn btn-sm btn-dark btn-sm float-right mb-2">
        <span className="bi bi-plus-lg"></span>&nbsp;Add new Certificate
      </button>
      <br />
      <br />
      <Row className="ml-1 mr-1">
        <table className="table table-bordered ">
          <thead className="thead-dark">
            <tr>
              <th>0 selected Certificates </th>
              <th>Select from exisiting certificates</th>
            </tr>
          </thead>
          <tbody>
            {state.data.form.CertIds.map((data: any, index: any) => {
              return (
                <tr key={index}>
                  <td>
                    <label>
                      {data}{" "}
                      <button type="button" className="btn ml-5">
                        -
                      </button>
                    </label>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </Row>
    </div>
  );
}

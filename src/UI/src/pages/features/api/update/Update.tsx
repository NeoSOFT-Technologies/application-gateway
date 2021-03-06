import React, { FormEvent, useEffect } from "react";
import Setting from "./setting/Setting";
import Version from "./version/Version";
import { Tab, Tabs, Form } from "react-bootstrap";
import Spinner from "../../../../components/loader/Loader";
import { useAppDispatch, useAppSelector } from "../../../../store/hooks";
import {
  updateApi,
  getApiById,
  setForm,
} from "../../../../store/features/api/update/slice";
import { ToastAlert } from "../../../../components/ToasterAlert/ToastAlert";
import { IApiGetByIdState } from "../../../../store/features/api/update";
import { useNavigate, useParams } from "react-router-dom";

export default function Update() {
  const state: IApiGetByIdState = useAppSelector(
    (RootState) => RootState.updateApiState
  );
  const failure: any = () => ToastAlert(state.error!, "error");
  const dispatch = useAppDispatch();
  const { id } = useParams();

  useEffect(() => {
    dispatch(getApiById(id));
  }, []);

  const navigate = useNavigate();

  async function handleSubmitApiUpdate(event: FormEvent) {
    event.preventDefault();
    let validate: any;
    if (state.data.errors !== undefined) {
      validate = Object.values(state.data.errors).every(
        (x) => x === null || x === ""
      );
    }
    if (validate) {
      const result = await dispatch(updateApi(state.data.form));
      if (result.meta.requestStatus === "rejected") {
        ToastAlert(result.payload.message, "error");
      } else if (result.meta.requestStatus === "fulfilled") {
        // if (state.data.form.IsVersioningDisabled === false) {
        //   <Version />;
        // }
        ToastAlert("Api Updated Successfully!!", "success");
      } else {
        ToastAlert("Api Updated request is not fulfilled!!", "error");
      }
    } else {
      ToastAlert("Please fill all the fields correctly! ", "error");
    }
  }
  const NavigateToApisList = (
    val: React.MouseEvent<HTMLButtonElement, MouseEvent>
  ) => {
    val.preventDefault();
    navigate("/api/list");
  };

  async function setKey(a: any) {
    console.log("selected tab : ", a);
    // state.data.form.SelectedTabIndex = a;
    dispatch(
      setForm({
        ...state.data.form,
        SelectedTabIndex: a,
      })
    );
  }
  console.log("selected tab : ", state.data.form.SelectedTabIndex);
  return (
    <div>
      {state.loading ? (
        <Spinner />
      ) : !state.loading && state.error !== null ? (
        <div>{failure()}</div>
      ) : (
        <div className="col-lg-12 grid-margin stretch-card">
          <div className="card">
            <div>
              <Form
                onSubmit={(e: FormEvent) => handleSubmitApiUpdate(e)}
                data-testid="form-input"
              >
                <div className="align-items-center">
                  <div
                    className="card-header bg-white mt-3 pt-1 pb-4"
                    style={{ padding: "0.5rem 1.5rem" }}
                  >
                    <button className=" btn btn-sm btn-success btn-md d-flex float-right mb-3">
                      {" "}
                      Update
                    </button>
                    <button
                      className=" btn  btn-sm btn-light btn-md d-flex float-right mb-3"
                      onClick={(e) => NavigateToApisList(e)}
                    >
                      {" "}
                      Cancel
                    </button>
                    <span>
                      <b>UPDATE API</b>
                    </span>
                  </div>
                  <div className="card-body pt-2">
                    <Tabs
                      defaultActiveKey={state.data.form?.SelectedTabIndex}
                      id="uncontrolled-tab"
                      // transition={false}
                      className="mb-2 small"
                      onSelect={(k) => setKey(k)}
                    >
                      <Tab eventKey="setting" title="Setting">
                        <Setting />
                      </Tab>
                      <Tab eventKey="version" title="Version">
                        <Version />
                      </Tab>
                    </Tabs>
                  </div>
                </div>
              </Form>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}

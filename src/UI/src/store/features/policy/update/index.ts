import { IError, IGetPolicyByIdData } from "../create";

export interface IPolicyUpdateState {
  data: IUpdateState;
  loading: boolean;
  error?: string | null;
}
export interface IUpdateState {
  form: IGetPolicyByIdData;
  errors?: IError;
}

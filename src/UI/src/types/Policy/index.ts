export interface IPolicyListState {
  data?: ISetPolicyList | null;
  loading: boolean;
  error?: string | null;
}
export interface IPolicyDataList {
  list: IPolicyData[];
  fields: string[];
}
export interface ISetPolicyList {
  Policies: IPolicyData[];
  TotalCount: number;
}
export interface IPolicyData {
  id: number;
  Name: string;
  State: string;
  Apis: string[];
  AuthType: string;
}

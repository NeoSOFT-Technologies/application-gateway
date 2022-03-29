export interface IApiListState {
  data?: ISetApiList | null;
  loading: boolean;
  error?: string | null;
}
export interface IApiDataList {
  list: IApiData[];
  fields: string[];
}
export interface ISetApiList {
  Apis: IApiData[];
  TotalCount: number;
}
export interface IApiData {
  Name: string;
  CreatedDate: string;
  TargetUrl: string;
  IsActive: boolean;
  Id?: string;
}

export interface IErrorApiInput {
  name: string;
  targetUrl: string;
  listenPath: string;
  status: boolean;
}
export interface IApiFormData {
  name: string;
  listenPath: string;
  targetUrl: string;
  isActive: Boolean;
  Id?: string;
}
export interface IErrorApiUpdateInput {
  apiName: string;
  targetUrl: string;
  listenPath: string;
  rate: string;
  perSecond: string;
}
export interface IApiUpdateFormData {
  apiName: string;
  listenPath: string;
  targetUrl: string;
  stripListenPath: Boolean;
  internal: Boolean;
  roundRobin: Boolean;
  service: Boolean;
  rateLimit: Boolean;
  rate: string;
  perSecond: string;
  quotas: Boolean;
}

export interface IApiUpdateError {
  apiName: string;
  listenPath: string;
}

export interface IApiUpdateForm {
  apiName: string;
  listenPath: string;
}

export interface IAddApiResponse {
  Succeeded: boolean;
  Message: string | null;
  Errors: string[] | null | unknown | any | string;
}

export interface IApiGetByIdState {
  data?: IGetApiByIdData | null;
  loading: boolean;
  error?: string | null;
}

export interface IGetApiByIdData {
  apiId: string;
  apiName: string;
  listenPath: string;
  stripListenPath: string;
  targetUrl: string;
  isActive: boolean;
  rateLimit: IRateLimitData[];
  versioningInfo: IVersioningInfo[];
  defaultVersion: string;
  version: IVersion[];
  isQuotaDisabled: boolean;
}

export interface IRateLimitData {
  rate: number;
  per: number;
  isDisabled: boolean;
}

export interface IVersioningInfo {
  location: number;
  key: string;
}

export interface IVersion {
  name: string;
  overrideTarget: string;
  expires: string;
}

export interface IProps {
  onChange: Function;
  updateApiData: any;
}

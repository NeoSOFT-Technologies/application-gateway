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
  stripListenPath?: Boolean;
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
  data: IUpdateState;
  loading: boolean;
  error?: string | null;
}
// get by id
export interface IGetApiByIdData {
  apiId: string;
  apiName: string;
  listenPath: string;
  stripListenPath: string;
  targetUrl: string;
  isActive: boolean;
  rateLimit: {
    rate: number;
    per: number;
    isDisabled: boolean;
  };
  versioningInfo: { location: number; key: string };
  defaultVersion: string;
  version: { name: string; overrideTarget: string; expires: string };
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

export interface IApiUpdateState {
  data?: IApiUpdateFormData;
  loading: boolean;
  error?: string | null;
}
// get by id error
export interface IError {
  apiId: string;
  apiName: string;
  listenPath: string;
  stripListenPath: string;
  targetUrl: string;
  isActive: string;
  rate: string;
  perSecond: string;
  versioningInfo: string;
  defaultVersion: string;
  version: string;
  isQuotaDisabled: string;
}
// update state slice

export interface IUpdateState {
  form: IGetApiByIdData | null;
  errors?: IError | null;
}

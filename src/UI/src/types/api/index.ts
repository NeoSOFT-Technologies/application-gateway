export interface IApiListState {
  data?: ISetApiList | null;
  loading?: boolean;
  error?: string | null;
}
export interface IAddApiState {
  apiAdded?: boolean;
  loading: boolean;
  error?: string | null;
}
export interface IDeleteApiState {
  isDeleted?: boolean;
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
  isActive: boolean;
  stripListenPath?: boolean;
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
  apiId: string;
  name: string;
  listenPath: string;
  stripListenPath: boolean;
  targetUrl: string;
  isActive: boolean;
  isInternal: boolean;
  protocol: string;
  rateLimit: {
    rate: number;
    per: number;
    isDisabled: boolean;
  };
  blacklist: [];
  whitelist: [];
  versioningInfo: {
    location: string;
    key: string;
  };
  defaultVersion: string;
  versions: [
    {
      name: string;
      overrideTarget: string;
      expires: string;
      globalRequestHeaders: {
        additionalProp1: string;
        additionalProp2: string;
        additionalProp3: string;
      };
      globalRequestHeadersRemove: [];
      globalResponseHeaders: {
        additionalProp1: string;
        additionalProp2: string;
        additionalProp3: string;
      };
      globalResponseHeadersRemove: [];
      extendedPaths: {
        methodTransforms: [];
        urlRewrites: [];
        validateJsons: [];
        transformHeaders: [];
        transformResponseHeaders: [];
        transform: [];
        transformResponse: [];
      };
    }
  ];
  authType: string;
  openidOptions: {
    providers: [];
  };
  loadBalancingTargets: [];
  isQuotaDisabled: boolean;
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
  ApiId: string;
  Name: string;
  ListenPath: string;
  StripListenPath: boolean;
  TargetUrl: string;
  IsActive: boolean;
  IsInternal: boolean;
  Protocol: string;
  RateLimit: {
    Rate: number;
    Per: number;
    IsDisabled: boolean;
  };
  Blacklist: [];
  Whitelist: [];
  VersioningInfo: {
    Location: number;
    Key: string;
  };
  DefaultVersion: string;
  Versions: [
    {
      Name: string;
      OverrideTarget: string;
      Expires: string;
      GlobalRequestHeaders: {};
      GlobalRequestHeadersRemove: [];
      GlobalResponseHeaders: {};
      GlobalResponseHeadersRemove: [];
      ExtendedPaths: null;
    }
  ];
  AuthType: string;
  OpenidOptions: {
    Providers: [];
  };
  LoadBalancingTargets: [];
  IsQuotaDisabled: boolean;
}

export interface RateLimit {
  Rate: number;
  Per: number;
  IsDisabled: boolean;
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
  data?: IApiUpdateFormData | null;
  loading: boolean;
  error?: string | null;
}
// get by id error
export interface IError {
  ApiId: string;
  Name: string;
  ListenPath: string;
  stripListenPath: string;
  TargetUrl: string;
  isActive: string;
  Rate: string;
  Per: string;
  versioningInfo: string;
  defaultVersion: string;
  version: string;
  isQuotaDisabled: string;
}
// update state slice

export interface IUpdateState {
  form: IGetApiByIdData;
  errors?: IError | null;
}

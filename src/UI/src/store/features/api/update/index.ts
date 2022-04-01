import { IError } from "../../../../types/api";

export interface IApiGetByIdState {
  data: IUpdateState;
  loading: boolean;
  error?: string | null;
}

export interface IUpdateState {
  form: IGetApiByIdData;
  errors?: IError;
}

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

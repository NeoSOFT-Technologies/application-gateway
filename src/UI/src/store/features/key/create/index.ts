export interface IKeyCreateState {
  data: ICreateState;
  loading: boolean;
  error?: string | null;
}
export interface ICreateState {
  form: IGetKeyByIdData;
  errors?: IError;
}

export interface IGetKeyByIdData {
  keyId?: string;
  keyName: string;
  per: number;
  rate: number;
  quota: number;
  expires: string | number;
  isInActive?: boolean;
  quotaRenewalRate: number;
  throttleInterval: number;
  throttleRetries: number;
  accessRights:
    | [
        {
          apiId: string | null;
          apiName: string | null;
          versions: string[];
          allowedUrls?: [
            {
              url: string;
              methods: string[];
            }
          ];
          limit: {
            rate?: number;
            throttle_interval?: number;
            throttle_retry_limit?: number;
            max_query_depth?: number;
            quota_max?: number;
            quota_renews?: number;
            quota_remaining?: number;
            quota_renewal_rate?: number;
          } | null;
        } | null
      ]
    | [];
  policies: [];
  policyByIds?: [];
  tags?: string[];
}
export interface IError {
  keyId?: string;
  keyName: string;
  accessRights?: string;
  policy?: string;
  per?: string;
  rate?: string;
  quota?: string;
  expires?: string;
  quotaRenewalRate?: string;
  throttleInterval?: string;
  throttleRetries?: string;
}

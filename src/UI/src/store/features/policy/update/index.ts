import { IError } from "../create";

export interface IPolicyUpdateState {
  data: IUpdateState;
  loading: boolean;
  error?: string | null;
}
export interface IUpdateState {
  form: IUpdatePolicyData;
  errors?: IError;
}
export interface IUpdatePolicyData {
  policyId?: string;
  name: string;
  active: boolean;
  keysInactive: boolean;
  maxQuota: number;
  quotaRate: number;
  rate: number;
  per: number;
  throttleInterval: number;
  throttleRetries: number;
  state: string;
  keyExpiresIn: number;
  tags?: string[];
  apIs: [
    {
      id: string | null;
      name: string;
      versions: string[];
      allowedUrls?: [
        {
          url: string;
          methods: string[];
        }
      ];
      limit: {
        rate?: number;
        per: number;
        throttle_interval?: number;
        throttle_retry_limit?: number;
        max_query_depth?: number;
        quota_max?: number;
        quota_renews?: number;
        quota_remaining?: number;
        quota_renewal_rate?: number;
        set_by_policy: boolean;
      } | null;
    }
  ];
  partitions: {
    quota: boolean;
    rate_limit: boolean;
    complexity: boolean;
    acl: boolean;
    per_api: boolean;
  };
}

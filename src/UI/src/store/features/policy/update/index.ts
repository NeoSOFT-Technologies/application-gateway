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
  PolicyId?: string;
  Name: string;
  Active: boolean;
  KeysInactive: boolean;
  MaxQuota: number;
  QuotaRate: number;
  Rate: number;
  Per: number;
  ThrottleInterval: number;
  ThrottleRetries: number;
  State: string;
  KeyExpiresIn: number;
  Tags?: string[];
  ApIs: [
    {
      Id: string | null;
      Name: string;
      Versions: string[];
      AllowedUrls?: [
        {
          url: string;
          methods: string[];
        }
      ];
      Limit: {
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
  Partitions: {
    quota: boolean;
    rate_limit: boolean;
    complexity: boolean;
    acl: boolean;
    per_api: boolean;
  };
}

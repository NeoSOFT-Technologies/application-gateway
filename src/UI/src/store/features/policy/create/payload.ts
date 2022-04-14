import { IPolicyCreateState } from "./index";

export const initialState: IPolicyCreateState = {
  data: {
    form: {
      name: "",
      active: true,
      keysInactive: true,
      maxQuota: 0,
      quotaRate: 0,
      rate: 0,
      per: 0,
      throttleInterval: 0,
      throttleRetries: 0,
      state: "",
      keyExpiresIn: 0,
      tags: [],
      apIs: [
        {
          id: null,
          name: "",
          versions: [],
          allowedUrls: [
            {
              url: "",
              methods: [],
            },
          ],
          limit: {
            rate: 0,
            per: 0,
            throttle_interval: 0,
            throttle_retry_limit: 0,
            max_query_depth: 0,
            quota_max: 0,
            quota_renews: 0,
            quota_remaining: 0,
            quota_renewal_rate: 0,
            set_by_policy: false,
          },
        },
      ],
      partitions: {
        quota: false,
        rate_limit: false,
        complexity: false,
        acl: false,
        per_api: false,
      },
    },
  },
  loading: false,
  error: null,
};

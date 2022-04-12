import { IKeyCreateState } from "./index";

export const initialState: IKeyCreateState = {
  data: {
    form: {
      keyName: "",
      per: 0,
      rate: 0,
      quota: 0,
      expires: 0,
      // isInActive: false,
      quotaRenewalRate: 0,
      throttleInterval: 0,
      throttleRetries: 0,
      accessRights: [],
      // [
      //   {
      //     apiId: "",
      //     apiName: "",
      //     versions: [],
      //     allowedUrls: [
      //       {
      //         url: "",
      //         methods: [],
      //       },
      //     ],
      //     limit: {
      //       rate: 0,
      //       throttle_interval: 0,
      //       throttle_retry_limit: 0,
      //       max_query_depth: 0,
      //       quota_max: 0,
      //       quota_renews: 0,
      //       quota_remaining: 0,
      //       quota_renewal_rate: 0,
      //     },
      //   },
      // ],
      policies: [],
      tags: [],
    },
    errors: {
      keyName: "",
    },
  },
  loading: false,
  error: null,
};

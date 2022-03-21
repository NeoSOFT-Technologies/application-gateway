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
}

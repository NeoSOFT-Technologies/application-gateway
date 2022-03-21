export interface IKeyListState {
  data?: ISetKeyList | null;
  loading: boolean;
  error?: string | null;
}
export interface IKeyDataList {
  list: IKeyData[];
  fields: string[];
}
export interface ISetKeyList {
  Keys: IKeyData[];
  TotalCount: number;
}
export interface IKeyData {
  KeyName: string;
  CreatedDate: string;
  IsActive: boolean;
  Id: string;
  Policies?: string[];
}

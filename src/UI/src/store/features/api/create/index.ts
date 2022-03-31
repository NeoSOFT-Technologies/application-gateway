export interface IAddApiState {
  apiAdded?: boolean;
  loading: boolean;
  error?: string | null;
}

export interface IApiFormData {
  name: string;
  listenPath: string;
  targetUrl: string;
  isActive: boolean;
  stripListenPath?: boolean;
  Id?: string;
}

export interface IErrorApiInput {
  name: string;
  targetUrl: string;
  listenPath: string;
  status: boolean;
}

export interface IData {
  memes: IMeme[];
}

export interface IMeme {
  id: string;
  name: string;
  url: URL;
  width: number;
  height: number;
  box_count: number;
}

export interface IMemeAPIResponse {
  success: boolean;
  data: IData;
}

export interface IGeneratedMemeAPIResponse {
    success: boolean;
    data: IGeneratedMemeAPIResponse_Data; 
}

export interface IGeneratedMemeAPIResponse_Data {
    url: URL;
    page_url: URL;
}

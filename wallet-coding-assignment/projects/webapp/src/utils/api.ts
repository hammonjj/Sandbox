import axios from 'axios';

export type Method = 'get' | 'post';

export const ApiRequest = ({ path, method, data }: { path: string; method: Method; data?: object }) => {
  return axios
    .request({
      url: `http://localhost:3001/${path}`,
      method,
      data,
    })
    .then((res) => res.data);
};

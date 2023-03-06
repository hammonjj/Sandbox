import { useQuery } from '@tanstack/react-query';
import { ApiRequest } from '../utils/api';

const getUserCount = () => {
  return ApiRequest({ path: 'users/count', method: 'get' });
};

export const UserCount = () => {
  const { data, isLoading } = useQuery({ queryKey: ['usercount'], queryFn: getUserCount });

  if (isLoading) return <p>Loading...</p>;
  console.log(data);

  return <p>We currently have {data.data.count} users!</p>;
};

import { useMutation, useQueryClient } from '@tanstack/react-query';
import { ApiRequest } from '../utils/api';
import { faker } from '@faker-js/faker';

export type User = {
  email: string;
  password: string;
};

const addUser = (data: User) => {
  return ApiRequest({ path: 'users', method: 'post', data });
};

export const AddUserButton = () => {
  const queryClient = useQueryClient();

  const mutation = useMutation({
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['usercount'] });
      queryClient.invalidateQueries({ queryKey: ['users'] });
    },
    mutationFn: addUser,
  });

  const onSubmit = () => {
    mutation.mutate({ email: faker.internet.email(), password: 'password' });
  };

  return <button onClick={onSubmit}>Add a user</button>;
};

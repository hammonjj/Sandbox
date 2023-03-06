import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import { ApiRequest } from '../utils/api';

const getAllUsers = () => {
  return ApiRequest({ path: 'users', method: 'get' });
};

export const Users = () => {
  const { data, isLoading } = useQuery({ queryKey: ['users'], queryFn: getAllUsers });

  if (isLoading) {
    return <p>Loading...</p>;
  }

  return (
    <table>
        <thead>
        <tr>
            <th>ID</th>
            <th>Email</th>
            <th>Transactions</th>
            <th>Account Summary</th>
        </tr>
        </thead>
        <tbody>
            {data.data.map((user: { id: string; email: string; }) => (
                <tr key={user.id}>
                <td>{user.id}</td>
                <td>{user.email}</td>
                <td><Link to={`/transactions/${user.id}`}>Add Transaction</Link></td>
                <td><Link to={`/accountsummary/${user.id}`}>Account Summary</Link></td>
            </tr>
            ))}
        </tbody>
    </table>
  );
};

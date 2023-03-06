import { useParams } from "react-router-dom";
import { ApiRequest } from "../utils/api";
import { QueryFunctionContext, useQuery } from '@tanstack/react-query';

const getAccountBalance = ({queryKey}: QueryFunctionContext<[string, string | null | undefined]>) => {
    const id = queryKey[1];
    return ApiRequest({ path: `transactions/balance/${id}`, method: 'get' });
  };
  
const getRecentTransactions = ({queryKey}: QueryFunctionContext<[string, string | null | undefined]>) => {
    const id = queryKey[1];
    return ApiRequest({ path: `transactions?ownerId=${id}&transactionCount=10`, method: 'get' });
  };

const formatNumber = (q: number) => {
    return q.toLocaleString('en-US', {
        style: 'currency',
        currency: 'USD'
    })
} 

export const AccountSummary = () => {
    const { ownerId } = useParams();

    const balanceDataResults = useQuery({ queryKey: ['accountBalance', ownerId], queryFn: getAccountBalance });
    const transactionDataResults = useQuery({ queryKey: ['accountTransactions', ownerId], queryFn: getRecentTransactions });

    if (balanceDataResults.isLoading || transactionDataResults.isLoading) {
      return <p>Loading...</p>;
    }

    console.log(transactionDataResults.data.data);

    return (
        <header className='App-header'>
            Summary for Account ID {ownerId} - Balance: {formatNumber(balanceDataResults.data.data.balance)}
            <table>
                <thead>
                <tr>
                    <th>ID</th>
                    <th>Date</th>
                    <th>Amount</th>
                </tr>
                </thead>
                <tbody>
                    {transactionDataResults.data.data.map((transaction: { id: number; ownerId: number; date: string; amount: string}) => (
                        <tr key={transaction.id}>
                            <td>{transaction.id}</td>
                            <td>{transaction.date}</td>
                            <td>{formatNumber(+transaction.amount)}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </header>
    );
}
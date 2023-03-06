import { useParams } from "react-router-dom";
import { ApiRequest } from "../utils/api";

type TransactionData = {
    ownerId: number;
    amount: number;
}

const addTransaction = (data: TransactionData) => {
    return ApiRequest({ path: `transactions`, method: 'post', data });
  };

export const Transaction = () => {
    const { ownerId } = useParams();

    return (
        <header className='App-header'>
            Add Transaction
            <form
                onSubmit={(e) => {
                    e.preventDefault();
                    const formData = new FormData(e.currentTarget);
                    const obj: TransactionData = {
                      ownerId: ownerId ? +ownerId : 0,
                      amount: formData.get("amount") ? +(formData.get("amount")!) : 0,
                    };
                    addTransaction(obj);
                  }}
                >
                <label htmlFor="amount">
                    OwnerId:
                    <input id="ownerId" name="ownerId" placeholder={ownerId} disabled={true} />
                </label>
                <label htmlFor="amount">
                    Amount: 
                    <input id="amount" name="amount" type="number" placeholder="100.00" />
                </label>
                <button>Submit</button>
            </form>
        </header>
    );
}
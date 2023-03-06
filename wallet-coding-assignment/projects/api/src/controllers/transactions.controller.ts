import { NextFunction, Request, Response } from 'express';
import TransactionService from '@/services/transaction.service';
import { CreateTransactionDto } from '@/dtos/transaction.dto';
import { Transaction } from '@prisma/client';

class TransactionsController {
  public transactionService = new TransactionService();

  public createTransaction = async (req: Request, res: Response, next: NextFunction): Promise<void> => {
    try {
      const transactionData: CreateTransactionDto = req.body;
      const createTransactionData = await this.transactionService.createTransaction(transactionData);

      res.status(201).json({
        transactionData: createTransactionData,
        message: 'created',
      });
    } catch (error) {
      next(error);
    }
  };

  public getRecentTransactions = async (req: Request, res: Response, next: NextFunction): Promise<void> => {
    try {
      const ownerId = Number(req.query.ownerId);
      const transactionCount = Number(req.query.transactionCount);

      const transactionList: Transaction[] = await this.transactionService.getRecentTransactionsByOwnerId(ownerId, transactionCount);

      res.status(200).json({ data: transactionList, message: 'recentTransactions' });
    } catch (error) {
      next(error);
    }
  };

  //This should really been on it's own route for account specific functions
  public getAccountBalance = async (req: Request, res: Response, next: NextFunction): Promise<void> => {
    try {
      const ownerId = Number(req.params.ownerId);
      const accountBalance: Number = await this.transactionService.getAccountBalanceByOwnerId(ownerId);

      res.status(200).json({ data: { balance: accountBalance }, message: 'balance' });
    } catch (error) {
      next(error);
    }
  };
}

export default TransactionsController;

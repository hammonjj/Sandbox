import { Router } from 'express';
import TransactionsController from '@controllers/transactions.controller';
import { CreateTransactionDto } from '@/dtos/transaction.dto';
import { Routes } from '@interfaces/routes.interface';
import validationMiddleware from '@middlewares/validation.middleware';

class TransactionsRoute implements Routes {
  public path = '/transactions';
  public router = Router();
  public transactionController = new TransactionsController();

  constructor() {
    this.initializeRoutes();
  }

  private initializeRoutes() {
    this.router.get(`${this.path}`, this.transactionController.getRecentTransactions);
    this.router.get(`${this.path}/balance/:ownerId(\\d+)`, this.transactionController.getAccountBalance);
    this.router.post(`${this.path}`, validationMiddleware(CreateTransactionDto, 'body'), this.transactionController.createTransaction);
  }
}

export default TransactionsRoute;

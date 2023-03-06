import request from 'supertest';
import { PrismaClient, Transaction } from '@prisma/client';
import App from '@/app';
import { CreateTransactionDto } from '@dtos/transaction.dto';
import TransactionRoute from '@routes/transactions.route';

afterAll(async () => {
  await new Promise<void>(resolve => setTimeout(() => resolve(), 500));
});

//Due to a lack of time I only tested the positive cases - I would normally exercise the failing
//code paths as well
describe('Testing Transactions', () => {
  describe('[GET] /transactions', () => {
    it('response getAccountBalance', async () => {
      const transactionRoute = new TransactionRoute();
      const transactions = transactionRoute.transactionController.transactionService.transaction;

      transactions.findFirst = jest.fn().mockReturnValue({
        id: 11,
        ownerId: 1,
        date: '2023-03-03T20:24:14.344Z',
        amount: '100.00',
      });

      transactions.findMany = jest.fn().mockReturnValue([
        {
          id: 11,
          ownerId: 1,
          date: '2023-03-03T20:24:14.344Z',
          amount: '100.00',
        },
        {
          id: 12,
          ownerId: 1,
          date: '2023-03-03T20:24:14.344Z',
          amount: '200.00',
        },
        {
          id: 13,
          ownerId: 1,
          date: '2023-03-03T20:24:14.344Z',
          amount: '300.00',
        },
      ]);

      const app = new App([transactionRoute]);
      const resp = await request(app.getServer()).get(`${transactionRoute.path}/balance/1`).expect(200);
      expect(resp.body.data.balance === 600);
    });
  });

  describe('[GET] /transactions', () => {
    it('response getRecentTransactionsByOwnerId', async () => {
      const transactionRoute = new TransactionRoute();
      const transactions = transactionRoute.transactionController.transactionService.transaction;

      transactions.findFirst = jest.fn().mockReturnValue({
        id: 11,
        ownerId: 1,
        date: '2023-03-03T20:24:14.344Z',
        amount: '100.00',
      });

      transactions.findMany = jest.fn().mockReturnValue([
        {
          id: 11,
          ownerId: 1,
          date: '2023-03-03T20:24:14.344Z',
          amount: '100.00',
        },
        {
          id: 12,
          ownerId: 1,
          date: '2023-03-03T20:24:14.344Z',
          amount: '200.00',
        },
        {
          id: 13,
          ownerId: 1,
          date: '2023-03-03T20:24:14.344Z',
          amount: '300.00',
        },
      ]);

      const app = new App([transactionRoute]);
      const resp = await request(app.getServer()).get(`${transactionRoute.path}`).expect(200);
      expect(resp.body.data.length === 3);
    });
  });

  describe('[POST] /transactions', () => {
    it('response Create transaction', async () => {
      const transactionData: CreateTransactionDto = {
        ownerId: 1,
        amount: 0,
      };

      const transactionRoute = new TransactionRoute();
      const transactions = transactionRoute.transactionController.transactionService.transaction;

      transactions.findFirst = jest.fn().mockReturnValue({
        id: 11,
        ownerId: 1,
        date: '2023-03-03T20:24:14.344Z',
        amount: '100.00',
      });

      transactions.create = jest.fn().mockReturnValue({
        id: 1,
        ownerId: 1,
        date: '2023-03-03T20:24:14.344Z',
        amount: 0,
      });

      const app = new App([transactionRoute]);
      return request(app.getServer()).post(`${transactionRoute.path}`).send(transactionData).expect(201);
    });
  });
});

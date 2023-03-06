import { CreateTransactionDto } from '@/dtos/transaction.dto';
import { PrismaClient, Transaction } from '@prisma/client';
import { HttpException } from '@exceptions/HttpException';
import { isEmpty } from '@utils/util';

class TransactionService {
  public prismaClient = new PrismaClient();
  public transaction = this.prismaClient.transaction;

  public async createTransaction(transactionData: CreateTransactionDto): Promise<Transaction> {
    if (isEmpty(transactionData)) {
      throw new HttpException(400, 'transactionData is empty');
    }

    //Real world account creation would be its own process (w/ accompanying routes/tables/services, etc.) and we
    //woulnd't be banking on the transaction amount being 0 to determine if this is a new account
    if (transactionData.amount !== 0) {
      const findAccount: Transaction = await this.transaction.findFirst({ where: { ownerId: transactionData.ownerId } });
      if (!findAccount) {
        throw new HttpException(409, "Owner doesn't exist");
      }
    }

    console.log(`OwnerId: ${transactionData.ownerId}`);
    const createTransactionData: Transaction = await this.transaction.create({ data: { ...transactionData } });
    console.log(`Data: ${JSON.stringify(createTransactionData)}`);
    return createTransactionData;
  }

  public async getRecentTransactionsByOwnerId(ownerId: number, transactions: number): Promise<Transaction[]> {
    if (isEmpty(ownerId)) {
      throw new HttpException(400, 'ownerId is empty');
    }

    const findAccount: Transaction = await this.transaction.findFirst({ where: { ownerId: ownerId } });
    if (!findAccount) {
      throw new HttpException(409, "Owner doesn't exist");
    }

    const transactionsList: Transaction[] = await this.transaction.findMany({
      take: transactions,
      where: {
        ownerId: ownerId,
      },
      orderBy: {
        date: 'desc',
      },
    });

    return transactionsList;
  }

  public async getAccountBalanceByOwnerId(ownerId: number): Promise<number> {
    if (isEmpty(ownerId)) {
      throw new HttpException(400, 'ownerId is empty');
    }

    const findAccount: Transaction = await this.transaction.findFirst({ where: { ownerId: ownerId } });
    if (!findAccount) {
      throw new HttpException(409, "Owner doesn't exist");
    }

    const transactionsList: Transaction[] = await this.transaction.findMany({
      where: {
        ownerId: ownerId,
      },
    });

    let balance = 0;
    for (let i = 0; i < transactionsList.length; i++) {
      //Apparently prisma decimal types don't play nicely with TypeScript or at least I didn't have enough time to
      //research what's going on here. In the real world I would find a cleaner way to do this
      //I'm going to need a shower after writing this
      const amountString = transactionsList[i].amount as unknown as string;
      const amountFloat: number = parseFloat(amountString);
      balance = balance + amountFloat;
    }

    return balance;
  }
}

export default TransactionService;

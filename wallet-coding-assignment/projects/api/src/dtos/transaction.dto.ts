import { IsNumber } from 'class-validator';

export class CreateTransactionDto {
  @IsNumber()
  public ownerId: number;

  @IsNumber()
  public amount: number;
}

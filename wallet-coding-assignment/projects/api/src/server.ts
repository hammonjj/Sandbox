import App from '@/app';
import IndexRoute from '@routes/index.route';
import UsersRoute from '@routes/users.route';
import validateEnv from '@utils/validateEnv';
import TransactionsRoute from './routes/transactions.route';

validateEnv();

const app = new App([new IndexRoute(), new UsersRoute(), new TransactionsRoute()]);

app.listen();

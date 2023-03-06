import './App.css';
import { BrowserRouter, Routes, Route, Link } from "react-router-dom";
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';

import { Home } from './components/home';
import { AccountSummary } from './components/accountsummary';
import { Transaction } from './components/transaction';

const queryClient = new QueryClient();

function App() {
  return (
    <div className='App'>
      <BrowserRouter>
      <QueryClientProvider client={queryClient}>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/accountsummary/:ownerId" element={<AccountSummary />} />
          <Route path="/transactions/:ownerId" element={<Transaction />} />
        </Routes>      
      </QueryClientProvider>
      </BrowserRouter>
    </div>
  );
}

export default App;

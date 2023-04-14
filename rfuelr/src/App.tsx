import './App.css';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Home from './components/Home';
import useAuth from './hooks/useAuth';
import Login from './components/Login';

const queryClient = new QueryClient();

function App() {
  const {username, password} = useAuth();

    if(!username || !password) {
        return (
            <div className="App">
                <Login />
            </div>
        );
    }
    
  return (
    <div className="App">
      <BrowserRouter>
        <QueryClientProvider client={queryClient}>
          <Routes>
            <Route path="/" element={<Home />} />
          </Routes>
        </QueryClientProvider>
      </BrowserRouter>
    </div>
  );
}

export default App;
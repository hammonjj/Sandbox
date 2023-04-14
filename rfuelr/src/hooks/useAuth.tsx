import { useState } from 'react';

const useAuth = () => {
  const [username, setUsername] = useState<string | null>(sessionStorage.getItem('username'));
  const [password, setPassword] = useState<string | null>(sessionStorage.getItem('password'));

  const setCredentials = (username: string, password: string) => {
    sessionStorage.setItem('username', username);
    sessionStorage.setItem('password', password);
    setUsername(username);
    setPassword(password);
  };

  const clearCredentials = () => {
    sessionStorage.removeItem('username');
    sessionStorage.removeItem('password');
    setUsername(null);
    setPassword(null);
  };

  return {
    username,
    password,
    setCredentials,
    clearCredentials,
  };
};

export default useAuth;

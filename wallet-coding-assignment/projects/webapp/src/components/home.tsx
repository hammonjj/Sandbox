import { AddUserButton } from "./adduser";
import { UserCount } from "./usercount";
import { Users } from "./users";

export const Home = () => {
    return (
        <header className='App-header'>
          Welcome to the Equi Coding Assignment!
          <UserCount />
          <AddUserButton />
          <Users />
        </header>
    );
}
import useAuth from "../hooks/useAuth";

export default function Home() {
    const {username, password} = useAuth();

    if(!username || !password) {
        return (
            <div>
                <h1>Home</h1>
                <p>You are not logged in.</p>
            </div>
        );
    }

    return (
        <div>
            <h1>Home</h1>
            <p>User is logged in</p>
        </div>
    );
}
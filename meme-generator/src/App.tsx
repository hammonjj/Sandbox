import { createRoot } from "react-dom/client";
import SearchParams from "./SearchParams";
import { Link, BrowserRouter, Routes, Route } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import Details from "./Details";
import { useState } from "react";
import { IMeme } from "./APIResponsesTypes";
import SelectedMemeContext from "./SelectedMemeContext";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: Infinity,
      cacheTime: Infinity,
    },
  },
});

const App = () => {
  const selectedMeme = useState(null as IMeme | null);

  return (
    <div className="m-0 p-0 min-h-screen bg-purple-900">
      <BrowserRouter>
        <QueryClientProvider client={queryClient}>
          <SelectedMemeContext.Provider value={selectedMeme}>
            <header className="mb-10 w-full bg-purple-500 p-7 text-left shadow-2xl">
              <Link className="font-semibold text-xl tracking-tight" to="/">
                Meme Generator
              </Link>
            </header>
            <Routes>
              <Route path="/details/" element={<Details />} />
              <Route path="/" element={<SearchParams />} />
            </Routes>
          </SelectedMemeContext.Provider>
        </QueryClientProvider>
      </BrowserRouter>
    </div>
  );
};

const container = document.getElementById("root");

if (!container) {
  throw new Error("No container to render to");
}

const root = createRoot(container);
root.render(<App />);

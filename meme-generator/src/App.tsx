import { createRoot } from "react-dom/client";
import SearchParams from "./SearchParams";
import { BrowserRouter, Routes, Route } from "react-router-dom";
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
    <div className="main-background">
      <BrowserRouter>
        <QueryClientProvider client={queryClient}>
          <SelectedMemeContext.Provider value={selectedMeme}>
            
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

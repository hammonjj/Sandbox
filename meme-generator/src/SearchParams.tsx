import { useState, useEffect } from "react";
import { useQuery } from "@tanstack/react-query";
import fetchSearch from "./fetchSearch";
import Results from "./Results";

const SearchParams = () => {
  const [memeTemplate, setMemeTemplate] = useState("");
  const results = useQuery(["search", ""], fetchSearch);
  const memeList = results?.data?.data?.memes ?? [];

  return (
    <div className="my-0 mx-auto w-11/12">
      <form
        className="mb-10 flex flex-col items-center justify-center rounded-lg bg-purple-700 p-10 shadow-lg">
        <label htmlFor="template">
          Template Name
          <input
            type="text"
            className="search-input"
            name="template"
            id="template"
            placeholder="Template"
            onChange={(e) => {
              setMemeTemplate(e.target.value);
            }}
          />
        </label>
      </form>

      <Results memes={memeList} memeSearch={memeTemplate} />
    </div>
  );
};

export default SearchParams;

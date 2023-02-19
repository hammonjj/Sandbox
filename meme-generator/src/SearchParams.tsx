import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import fetchSearch from "./fetchSearch";
import Results from "./Results";

const SearchParams = () => {
  const [memeTemplate, setMemeTemplate] = useState("");
  const results = useQuery(["search", ""], fetchSearch);
  const memeList = results?.data?.data?.memes ?? [];

  return (
    <div className="my-0 mx-auto w-11/12 py-4">
      <form
        className="template-search">
        <label htmlFor="template">
          <input
            type="text"
            className="search-input"
            name="template"
            id="template"
            placeholder="Search Template"
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

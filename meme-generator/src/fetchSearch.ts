import { QueryFunction } from "@tanstack/query-core";
import { IMemeAPIResponse } from "./APIResponsesTypes";

const fetchSearch: QueryFunction<IMemeAPIResponse, ["search", ""]> = async () => {
  const res = await fetch(`https://api.imgflip.com/get_memes`);

  if (!res.ok) {
    throw new Error(`meme search not ok`);
  }

  return res.json();
};

export default fetchSearch;

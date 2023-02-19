import { IMeme } from "./APIResponsesTypes";
import Meme from "./Meme";

const Results = ({ memes, memeSearch }: { memes: IMeme[], memeSearch: string}) => {
  const filteredMemes = memes.filter(x => 
    (x.box_count === 2 || x.box_count === 3) &&
      x.name.toLowerCase().includes(memeSearch)).sort((a, b) => b.height - a.height);
    
  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-3">
      {!filteredMemes.length ? (
        <h1>No Memes Found</h1>
      ) : (
        filteredMemes.map((meme) => (
          <Meme
            {...meme}
            key={meme.id}
          />
        ))
      )}
    </div>
  );
};

export default Results;

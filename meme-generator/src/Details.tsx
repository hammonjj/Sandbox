import { useContext, useState } from "react";
import SelectedMemeContext from "./SelectedMemeContext";
import Draggable from "react-draggable";
import * as htmlToImage from 'html-to-image';

const Details = () => {
  const [topText, setTopText] = useState("");
  const [bottomText, setBottomText] = useState("");
  const [thirdText, setThirdText] = useState("");

  const [meme] = useContext(SelectedMemeContext);
 
  const downloadMeme = async () => {
    const element = document.getElementById("meme-div");

    if(!element) {
      return;
    }

    const dataUrl = await htmlToImage.toJpeg(element);
    const link = document.createElement('a');
    link.download = "generated-meme.jpg";
    link.href = dataUrl;
    link.click();
  };

  let renderThirdText =  meme?.box_count == 3;

  if(!meme) {
    return (<div></div>);
  }

  let memeHeight = meme.height;
  let memeWidth = meme.width;
  if(meme.height > 450 && (meme.height >= meme.width)) {
    memeHeight = 450;
    let memeShrinkage = (450 / meme.height);
    memeWidth = memeShrinkage * meme?.width;
  } 
  else if(meme.width > 450 && (meme.height < meme.width)) {
    memeWidth = 450;
    let memeShrinkage = (450 / meme.width);
    memeHeight = memeShrinkage * meme?.height;
  }

  return (
    <div className="grid grid-cols-2 gap-4">
      <div
        id="meme-div"
        className="meme" 
        style={{
          backgroundImage: `url("${meme.url.toString()}")`,
          width: memeWidth + "px",
          height: memeHeight + "px",
          maxWidth: memeWidth + "px",
          maxHeight: memeHeight + "px"
        }}>
        <Draggable bounds="parent">
          <div className="meme-text">
            {topText}
          </div>
        </Draggable>
        <Draggable bounds="parent">
          <div className="meme-text">
            {bottomText}
          </div>
        </Draggable>
        {renderThirdText ? (
          <Draggable bounds="parent">
          <div className="meme-text">
            {thirdText}
          </div>
        </Draggable>
        ) : null}
      </div>

      <form
        onSubmit={(e) => {
          e.preventDefault();
          downloadMeme();
        }}>
        <label htmlFor="topText">Top Text
          <input
            type="text"
            className="search-input"
            name="topText"
            id="topText"
            placeholder="topText"
            onChange={(e) => {
              setTopText(e.target.value);
            }} />
        </label>
        <label htmlFor="bottomText">Top Text
          <input
            type="text"
            className="search-input"
            name="bottomText"
            id="bottomText"
            placeholder="bottomText" 
            onChange={(e) => {
              setBottomText(e.target.value);
            }} />
        </label>
        {renderThirdText ? (<label htmlFor="thirdText">Third Text
          <input
            type="text"
            className="search-input"
            name="thirdText"
            id="thirdText"
            placeholder="thirdText" 
            onChange={(e) => {
              setThirdText(e.target.value);
            }} />
        </label>): null}
        <button className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">Download Meme</button>
      </form>
    </div>
  );
};

export default Details;

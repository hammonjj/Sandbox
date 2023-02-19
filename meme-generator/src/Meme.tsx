import { useContext } from "react";
import { Link } from "react-router-dom";
import { IMeme } from "./APIResponsesTypes";
import SelectedMemeContext from "./SelectedMemeContext";

const Meme = (props: IMeme) => {
    const [_, setMemeTemplate] = useContext(SelectedMemeContext);

    return (
        <Link to={`/details`} onClick={() => (
            setMemeTemplate(props)
        )}>
            <div className="image-container">
                <img src={props.url.toString()} alt={props.name} />
            </div>
        </Link>
    );
}

export default Meme;
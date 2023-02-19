import { createContext } from "react";
import { IMeme } from "./APIResponsesTypes";

const SelectedMemeContext = createContext<[IMeme | null, (selectedMeme: IMeme) => void]>([
    {
        id: "",
        name: "None",
        url: new URL("http://www.google.com"),
        width: 500,
        height: 500,
        box_count: 0
    },
    () => {}
]);

export default SelectedMemeContext;
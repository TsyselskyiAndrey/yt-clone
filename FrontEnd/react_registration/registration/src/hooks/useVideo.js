import { useContext } from "react";
import {VideoContext} from "../Contexts/VideoContext"

const useVideo = () => {
    return useContext(VideoContext);
}

export default useVideo;
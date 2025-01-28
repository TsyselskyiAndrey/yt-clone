import { useContext } from "react";
import {PlaylistVideoContext} from "../Contexts/PlaylistVideoContext"

const usePlaylistVideo = () => {
    return useContext(PlaylistVideoContext);
}

export default usePlaylistVideo;
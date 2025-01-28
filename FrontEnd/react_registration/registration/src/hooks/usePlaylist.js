import { useContext } from "react";
import { PlaylistContext } from "../Contexts/PlaylistContext"

const usePlaylist = () => {
    return useContext(PlaylistContext);
}

export default usePlaylist;
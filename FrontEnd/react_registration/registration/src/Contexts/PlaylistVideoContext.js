import { createContext, useState, useRef, useEffect } from 'react';
import { axiosWithToken } from "../API/axioscfg"
import useAxiosWithToken from "../hooks/useAxiosWithToken"
import qs from 'qs'

export const PlaylistVideoContext = createContext({});

export function PlaylistVideoProvider({ children, ...props }) {
    const [playlistVideos, setPlaylistVideos] = useState([]);
    const [sortMethod, setSortMethod] = useState("Publication")
    const [isAscending, setIsAscending] = useState(false)
    const [filters, setFilters] = useState({
        publicationDate: '',
        videoFormat: [],
        duration: '',
        likes: '',
        size: '',
        views: '',
        visibility: '',
        category: '',
    })
    const [searchValuePlaylistVideos, setSearchValuePlaylistVideos] = useState("")
    useAxiosWithToken();
    useEffect(() => {
        GetPlaylistVideos();
    }, [sortMethod, isAscending, filters])
    async function GetPlaylistVideos() {
        try {
            const response = await axiosWithToken.get(
                `/api/playlist/getplaylistvideos/${props.playlistId}/${sortMethod}/${isAscending}/${searchValuePlaylistVideos}`,
                {
                    withCredentials: true,
                    params: filters,
                    paramsSerializer: params => {
                        return qs.stringify(params)
                    }
                }
            );
            if (response.status === 200) {
                setPlaylistVideos(response.data)
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else {
                console.error('Error during retrieving video content:', error.response?.data || error.message);
            }
        }
    }

    return (
        <PlaylistVideoContext.Provider value={{ playlistVideos, setPlaylistVideos, GetPlaylistVideos, sortMethod, setSortMethod, isAscending, setIsAscending, filters, setFilters, searchValuePlaylistVideos, setSearchValuePlaylistVideos }}>
            {children}
        </PlaylistVideoContext.Provider>
    );
}
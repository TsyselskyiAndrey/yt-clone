import { createContext, useState, useRef, useEffect } from 'react';
import { axiosWithToken } from "../API/axioscfg"
import useAxiosWithToken from "../hooks/useAxiosWithToken"
import qs from 'qs'

export const PlaylistContext = createContext({});

export function PlaylistProvider({ children }) {
    const [playlists, setPlaylists] = useState([]);
    const [sortMethod, setSortMethod] = useState("Publication")
    const [isAscending, setIsAscending] = useState(false)
    const [filters, setFilters] = useState({
        publicationDate: '',
        lastUpdated: '',
        videoCount: '',
        visibility: ''
    })
    const [searchValuePlaylist, setSearchValuePlaylist] = useState("")
    useAxiosWithToken();
    useEffect(() => {
        GetPlaylists();
    }, [sortMethod, isAscending, filters])

    async function GetPlaylists() {
        try {
            const response = await axiosWithToken.get(
                `/api/playlist/mychannelplaylists/${sortMethod}/${isAscending}/${encodeURIComponent(searchValuePlaylist)}`,
                {
                    withCredentials: true,
                    params: filters,
                    paramsSerializer: params => {
                        return qs.stringify(params)
                    }
                }
            );
            if (response.status === 200) {
                setPlaylists(response.data)
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
        <PlaylistContext.Provider value={{ playlists, setPlaylists, GetPlaylists, sortMethod, setSortMethod, isAscending, setIsAscending, filters, setFilters, searchValuePlaylist, setSearchValuePlaylist }}>
            {children}
        </PlaylistContext.Provider>
    );
}
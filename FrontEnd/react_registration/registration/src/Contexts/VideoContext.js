import { createContext, useState, useRef, useEffect } from 'react';
import { axiosWithToken } from "../API/axioscfg"
import useAxiosWithToken from "../hooks/useAxiosWithToken"
import qs from 'qs'

export const VideoContext = createContext({});

export function VideoProvider({ children }) {
    const [videos, setVideos] = useState([]);
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
    const [searchValue, setSearchValue] = useState("")
    useAxiosWithToken();
    useEffect(() => {
        GetVideos();
    }, [sortMethod, isAscending, filters])

    async function GetVideos() {
        try {
            const response = await axiosWithToken.get(
                `/api/content/mychannelvideos/${sortMethod}/${isAscending}/${encodeURIComponent(searchValue)}`,
                {
                    withCredentials: true,
                    params: filters,
                    paramsSerializer: params => {
                        return qs.stringify(params)
                    }
                }
            );
            if (response.status === 200) {
                setVideos(response.data)
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
        <VideoContext.Provider value={{ videos, setVideos, GetVideos, sortMethod, setSortMethod, isAscending, setIsAscending, filters, setFilters, searchValue, setSearchValue }}>
            {children}
        </VideoContext.Provider>
    );
}
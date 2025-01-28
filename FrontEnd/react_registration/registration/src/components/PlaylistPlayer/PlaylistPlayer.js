import './PlaylistPlayer.css';
import sort from "../resources/icons/sorticon.svg"
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import TimeAgo from 'javascript-time-ago'

export default function PlaylistPlayer() {
    useAxiosWithToken();
    const { playlistId } = useParams();
    const { videoId } = useParams();
    const { showPrivate } = useParams();
    const [isSortOpen, setIsSortOpen] = useState(false);
    const [playlist, setPlaylist] = useState({});
    const timeAgo = new TimeAgo('en-US')
    const navigate = useNavigate();
    const [sortOption, setSortOption] = useState("Newest");
    useEffect(() => {
        GetPlaylistInfo();
    }, [sortOption, playlistId])

    async function GetPlaylistInfo() {
        try {
            const response = await axiosWithToken.get(
                `/api/media/getplaylistplayerinfo/${playlistId}/${sortOption}?getPrivate=${showPrivate}`,
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setPlaylist(response.data)
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else {
                console.error('Error during retrieving categories:', error.response?.data || error.message);
            }
        }
    }
    function handleSortOption(option) {
        setSortOption(option);
        setIsSortOpen(false);
    }
    const currentIndex = playlist.videos
    ? playlist.videos.findIndex(video => video.id == videoId)
    : -1;
    return (
        <div className='playlistPlayer'>
            <div className='playerInfo'>
                <h3>{playlist?.title}</h3>
                <p>{playlist?.channelTitle} - {currentIndex + 1}/{playlist.videos && playlist.videos.length}</p>
                <div className={'prevent-select sortBlock ' + (isSortOpen ? "sortopen" : "")} onClick={() => setIsSortOpen(!isSortOpen)}>
                    <img src={sort} alt="" />
                    Sort by
                    <div className="sortOptions">
                        {["Most liked", "Most hated", "Longest", "Shortest", "Popular", "Newest", "Oldest"].map(option => (
                            <div
                                key={option}
                                className={`sortOption ${sortOption === option ? "chosenOption" : ""}`}
                                onClick={() => handleSortOption(option)}
                            >
                                {option} videos first
                            </div>
                        ))}
                    </div>
                </div>
            </div>
            <div className="videosOfPlayer">
                {
                    playlist?.videos
                    &&
                    Object.keys(playlist?.videos).map((videoIdx) => {
                        return (
                            <div key={playlist.videos[videoIdx].id} className="videoPlayerVideoContainer" onClick={() => navigate(`/watch/${playlist.videos[videoIdx].id}/${playlistId}/${showPrivate}`)}>
                                <div className='playerStatus'>
                                    {
                                        videoId == playlist.videos[videoIdx].id
                                            ?
                                            <span>&#10148;</span>
                                            :
                                            <span>{parseInt(videoIdx) + 1}</span>

                                    }
                                </div>
                                <div className={"video-preview-small " + (videoId == playlist.videos[videoIdx].id ? "activeVideo" : "")}>
                                    <div className="thumbnail-row-small">
                                        <img className="thumbnail-small" src={playlist.videos[videoIdx].previewUrl + `?timestamp=${Date.now()}`}></img>
                                        <div className="video-time-small">{playlist.videos[videoIdx].duration}</div>
                                    </div>
                                    <div className="video-info">
                                        <p className="video-title-small">{playlist.videos[videoIdx].title}</p>
                                        <p className="video-author-small">{playlist.videos[videoIdx].channelTitle}</p>
                                        <p className="video-stats-small">{playlist.videos[videoIdx].numberOfViews} views &#183; {timeAgo.format(new Date(playlist.videos[videoIdx].publicationDate))}</p>
                                    </div>
                                </div>
                            </div>
                        )
                    })
                }

            </div>
        </div>
    );
}

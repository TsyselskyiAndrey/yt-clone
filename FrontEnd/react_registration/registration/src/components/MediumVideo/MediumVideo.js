import './MediumVideo.css';
import { useEffect, useState } from 'react';
import { axiosWithToken } from "../../API/axioscfg"
import { useNavigate } from 'react-router-dom';
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import TimeAgo from 'javascript-time-ago'
import en from 'javascript-time-ago/locale/en'
TimeAgo.addDefaultLocale(en)
export default function MediumVideo(props) {
    useAxiosWithToken();
    const navigate = useNavigate()
    const videoId = props.id;
    const playlistId = props.playlistId;
    const video = props.video;

    const timeAgo = new TimeAgo('en-US')
    return (
        <>
            {
                !props.isPlaylist
                    ?
                    <div className="video-preview" onClick={() => navigate(`/watch/${videoId}`)}>
                        <div className="thumbnail-row">
                            <img className="thumbnail" src={video.previewUrl + `?timestamp=${Date.now()}`}></img>
                            <div className="video-time">{video.duration}</div>
                        </div>
                        <div className="video-info-grid">
                            <div className="channel-picture">
                                <img className="profile-picture" src={video.channelLogo  + `?timestamp=${Date.now()}`}></img>
                            </div>
                            <div className="video-info">
                                <p className="video-title">
                                    {video.title}
                                </p>
                                <p className="video-author" onClick={(e) => {e.stopPropagation(); navigate(`/channel/${video.channelId}`)}}> 
                                    {video.channelTitle}
                                </p>
                                <p className="video-stats">
                                    {video.numberOfViews} views &#183; {timeAgo.format(new Date(video.publicationDate))}
                                </p>
                            </div>
                        </div>
                    </div>
                    :
                    <div className="playlist-preview" onClick={() => navigate(`/watch/${videoId}/${playlistId}/${props?.showPrivate ? true : false}`)}>
                        <div className="playlist-thumbnail">
                            <img src={video.previewUrl + `?timestamp=${Date.now()}`} alt="Playlist Thumbnail"></img>
                            <div className="video-count">{video.videoCount} videos</div>
                        </div>

                        <div className="playlist-details">
                            <p className="playlist-title">{video.title}</p>
                            <p className="playlist-meta">{video.channelTitle} · Updated {timeAgo.format(new Date(video.lastUpdated))}</p>
                            <a href="#" className="playlist-link">View full playlist</a>
                        </div>
                    </div>
            }

        </>
    );
}

import './PlaylistVideoRow.css';
import { useEffect, useState } from 'react';
import { axiosWithToken } from "../../API/axioscfg"
import { partial } from "filesize";
import useAuth from "../../hooks/useAuth";
import { useNavigate } from 'react-router-dom';
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import privatepng from "../../resources/private.png"
import publicpng from "../../resources/public.png"
import ConfirmationWindow from '../ConfirmationWindow/ConfirmationWindow';
export default function PlaylistVideoRow(props) {
    const [isDeleting, setIsDeleting] = useState(false);
    useAxiosWithToken();
    const navigate = useNavigate()
    const size = partial({ standard: "jedec" });
    const videos = props.videos;
    const setVideos = props.setVideos;
    const videoId = props.id;
    const playlistId = props.playlistId;

    async function handleDelete() {
        try {
            const response = await axiosWithToken.delete(
                `/api/playlist/removevideofromplaylist/${playlistId}/${videoId}`,
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setVideos(prevVideos => prevVideos.filter(video => video.id !== videoId));
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else {
                console.error('Error during changing visibility:', error.response || error.message);
            }
        }
    }

    return (
        <>
            {
                isDeleting
                    ?
                    <ConfirmationWindow message="Are you sure you want to delete the video from the playlist?" onCancel={() => setIsDeleting(false)} onConfirm={handleDelete}></ConfirmationWindow>
                    :
                    <></>
            }

            <li className="table-row">
                <div className="col col-3">
                    <img className='previewImg' src={props.video.previewUrl + `?timestamp=${Date.now()}`} alt="" />
                </div>
                <div className="col col-4">{
                    props.video.isPublic ?
                        <img className='visibilityImg' src={publicpng} alt=''></img>
                        :
                        <img className='visibilityImg' src={privatepng} alt=''></img>
                }</div>
                <div className="col col-2 textalgn">{props.video.title}</div>
                <div className="col col-2">{props.video.publicationDate}</div>
                <div className="col col-1">{props.video.format}</div>
                <div className="col col-1">{props.video.numberOfViews}</div>
                <div className="col col-1">{props.video.numberOfComments}</div>
                <div className="col col-1">{props.video.numberOfLikes}/{props.video.numberOfDisLikes}</div>
                <div className="col col-1">{props.video.duration}</div>
                <div className="col col-1">{size(props.video.size)}</div>
                <div className="col col-1">
                    <button className='removeBtn' onClick={() => setIsDeleting(true)}></button>
                </div>
            </li>
        </>

    );
}

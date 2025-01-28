import './VideoRow.css';
import { useEffect, useState } from 'react';
import { axiosWithToken } from "../../API/axioscfg"
import { partial } from "filesize";
import useAuth from "../../hooks/useAuth";
import { useNavigate } from 'react-router-dom';
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import privatepng from "../../resources/private.png"
import publicpng from "../../resources/public.png"
import ConfirmationWindow from '../ConfirmationWindow/ConfirmationWindow';
export default function VideoRow(props) {
    const [isDeleting, setIsDeleting] = useState(false);
    useAxiosWithToken();
    const navigate = useNavigate()
    const size = partial({ standard: "jedec" });
    const videos = props.videos;
    const setVideos = props.setVideos;
    const videoId = props.id;

    async function handleChangeVisibility() {
        try {
            const response = await axiosWithToken.patch(
                `/api/content/changevisibility/${videoId}`,
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setVideos(videos.map(video =>
                    video.id === videoId
                        ? { ...video, isPublic: response.data }
                        : video
                ))
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

    async function handleDelete() {
        try {
            const response = await axiosWithToken.delete(
                `/api/content/deletevideo/${videoId}`,
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
                    <ConfirmationWindow message="Are you sure you want to delete the video with all associated comments and reactions?" onCancel={() => setIsDeleting(false)} onConfirm={handleDelete}></ConfirmationWindow>
                    :
                    <></>
            }

            <li className="table-row">
                <div className="col col-3">
                    <img className='previewImg' src={props.video.previewUrl + `?timestamp=${Date.now()}`} alt="" />
                </div>
                <div className="col col-4">{
                    props.video.isPublic ?
                        <img className='visibilityImg' src={publicpng} alt='' onClick={handleChangeVisibility}></img>
                        :
                        <img className='visibilityImg' src={privatepng} alt='' onClick={handleChangeVisibility}></img>
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
                    <div className='functionalBtns'>
                        <button className='removeBtn' onClick={() => setIsDeleting(true)}></button>
                        <button className='editBtn' onClick={() => navigate(`/channelmanagement/video/${videoId}/edit`)}></button>
                        <button className='statsBtn' onClick={() => navigate(`/channelmanagement/video/${videoId}/statistics`)}></button>
                    </div>

                </div>
            </li>
        </>

    );
}

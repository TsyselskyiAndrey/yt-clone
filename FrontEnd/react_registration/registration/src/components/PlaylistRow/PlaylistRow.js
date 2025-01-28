import './PlaylistRow.css';
import { useEffect, useState } from 'react';
import { axiosWithToken } from "../../API/axioscfg"
import { useNavigate } from 'react-router-dom';
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import privatepng from "../../resources/private.png"
import publicpng from "../../resources/public.png"
import ConfirmationWindow from '../ConfirmationWindow/ConfirmationWindow';
export default function PlaylistRow(props) {
  const [isDeleting, setIsDeleting] = useState(false);
  useAxiosWithToken();
  const navigate = useNavigate()
  const playlists = props.playlists;
  const setPlaylists = props.setPlaylists;
  const playlistId = props.id;

  async function handleChangeVisibility() {
    try {
      const response = await axiosWithToken.patch(
        `/api/playlist/changevisibility/${playlistId}`,
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setPlaylists(playlists.map(playlist =>
          playlist.id === playlistId
            ? { ...playlist, isPublic: response.data }
            : playlist
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
        `/api/playlist/deleteplaylist/${playlistId}`,
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setPlaylists(prevPlaylists => prevPlaylists.filter(playlist => playlist.id !== playlistId));
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
          <ConfirmationWindow message="Are you sure you want to delete the playlist? You will not be able to restore the data!" onCancel={() => setIsDeleting(false)} onConfirm={handleDelete}></ConfirmationWindow>
          :
          <></>
      }
      <li className="table-row">
        <div className="col col-6">
          <img className='previewImg' src={props.playlist.previewUrl + `?timestamp=${Date.now()}`} alt="" />
        </div>
        <div className="col col-5">{
          props.playlist.isPublic ?
            <img className='visibilityImg' src={publicpng} alt='' onClick={handleChangeVisibility}></img>
            :
            <img className='visibilityImg' src={privatepng} alt='' onClick={handleChangeVisibility}></img>
        }</div>
        <div className="col col-7 textalgn">{props.playlist.title}</div>
        <div className="col col-7">{props.playlist.publicationDate}</div>
        <div className="col col-7">{props.playlist.lastUpdated}</div>
        <div className="col col-8">{props.playlist.videoCount}</div>
        <div className="col col-6">
          <div className='functionalBtns'>
            <button className='removeBtn' onClick={() => setIsDeleting(true)}></button>
            <button className='editBtn' onClick={() => navigate(`/channelmanagement/playlist/${playlistId}/edit`)}></button>
          </div>
        </div>
      </li>

    </>

  );
}

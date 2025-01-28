import './Playlists.css';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import MediumVideo from '../../components/MediumVideo/MediumVideo';
export default function Playlists() {
  useAxiosWithToken();
  const [homePlaylists, setHomePlaylists] = useState();

  useEffect(() => {
    GetHomePlaylists();
  }, [])

  async function GetHomePlaylists() {
    try {
      const response = await axiosWithToken.get(
        `/api/media/homeplaylists`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setHomePlaylists(response.data)
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

  return (
    <>
      <div className='videosContainer'>
        <section className='video-grid'>
          
          {
            homePlaylists
            &&
            Object.keys(homePlaylists).map((videoIdx) => {
              return (
                <MediumVideo key={homePlaylists[videoIdx].id + (homePlaylists[videoIdx].isPlaylist ? ("_playlist_" + homePlaylists[videoIdx].playlistId) : "_video")} 
                video={homePlaylists[videoIdx]} 
                id={homePlaylists[videoIdx].id} 
                isPlaylist={homePlaylists[videoIdx].isPlaylist}
                playlistId={homePlaylists[videoIdx].playlistId}
                ></MediumVideo>
              )
            })
          }
        </section>

      </div>
    </>
  );
}

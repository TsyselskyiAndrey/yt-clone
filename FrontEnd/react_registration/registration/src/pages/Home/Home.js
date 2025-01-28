import './Home.css';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import MediumVideo from '../../components/MediumVideo/MediumVideo';
export default function Home() {
  useAxiosWithToken();
  const [homeVideos, setHomeVideos] = useState();

  useEffect(() => {
    GetHomeVideos();
  }, [])

  async function GetHomeVideos() {
    try {
      const response = await axiosWithToken.get(
        `/api/media/homevideos`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setHomeVideos(response.data)
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
            homeVideos
            &&
            Object.keys(homeVideos).map((videoIdx) => {
              return (
                <MediumVideo key={homeVideos[videoIdx].id + (homeVideos[videoIdx].isPlaylist ? ("_playlist_" + homeVideos[videoIdx]?.playlistId) : "_video")} 
                video={homeVideos[videoIdx]} 
                id={homeVideos[videoIdx].id} 
                isPlaylist={homeVideos[videoIdx].isPlaylist}
                playlistId={homeVideos[videoIdx].playlistId}
                ></MediumVideo>
              )
            })
          }
        </section>

      </div>
    </>
  );
}

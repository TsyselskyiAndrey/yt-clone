import './Subscriptions.css';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import MediumVideo from '../../components/MediumVideo/MediumVideo';
export default function Subscriptions() {
  useAxiosWithToken();
  const [subVideos, setSubVideos] = useState();
  const navigate = useNavigate();
  useEffect(() => {
    GetSubVideos();
  }, [])

  async function GetSubVideos() {
    try {
      const response = await axiosWithToken.get(
        `/api/media/subscriptionvideos`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setSubVideos(response.data)
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
        <div className='subscriptionHeader'>
          <h2>Latest</h2>
          <div className="actionDiv" onClick={() => {navigate("/managesubs")}}>
            Manage subscriptions
          </div>
        </div>
        <section className='video-grid'>

          {
            subVideos
            &&
            Object.keys(subVideos).map((videoIdx) => {
              return (
                <MediumVideo key={subVideos[videoIdx].id + (subVideos[videoIdx].isPlaylist ? ("_playlist_" + subVideos[videoIdx].playlistId) : "_video")}
                  video={subVideos[videoIdx]}
                  id={subVideos[videoIdx].id}
                  isPlaylist={subVideos[videoIdx].isPlaylist}
                  playlistId={subVideos[videoIdx].playlistId}
                ></MediumVideo>
              )
            })
          }
        </section>

      </div>
    </>
  )
}

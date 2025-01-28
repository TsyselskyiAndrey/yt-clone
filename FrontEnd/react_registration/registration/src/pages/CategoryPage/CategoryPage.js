import './CategoryPage.css';
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import MediumVideo from '../../components/MediumVideo/MediumVideo';
export default function CategoryPage() {
  const {categoryId} = useParams();
  const navigate = useNavigate();
  useAxiosWithToken();
  const [categoryVideos, setCategoryVideos] = useState();

  useEffect(() => {
    GetCategoryVideos();
  }, [])

  async function GetCategoryVideos() {
    try {
      const response = await axiosWithToken.get(
        `/api/media/categoryvideos/${categoryId}`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setCategoryVideos(response.data)
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
            categoryVideos
            &&
            Object.keys(categoryVideos).map((videoIdx) => {
              return (
                <MediumVideo key={categoryVideos[videoIdx].id + (categoryVideos[videoIdx].isPlaylist ? "_playlist" : "_video")} 
                video={categoryVideos[videoIdx]} 
                id={categoryVideos[videoIdx].id} 
                isPlaylist={categoryVideos[videoIdx].isPlaylist}
                playlistId={categoryVideos[videoIdx].playlistId}
                ></MediumVideo>
              )
            })
          }
        </section>

      </div>
    </>
  );
}

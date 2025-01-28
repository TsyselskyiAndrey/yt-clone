import './Favorite.css';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import TimeAgo from 'javascript-time-ago'
import sort from "../../components/resources/icons/sorticon.svg"
export default function Favorite() {
  useAxiosWithToken();
  const [favoritevideos, setFavoriteVideos] = useState();
  const navigate = useNavigate();
  const [isSortOpen, setIsSortOpen] = useState(false);
  const [sortOption, setSortOption] = useState("Newest");
  const timeAgo = new TimeAgo('en-US')
  useEffect(() => {
    GetFavoriteVideos();
  }, [sortOption])

  async function GetFavoriteVideos() {
    try {
      const response = await axiosWithToken.get(
        `/api/media/favoritevideos/${sortOption}`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setFavoriteVideos(response.data)
      }
    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else {
        console.error('Error during retrieving favorite videos:', error.response?.data || error.message);
      }
    }
  }

  async function handleDeleteFavoriteVideo(e, videoId) {
    e.stopPropagation()
    try {
      const response = await axiosWithToken.patch(
        `/api/media/removefromfavorite/${videoId}`,
        {},
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setFavoriteVideos(prevVideos => prevVideos.filter(video => video.id !== videoId));
      }
    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else {
        console.error('Error during removing from the favorite:', error.response?.data || error.message);
      }
    }
  }

  function handleSortOption(option) {
    setSortOption(option);
    setIsSortOpen(false);
  }

  return (
    <>
      <div className='centeredMain'>
        <div className='centeredMainContentContainer gap20'>
          <h1>Favorite videos</h1>
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
          {
            favoritevideos
            &&
            Object.keys(favoritevideos).map((videoIdx) => {
              return (
                <div className="video-preview-big" key={favoritevideos[videoIdx]?.id} onClick={() => navigate(`/watch/${favoritevideos[videoIdx]?.id}`)}>
                  <div className="thumbnail-row-big ">
                    <img className="thumbnail-big" src={favoritevideos[videoIdx]?.previewUrl + `?timestamp=${Date.now()}`}></img>
                    <div className="video-time-big">{favoritevideos[videoIdx]?.duration}</div>
                  </div>
                  <div className="video-info-big">
                    <p className="video-title-big">
                      {favoritevideos[videoIdx]?.title}
                    </p>
                    <p className="video-author-big">
                      <span className='author-big' onClick={(e) => {e.stopPropagation(); navigate(`/channel/${favoritevideos[videoIdx]?.channelId}`)}}>{favoritevideos[videoIdx]?.channelTitle}</span> &#183; {favoritevideos[videoIdx]?.numberOfViews} views &#183; published {timeAgo.format(new Date(favoritevideos[videoIdx]?.publicationDate))}
                    </p>
                    <p className="video-stats-big">
                      {favoritevideos[videoIdx]?.description}
                    </p>
                  </div>
                  <div className="btn-video-big">
                    <button className='removeBtn' onClick={(e) => handleDeleteFavoriteVideo(e, favoritevideos[videoIdx]?.id)}></button>
                  </div>

                </div>
              );
            })
          }



        </div>
      </div>
    </>
  )
}

import './Watchlater.css';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import TimeAgo from 'javascript-time-ago'
import sort from "../../components/resources/icons/sorticon.svg"
export default function Watchlater() {
  useAxiosWithToken();
  const [watchlatervideos, setWatchlaterVideos] = useState();
  const navigate = useNavigate();
  const [isSortOpen, setIsSortOpen] = useState(false);
  const [sortOption, setSortOption] = useState("Newest");
  const timeAgo = new TimeAgo('en-US')
  useEffect(() => {
    GetWatchlaterVideos();
  }, [sortOption])

  async function GetWatchlaterVideos() {
    try {
      const response = await axiosWithToken.get(
        `/api/media/watchlatervideos/${sortOption}`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setWatchlaterVideos(response.data)
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

  async function handleDeleteWatchlaterVideo(e, videoId) {
    e.stopPropagation()
    try {
      const response = await axiosWithToken.patch(
        `/api/media/removefromwatchlater/${videoId}`,
        {},
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setWatchlaterVideos(prevVideos => prevVideos.filter(video => video.id !== videoId));
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
          <h1>Watch later videos</h1>
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
            watchlatervideos
            &&
            Object.keys(watchlatervideos).map((videoIdx) => {
              return (
                <div className="video-preview-big" key={watchlatervideos[videoIdx]?.id} onClick={() => navigate(`/watch/${watchlatervideos[videoIdx]?.id}`)}>
                  <div className="thumbnail-row-big ">
                    <img className="thumbnail-big" src={watchlatervideos[videoIdx]?.previewUrl + `?timestamp=${Date.now()}`}></img>
                    <div className="video-time-big">{watchlatervideos[videoIdx]?.duration}</div>
                  </div>
                  <div className="video-info-big">
                    <p className="video-title-big">
                      {watchlatervideos[videoIdx]?.title}
                    </p>
                    <p className="video-author-big">
                      <span className='author-big' onClick={(e) => {e.stopPropagation(); navigate(`/channel/${watchlatervideos[videoIdx]?.channelId}`)}}>{watchlatervideos[videoIdx]?.channelTitle}</span> &#183; {watchlatervideos[videoIdx]?.numberOfViews} views &#183; published {timeAgo.format(new Date(watchlatervideos[videoIdx]?.publicationDate))}
                    </p>
                    <p className="video-stats-big">
                      {watchlatervideos[videoIdx]?.description}
                    </p>
                  </div>
                  <div className="btn-video-big">
                    <button className='removeBtn' onClick={(e) => handleDeleteWatchlaterVideo(e, watchlatervideos[videoIdx]?.id)}></button>
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

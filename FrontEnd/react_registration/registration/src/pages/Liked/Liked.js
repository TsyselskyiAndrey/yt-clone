import './Liked.css';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import TimeAgo from 'javascript-time-ago'
import sort from "../../components/resources/icons/sorticon.svg"
export default function Liked() {
  useAxiosWithToken();
  const [likedvideos, setLikedVideos] = useState();
  const navigate = useNavigate();
  const [isSortOpen, setIsSortOpen] = useState(false);
  const [sortOption, setSortOption] = useState("Newest");
  const timeAgo = new TimeAgo('en-US')
  useEffect(() => {
    GetLikedVideos();
  }, [sortOption])

  async function GetLikedVideos() {
    try {
      const response = await axiosWithToken.get(
        `/api/media/likedvideos/${sortOption}`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setLikedVideos(response.data)
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

  function handleSortOption(option) {
    setSortOption(option);
    setIsSortOpen(false);
  }

  return (
    <>
      <div className='centeredMain'>
        <div className='centeredMainContentContainer gap20'>
          <h1>Liked videos</h1>
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
            likedvideos
            &&
            Object.keys(likedvideos).map((videoIdx) => {
              return (
                <div className="video-preview-big" key={likedvideos[videoIdx]?.id} onClick={() => navigate(`/watch/${likedvideos[videoIdx]?.id}`)}>
                  <div className="thumbnail-row-big ">
                    <img className="thumbnail-big" src={likedvideos[videoIdx]?.previewUrl + `?timestamp=${Date.now()}`}></img>
                    <div className="video-time-big">{likedvideos[videoIdx]?.duration}</div>
                  </div>
                  <div className="video-info-big">
                    <p className="video-title-big">
                      {likedvideos[videoIdx]?.title}
                    </p>
                    <p className="video-author-big">
                      <span className='author-big' onClick={(e) => {e.stopPropagation(); navigate(`/channel/${likedvideos[videoIdx]?.channelId}`)}}>{likedvideos[videoIdx]?.channelTitle}</span> &#183; {likedvideos[videoIdx]?.numberOfViews} views &#183; published {timeAgo.format(new Date(likedvideos[videoIdx]?.publicationDate))}
                    </p>
                    <p className="video-stats-big">
                      {likedvideos[videoIdx]?.description}
                    </p>
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

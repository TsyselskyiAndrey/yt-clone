import './History.css';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import TimeAgo from 'javascript-time-ago'
export default function History() {
  useAxiosWithToken();
  const [historyvideos, setHistoryVideos] = useState();
  const navigate = useNavigate();
  const timeAgo = new TimeAgo('en-US')
  useEffect(() => {
    GetHistoryVideos();
  }, [])

  async function GetHistoryVideos() {
    try {
      const response = await axiosWithToken.get(
        `/api/media/historyvideos`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setHistoryVideos(response.data)
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
      <div className='centeredMain'>
        <div className='centeredMainContentContainer gap20'>
          <h1>History</h1>

          {
            historyvideos
            &&
            Object.keys(historyvideos).map((videoIdx) => {
              return (
                <div className="video-preview-big" key={historyvideos[videoIdx]?.id} onClick={() => navigate(`/watch/${historyvideos[videoIdx]?.id}`)}>
                  <div className="thumbnail-row-big ">
                    <img className="thumbnail-big" src={historyvideos[videoIdx]?.previewUrl + `?timestamp=${Date.now()}`}></img>
                    <div className="video-time-big">{historyvideos[videoIdx]?.duration}</div>
                  </div>
                  <div className="video-info-big">
                    <p className="video-title-big">
                      {historyvideos[videoIdx]?.title}
                    </p>
                    <p className="video-author-big">
                      <span className='author-big' onClick={(e) => {e.stopPropagation(); navigate(`/channel/${historyvideos[videoIdx]?.channelId}`)}}>{historyvideos[videoIdx]?.channelTitle}</span> &#183; {historyvideos[videoIdx]?.numberOfViews} views &#183; watched {timeAgo.format(new Date(historyvideos[videoIdx]?.watchedAt))}
                    </p>
                    <p className="video-stats-big">
                      {historyvideos[videoIdx]?.description}
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

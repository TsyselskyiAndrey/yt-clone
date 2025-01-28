import React, { useState, useEffect } from 'react';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import "./VideoStatistics.css"
import { useParams } from 'react-router-dom';
export default function VideoStatistics() {
  const { videoId } = useParams()
  useAxiosWithToken();
  const [selectedPeriod, setSelectedPeriod] = useState('all');
  const [generalStats, setGeneralStats] = useState();
  const [periodicStats, setPeriodicStats] = useState();

  useEffect(() => {
    GetGeneralStats();
  }, [])

  useEffect(() => {
    GetPeriodicStats();
  }, [selectedPeriod])

  async function GetGeneralStats() {
    try {
      const response = await axiosWithToken.get(
        `/api/content/generalvideostats/${videoId}`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setGeneralStats(response.data)
      }
    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else {
        console.error('Error during retrieving video:', error.response?.data || error.message);
      }
    }
  }

  async function GetPeriodicStats() {
    try {
      const response = await axiosWithToken.get(
        `/api/content/periodicvideostats/${videoId}/${selectedPeriod}`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setPeriodicStats(response.data)
      }
    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else {
        console.error('Error during retrieving video:', error.response?.data || error.message);
      }
    }
  }
  const handlePeriodSelect = (period) => {
    setSelectedPeriod(period);
  };
  async function handleReport() {
    try {
      const response = await axiosWithToken.get(`/api/content/videocommentsreport/${videoId}`, {
        responseType: 'blob',
        withCredentials: true
      });

      if (response.status === 200) {
        const blob = new Blob([response.data], { type: 'application/pdf' });
        const url = window.URL.createObjectURL(blob);
        window.open(url, '_blank');
        setTimeout(() => window.URL.revokeObjectURL(url), 1000);
      }
    } catch (error) {
      if (!error?.response) {
        console.error("No Server Response");
      } else {
        console.error('Error during retrieving video:', error.response?.data || error.message);
      }
    }
  }
  return (
    <>
      <div className='editForm'>
        <div className='editContainer centerDiv'>
          <div className="dataBlockStatistics">
            <h2>General Statistics</h2>
            <div className="standardInfoBlock statisticsStandardBlock">
              <div className="statisticsContentBlock">
                <div className="statsItem">
                  <span className="statsTitle">The number of all replies:</span>
                  <span className="statsValue">{generalStats?.numberOfReplies}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of unique views:</span>
                  <span className="statsValue">{generalStats?.uniqueViews}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of comment likes:</span>
                  <span className="statsValue">{generalStats?.totalLikes}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of comment dislikes:</span>
                  <span className="statsValue">{generalStats?.totalDislikes} </span>
                </div>
              </div>

            </div>

            <h2>Periodic Statistics</h2>
            <div className="standardInfoBlock statisticsStandardBlock">
              <div className="navDivs prevent-select nomargin">
                <div
                  className={`navDiv ${selectedPeriod === 'all' ? 'selected' : ''}`}
                  onClick={() => handlePeriodSelect('all')}
                >
                  All time
                </div>
                <div
                  className={`navDiv ${selectedPeriod === 'year' ? 'selected' : ''}`}
                  onClick={() => handlePeriodSelect('year')}
                >
                  One year
                </div>
                <div
                  className={`navDiv ${selectedPeriod === 'month' ? 'selected' : ''}`}
                  onClick={() => handlePeriodSelect('month')}
                >
                  One month
                </div>
                <div
                  className={`navDiv ${selectedPeriod === 'day' ? 'selected' : ''}`}
                  onClick={() => handlePeriodSelect('day')}
                >
                  One day
                </div>
              </div>

              <div className="statisticsContentBlock">
                <div className="statsItem">
                  <span className="statsTitle">The number of comments:</span>
                  <span className="statsValue">{periodicStats?.numberOfComments}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of views:</span>
                  <span className="statsValue">{periodicStats?.numberOfViews}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of unique views:</span>
                  <span className="statsValue">{periodicStats?.uniqueViews}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of all replies:</span>
                  <span className="statsValue">{periodicStats?.totalReplies}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of viewers without a subscription:</span>
                  <span className="statsValue">{periodicStats?.viewersWithoutSubscription}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of new viewers this video attracted:</span>
                  <span className="statsValue">{periodicStats?.newViewersForVideo}</span>
                </div>
              </div>
              <br />
              <h3>Report</h3>
              <button className='submitBtn reportBtn' onClick={handleReport}>Get video report!</button>
            </div>
          </div>
        </div>
      </div>


    </>
  );
}


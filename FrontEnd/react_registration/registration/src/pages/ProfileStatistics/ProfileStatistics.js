import React, { useState, useEffect } from 'react';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import "./ProfileStatistics.css"
export default function ProfileStatistics() {
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
        `/api/media/generalprofilestats`,
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
        `/api/media/periodicprofilestats/${selectedPeriod}`,
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
  return (
    <>
      <div className='editForm'>
        <div className='editContainer centerDiv'>
          <div className="dataBlockStatistics">
            <h2>General Statistics</h2>
            <div className="standardInfoBlock statisticsStandardBlock">
              <div className="statisticsContentBlock">
                <div className="statsItem">
                  <span className="statsTitle">The most watched video:</span>
                  <span className="statsValue">"{generalStats?.mostWatchedVideo}"</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of liked/disliked comments:</span>
                  <span className="statsValue">{generalStats?.numberOfLikedComments}/{generalStats?.numberOfDislikedComments} </span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of liked/disliked videos:</span>
                  <span className="statsValue">{generalStats?.numberOfLikedVideos}/{generalStats?.numberOfDislikedVideos}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of "Watch later" videos:</span>
                  <span className="statsValue">{generalStats?.numberOfWatchLaterVideos} </span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of favorite videos:</span>
                  <span className="statsValue">{generalStats?.numberOfFavoriteVideos} </span>
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
                  <span className="statsTitle">The number of the new channels you've subscibed to:</span>
                  <span className="statsValue">{periodicStats?.numberOfFollowedChannels}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The most watched channel:</span>
                  <span className="statsValue">"{periodicStats?.favoriteChannel}"</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">Number of video views:</span>
                  <span className="statsValue">{periodicStats?.numberOfViews}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">Number of watched videos:</span>
                  <span className="statsValue">{periodicStats?.numberOfWathedVideos}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>


    </>
  );
}


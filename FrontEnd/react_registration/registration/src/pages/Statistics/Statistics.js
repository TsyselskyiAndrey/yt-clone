import React, { useState, useEffect } from 'react';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import "./Statistics.css"
export default function Statistics() {
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
        `/api/channel/generalchannelstats`,
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
        `/api/channel/periodicchannelstats/${selectedPeriod}`,
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
                  <span className="statsTitle">The video with the highest rating:</span>
                  <span className="statsValue">"{generalStats?.highestRatingVideo}"</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of likes/dislikes:</span>
                  <span className="statsValue">{generalStats?.numberOfLikes}/{generalStats?.numberOfDislikes} </span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The avarage number of likes/dislikes:</span>
                  <span className="statsValue">{generalStats?.avgNumberOfLikes}/{generalStats?.avgNumberOfDislikes} </span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The max number of likes/dislikes:</span>
                  <span className="statsValue">{generalStats?.maxNumberOfLikes}/{generalStats?.maxNumberOfDislikes} </span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The most frequently used tag:</span>
                  <span className="statsValue">{generalStats?.mostPopularTag}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The most frequently used category:</span>
                  <span className="statsValue">{generalStats?.mostPopularCategory}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The most popular video:</span>
                  <span className="statsValue">"{generalStats?.mostPopularVideo}"</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The video with the most likes:</span>
                  <span className="statsValue">"{generalStats?.mostLikedVideo}" </span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of regular viewers:</span>
                  <span className="statsValue">{generalStats?.persistentViewers} </span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The video with the most dislikes:</span>
                  <span className="statsValue">"{generalStats?.mostDislikedVideo}" </span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The most liked comment:</span>
                  <span className="statsValue">"{generalStats?.mostLikedComment}" </span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of replies in the longest discussion:</span>
                  <span className="statsValue">{generalStats?.numberOfReplies} </span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The most discussed video:</span>
                  <span className="statsValue">"{generalStats?.mostDiscussedVideo}" </span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of the videos added to playlists:</span>
                  <span className="statsValue">{generalStats?.numberOfVideoAddedToPlaylists} </span>
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
                  <span className="statsTitle">The number of viewers without a subscription:</span>
                  <span className="statsValue">{periodicStats?.viewersWithoutSubscription}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of views:</span>
                  <span className="statsValue">{periodicStats?.views}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The avarage number views:</span>
                  <span className="statsValue">{periodicStats?.avgViews}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of comments:</span>
                  <span className="statsValue">{periodicStats?.comments}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The avarage number of comments:</span>
                  <span className="statsValue">{periodicStats?.avgComments}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The max number of comments:</span>
                  <span className="statsValue">{periodicStats?.maxComments}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of subscribers:</span>
                  <span className="statsValue">{periodicStats?.subscribers}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of new viewers:</span>
                  <span className="statsValue">{periodicStats?.newViewers}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of replies:</span>
                  <span className="statsValue">{periodicStats?.numberOfReplies}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number my comments:</span>
                  <span className="statsValue">{periodicStats?.myComments}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of unique views:</span>
                  <span className="statsValue">{periodicStats?.uniqueViews}</span>
                </div>
                <div className="statsItem">
                  <span className="statsTitle">The number of unique viewers:</span>
                  <span className="statsValue">{periodicStats?.uniqueViewers}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>


    </>
  );
}


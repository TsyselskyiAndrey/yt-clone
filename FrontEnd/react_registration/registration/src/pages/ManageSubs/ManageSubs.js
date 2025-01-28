import './ManageSubs.css';
import bell from "../../components/resources/icons/bell.svg"
import bell_crossed from "../../components/resources/icons/bell-crossed.svg"
import sort from "../../components/resources/icons/sorticon.svg"
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
export default function ManageSubs() {
  useAxiosWithToken();
  const [channels, setChannels] = useState();
  const navigate = useNavigate();
  const [isSortOpen, setIsSortOpen] = useState(false);
  const [sortOption, setSortOption] = useState("A-Z")
  useEffect(() => {
    GetSubChannels();
  }, [sortOption])

  async function handleSubscribe(e, channelId) {
    e.stopPropagation()
    try {
      const response = await axiosWithToken.post(
        `/api/media/subscribe/${channelId}`,
        null,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setChannels(prevChannels =>
          prevChannels.map(channel =>
            channel.id === channelId
              ? { ...channel,
                 isSubscribed: response.data,
                 receiveNotifications: response.data ? false : true,
                 numberOfSubscribers: (channel?.numberOfSubscribers || 0) + (channel.isSubscribed ? -1 : 1) 
                } 
              : channel
          )
        );
      }
    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else {
        console.error('Error during subscribing:', error.response?.data || error.message);
      }
    }
  }

  async function handleNotifications(e, channelId) {
    e.stopPropagation()
    try {
      const response = await axiosWithToken.patch(
        `/api/media/receivenotifications/${channelId}`,
        {},
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setChannels(prevChannels =>
          prevChannels.map(channel =>
            channel.id === channelId
              ? { ...channel, receiveNotifications: response.data }
              : channel
          )
        );
      }
    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else {
        console.error('Error during managing notifications:', error.response?.data || error.message);
      }
    }
  }

  async function GetSubChannels() {
    try {
      const response = await axiosWithToken.get(
        `/api/media/subscriptionchannels/${sortOption}`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setChannels(response.data)
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

  function handleSortOption(option) {
    setSortOption(option);
    setIsSortOpen(false);
  }
  return (
    <div className='centeredMain'>
      <div className='centeredMainContentContainer'>
        <h1>All subscriptions</h1>
        <div className={'prevent-select sortBlock ' + (isSortOpen ? "sortopen" : "")} onClick={() => setIsSortOpen(!isSortOpen)}>
          <img src={sort} alt="" />
          Sort by
          <div className="sortOptions">
            {["Most popular", "Most unknown",  "New activity", "Most inactive", "A-Z", "Z-A"].map(option => (
              <div
                key={option}
                className={`sortOption ${sortOption === option ? "chosenOption" : ""}`}
                onClick={() => handleSortOption(option)}
              >
                {option} channels
              </div>
            ))}
          </div>
        </div>
        {
          channels
          &&
          Object.keys(channels).map((channelIdx) => {
            return (
              <div className="bigChannelContainer" key={channels[channelIdx]?.id} onClick={(e) => {e.stopPropagation(); navigate(`/channel/${channels[channelIdx]?.id}`)}}>
                <img className='bigChannelLogo' src={channels[channelIdx]?.logoUrl + `?timestamp=${Date.now()}`} alt="" />
                <div className="bigChannelInfo">
                  <p className="bigChannelTitle">
                    {channels[channelIdx]?.title}
                  </p>
                  <p className="channelStats">
                    @{channels[channelIdx]?.handle} &#183; {channels[channelIdx]?.numberOfSubscribers} subscribers
                  </p>
                  <p className="channelStats">
                    {channels[channelIdx]?.description}
                  </p>
                </div>
                <div className='channelBtns'>
                  <button className={'submitBtn subscribeBtn ' + (channels[channelIdx]?.isSubscribed ? "subscribed" : "")} onClick={(e) => handleSubscribe(e, channels[channelIdx]?.id)}>{channels[channelIdx]?.isSubscribed ? "Subscribed" : "Subscribe"}</button>
                  {
                    channels[channelIdx]?.isSubscribed
                    &&
                    <img className='bellImg' src={channels[channelIdx]?.receiveNotifications ? bell : bell_crossed} onClick={(e) => handleNotifications(e, channels[channelIdx]?.id)} alt="" />
                  }
                </div>
              </div>
            );
          })
        }



      </div>
    </div>
  );
}

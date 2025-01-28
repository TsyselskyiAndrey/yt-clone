import './ChannelPlaylists.css';
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import TimeAgo from 'javascript-time-ago'
import sort from "../../components/resources/icons/sorticon.svg"
import filter from "../../resources/filter_black.png"
import cross from "../../resources/cross_black.png"
import MediumVideo from '../../components/MediumVideo/MediumVideo';
import useCategories from '../../hooks/useCategories';
import qs from 'qs'
import useAuth from '../../hooks/useAuth';
export default function ChannelPlaylists() {
  useAxiosWithToken();
  const [channelPlaylists, setChannelPlaylists] = useState();
  const navigate = useNavigate();
  const { channelId } = useParams();
  const [isSortOpen, setIsSortOpen] = useState(false);
  const [sortOption, setSortOption] = useState("Newest");
  const timeAgo = new TimeAgo('en-US')
  const [isFilterOpen, setIsFilterOpen] = useState(false);
  const { categories } = useCategories();
  const { auth } = useAuth();
  const [filters, setFilters] = useState({
    publicationDate: '',
    lastUpdated: [],
    videoCount: '',
    visibility: ''
  })
  useEffect(() => {
    GetChannelPlaylists();
  }, [sortOption, filters, channelId])

  async function GetChannelPlaylists() {
    try {
      const response = await axiosWithToken.get(
        `/api/media/channelplaylists/${channelId}/${sortOption}`,
        {
          withCredentials: true,
          params: filters,
          paramsSerializer: params => {
            return qs.stringify(params)
          }
        }
      );
      if (response.status === 200) {
        setChannelPlaylists(response.data)
      }
    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else {
        console.error('Error during retrieving video content:', error.response?.data || error.message);
      }
    }
  }


  function handleSortOption(option) {
    setSortOption(option);
    setIsSortOpen(false);
  }
  function handleFilterSelect(key, value) {
    if (key == "videoFormat") {
      setFilters(prevFilters => {
        const currentArray = prevFilters[key];
        if (currentArray.includes(value)) {
          return {
            ...prevFilters,
            [key]: currentArray.filter(item => item !== value),
          };
        } else {
          return {
            ...prevFilters,
            [key]: [...currentArray, value],
          };
        }
      });
    }
    else {
      setFilters(prevFilters => {
        if (value === prevFilters[key]) {
          return {
            ...prevFilters,
            [key]: "",
          }
        }
        else {
          return {
            ...prevFilters,
            [key]: value,
          }
        }
      });
    }

  }
  return (
    <>
      <div className="sortFilterBlock">
        <div className={'prevent-select sortBlock ' + (isSortOpen ? "sortopen" : "")} onClick={() => setIsSortOpen(!isSortOpen)}>
          <img src={sort} alt="" />
          Sort by
          <div className="sortOptions">
            {["A-Z", "Z-A", "Longest", "Shortest", "Recently updated", "Long untouched", "Newest", "Oldest"].map(option => (
              <div
                key={option}
                className={`sortOption ${sortOption === option ? "chosenOption" : ""}`}
                onClick={() => handleSortOption(option)}
              >
                {option} playlists first
              </div>
            ))}
          </div>
        </div>
        <img className='filterImg' src={isFilterOpen ? cross : filter} alt="" onClick={() => setIsFilterOpen(!isFilterOpen)} />
        <div className={"filterContainerChannel prevent-select " + (isFilterOpen ? "open" : "")}>
          <div className='filterRow'>
            <div className="filterColumn">
              <p className="headerName">Publication Date</p>
              <p className={"filterOption " + (filters["publicationDate"] === "last_hour" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("publicationDate", "last_hour")}>Last hour</p>
              <p className={"filterOption " + (filters["publicationDate"] === "today" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("publicationDate", "today")}>Today</p>
              <p className={"filterOption " + (filters["publicationDate"] === "this_week" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("publicationDate", "this_week")}>This week</p>
              <p className={"filterOption " + (filters["publicationDate"] === "this_month" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("publicationDate", "this_month")}>This month</p>
              <p className={"filterOption " + (filters["publicationDate"] === "this_year" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("publicationDate", "this_year")}>This year</p>
            </div>
            <div className="filterColumn">
              <p className="headerName">Last Updated</p>
              <p className={"filterOption " + (filters["lastUpdated"] === "last_hour" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("lastUpdated", "last_hour")}>Last hour</p>
              <p className={"filterOption " + (filters["lastUpdated"] === "today" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("lastUpdated", "today")}>Today</p>
              <p className={"filterOption " + (filters["lastUpdated"] === "this_week" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("lastUpdated", "this_week")}>This week</p>
              <p className={"filterOption " + (filters["lastUpdated"] === "this_month" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("lastUpdated", "this_month")}>This month</p>
              <p className={"filterOption " + (filters["lastUpdated"] === "this_year" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("lastUpdated", "this_year")}>This year</p>
            </div>
            <div className="filterColumn">
              <div className="filterColumn">
                <p className="headerName">VideoCount</p>
                <p className={"filterOption " + (filters["videoCount"] === "under_10" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("videoCount", "under_10")}>Under 10</p>
                <p className={"filterOption " + (filters["videoCount"] === "10_50" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("videoCount", "10_50")}>10 to 50</p>
                <p className={"filterOption " + (filters["videoCount"] === "50_100" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("videoCount", "50_100")}>50 to 100</p>
                <p className={"filterOption " + (filters["videoCount"] === "100_500" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("videoCount", "100_500")}>100 to 500</p>
                <p className={"filterOption " + (filters["videoCount"] === "over_500" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("videoCount", "over_500")}>over 500</p>
              </div>
            </div>
          </div>
          <div className="filterRowBtns">
            <button className='submitBtn' onClick={() => (setFilters(prev => prev = {
              publicationDate: '',
              lastUpdated: '',
              videoCount: '',
              visibility: ''
            }))}>Reset filters</button>
          </div>
        </div>
      </div>
      <section className='video-grid-channel'>

        {
          channelPlaylists
          &&
          Object.keys(channelPlaylists).map((videoIdx) => {
            return (
              <MediumVideo key={channelPlaylists[videoIdx].id + (channelPlaylists[videoIdx].isPlaylist ? ("_playlist_" + channelPlaylists[videoIdx].playlistId) : "_video")}
                video={channelPlaylists[videoIdx]}
                id={channelPlaylists[videoIdx].id}
                isPlaylist={channelPlaylists[videoIdx].isPlaylist}
                playlistId={channelPlaylists[videoIdx].playlistId}
                showPrivate={ auth?.channelId == channelId ? true : false}
              ></MediumVideo>
            )
          })
        }
      </section>
    </>
  )
}

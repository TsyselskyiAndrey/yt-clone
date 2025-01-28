import './ChannelVideos.css';
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
export default function ChannelVideos() {
  useAxiosWithToken();
  const [channelvideos, setChannelVideos] = useState();
  const navigate = useNavigate();
  const { channelId } = useParams();
  const [isSortOpen, setIsSortOpen] = useState(false);
  const [sortOption, setSortOption] = useState("Newest");
  const timeAgo = new TimeAgo('en-US')
  const [isFilterOpen, setIsFilterOpen] = useState(false);
  const { categories } = useCategories();
  const [filters, setFilters] = useState({
    publicationDate: '',
    videoFormat: [],
    duration: '',
    likes: '',
    size: '',
    views: '',
    visibility: '',
    category: '',
  })
  useEffect(() => {
    GetChannelVideos();
  }, [sortOption, filters, channelId])

  async function GetChannelVideos() {
    try {
      const response = await axiosWithToken.get(
        `/api/media/channelvideos/${channelId}/${sortOption}`,
        {
          withCredentials: true,
          params: filters,
          paramsSerializer: params => {
            return qs.stringify(params)
          }
        }
      );
      if (response.status === 200) {
        setChannelVideos(response.data)
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
              <div className="filterColumn">
                <p className="headerName">Duration</p>
                <p className={"filterOption " + (filters["duration"] === "under_4_minutes" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("duration", "under_4_minutes")}>Under 4 minutes</p>
                <p className={"filterOption " + (filters["duration"] === "4_20_minutes" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("duration", "4_20_minutes")}>4-20 minutes</p>
                <p className={"filterOption " + (filters["duration"] === "20_60_minutes" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("duration", "20_60_minutes")}>20-60 minutes</p>
                <p className={"filterOption " + (filters["duration"] === "over_1_hour" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("duration", "over_1_hour")}>Over 1 hour</p>
              </div>
            </div>
            <div className="filterColumn">
              <div className="filterColumn">
                <p className="headerName">Likes</p>
                <p className={"filterOption " + (filters["likes"] === "under_1_thousand" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("likes", "under_1_thousand")}>Under 1 thousand</p>
                <p className={"filterOption " + (filters["likes"] === "1_10_thousand" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("likes", "1_10_thousand")}>1-10 thousand</p>
                <p className={"filterOption " + (filters["likes"] === "10_100_thousand" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("likes", "10_100_thousand")}>10-100 thousand</p>
                <p className={"filterOption " + (filters["likes"] === "over_100_thousand" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("likes", "over_100_thousand")}>Over 100 thousand</p>
              </div>
            </div>
          </div>
          <div className='filterRow'>
            <div className="filterColumn">
              <div className="filterColumn">
                <p className="headerName">Views</p>
                <p className={"filterOption " + (filters["views"] === "under_1_thousand" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("views", "under_1_thousand")}>Under 1 thousand</p>
                <p className={"filterOption " + (filters["views"] === "1_10_thousand" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("views", "1_10_thousand")}>1-10 thousand</p>
                <p className={"filterOption " + (filters["views"] === "10_100_thousand" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("views", "10_100_thousand")}>10-100 thousand</p>
                <p className={"filterOption " + (filters["views"] === "100_500_thousand" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("views", "100_500_thousand")}>100-500 thousand</p>
                <p className={"filterOption " + (filters["views"] === "over_500_thousand" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("views", "over_500_thousand")}>over 500 thousand</p>
              </div>
            </div>
            <div className="filterColumn">
              <div className="filterColumn">
                <p className="headerName">Category</p>
                <div className="container">
                  <div className={"select " + (filters["category"] === '' ? "inactive" : "")}>
                    <select onChange={(e) => handleFilterSelect("category", e.target.value)} value={filters["category"] === '' ? "" : filters["category"]} readOnly>
                      <option value="">Select...</option>

                      {
                        categories
                        &&
                        Object.keys(categories).map((categoryIdx) => {

                          return (
                            <option key={categories[categoryIdx].id} value={categories[categoryIdx].name}>{categories[categoryIdx].name}</option>
                          )
                        })
                      }
                    </select>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="filterRowBtns">
            <button className='submitBtn' onClick={() => (setFilters(prev => prev = {
              publicationDate: '',
              videoFormat: [],
              duration: '',
              likes: '',
              size: '',
              views: '',
              visibility: '',
              category: '',
            }))}>Reset filters</button>
          </div>
        </div>
      </div>
      <section className='video-grid-channel'>

        {
          channelvideos
          &&
          Object.keys(channelvideos).map((videoIdx) => {
            return (
              <MediumVideo key={channelvideos[videoIdx].id + (channelvideos[videoIdx].isPlaylist ? ("_playlist_" + channelvideos[videoIdx].playlistId) : "_video")}
                video={channelvideos[videoIdx]}
                id={channelvideos[videoIdx].id}
                isPlaylist={channelvideos[videoIdx].isPlaylist}
                playlistId={channelvideos[videoIdx].playlistId}
              ></MediumVideo>
            )
          })
        }
      </section>
    </>
  )
}

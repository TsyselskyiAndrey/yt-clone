import './ContentPlaylistVideos.css';
import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import usePlaylistVideo from "../../hooks/usePlaylistVideo";
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import VideoRow from '../../components/VideoRow/VideoRow';
import arrow from "../../resources/arrow.png"
import filter from "../../resources/filter.png"
import cross from "../../resources/cross.png"
import useCategories from '../../hooks/useCategories';
import PlaylistVideoRow from '../../components/PlaylistVideoRow/PlaylistVideoRow';

export default function ContentPlaylistVideos() {
  const { playlistVideos, setPlaylistVideos, GetPlaylistVideos, sortMethod, setSortMethod, isAscending, setIsAscending, filters, setFilters } = usePlaylistVideo();
  const { categories } = useCategories();
  useAxiosWithToken();
  const { playlistId } = useParams()
  const [isFilterOpen, setIsFilterOpen] = useState(false);
  useEffect(() => {
    GetPlaylistVideos()
  }, [])
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
      <ul className="responsive-table">
        <li className="table-header prevent-select">
          <div className="col col-3 ignore">Video</div>
          <div className="col col-4"></div>
          <div className={'col col-2 ' + (sortMethod == "Title" && "selected")} onClick={() => sortMethod == "Title" ? setIsAscending(!isAscending) : (setSortMethod("Title"), setIsAscending(false))}>
            Title
            {sortMethod == "Title" && <img className={'arrowImg ' + (isAscending ? "" : "selected")} src={arrow} alt="" />}
          </div>
          <div className={'col col-2 ' + (sortMethod == "Publication" && "selected")} onClick={() => sortMethod == "Publication" ? setIsAscending(!isAscending) : (setSortMethod("Publication"), setIsAscending(false))}>
            Publication
            {sortMethod == "Publication" && <img className={'arrowImg ' + (isAscending ? "" : "selected")} src={arrow} alt="" />}
          </div>
          <div className={'col col-1 ' + (sortMethod == "Format" && "selected")} onClick={() => sortMethod == "Format" ? setIsAscending(!isAscending) : (setSortMethod("Format"), setIsAscending(false))}>
            Format
            {sortMethod == "Format" && <img className={'arrowImg ' + (isAscending ? "" : "selected")} src={arrow} alt="" />}
          </div>
          <div className={'col col-1 ' + (sortMethod == "Views" && "selected")} onClick={() => sortMethod == "Views" ? setIsAscending(!isAscending) : (setSortMethod("Views"), setIsAscending(false))}>
            Views
            {sortMethod == "Views" && <img className={'arrowImg ' + (isAscending ? "" : "selected")} src={arrow} alt="" />}
          </div>
          <div className={'col col-1 ' + (sortMethod == "Comments" && "selected")} onClick={() => sortMethod == "Comments" ? setIsAscending(!isAscending) : (setSortMethod("Comments"), setIsAscending(false))}>
            Comments
            {sortMethod == "Comments" && <img className={'arrowImg ' + (isAscending ? "" : "selected")} src={arrow} alt="" />}
          </div>
          <div className={'col col-1 ' + (sortMethod == "Rating" && "selected")} onClick={() => sortMethod == "Rating" ? setIsAscending(!isAscending) : (setSortMethod("Rating"), setIsAscending(false))}>
            Likes/Dislikes
            {sortMethod == "Rating" && <img className={'arrowImg ' + (isAscending ? "" : "selected")} src={arrow} alt="" />}
          </div>
          <div className={'col col-1 ' + (sortMethod == "Duration" && "selected")} onClick={() => sortMethod == "Duration" ? setIsAscending(!isAscending) : (setSortMethod("Duration"), setIsAscending(false))}>
            Duration
            {sortMethod == "Duration" && <img className={'arrowImg ' + (isAscending ? "" : "selected")} src={arrow} alt="" />}
          </div>
          <div className={'col col-1 ' + (sortMethod == "Size" && "selected")} onClick={() => sortMethod == "Size" ? setIsAscending(!isAscending) : (setSortMethod("Size"), setIsAscending(false))}>
            Size
            {sortMethod == "Size" && <img className={'arrowImg ' + (isAscending ? "" : "selected")} src={arrow} alt="" />}
          </div>
          <div className="col col-1">
            <img className='filterImg' src={isFilterOpen ? cross : filter} alt="" onClick={() => setIsFilterOpen(!isFilterOpen)} />
          </div>
        </li>

        <div className={"filterContainer prevent-select " + (isFilterOpen ? "open" : "")}>
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
              <p className="headerName">Video Format</p>
              <p className={"filterOption " + (filters["videoFormat"].includes(".mp4") ? "selectedFilter" : "")} onClick={() => handleFilterSelect("videoFormat", ".mp4")}>.mp4</p>
              <p className={"filterOption " + (filters["videoFormat"].includes(".avi") ? "selectedFilter" : "")} onClick={() => handleFilterSelect("videoFormat", ".avi")}>.avi</p>
              <p className={"filterOption " + (filters["videoFormat"].includes(".mkv") ? "selectedFilter" : "")} onClick={() => handleFilterSelect("videoFormat", ".mkv")}>.mkv</p>
              <p className={"filterOption " + (filters["videoFormat"].includes(".mov") ? "selectedFilter" : "")} onClick={() => handleFilterSelect("videoFormat", ".mov")}>.mov</p>
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
                <p className="headerName">Size</p>
                <p className={"filterOption " + (filters["size"] === "0_50_mb" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("size", "0_50_mb")}>0-50 MB</p>
                <p className={"filterOption " + (filters["size"] === "50_200_mb" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("size", "50_200_mb")}>50-200 MB</p>
                <p className={"filterOption " + (filters["size"] === "200_500_mb" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("size", "200_500_mb")}>200-500 MB</p>
                <p className={"filterOption " + (filters["size"] === "over_500_mb" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("size", "over_500_mb")}>over 500 MB</p>
              </div>
            </div>
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
                <p className="headerName">Visibility</p>
                <p className={"filterOption " + (filters["visibility"] === "private" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("visibility", "private")}>Private</p>
                <p className={"filterOption " + (filters["visibility"] === "public" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("visibility", "public")}>Public</p>
              </div>
            </div>
            <div className="filterColumn">
              <div className="filterColumn">
                <p className="headerName">Category</p>
                <div className="container">
                  <div className={"select " + (filters["category"] === '' ? "inactive" : "")}>
                    <select onChange={(e) => handleFilterSelect("category", e.target.value)}  value={filters["category"] === '' ? "" : filters["category"]} readOnly>
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

        <div className="contentTable">
          {
            playlistVideos.length == 0
              ?
              <div className="noinfo"></div>
              :
              Object.keys(playlistVideos).map((videoIdx) => {

                return (
                  <PlaylistVideoRow
                    key={playlistVideos[videoIdx].id}
                    id={playlistVideos[videoIdx].id}
                    video={playlistVideos[videoIdx]}
                    videos={playlistVideos}
                    setVideos={setPlaylistVideos}
                    playlistId = {playlistId}
                  >

                  </PlaylistVideoRow>

                )
              })
          }
        </div>


      </ul>
    </>


  );
}

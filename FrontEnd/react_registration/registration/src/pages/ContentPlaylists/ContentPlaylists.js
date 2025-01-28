import './ContentPlaylists.css';
import { useEffect, useState } from 'react';
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import usePlaylist from '../../hooks/usePlaylist';
import validateControl from '../Auth/Forms/Validations/GeneralValidation';
import DataInput from '../../components/DataInput/DataInput';
import arrow from "../../resources/arrow.png"
import filter from "../../resources/filter.png"
import cross from "../../resources/cross.png"
import loadanimation from "../../resources/loadanimation.gif"
import { axiosWithToken } from "../../API/axioscfg"
import PlaylistRow from '../../components/PlaylistRow/PlaylistRow';


export default function ContentPlaylists() {
  const { playlists, setPlaylists, GetPlaylists, sortMethod, setSortMethod, isAscending, setIsAscending, filters, setFilters } = usePlaylist();
  const [isCreatingPlaylist, setIsCreatingPlaylist] = useState(false)
  const [isPlaylistLoading, setIsPlaylistLoading] = useState(false)
  const [isFilterOpen, setIsFilterOpen] = useState(false);
  const [formControls, setFormControls] = useState({
    title: {
      type: 'text',
      name: 'title',
      label: 'Title:',
      errorMessage: '',
      value: '',
      valid: false,
      validation: {
        required: true,
        minLength: 2,
        maxLength: 255
      },
      touched: false,
      shake: false
    },
    description: {
      type: 'textarea',
      name: 'description',
      label: 'Description:',
      errorMessage: '',
      value: '',
      valid: true,
      validation: {
        minLength: 0,
        maxLength: 1000
      },
      touched: false,
      shake: false
    }
  });
  useAxiosWithToken();
  useEffect(() => {
    GetPlaylists()
  }, [])


  function changeHandler(e, controlName) {
    const formControlsCopy = { ...formControls }
    const control = { ...formControlsCopy[controlName] }
    control.value = e.target.value
    const [isValid, newErrorMessage] = validateControl(e.target.value, control.validation, control.name)
    control.valid = isValid
    control.errorMessage = newErrorMessage
    control.touched = true;

    formControlsCopy[controlName] = control

    setFormControls(formControlsCopy)
  }

  function IsFormValid() {
    let isFormValid = true
    Object.keys(formControls).forEach(name => {
      isFormValid = formControls[name].valid && isFormValid
    })
    return isFormValid;
  }

  function shakeInvalidElems() {
    if (!IsFormValid()) {
      const formControlsCopy = { ...formControls }
      Object.keys(formControlsCopy).map((controlName) => {
        const control = formControlsCopy[controlName];
        if (!control.valid) {
          control.shake = true;
          if (!control.touched) {
            control.touched = true;
            const [isValid, newErrorMessage] = validateControl(control.value, control.validation, control.name)
            control.valid = isValid
            control.errorMessage = newErrorMessage
          }

        }
      })
      setFormControls(formControlsCopy)
      setTimeout(() => {
        const resetControls = { ...formControls };
        Object.keys(resetControls).forEach((controlName) => {
          resetControls[controlName].shake = false;
        });
        setFormControls(resetControls);
      }, 300);
    }
  }

  function handleFilterSelect(key, value) {
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

  async function handlePlaylistCreation() {
    if (!IsFormValid()) {
      shakeInvalidElems();
    }
    else {
      const reqData = {
        title: formControls.title.value,
        description: formControls.description.value
      }
      try {
        setIsPlaylistLoading(true)
        const response = await axiosWithToken.post(
          "/api/playlist/create",
          reqData,
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        );
        if (response.status == 200) {
          await GetPlaylists();
          setIsCreatingPlaylist(false)
        }
      } catch (error) {
        if (!error?.response) {
          console.log("No Server Response");
        }
        else if (error.response?.status === 400 || error.response?.status === 409) {
          const errorData = error.response.data;
          const formControlsCopy = { ...formControls }
          for (const particularError of errorData) {
            const fieldName = particularError.field.toLowerCase();
            formControlsCopy[fieldName].valid = false
            formControlsCopy[fieldName].errorMessage = "* " + particularError.details
            formControlsCopy[fieldName].touched = true
          }
          setFormControls(formControlsCopy)

        }
        else {
          console.error('Error during login:', error.response || error.message);
        }
        shakeInvalidElems();
      } finally {
        setIsPlaylistLoading(false)
      }
    }
  }
  return (
    <>
      <ul className="responsive-table">
        <li className="table-header prevent-select">
          <div className="col col-6 ignore ignore-opacity"><button className='createPlaylistBtn' onClick={() => setIsCreatingPlaylist(true)}>Create</button></div>
          <div className="col col-5"></div>
          <div className={'col col-7 ' + (sortMethod == "Title" && "selected")} onClick={() => sortMethod == "Title" ? setIsAscending(!isAscending) : (setSortMethod("Title"), setIsAscending(false))}>
            Title
            {sortMethod == "Title" && <img className={'arrowImg ' + (isAscending ? "" : "selected")} src={arrow} alt="" />}
          </div>
          <div className={'col col-7 ' + (sortMethod == "Publication" && "selected")} onClick={() => sortMethod == "Publication" ? setIsAscending(!isAscending) : (setSortMethod("Publication"), setIsAscending(false))}>
            Creation Date
            {sortMethod == "Publication" && <img className={'arrowImg ' + (isAscending ? "" : "selected")} src={arrow} alt="" />}
          </div>
          <div className={'col col-7 ' + (sortMethod == "LastUpdated" && "selected")} onClick={() => sortMethod == "LastUpdated" ? setIsAscending(!isAscending) : (setSortMethod("LastUpdated"), setIsAscending(false))}>
            Last Updated
            {sortMethod == "LastUpdated" && <img className={'arrowImg ' + (isAscending ? "" : "selected")} src={arrow} alt="" />}
          </div>
          <div className={'col col-8 ' + (sortMethod == "VideoCount" && "selected")} onClick={() => sortMethod == "VideoCount" ? setIsAscending(!isAscending) : (setSortMethod("VideoCount"), setIsAscending(false))}>
            Video Count
            {sortMethod == "VideoCount" && <img className={'arrowImg ' + (isAscending ? "" : "selected")} src={arrow} alt="" />}
          </div>
          <div className="col col-6">
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
              <p className="headerName">Last Updated</p>
              <p className={"filterOption " + (filters["lastUpdated"] === "last_hour" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("lastUpdated", "last_hour")}>Last hour</p>
              <p className={"filterOption " + (filters["lastUpdated"] === "today" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("lastUpdated", "today")}>Today</p>
              <p className={"filterOption " + (filters["lastUpdated"] === "this_week" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("lastUpdated", "this_week")}>This week</p>
              <p className={"filterOption " + (filters["lastUpdated"] === "this_month" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("lastUpdated", "this_month")}>This month</p>
              <p className={"filterOption " + (filters["lastUpdated"] === "this_year" ? "selectedFilter" : "")} onClick={() => handleFilterSelect("lastUpdated", "this_year")}>This year</p>
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

        <div className="contentTable">
          {
            playlists.length == 0
              ?
              <div className="noinfo"></div>
              :
              Object.keys(playlists).map((playlistIdx) => {
                return (
                  <PlaylistRow
                    key={playlists[playlistIdx].id}
                    id={playlists[playlistIdx].id}
                    playlist={playlists[playlistIdx]}
                    playlists={playlists}
                    setPlaylists={setPlaylists}
                  >

                  </PlaylistRow>

                )
              })
          }
        </div>


      </ul>


      {
        isCreatingPlaylist
          ?
          <div className='blackbg'>
            <div className='playlistCreationForm'>
              <h2>Create a playlist!</h2>
              {
                Object.keys(formControls).map((controlName, index) => {
                  const control = formControls[controlName];
                  return (
                    <DataInput
                      key={`${index}_createPlaylistField`}
                      type={control.type}
                      name={control.name}
                      label={control.label}
                      value={control.value}
                      valid={control.valid}
                      errorMessage={control.errorMessage}
                      onChange={(e) => changeHandler(e, controlName)}
                      touched={control.touched}
                      shake={control.shake} />
                  )
                })
              }
              <div className="buttonCont">
                <button className='submitBtn' disabled={isPlaylistLoading ? true : false} onClick={() => setIsCreatingPlaylist(false)}>Cancel</button>
                <button className='submitBtn' disabled={isPlaylistLoading ? true : false} onClick={handlePlaylistCreation}>
                  {
                    isPlaylistLoading
                      ?
                      <img src={loadanimation}></img>
                      :
                      <p>Create</p>
                  }
                </button>
              </div>
            </div>
          </div>
          :
          <></>
      }

    </>
  );
}

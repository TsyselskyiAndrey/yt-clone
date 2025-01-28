import { useState, useRef } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useVideo from "../../hooks/useVideo";
import usePlaylist from "../../hooks/usePlaylist";
import '../Header/Header.css';
import './CMHeader.css';
import logo from '../resources/logo.png';
import cross from '../resources/cross.png';
import uploadicon from '../resources/icons/upload.svg'
import sign_out from '../resources/icons/sign_out.svg'
import channel from '../resources/icons/channel.svg'
import upload from '../resources/upload.png'
import SearchInput from '../SearchInput/SearchInput';
import useAuth from "../../hooks/useAuth";
import loadanimation from '../../resources/loadanimation.gif'
import useAxiosWithToken from "../../hooks/useAxiosWithToken"

export default function CMHeader(props) {
  const { isLoading, auth, setAuth } = useAuth();
  const navigate = useNavigate();
  const [isVideoLoading, setIsVideoLoading] = useState(false);
  const [isDragging, setIsDragging] = useState(false);
  const location = useLocation();
  useAxiosWithToken();
  const [isCreatingVideo, setIsCreatingVideo] = useState(false);
  const fileInputRef = useRef(null);
  const { GetVideos, searchValue, setSearchValue } = useVideo();
  const { GetPlaylists, searchValuePlaylist, setSearchValuePlaylist } = usePlaylist();



  const handleDragOver = (e) => {
    e.preventDefault();
    setIsDragging(true);
  };

  const handleDragLeave = () => {
    setIsDragging(false);
  };

  const handleDrop = (e) => {
    if (isVideoLoading) {
      return;
    }
    e.preventDefault();
    setIsDragging(false);
    const file = e.dataTransfer.files[0];
    uploadVideo(file)
  };

  const handleInputChange = (e) => {
    if (isVideoLoading) {
      return;
    }
    const file = e.target.files[0];
    uploadVideo(file)
  };


  const handleUploadBtnClick = () => {
    fileInputRef.current.click();
  };


  async function handleSignOut() {
    if (!localStorage.getItem('accessToken')) {
      setAuth(null);
      return;
    }

    try {
      const response = await axiosWithToken.post(
        "/api/auth/logout",
        {},
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setAuth(null);
        localStorage.removeItem('accessToken');
        navigate("/");
      }
    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else {
        console.error('Error during logging out:', error.response?.data || error.message);
      }
    }
  }


  async function uploadVideo(file) {
    if (!file) {
      alert('Please select a file!');
      return;
    }
    setIsVideoLoading(true)
    const formData = new FormData();
    formData.append("file", file);
    try {
      const response = await axiosWithToken.post('/api/content/upload', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        }
      });

      if (response.status === 200) {
        const result = response.data;
        await GetVideos();
        setIsCreatingVideo(false)
      } else {
        alert('Error uploading file.');
      }
    } catch (error) {
      console.error('Error:', error);
      alert('An error occurred.');
    } finally {
      setIsVideoLoading(false)
    }
  }

  async function handleSearch() {
    await GetVideos();
  }
  async function handleSearchPlaylists() {
    await GetPlaylists();
  }

  if (isLoading) {
    return <div className="headerLoading">Loading...</div>;
  }

  return (
    <>
      <header>
        <div className="headerBlock">
          <div className={"menu_icon " + (props.menuOpen ? "clicked" : "")} onClick={() => props.setMenuOpen(!props.menuOpen)}>
            <span className="one"></span>
            <span className="two"></span>
            <span className="three"></span>
          </div>
          <div className="mainlogo prevent-select" onClick={() => navigate("/channelmanagement/content/videos")}>
            <img src={logo} alt="mainlogo" />
            <p>Studio</p>
          </div>
        </div>
        <div className="headerBlock searchContainer">
          <SearchInput label="Search across your content" searchValue={location.pathname.includes("channelmanagement/content/playlists") ? searchValuePlaylist : searchValue} setSearchValue={location.pathname.includes("channelmanagement/content/playlists") ? setSearchValuePlaylist : setSearchValue} handleSearch={location.pathname.includes("channelmanagement/content/playlists") ? handleSearchPlaylists : handleSearch}></SearchInput>
        </div>

        <div className='headerBlock infoContainer' style={{ width: "200px" }}>
          <button className="sinInBtn" onClick={() => setIsCreatingVideo(true)} style={{ marginRight: "0px" }}>
            <img src={uploadicon} alt="" className='authLogo' />
            Create
          </button>
          <div className="itemContainer">
            <img className="currentUserPicture" src={auth?.avatarUrl + `?timestamp=${Date.now()}`} alt="" tabIndex="0"></img>
            <div className='subContainer' tabIndex="0">
              <div className="accountInfo">
                <img src={auth?.avatarUrl + `?timestamp=${Date.now()}`} alt="" tabIndex="0"></img>
                <div className="credentials">
                  <p>{auth?.firstName + " " + auth?.lastName}</p>
                  <p className='email'>{auth?.email}</p>
                  <p className='channelHandle'>@{auth?.handle}</p>
                </div>
              </div>

              <hr />
              <button className='subContainerBtn' onClick={() => navigate("/")}>
                <img src={channel} alt="" />
                Back to Vibeo
              </button>
              <button className='subContainerBtn' onClick={handleSignOut}>
                <img src={sign_out} alt="" />
                Sign out
              </button>
            </div>
          </div>
        </div>


      </header>

      {
        isCreatingVideo
          ?
          <div className='blackbg'>
            <div className='videoCreationForm'>
              <div className="head">
                <h3>Upload your video</h3>
                <img src={cross} alt="" onClick={() => setIsCreatingVideo(false)} />
              </div>
              <div className={`uploadingForm ${isDragging && !isVideoLoading ? "dragging" : ""}`}
                onDragOver={handleDragOver}
                onDragLeave={handleDragLeave}
                onDrop={handleDrop}>

                <div className="info">
                  <img src={isVideoLoading ? loadanimation : upload} alt="" />
                  <h5>Drag and drop video files to upload</h5>
                  <p>Your videos will be private until you publish them.</p>
                  <button className='submitBtn' onClick={handleUploadBtnClick}>Upload <input
                    type="file"
                    accept="video/mp4, video/avi, video/x-matroska, video/quicktime"
                    onChange={handleInputChange}
                    style={{ display: "none" }}
                    ref={fileInputRef}
                  /></button>
                  <p>Supports: .mp4, .avi, .mkv, .mov</p>
                </div>
              </div>

            </div>
          </div>

          :
          <></>
      }

    </>
  );
}

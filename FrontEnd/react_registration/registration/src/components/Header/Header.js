import { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import './Header.css';
import logo from '../resources/logo.png';
import uploadicon from '../resources/icons/upload.svg'
import notifications from '../resources/icons/notifications.svg'
import voiceSearch from '../resources/icons/voice-search-icon.svg'
import sign_out from '../resources/icons/sign_out.svg'
import profile from '../resources/icons/profile.svg'
import channel from '../resources/icons/channel.svg'
import authpng from '../resources/auth.png'
import SearchInput from '../SearchInput/SearchInput';
import useAuth from "../../hooks/useAuth";
import loadanimation from '../../resources/loadanimation.gif'
import DataInput from "../DataInput/DataInput"
import validateControl from '../../pages/Auth/Forms/Validations/GeneralValidation'
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import TimeAgo from 'javascript-time-ago'
import SpeechRecognition, { useSpeechRecognition } from 'react-speech-recognition';
import cross from '../resources/cross.png';
export default function Header(props) {
  const { isLoading, auth, setAuth } = useAuth();
  const navigate = useNavigate();
  const [isChannelLoading, setIsChannelLoading] = useState(false);
  useAxiosWithToken();
  const [searchValue, setSearchValue] = useState("")
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
        allowSymbols: false,
        minLength: 2,
        maxLength: 20
      },
      touched: false,
      shake: false
    },
    handle: {
      type: 'text',
      name: 'handle',
      label: 'Handle:',
      errorMessage: '',
      value: '',
      valid: false,
      validation: {
        required: true,
        allowSpaces: false,
        allowSymbols: false,
        minLength: 2,
        maxLength: 20
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
  const timeAgo = new TimeAgo('en-US')
  const [notificationData, setNotificationData] = useState();
  const [isCreatingChannel, setIsCreatingChannel] = useState(false);
  const {
    transcript,
    listening
  } = useSpeechRecognition();
  useEffect(() => {
    setSearchValue(transcript);
  }, [transcript]);

  useEffect(() => {
    GetNotifications();
  }, [])

  async function GetNotifications() {
    try {
      const response = await axiosWithToken.get(
        `/api/media/notifications`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setNotificationData(response.data)
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
        navigate("/")
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


  async function handleCreateChannel() {
    if (!IsFormValid()) {
      shakeInvalidElems();
    }
    else {
      const reqData = {
        title: formControls.title.value,
        description: formControls.description.value,
        handle: formControls.handle.value,
      }
      setIsChannelLoading(true)
      try {
        const response = await axiosWithToken.post(
          "/api/channel/create",
          reqData,
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        );
        setAuth({
          ...auth,
          handle: response.data.handle,
          title: response.data.title,
          logoUrl: response.data.logoUrl,
          channelId: response.data.id,
          roles: [...(auth?.roles || []), "channel_owner"]
        })
        setIsCreatingChannel(false);
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
        setIsChannelLoading(false)
      }
    }

  }

  async function handleCreateVideo() {
    if (auth?.roles?.includes("channel_owner")) {
      navigate("/channelmanagement/content/videos")
    }
    else {
      setIsCreatingChannel(true);
    }
  }

  function handleSearch() {
    if (searchValue.trim().length === 0) {
      return;
    }
    navigate(`/search?search_query=${encodeURIComponent(searchValue)}`)
  }

  async function handleReadNotification(notificationId) {
    try {
      const response = await axiosWithToken.patch(
        `/api/media/readnotification/${notificationId}`,
        {},
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        }
      );
      if (response.status == 200) {
        setNotificationData(prevNotifications => ({
          ...prevNotifications,
          notifications: prevNotifications.notifications.map(notification =>
            notification.id === notificationId
              ? { ...notification, isRead: response.data.isRead }
              : notification
          ),
          numberOfNotifications: prevNotifications.numberOfNotifications - 1
        }));
        
      }

    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else {
        console.error('Error during login:', error.response || error.message);
      }
    }
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
          <div className="mainlogo prevent-select" onClick={() => navigate("/")}>
            <img src={logo} alt="mainlogo" />
            <p>Vibeo</p>
          </div>
        </div>
        <div className="headerBlock searchContainer">
          <SearchInput label="Search" searchValue={searchValue} setSearchValue={setSearchValue} handleSearch={handleSearch}></SearchInput>
          <div className="itemContainer voiceSearchContainer">
            {
              listening
                ?
                <img className="uploadIcon" src={cross} alt="" onClick={SpeechRecognition.stopListening}></img>
                :
                <img className="uploadIcon" src={voiceSearch} alt="" onClick={SpeechRecognition.startListening}></img>
            }

            <div className="tooltip">Search with your voice</div>
          </div>
        </div>

        {
          isLoading
            ?
            <></>
            :
            auth
              ?
              <div className='headerBlock infoContainer'>
                <div className="itemContainer" onClick={handleCreateVideo}>
                  <img className="uploadIcon" src={uploadicon} alt=""></img>
                  <div className="tooltip">Create</div>
                </div>
                <div className="itemContainer">
                  <img className="notificationsIcon" src={notifications} alt="" tabIndex="0"></img>
                  <div className="tooltip">Notifiction</div>
                  {
                    notificationData?.numberOfNotifications > 0
                      ?
                      <div className="notificationsCount" tabIndex="0">{notificationData?.numberOfNotifications}</div>
                      :
                      <></>
                  }
                  <div className='subContainerNotifications' tabIndex="0">
                    <div className="notificationsHeader">Notifications</div>

                    <div className="notificationFeed">
                      {
                        notificationData
                        &&
                        Object.keys(notificationData.notifications).map((notificationIdx) => {
                          return (
                            <div key={notificationData.notifications[notificationIdx]?.id}
                              className={"notification " + (notificationData.notifications[notificationIdx]?.isRead ? "read" : "unread")}
                              onClick={() => {
                                if(!notificationData.notifications[notificationIdx]?.isRead){
                                  handleReadNotification(notificationData.notifications[notificationIdx]?.id)
                                }
                                if (notificationData.notifications[notificationIdx]?.notificationType == "NewVideo") {
                                  navigate(`/watch/${notificationData.notifications[notificationIdx]?.videoId}`)
                                }
                                else if (notificationData.notifications[notificationIdx]?.notificationType == "CommentReply") {
                                  navigate(`/watch/${notificationData.notifications[notificationIdx]?.commentVideoId}`)
                                }
                              }}>
                              <div className="notificationIcon">
                                <img src={notificationData.notifications[notificationIdx]?.channelLogo} alt="Channel Logo" />
                              </div>
                              <div className="notificationContent">
                                <p className="notificationTitle"><strong>{notificationData.notifications[notificationIdx]?.title}</strong></p>
                                <p className="notificationText">{notificationData.notifications[notificationIdx]?.message}</p>
                                <p className="notificationTime">{notificationData.notifications[notificationIdx]?.createdAt && timeAgo.format(new Date(notificationData.notifications[notificationIdx].createdAt))}</p>
                              </div>
                            </div>
                          )
                        })
                      }



                    </div>
                  </div>
                </div>
                <div className="itemContainer">
                  <img className="currentUserPicture" src={auth?.avatarUrl + `?timestamp=${Date.now()}`} alt="" tabIndex="0"></img>
                  <div className='subContainer' tabIndex="0">
                    <div className="accountInfo">
                      <img src={auth?.avatarUrl + `?timestamp=${Date.now()}`} alt="" tabIndex="0"></img>
                      <div className="credentials">
                        <p>{auth?.firstName + " " + auth?.lastName}</p>
                        <p className='email'>{auth?.email}</p>
                        {
                          auth?.roles?.includes("channel_owner")
                            ?
                            <p className='channelHandle'>@{auth?.handle}</p>
                            :
                            <h4 onClick={() => setIsCreatingChannel(true)}>Create your channel</h4>
                        }

                      </div>
                    </div>

                    <hr />

                    {
                      auth?.roles?.includes("channel_owner") &&
                      <>
                        <button className='subContainerBtn' onClick={() => navigate("/channelmanagement/content/videos")}>
                          <img src={channel} alt="" />
                          Vibeo Studio
                        </button>
                        <hr />
                      </>
                    }

                    <button className='subContainerBtn' onClick={handleSignOut}>
                      <img src={sign_out} alt="" />
                      Sign out
                    </button>
                    <button className='subContainerBtn' onClick={() => navigate("/profile")}>
                      <img src={profile} alt="" />
                      Profile
                    </button>
                  </div>
                </div>
              </div>
              :
              <div className='headerBlock'>
                <button className="sinInBtn" onClick={() => navigate('/auth')}>
                  <img src={authpng} alt="" className='authLogo' />
                  Sign In</button>
              </div>
        }
      </header>
      {
        isCreatingChannel
          ?
          <div className='blackbg'>
            <div className='channelCreationForm'>
              <h2>Create your channel first!</h2>
              {
                Object.keys(formControls).map((controlName, index) => {
                  const control = formControls[controlName];
                  return (
                    <DataInput
                      key={`${index}_createChannelField`}
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
                <button className='submitBtn' disabled={isChannelLoading ? true : false} onClick={() => setIsCreatingChannel(false)}>Cancel</button>
                <button className='submitBtn' onClick={handleCreateChannel} disabled={isChannelLoading ? true : false}>
                  {
                    isChannelLoading
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

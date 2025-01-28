import { useParams } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import { useEffect, useState, useRef } from 'react';
import './Customisation.css';
import loadanimation from '../../resources/loadanimation.gif'
import DataInput from '../../components/DataInput/DataInput';
import validateControl from '../../pages/Auth/Forms/Validations/GeneralValidation'
import useAuth from '../../hooks/useAuth';
import ConfirmationWindow from '../../components/ConfirmationWindow/ConfirmationWindow';
export default function Customisation() {
  useAxiosWithToken();
  const navigate = useNavigate();
  const [isUpdating, setIsUpdating] = useState(false);
  const [profile, setProfile] = useState({});
  const [isUploading, setIsUploading] = useState(false);
  const fileInputRef = useRef(null);
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
  const { auth, setAuth } = useAuth();
  const [isDeleting, setIsDeleting] = useState(false);
  useEffect(() => {
    GetProfile();
  }, []);
  useEffect(() => {
    handleReset();
  }, [profile.title, profile.description, profile.handle]);

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
  function handleReset() {
    if (profile) {
      setFormControls((prevState) => ({
        ...prevState,
        title: {
          ...prevState.title,
          value: profile.title || '',
          touched: false,
          valid: true
        },
        handle: {
          ...prevState.handle,
          value: profile.handle || '',
          touched: false,
          valid: true
        },
        description: {
          ...prevState.description,
          value: profile.description || '',
          touched: false,
          valid: true
        }
      }));
    }
  }
  async function GetProfile() {
    try {
      const response = await axiosWithToken.get(
        `/api/channel/getchannel`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setProfile(response.data)
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

  async function handleInfoUpdate() {
    if (!IsFormValid()) {
      shakeInvalidElems();
    }
    else {
      const reqData = {
        title: formControls.title.value,
        handle: formControls.handle.value,
        description: formControls.description.value
      }
      setIsUpdating(true)
      try {
        const response = await axiosWithToken.patch(
          `/api/channel/updatechannel`,
          reqData,
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        );
        if (response.status == 200) {
          setProfile({
            ...profile,
            title: response.data.title,
            handle: response.data.handle,
            description: response.data.description
          })

          setAuth(prevAuth => ({
            ...prevAuth,
            title: response.data.title,
            handle: response.data.handle,
            description: response.data.description
          }));
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
          console.error('Error during updating:', error.response || error.message);
        }
        shakeInvalidElems();
      } finally {
        setIsUpdating(false)
      }
    }

  }

  const handleInputChange = (e) => {
    if (isUploading) {
      return;
    }
    const file = e.target.files[0];
    uploadAvatar(file)
  };


  const handleUploadBtnClick = () => {
    fileInputRef.current.click();
  };

  async function uploadAvatar(file) {
    if (!file) {
      alert('Please select a file!');
      return;
    }
    setIsUploading(true)
    const formData = new FormData();
    formData.append("file", file);
    try {
      const response = await axiosWithToken.patch(`/api/channel/uploadlogo`,
        formData,
        {
          headers: {
            'Content-Type': 'multipart/form-data',
          }
        }
      );

      if (response.status === 200) {
        const result = response.data;
        setAuth(prevAuth => ({
          ...prevAuth,
          logoUrl: result.logoUrl + `?timestamp=${Date.now()}`
        }));
        setProfile({
          ...profile,
          logoUrl: result.logoUrl + `?timestamp=${Date.now()}`
        })
      } else {
        alert('Error uploading file.');
      }
    } catch (error) {
      console.error('Error:', error);
      alert('An error occurred.');
    } finally {
      setIsUploading(false)
    }
  }

  async function handleReport() {
    try {
      const response = await axiosWithToken.get(`/api/content/channelvideosreport`, {
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

  async function handleDelete() {
    try {
      const response = await axiosWithToken.delete(
        `/api/channel/deletechannel`,
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setAuth({
          ...auth,
          handle: null,
          title: null,
          logoUrl: null,
          channelId: null,
          roles: auth?.roles?.filter(role => role !== "channel_owner")
        })
        navigate("/")
      }
    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else {
        console.error('Error during changing visibility:', error.response || error.message);
      }
    }
  }

  return (
    <div className='editForm'>
      <div className='editContainer centerDiv'>
        <div className='dataBlockPlaylist'>
          <div className="standardInfoBlock">
            <h3>Static Info</h3>
            <p><span style={{ fontWeight: "bold" }}>Email:</span> {profile.email}</p>
            <p><span style={{ fontWeight: "bold" }}>Created at:</span> {profile.createdAt}</p>
          </div>
          <div className="standardInfoBlock">
            <h3>General</h3>
            {
              Object.keys(formControls).map((controlName, index) => {
                const control = formControls[controlName];
                return (
                  <DataInput
                    key={`${index}_updateProfileField`}
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
            <div className="editFormButtonCont">
              <button className='submitBtn editFormBtn resetbtn ' disabled={isUpdating ? true : false} onClick={handleReset}>Reset</button>
              <button className='submitBtn editFormBtn ' disabled={isUpdating ? true : false} onClick={handleInfoUpdate}>
                {
                  isUpdating
                    ?
                    <img src={loadanimation}></img>
                    :
                    <p>Save</p>
                }
              </button>
            </div>
          </div>

        </div>
        <div className='dataBlockPlaylist'>
          <div className="standardInfoBlock">
            <h3>Channel Logo</h3>
            <div className="previewContainer">
              <img src={profile?.logoUrl + `?timestamp=${Date.now()}`} className='thumbnailImg avatarEditImg' alt="" />
              <div className="previewInfoBlock">
                <p>You can change the avatar of your account here. Just press the button below...</p>
                <button className='submitBtn' onClick={handleUploadBtnClick}>
                  {
                    isUploading
                      ?
                      <img src={loadanimation} alt="" />
                      :
                      <>Upload an avatar</>
                  }
                  <input
                    type="file"
                    accept="image/png, image/jpeg, image/jpg"
                    onChange={handleInputChange}
                    style={{ display: "none" }}
                    ref={fileInputRef}
                  />
                </button>
                <p className='centerP'>Supports: .png, .jpeg, .jpg</p>
              </div>
            </div>
          </div>
          <div className="standardInfoBlock">
            <h3>Report</h3>
            <button className='submitBtn reportBtn' onClick={handleReport}>Get channel report!</button>
          </div>
          <div className="standardInfoBlock">
            <h3>Remove channel</h3>
            <button className='submitBtn reportBtn cancelBtn' onClick={() => { setIsDeleting(true) }}>Remove channel!</button>
          </div>
        </div>
      </div>
      {
        isDeleting
          ?
          <ConfirmationWindow message="Are you sure you want to delete the channel with all the progress?" onCancel={() => setIsDeleting(false)} onConfirm={handleDelete}></ConfirmationWindow>
          :
          <></>
      }
    </div>
  );
}

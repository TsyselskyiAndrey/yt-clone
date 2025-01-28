import { useParams } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import useCategories from "../../hooks/useCategories"
import { useEffect, useState, useRef } from 'react';
import './VideoEdit.css';
import { partial } from "filesize";
import loadanimation from '../../resources/loadanimation.gif'
import DataInput from '../../components/DataInput/DataInput';
import validateControl from '../../pages/Auth/Forms/Validations/GeneralValidation'
import CommentInput from '../../components/CommentInput/CommentInput';
import cross from "../../resources/cross-red.png"
export default function VideoEdit() {
  const { videoId } = useParams()
  useAxiosWithToken();
  const navigate = useNavigate();
  const [isUpdating, setIsUpdating] = useState(false);
  const [selectedCategory, setSelectedCategory] = useState("");
  const { categories } = useCategories();
  const [playlists, setPlaylists] = useState([]);
  const [video, setVideo] = useState({});
  const [hoveredCategory, setHoveredCategory] = useState(null);
  const [hoveredTag, setHoveredTag] = useState(null);
  const [isUploading, setIsUploading] = useState(false);
  const [isAddingToPlaylist, setIsAddingToPlaylist] = useState(false)
  const [playlistUpdatingInProgress, setPlaylistUpdatingInProgress] = useState(false);
  const fileInputRef = useRef(null);
  useEffect(() => {
    GetVideo();
    GetPlaylists();
  }, []);
  useEffect(() => {
    handleReset();
  }, [video.title, video.description]);

  const [formControls, setFormControls] = useState({
    title: {
      type: 'text',
      name: 'title',
      label: 'Title:',
      errorMessage: '',
      value: '',
      valid: true,
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

  const [categoryControl, setCategoryControl] = useState({
    name: 'category',
    errorMessage: '',
    value: '',
    valid: false,
    touched: false,
    shake: false
  });

  const [tagControl, setTagControl] = useState({
    name: 'tag',
    errorMessage: '',
    value: '',
    valid: false,
    validation: {
      minLength: 2,
      maxLength: 20,
      allowSymbols: false,
      allowSpaces: false
    },
    touched: false,
    shake: false
  });

  const size = partial({ standard: "jedec" });

  async function GetPlaylists() {
    try {
      const response = await axiosWithToken.get(
        `/api/playlist/getvideoplaylists/${videoId}`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setPlaylists(response.data)
      }
    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else {
        console.error('Error during retrieving playlists:', error.response?.data || error.message);
      }
    }
  }
  async function GetVideo() {
    try {
      const response = await axiosWithToken.get(
        `/api/content/getvideo/${videoId}`,
        {
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setVideo(response.data)
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
    if (video) {
      setFormControls((prevState) => ({
        ...prevState,
        title: {
          ...prevState.title,
          value: video.title || '',
          touched: false,
          valid: true
        },
        description: {
          ...prevState.description,
          value: video.description || '',
          touched: false,
          valid: true
        }
      }));
    }
  }

  async function handleInfoUpdate() {

    if (!IsFormValid()) {
      shakeInvalidElems();
    }
    else {
      const reqData = {
        title: formControls.title.value,
        description: formControls.description.value
      }
      setIsUpdating(true)
      try {
        const response = await axiosWithToken.patch(
          `/api/content/updatevideo/${videoId}`,
          reqData,
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        );
        if (response.status == 200) {
          setVideo({
            ...video,
            title: response.data.title,
            description: response.data.description
          })
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
        setIsUpdating(false)
      }
    }

  }

  async function handleAddCategory() {
    try {
      const response = await axiosWithToken.patch(
        `/api/content/addcategory/${videoId}`,
        {
          name: selectedCategory
        },
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        }
      );
      if (response.status == 200) {
        setVideo({
          ...video,
          categories: [...response.data.categories]
        })
      }

    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else if (error.response?.status === 400 || error.response?.status === 409) {
        const errorData = error.response.data;
        const categoryControlCopy = { ...categoryControl };

        for (const particularError of errorData) {
          categoryControlCopy.valid = false;
          categoryControlCopy.errorMessage = "* " + particularError.details;
          categoryControlCopy.touched = true;
          setCategoryControl({ ...categoryControlCopy, shake: true });

          setTimeout(() => {
            setCategoryControl(prev => ({ ...prev, shake: false }));
          }, 300);
        }

      }
      else {
        console.error('Error during login:', error.response || error.message);
      }
    }
  }

  async function handleDeleteCategory(categoryId) {
    try {
      const response = await axiosWithToken.delete(
        `/api/content/deletecategory/${videoId}/${categoryId}`,
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        }
      );
      if (response.status == 200) {
        setHoveredCategory(null)
        setVideo({
          ...video,
          categories: [...response.data.categories]
        })
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

  function handleTagChange(e) {
    const tagControlCopy = { ...tagControl }
    tagControlCopy.value = e.target.value
    const [isValid, newErrorMessage] = validateControl(e.target.value, tagControlCopy.validation, tagControlCopy.name)
    tagControlCopy.valid = isValid
    tagControlCopy.errorMessage = newErrorMessage
    tagControlCopy.touched = true;
    if (tagControlCopy.value == "") {
      tagControlCopy.touched = false
    }
    setTagControl(tagControlCopy);
  }

  async function handleAddTag() {
    if (tagControl.valid == false) {
      setTagControl({ ...tagControl, shake: true });

      setTimeout(() => {
        setTagControl(prev => ({ ...prev, shake: false }));
      }, 300);
      return;
    }
    try {
      const response = await axiosWithToken.patch(
        `/api/content/addtag/${videoId}`,
        {
          name: tagControl.value
        },
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        }
      );
      if (response.status == 200) {
        setVideo({
          ...video,
          tags: [...response.data.tags]
        })
      }

    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else if (error.response?.status === 400 || error.response?.status === 409) {
        const errorData = error.response.data;
        const tagControlCopy = { ...tagControl };

        for (const particularError of errorData) {
          tagControlCopy.valid = false;
          tagControlCopy.errorMessage = "* " + particularError.details;
          tagControlCopy.touched = true;
          setTagControl({ ...tagControlCopy, shake: true });

          setTimeout(() => {
            setTagControl(prev => ({ ...prev, shake: false }));
          }, 300);
        }

      }
      else {
        console.error('Error during login:', error.response || error.message);
      }
    }
  }

  async function handleDeleteTag(tagId) {
    try {
      const response = await axiosWithToken.delete(
        `/api/content/deletetag/${videoId}/${tagId}`,
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        }
      );
      if (response.status == 200) {
        setHoveredTag(null)
        setVideo({
          ...video,
          tags: [...response.data.tags]
        })
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

  async function handleChangeVisibility() {
    try {
      const response = await axiosWithToken.patch(
        `/api/content/changevisibility/${videoId}`,
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setVideo({
          ...video,
          isPublic: response.data
        })
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

  async function handleAddRemovePlaylist(playlistId) {
    if (playlistUpdatingInProgress) {
      return;
    }
    try {
      setPlaylistUpdatingInProgress(true)
      const response = await axiosWithToken.post(
        `/api/playlist/addremovevideo/${playlistId}/${videoId}`,
        {
          headers: { 'Content-Type': 'application/json' },
          withCredentials: true
        }
      );
      if (response.status === 200) {
        setPlaylists(playlists.map(item =>
          item.id == playlistId
            ? response.data
            : item
        ))
      }
    } catch (error) {
      if (!error?.response) {
        console.log("No Server Response");
      }
      else {
        console.error('Error during request:', error.response || error.message);
      }
    } finally {
      setPlaylistUpdatingInProgress(false)
    }
  }

  const handleInputChange = (e) => {
    if (isUploading) {
      return;
    }
    const file = e.target.files[0];
    uploadPreview(file)
  };


  const handleUploadBtnClick = () => {
    fileInputRef.current.click();
  };

  async function uploadPreview(file) {
    if (!file) {
      alert('Please select a file!');
      return;
    }
    setIsUploading(true)
    const formData = new FormData();
    formData.append("file", file);
    try {
      const response = await axiosWithToken.patch(`/api/content/uploadpreview/${videoId}`,
        formData,
        {
          headers: {
            'Content-Type': 'multipart/form-data',
          }
        }
      );

      if (response.status === 200) {
        const result = response.data;
        console.log(result)
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
  const apiUrl = `https://localhost:44302/api/content/watch/${videoId}`;
  return (
    <>
      <div className='editForm'>
        <div className='editContainer'>
          <div className='dataBlock'>
            <div className="standardInfoBlock">
              <h3>General</h3>
              {
                Object.keys(formControls).map((controlName, index) => {
                  const control = formControls[controlName];
                  return (
                    <DataInput
                      key={`${index}_updateVideoField`}
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
            <div className="standardInfoBlock">
              <h3>Categories</h3>
              <div className="categories-container">
                {
                  video?.categories
                  &&
                  Object.keys(video?.categories).map((categoryIdx) => {
                    return (
                      <div
                        key={video?.categories[categoryIdx].id}
                        className="category-card"
                        onClick={() => handleDeleteCategory(video?.categories[categoryIdx].id)}
                        onMouseEnter={() => setHoveredCategory(video?.categories[categoryIdx].id)}
                        onMouseLeave={() => setHoveredCategory(null)}
                      >
                        {video?.categories[categoryIdx].name}
                        {
                          hoveredCategory === video?.categories[categoryIdx].id
                            ?
                            <img src={cross} alt=''></img>
                            :
                            <></>
                        }
                      </div>
                    )
                  })
                }
              </div>
              <div className="categoryAddContainer">
                <div className={"editCategorySelect " + (selectedCategory === "" ? "inactive_edit" : "")}>
                  <select onChange={(e) => (setSelectedCategory(e.target.value), setCategoryControl({ ...categoryControl, valid: true, errorMessage: "", touched: false }))}>
                    <option value="">Select a category...</option>
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
                <button className='submitBtn' onClick={handleAddCategory}>Add</button>
              </div>
              <span className={"errorMessage " + (categoryControl.shake ? "shake" : "")}>{(categoryControl.valid || !categoryControl.touched) ? <span>&nbsp;</span> : categoryControl.errorMessage}</span>
            </div>
            <div className="standardInfoBlock">
              <h3>Tags</h3>
              <div className="categories-container">
                {
                  video?.tags
                  &&
                  Object.keys(video?.tags).map((tagIdx) => {
                    return (
                      <div
                        key={video?.tags[tagIdx].id}
                        className="category-card"
                        onClick={() => handleDeleteTag(video?.tags[tagIdx].id)}
                        onMouseEnter={() => setHoveredTag(video?.tags[tagIdx].id)}
                        onMouseLeave={() => setHoveredTag(null)}>
                        {video?.tags[tagIdx].name}
                        {
                          hoveredTag === video?.tags[tagIdx].id
                            ?
                            <img src={cross} alt=''></img>
                            :
                            <></>
                        }
                      </div>
                    )
                  })
                }
              </div>

              <div className="addTagContainder">
                <CommentInput label="Enter a tag..." onChange={handleTagChange} value={tagControl.value}></CommentInput>
                <button className='submitBtn' onClick={handleAddTag}>Add</button>
              </div>
              <span className={"errorMessage " + (tagControl.shake ? "shake" : "")}>{(tagControl.valid || !tagControl.touched) ? <span>&nbsp;</span> : tagControl.errorMessage}</span>
            </div>
            <div className="standardInfoBlock previewBlock">
              <h3>Preview</h3>
              <div className="previewContainer">
                <img src={video?.previewUrl + `?timestamp=${Date.now()}`} className='thumbnailImg' alt="" />
                <div className="previewInfoBlock">
                  <p>You can change the thumbnail of your video here. Just press the button below...</p>
                  <button className='submitBtn' onClick={handleUploadBtnClick}>
                    {
                      isUploading
                        ?
                        <img src={loadanimation} alt="" />
                        :
                        <>Upload a thumbnail</>
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
          </div>
          <div className='videoBlock'>
            <div className="standardInfoBlock videoContainer">
              <h3>Video</h3>
              <video id="videoPlayer" width="640" height="360" controls>
                <source id="videoSource" src={apiUrl} type={video?.format ? `video/${video?.format.replaceAll('.', '')}` : "video/mp4"}></source>
                Your browser does not support the video tag.
              </video>
              <div className="videoGeneralInfo">
                <div className='videoInfo'>
                  <div className="particularInfo">
                    <p>Video</p>
                    <h4>{video?.title}</h4>
                  </div>
                  <div className="particularInfo">
                    <p>Format</p>
                    <h4>{video?.format}</h4>
                  </div>
                  <div className="particularInfo">
                    <p>File size</p>
                    <h4>{video?.size ? size(video?.size) : ""}</h4>
                  </div>
                </div>
                <div className='videoManagement'>
                  <div className="editVisibilitySelect">
                    <select value={video?.isPublic ? "public" : "private"} onChange={handleChangeVisibility}>
                      <option value="public">Public</option>
                      <option value="private">Private</option>
                    </select>
                  </div>
                  <button className='submitBtn addToPlaylistBtn' onClick={() => setIsAddingToPlaylist(true)}>Add to a playlist</button>
                </div>
              </div>

            </div>

          </div>
        </div>
      </div>
      {
        isAddingToPlaylist
          ?
          <div className='blackbg'>
            <div className='addingToPlaylistForm prevent-select'>
              <h2>Choose playlists!</h2>
              <div className="playlistContainer">
                {
                  Object.keys(playlists).map((playlistIdx) => {

                    return (
                      <label className="playlistItem" key={playlists[playlistIdx].id}>
                        <input type="checkbox" checked={playlists[playlistIdx].hasVideo} onChange={() => handleAddRemovePlaylist(playlists[playlistIdx].id)} />
                        <span>{playlists[playlistIdx].title}</span>
                      </label>
                    )
                  })
                }
              </div>
              <div className="buttonCont okBtnCont">
                <button className='submitBtn' onClick={() => setIsAddingToPlaylist(false)}>Ok</button>
              </div>
            </div>
          </div>
          :
          <></>
      }
    </>

  );
}

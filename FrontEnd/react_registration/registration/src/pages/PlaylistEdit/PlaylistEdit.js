import { useParams } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import { useEffect, useState, useRef } from 'react';
import './PlaylistEdit.css';
import { partial } from "filesize";
import loadanimation from '../../resources/loadanimation.gif'
import DataInput from '../../components/DataInput/DataInput';
import validateControl from '../../pages/Auth/Forms/Validations/GeneralValidation'
import CommentInput from '../../components/CommentInput/CommentInput';
import cross from "../../resources/cross-red.png"
export default function PlaylistEdit() {
    const { playlistId } = useParams()
    useAxiosWithToken();
    const navigate = useNavigate();
    const [isUpdating, setIsUpdating] = useState(false);
    const [playlist, setPlaylist] = useState({});

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

    useEffect(() => {
        GetPlaylist();
    }, []);
    useEffect(() => {
        handleReset();
    }, [playlist.title, playlist.description]);

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
        if (playlist) {
            setFormControls((prevState) => ({
                ...prevState,
                title: {
                    ...prevState.title,
                    value: playlist.title || '',
                    touched: false,
                    valid: true
                },
                description: {
                    ...prevState.description,
                    value: playlist.description || '',
                    touched: false,
                    valid: true
                }
            }));
        }
    }
    async function GetPlaylist() {
        try {
            const response = await axiosWithToken.get(
                `/api/playlist/getplaylist/${playlistId}`,
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setPlaylist(response.data)
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
                description: formControls.description.value
            }
            setIsUpdating(true)
            try {
                const response = await axiosWithToken.patch(
                    `/api/playlist/updateplaylist/${playlistId}`,
                    reqData,
                    {
                        headers: { 'Content-Type': 'application/json' },
                        withCredentials: true
                    }
                );
                if (response.status == 200) {
                    setPlaylist({
                        ...playlist,
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
                    console.error('Error during updating:', error.response || error.message);
                }
                shakeInvalidElems();
            } finally {
                setIsUpdating(false)
            }
        }

    }


    async function handleChangeVisibility() {
        try {
            const response = await axiosWithToken.patch(
                `/api/playlist/changevisibility/${playlistId}`,
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setPlaylist({
                    ...playlist,
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


    return (
        <div className='editForm'>
            <div className='editContainer centerDiv'>
                <div className='dataBlockPlaylist'>
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
                        <h3>Visibilty</h3>
                        <div className="editVisibilitySelect">
                            <select value={playlist?.isPublic ? "public" : "private"} onChange={handleChangeVisibility}>
                                <option value="public">Public</option>
                                <option value="private">Private</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

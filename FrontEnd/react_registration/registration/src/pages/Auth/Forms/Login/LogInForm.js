import { useContext, useState } from 'react';
import Input from '../components/Input/Input';
import { FormControlsLogInContext } from '../../Contexts/FormControlsLogInProvider';
import { IsSmallContext } from '../../../../Contexts/IsSmallContext';
import { IsSubmitLoadingContext } from '../../Contexts/IsSubmitLoadingContext';
import googleImage from '../../resources/images/google2.png';
import loadanimation from '../../../../resources/loadanimation.gif'
import axios from "../../../../API/axioscfg"
import validateControl from '../Validations/GeneralValidation'
import useAuth from "../../../../hooks/useAuth";
import './LogInForm.css';
import '../Forms.css';
import { useGoogleLogin } from '@react-oauth/google';
import { useNavigate } from 'react-router-dom';

export function LogInForm(props) {
  const { isSmall } = useContext(IsSmallContext);
  const { formControls, setFormControls } = useContext(FormControlsLogInContext)
  const { setAuth } = useAuth();
  const { isSubmitLoading, setIsSubmitLoading } = useContext(IsSubmitLoadingContext);
  const navigate = useNavigate();
  const googleLogin = useGoogleLogin({
    onSuccess: async (tokenResponse) => {

      try {
        const response = await axios.get("https://www.googleapis.com/oauth2/v3/userinfo", {
          headers: {
            "Authorization": `Bearer ${tokenResponse.access_token}`
          }
        })
        console.log("Google login success:", response);
        const res = await axios.post('https://localhost:44302/api/auth/google-response', {
          email: response.data.email,
          email_verified: response.data.email_verified,
          family_name: response.data.family_name,
          given_name: response.data.given_name,
          picture: response.data.picture
        }, {
          withCredentials: true
        });
        console.log(res.data.accessToken);
        localStorage.setItem('accessToken', res.data.accessToken);
        setAuth(res.data.user)
        navigate("/");
      } catch (err) {
        console.log('Ошибка при авторизации через Google', err);
      }

    },
    onError: () => {
      console.log('Не удалось войти через Google');
    }
  });
  let order = -1;
  function GetDelay() {
    order += 1;
    return {
      transitionProperty: "filter, left, color, background-color",
      transitionDelay: props.isSignUp ? `${order / 9}s, ${order / 9}s, 0s, 0s` : `${order / 9 + 1}s, ${order / 9 + 1}s, 0s, 0s`
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

    if (controlName === "email") {
      formControlsCopy.password = {
        ...formControlsCopy.password,
        touched: false,
        valid: true,
        errorMessage: ""
      };
    }


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

  async function handleSubmit(e) {
    e.preventDefault();
    if (!IsFormValid()) {
      shakeInvalidElems();
    }
    else {
      setIsSubmitLoading(true)
      const loginData = {
        email: formControls.email.value,
        password: formControls.password.value
      }

      try {
        const response = await axios.post(
          "/api/auth/login",
          loginData,
          {
            headers: { 'Content-Type': 'application/json' },
            withCredentials: true
          }
        );
        if (response.data.accessToken) {
          localStorage.setItem('accessToken', response.data.accessToken);
          setAuth(response.data.user)
          navigate("/");
        }
      } catch (error) {
        setAuth(null);
        if (!error?.response) {
          console.log("No Server Response");
        }
        else if (error.response?.status === 400) {
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
        setIsSubmitLoading(false);
      }
    }
  }

  const elemToRender = props.isSignUp ? "logInElemAnim" : "logInElem"
  return (
    <div className={isSmall ? "logInSmall" : "logIn"}>
      <div className={"logo " + (isSmall ? "" : elemToRender)} style={GetDelay()}>
        <p>Log In</p>
        <div className="dash"></div>
      </div>
      <form action="#" method='POST' onSubmit={handleSubmit} noValidate="novalidate">
        {
          Object.keys(formControls).map((controlName, index) => {
            const control = formControls[controlName];
            return (
              <Input
                key={`${index}_logInField`}
                elemToRender={elemToRender}
                style={GetDelay()}
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
        <div className="placeholder"></div>
        <button type="submit" className={"submitBtn " + (isSmall ? "" : elemToRender)} style={GetDelay()} disabled={isSubmitLoading ? true : false}>
          {
            isSubmitLoading
              ?
              <img src={loadanimation}></img>
              :
              <p>Log In</p>
          }
        </button>
        {isSmall ? null :
          <span className={"linkText " + elemToRender} style={GetDelay()}>Don't have an account? <a className={"link " + (props.isSignUp ? "disabled" : "")} href="" onClick={props.onLink}>Sign Up</a></span>
        }
        <div className={"divider " + elemToRender} style={GetDelay()}>
          <div className='line'></div>
          <p>&nbsp;&nbsp;OR&nbsp;&nbsp;</p>
          <div className='line'></div>
        </div>

        <button type='button' className={"googleLogin " + (isSmall ? "" : elemToRender)} style={GetDelay()} disabled={isSubmitLoading ? true : false} onClick={() => googleLogin()}>
          <img src={googleImage} alt="" className='googleLogo' />
          <p>Continue with Google</p>
        </button>
      </form>
    </div>
  );
}
import { useState, useRef, useEffect, useContext } from 'react';
import Input from '../components/Input/Input';
import './SignUpForm.css';
import '../Forms.css';
import loadanimation from '../../../../resources/loadanimation.gif'
import { FormControlsSignUpContext } from '../../Contexts/FormControlsSignUpProvider';
import { IsSubmitLoadingContext } from '../../Contexts/IsSubmitLoadingContext';
import axios from "../../../../API/axioscfg"
import { IsSmallContext } from '../../../../Contexts/IsSmallContext';
import validateControl from '../Validations/GeneralValidation'
export function SignUpForm(props) {
  const { isSmall } = useContext(IsSmallContext);
  const { formControls, setFormControls } = useContext(FormControlsSignUpContext)
  const [selected, setSelected] = useState(0);
  const { isSubmitLoading, setIsSubmitLoading } = useContext(IsSubmitLoadingContext);

  let order = -1;
  function GetDelay() {
    order += 1
    return {
      transitionProperty: "filter, right, color, background-color",
      transitionDelay: props.isSignUp ? `${order / 9 + 1}s, ${order / 9 + 1}s, 0s, 0s` : `${order / 9}s, ${order / 9}s, 0s, 0s`
    }
  }

  function IsFormValid() {
    let isFormValid = true
    Object.keys(formControls[selected]).forEach(name => {
      isFormValid = formControls[selected][name].valid && isFormValid
    })
    return isFormValid;
  }

  function shakeInvalidElems() {
    if (!IsFormValid()) {
      const formControlsCopy = formControls.map(group => {
        return {
          ...group
        };
      });
      Object.keys(formControlsCopy[selected]).map((controlName) => {
        const control = formControlsCopy[selected][controlName];
        if (!control.valid) {
          control.shake = true;
          if (!control.touched) {
            control.touched = true;
            const passwordValue = formControls[selected]?.password?.value;
            const [isValid, newErrorMessage] = validateControl(control.value, control.validation, control.name, controlName === 'repassword' ? passwordValue : '')
            control.valid = isValid
            control.errorMessage = newErrorMessage
          }

        }
      })
      setFormControls(formControlsCopy)
      setTimeout(() => {
        const resetControls = formControls.map(group => {
          return {
            ...group
          };
        });;
        Object.keys(resetControls[selected]).forEach((controlName) => {
          resetControls[selected][controlName].shake = false;
        });
        setFormControls(resetControls);
      }, 300);
    }

  }


  async function continueHandler(e) {
    e.preventDefault();
    if (!IsFormValid()) {
      shakeInvalidElems();
    }
    else {
      setIsSubmitLoading(true);
      if (selected == 0) {
        const signUpData = {
          email: formControls[0].email.value,
          password: formControls[0].password.value,
          repassword: formControls[0].repassword.value
        }
        try {
          const response = await axios.post(
            "/api/auth/register-step1",
            signUpData,
            {
              headers: { 'Content-Type': 'application/json' },
              withCredentials: true
            }
          );
          if (selected < formControls.length - 1) {
            setSelected(prev => prev + 1)
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
              formControlsCopy[0][fieldName].valid = false
              formControlsCopy[0][fieldName].errorMessage = "* "+particularError.details
              formControlsCopy[0][fieldName].touched = true
            }
            setFormControls(formControlsCopy)

          }
          else {
            console.error('Error during registration:', error.response || error.message);
          }
        } finally {
          setIsSubmitLoading(false);
        }


      }
      else if (selected == 1) {
        const signUpData = {
          email: formControls[0].email.value,
          firstname: formControls[1].firstname.value,
          lastname: formControls[1].lastname.value,
          birthdate: formControls[1].birthdate.value.replaceAll(" ", "")
        }
        try {
          const response = await axios.post(
            "/api/auth/register-step2",
            signUpData,
            {
              headers: { 'Content-Type': 'application/json' },
              withCredentials: true
            }
          );
          if (selected < formControls.length - 1) {
            setSelected(prev => prev + 1)
          }
        } catch (error) {
          if (!error?.response) {
            console.log("No Server Response");
          }
          else if (error.response?.status === 400) {
            const errorData = error.response.data;
            const formControlsCopy = { ...formControls }
            for (const particularError of errorData) {
              const fieldName = particularError.field.toLowerCase();
              formControlsCopy[1][fieldName].valid = false
              formControlsCopy[1][fieldName].errorMessage = "* "+particularError.details
              formControlsCopy[1][fieldName].touched = true
            }
            setFormControls(formControlsCopy)

          }
          else if (error.response?.status === 404) {
            alert(error.message);
          }
          else {
            console.error('Error during registration:', error.response || error.message);
          }
        } finally {
          setIsSubmitLoading(false);
        }
      }
      else {
        const signUpData = {
          email: formControls[0].email.value,
          code: formControls[2].code.value
        }
        try {
          const response = await axios.post(
            "/api/auth/register-step3",
            signUpData,
            {
              headers: { 'Content-Type': 'application/json' },
              withCredentials: true
            }
          );
          if (selected < formControls.length - 1) {
            setSelected(prev => prev + 1)
          }
          props.setIsSignUp(false);
        } catch (error) {
          if (!error?.response) {
            console.log("No Server Response");
          }
          else if (error.response?.status === 400) {
            const errorData = error.response.data;
            const formControlsCopy = { ...formControls }
            for (const particularError of errorData) {
              const fieldName = particularError.field.toLowerCase();
              formControlsCopy[2][fieldName].valid = false
              formControlsCopy[2][fieldName].errorMessage = "* "+particularError.details
              formControlsCopy[2][fieldName].touched = true
            }
            setFormControls(formControlsCopy)

          }
          else if (error.response?.status === 404) {
            alert(error.message);
          }
          else {
            console.error('Error during registration:', error.response || error.message);
          }
        } finally {
          setIsSubmitLoading(false);
        }
      }

      shakeInvalidElems();

    }
  }


  function backHandler() {
    if (selected > 0) {
      setSelected(prev => prev - 1)
    }
  }
  function changeHandler(e, controlName) {
    const formControlsCopy = formControls.map(group => {
      return {
        ...group
      };
    });
    const control = { ...formControlsCopy[selected][controlName] }

    const passwordValue = formControls[selected]?.password?.value;

    control.value = e.target.value
    const [isValid, newErrorMessage] = validateControl(e.target.value, control.validation, control.name, controlName === 'repassword' ? passwordValue : '')
    control.valid = isValid
    control.errorMessage = newErrorMessage
    control.touched = true;

    if (controlName === 'password') {
      const repasswordControl = { ...formControlsCopy[selected]['repassword'] };
      const [isRepasswordValid, rePasswordErrorMessage] = validateControl(
        repasswordControl.value,
        repasswordControl.validation,
        repasswordControl.name,
        control.value
      );
      repasswordControl.valid = isRepasswordValid;
      repasswordControl.errorMessage = rePasswordErrorMessage;
      repasswordControl.touched = true;
      formControlsCopy[selected]['repassword'] = repasswordControl;
    }


    formControlsCopy[selected][controlName] = control
    setFormControls(formControlsCopy)
  }
  const elemToRender = props.isSignUp ? "signUpElem" : "signUpElemAnim"
  return (
    <div className={isSmall ? "signUpSmall" : "signUp"}>
      <div className={"logo " + (isSmall ? "" : elemToRender)} style={GetDelay()}>
        <p>Sign Up</p>
        <div className="dash"></div>
      </div>
      <form action="#" noValidate="novalidate">
        <div className={'pageSelector ' + (isSmall ? "" : elemToRender)} style={GetDelay()}>
          {
            Object.keys(formControls).map((cn, index) => {
              return (
                <div key={`${index}_selector`} className={'selector ' + (selected === index ? "active" : "")}></div>
              )
            })
          }
        </div>
        {
          Object.keys(formControls[selected]).map((controlName, index) => {
            const control = formControls[selected][controlName];
            return (
              <Input
                key={`${index}_signUpField`}
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
        <div className={"btnContainer " + (isSmall ? "" : elemToRender)} style={GetDelay()}>
          <button type="button" className={"submitBtn btnBack "} onClick={backHandler} disabled={selected === 0 ? true : false}>🡐</button>
          <button type="button" className={"submitBtn btnContinue "} onClick={continueHandler}  disabled={isSubmitLoading ? true : false}>
            {
              isSubmitLoading
                ?
                <img src={loadanimation}></img>
                :
                <p>{selected === formControls.length - 1 ? "Sign Up" : "Continue"}</p>
            }
          </button>
        </div>

        {isSmall ? null :
          <span className={"linkText " + elemToRender} style={GetDelay()}>Already have an account? <a className={"link " + (props.isSignUp ? "" : "disabled")} href="" onClick={props.onLink}>Log In</a></span>
        }

      </form>
    </div>

  );
}

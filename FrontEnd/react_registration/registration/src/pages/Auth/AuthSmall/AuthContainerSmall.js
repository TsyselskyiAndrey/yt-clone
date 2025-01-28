import { useState, useRef, useEffect } from 'react';
import { LogInForm } from "../Forms/Login/LogInForm";
import { SignUpForm } from "../Forms/Registration/SignUpForm";
import './AuthContainerSmall.css';

export default function AuthContainerSmall({ isSignUp, setIsSignUp }) {

  return (
    <>
      <div className="containerForSmall" >
        <div className="tabs">
          <div className={"tab " + (isSignUp ? "" : "active")} onClick={() => setIsSignUp(false)}>
            Log In
          </div>
          <div className={"tab " + (isSignUp ? "active" : "")} onClick={() => setIsSignUp(true)}>
            Sign Up
          </div>
        </div>
        {
          isSignUp
          ?
          <SignUpForm isSignUp={isSignUp} setIsSignUp={setIsSignUp}></SignUpForm>
          :
          <LogInForm isSignUp={isSignUp} setIsSignUp={setIsSignUp}></LogInForm>
        }
        
      </div>

    </>
  );
}

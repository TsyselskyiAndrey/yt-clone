import { useState, useRef, useEffect } from 'react';
import { LogInForm } from "../Forms/Login/LogInForm";
import { SignUpForm } from "../Forms/Registration/SignUpForm";
import './AuthContainer.css';

export default function AuthContainer({ isSignUp, setIsSignUp }) {
  const entity = useRef(null);
  const entityBack = useRef(null);
  useEffect(() => {
    function handleResize() {
      if (entity.current) {
        entity.current.style.transition = "none";
        setTimeout(() => {
          if (entityBack.current) {
            entity.current.style.transition = "all 1.3s ease 0.6s";
          }
        }, 0);
      }
      if (entityBack.current) {
        entityBack.current.style.transition = "none";
        setTimeout(() => {
          if (entityBack.current) {
            entityBack.current.style.transition = "all 1.3s ease 0.6s";
          }
        }, 0);
      }
    }
    window.addEventListener("resize", handleResize);
    return () => {
      window.removeEventListener('resize', handleResize);
    }
  }, [])
  function handleLink(e) {
    e.preventDefault();
    setIsSignUp(!isSignUp);
  }
  return (
    <>
      <div className="containerForLarge">
        <LogInForm onLink={handleLink} isSignUp={isSignUp} setIsSignUp={setIsSignUp}></LogInForm>
        <h2 className={"description " + (isSignUp ? "descriptionLeft" : "descriptionLeftAnim")} style={
          {
            transitionProperty: "filter, left",
            transitionDelay: isSignUp ? `${0 / 9 + 1}s, ${0 / 9 + 1}s` : `${0 / 9}s, ${0 / 9}s`
          }
        }>"Stream Freely, Watch Endlessly – Your Entertainment, Your Way!", – Vibeo</h2>
        <h2 className={"year " + (isSignUp ? "yearLeft" : "yearLeftAnim")} style={
          {
            transitionProperty: "filter, left",
            transitionDelay: isSignUp ? `${2 / 9 + 1}s, ${2 / 9 + 1}s` : `${1 / 9}s, ${1 / 9}s`
          }
        }> 🚀 {(new Date()).getFullYear()}</h2>
        <SignUpForm onLink={handleLink} isSignUp={isSignUp} setIsSignUp={setIsSignUp}></SignUpForm>
        <h2 className={"description " + (isSignUp ? "descriptionRightAnim" : "descriptionRight")} style={
          {
            transitionProperty: "filter, right",
            transitionDelay: isSignUp ? `${0 / 9}s, ${0 / 9}s` : `${0 / 9 + 1}s, ${0 / 9 + 1}s`
          }
        }>"Stream Freely, Watch Endlessly – Your Entertainment, Your Way!", – Vibeo</h2>
        <h2 className={"year " + (isSignUp ? "yearRightAnim" : "yearRight")} style={
          {
            transitionProperty: "filter, right",
            transitionDelay: isSignUp ? `${1 / 9}s, ${1 / 9}s` : `${2 / 9 + 1}s, ${2 / 9 + 1}s`
          }
        }>{(new Date()).getFullYear()} <span style={{ display: "inlineBlock", transform: "scaleX(-1)", textShadow: "-3px 3px #9239ff" }}></span>🚀</h2>
        <div className={"entity" + (!isSignUp ? " entity_change" : "")} id="entity" ref={entity}>
          <div className={"back-container" + (!isSignUp ? " back-container-change" : "")} ref={entityBack}></div>
        </div>
      </div>
    </>
  );
}

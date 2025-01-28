import { useState, useContext } from 'react';
import AuthContainer from "./AuthLarge/AuthContainer";
import AuthContainerSmall from "./AuthSmall/AuthContainerSmall";
import { IsSmallContext } from '../../Contexts/IsSmallContext';
import './Auth.css';
import { FormControlsLogInProvider } from './Contexts/FormControlsLogInProvider';
import { FormControlsSignUpProvider } from './Contexts/FormControlsSignUpProvider';
import { IsSubmitLoadingProvider } from './Contexts/IsSubmitLoadingContext';
import { GoogleOAuthProvider } from '@react-oauth/google';

export default function Auth() {
    const [isSignUp, setIsSignUp] = useState(false);
    const { isSmall } = useContext(IsSmallContext);
    return (
        <GoogleOAuthProvider clientId='784050525038-kni6d3in48kges4js2jlkaad7d7iqenn.apps.googleusercontent.com'>
            <FormControlsLogInProvider>
                <FormControlsSignUpProvider>
                    <IsSubmitLoadingProvider>
                        <div className='authContainer'>
                            {
                                isSmall
                                    ?
                                    <AuthContainerSmall isSignUp={isSignUp} setIsSignUp={setIsSignUp}></AuthContainerSmall>
                                    :
                                    <AuthContainer isSignUp={isSignUp} setIsSignUp={setIsSignUp}></AuthContainer>
                            }
                        </div>
                    </IsSubmitLoadingProvider>
                </FormControlsSignUpProvider>
            </FormControlsLogInProvider>
        </GoogleOAuthProvider>
    );
}

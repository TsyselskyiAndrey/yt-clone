import { useNavigate } from 'react-router-dom';
import './InfoWindow.css';
export default function InfoWindow(props) {
    const navigate = useNavigate();
    function handleLink(e) {
        e.preventDefault();
        navigate("/auth")
      }
    return (
        <>
            <div className='blackbg'>
                <div className="confirmationWindow">
                    <h3>{props.message} <span className="linkText"><a className="link lingUnderline" onClick={handleLink}>Sign in!</a></span></h3>
                    <div className='btnConfirmContainer'>
                        <button className='confirmWindowBtns confirmBtn' onClick={props.onOk}>Ok</button>
                    </div>
                </div>
            </div>

        </>
    );
}


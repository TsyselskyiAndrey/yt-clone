import { useState, useId } from 'react';
import './ConfirmationWindow.css';
import cross from '../resources/cross.png';
export default function ConfirmationWindow(props) {
    return (
        <>
            <div className='blackbg'>
                <div className="confirmationWindow">
                    <h3>{props.message}</h3>
                    <div className='btnConfirmContainer'>
                        <button className='confirmWindowBtns cancelBtn' onClick={props.onCancel}>Cancel</button>
                        <button className='confirmWindowBtns confirmBtn' onClick={props.onConfirm}>Confirm</button>
                    </div>
                </div>
            </div>

        </>
    );
}


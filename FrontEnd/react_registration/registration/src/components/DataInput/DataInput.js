import { useId } from "react";

import "./DataInput.css"

export default function DataInput(props) {
    const id = props.type + "_" + useId();
    function dateChangeHandler(e) {
        let value = e.target.value.replace(/[^0-9]/g, '');
        let cursorPosition = e.target.selectionStart;

        if (value.length > 8) {
            value = value.slice(0, 8);
        }
        if (value.length >= 3) {
            value = value.slice(0, 2) + ' / ' + value.slice(2);
        }
        if (value.length >= 8) {
            value = value.slice(0, 7) + ' / ' + value.slice(7);
        }
    
        e.target.value = value; 

        if (cursorPosition === 3 || cursorPosition === 8) {
            cursorPosition += 3; 
        }
    
        e.target.setSelectionRange(cursorPosition, cursorPosition);
    }
    return (
        <>
            <div className="coolinput">
                <label htmlFor={props.name} className="text">{props.label}</label>
                {
                    props.type == "textarea"
                        ?
                        <textarea placeholder="Write here..." value={props.value} onChange={(e) => props.onChange(e)} name={props.name} id={id} className="input" autoComplete="off"></textarea>
                        :
                        <input type={ props.type==="date" ? "text" : props.type} placeholder="Write here..." value={props.value} onChange={(e)=>{props.type==="date" && dateChangeHandler(e); props.onChange(e);}} name={props.name} id={id} className="input" autoComplete="off"></input>
                }
                <span className={"errorMessage " + (props.shake ? "shake" : "")}>{(props.valid || !props.touched) ? <span>&nbsp;</span> : props.errorMessage}</span>
            </div>
        </>
    );
}
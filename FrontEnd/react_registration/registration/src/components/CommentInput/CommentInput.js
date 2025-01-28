
import { useState } from "react";
import './CommentInput.css'
const CommentInput = (props) => {
  const [isFocused, setIsFocused] = useState(false);

  return (
    <div className={`input-container ${isFocused ? "focused" : ""}`}>
      <input
        type="text"
        className="input-field"
        placeholder={props.label}
        onFocus={() => setIsFocused(true)}
        onBlur={() => setIsFocused(false)}
        onChange={props.onChange}
        value={props.value}
        onFocusCapture={props.OnFocus}
      />
      <div className="underline"></div>
      <div className="underline-placeholder"></div>
    </div>
  );
};

export default CommentInput;

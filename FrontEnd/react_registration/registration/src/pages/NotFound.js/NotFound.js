import { Link } from "react-router-dom"
import './NotFound.css';

export default function NotFound() {
  return (
    <div className="notFoundContainer">
        <h1>Oops!</h1>
        <div className="notFoundInfo">
            <h2>404 - PAGE NOT FOUND!</h2>
            <p>The page you are looking for might have been removed, had its name changed or is temporarily unavailable.</p>
            <Link to="/"><button className="submitBtn">GO TO THE HOMEPAGE</button></Link>
        </div>
    </div>
  )
}

import './ChannelManagement.css';
import { Outlet, useLocation, useNavigate } from 'react-router-dom';
export default function ChannelManagement() {
  const location = useLocation();
  const navigate = useNavigate();
  return (
    <div className="contentPanel">
      <h2>Channel content</h2>
      <div className='navDivs prevent-select'>
        <div className={"navDiv " + (location.pathname==='/channelmanagement/content/videos' ? "selected" : "")} onClick={() => navigate("/channelmanagement/content/videos")}>Videos</div>
        <div className={"navDiv " + (location.pathname==='/channelmanagement/content/playlists' ? "selected" : "")} onClick={() => navigate("/channelmanagement/content/playlists")}>Playlists</div>
      </div>
      <Outlet />
    </div>

  );
}

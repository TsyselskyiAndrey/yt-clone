import { PlaylistVideoProvider } from '../../Contexts/PlaylistVideoContext';
import './PlaylistManagement.css';
import { Outlet, useLocation, useNavigate, useParams, generatePath } from 'react-router-dom';

export default function PlaylistManagement() {
  const location = useLocation();
  const navigate = useNavigate();
  const { playlistId } = useParams();

  return (
    <PlaylistVideoProvider playlistId = {playlistId}>
      <div className="contentPanel">
        <h2>Playlist management</h2>
        <div className='navDivs prevent-select'>
          <div
            className={"navDiv " + (location.pathname === `/channelmanagement/playlist/${playlistId}/edit` ? "selected" : "")}
            onClick={() => navigate(`/channelmanagement/playlist/${playlistId}/edit`)}
          >
            Edit playlist
          </div>
          <div
            className={"navDiv " + (location.pathname === `/channelmanagement/playlist/${playlistId}/videos` ? "selected" : "")}
            onClick={() => navigate(`/channelmanagement/playlist/${playlistId}/videos`)}
          >
            Manage videos
          </div>
        </div>
        <Outlet />
      </div>
    </PlaylistVideoProvider>

  );
}

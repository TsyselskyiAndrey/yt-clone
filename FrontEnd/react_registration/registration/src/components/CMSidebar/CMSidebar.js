import { useLocation, useNavigate } from 'react-router-dom';
import useAuth from "../../hooks/useAuth";
import '../CMSidebar/CMSidebar.css';
import content from '../resources/icons/content.svg'
import chart from '../resources/chart.png';
import magicwand from '../resources/magicwand.png';
import share from '../resources/share.png';
export default function Sidebar(props) {
  const { auth } = useAuth();
  const location = useLocation();
  const navigate = useNavigate();
  return (
    <>
      <nav className={"prevent-select sidebarCM " + (props.menuOpen && "open")}>

        <div className='avatar'>
          <img src={auth?.logoUrl + `?timestamp=${Date.now()}`} onClick={()=>navigate(`/channel/${auth?.channelId}`)}></img>
        </div>
        <div className="channelInfo">
          <h4>Your channel</h4>
          <p>{auth.title}</p>
        </div>
        <div className='sidebarLinks'>
          <div className={"sidebarLinkCM " + (location.pathname.includes('/channelmanagement/content') ? "chosen" : "")} onClick={() => navigate("/channelmanagement/content/videos")}>
            <img src={content}></img>
            <p>Content</p>
          </div>
          <div className={"sidebarLinkCM " + (location.pathname === '/channelmanagement/statistics' ? "chosen" : "")} onClick={() => navigate("/channelmanagement/statistics")}>
            <img src={chart}></img>
            <p>Statistics</p>
          </div>
          <div className={"sidebarLinkCM " + (location.pathname === '/channelmanagement/customisation' ? "chosen" : "")} onClick={() => navigate("/channelmanagement/customisation")}>
            <img src={magicwand}></img>
            <p>Customisation</p>
          </div>
        </div>

      </nav>
    </>
  );
}

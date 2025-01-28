import { useState, useRef, useEffect } from 'react';
import { useLocation, useMatch, useNavigate, useParams } from 'react-router-dom';
import useAuth from "../../hooks/useAuth";
import './Sidebar.css';
import home from '../resources/icons/home.svg'
import subscriptions from '../resources/icons/subscriptions.svg'
import history from '../resources/icons/history.svg'
import watchlater from '../resources/icons/watchlater.svg'
import explore from '../resources/icons/explore.svg'
import playlists from '../resources/icons/playlists.svg'
import liked from '../resources/icons/liked.svg'
import home_active from '../resources/icons/home-active.svg'
import subscriptions_active from '../resources/icons/subscriptions-active.svg'
import history_active from '../resources/icons/history-active.svg'
import watchlater_active from '../resources/icons/watchlater-active.svg'
import playlists_active from '../resources/icons/playlists-active.svg'
import liked_active from '../resources/icons/liked-active.svg'
import logo from '../resources/logo.png';
import mychannel from '../resources/icons/mychannel.svg';
import mychannel_active from '../resources/icons/mychannel-active.svg';
import star from '../../resources/star.png'
import star_active from '../../resources/star_active.png'

export default function Sidebar(props) {
  const { auth } = useAuth();
  const location = useLocation();
  const navigate = useNavigate();
  const isMyChannelSelectedVid = useMatch('/channel/:channelId');
  const isMyChannelSelectedPl = useMatch('/channel/:channelId/playlists');
  const { channelId } = useParams();
  const isMyChannelSelected = (isMyChannelSelectedVid || isMyChannelSelectedPl) && channelId == auth?.channelId

  return (
    <>
      <nav className={"prevent-select sidebar-large " + (props.menuOpen ? "activated" : "")}>
        <div className="headerBlock">
          <div className={"menu_icon " + (props.menuOpen ? "clicked" : "")} onClick={() => props.setMenuOpen(!props.menuOpen)}>
            <span className="one"></span>
            <span className="two"></span>
            <span className="three"></span>
          </div>
          <div className="mainlogo">
            <img src={logo} alt="mainlogo" />
            <p>Vibeo</p>
          </div>
        </div>
        <div className={"sidebarLink-large " + (location.pathname === '/' ? "chosen" : "")} onClick={() => navigate("/")}>
          <img src={location.pathname === '/' ? home_active : home}></img>
          <div>Home</div>
        </div>
        <div className={"sidebarLink-large " + (location.pathname === '/subscriptions' ? "chosen" : "")} onClick={() => navigate("/subscriptions")}>
          <img src={location.pathname === '/subscriptions' ? subscriptions_active : subscriptions}></img>
          <div>Subscriptions</div>
        </div>
        <hr />
        <div className={"sidebarLink-large " + (location.pathname === '/history' ? "chosen" : "")} onClick={() => navigate("/history")}>
          <img src={location.pathname === '/history' ? history_active : history}></img>
          <div>History</div>
        </div>
        <div className={"sidebarLink-large " + (location.pathname === '/playlists' ? "chosen" : "")} onClick={() => navigate("/playlists")}>
          <img src={location.pathname === '/playlists' ? playlists_active : playlists}></img>
          <div>Playlists</div>
        </div>
        <div className={"sidebarLink-large " + (location.pathname === '/watchlater' ? "chosen" : "")} onClick={() => navigate("/watchlater")}>
          <img src={location.pathname === '/watchlater' ? watchlater_active : watchlater}></img>
          <div>Watch later</div>
        </div>
        <div className={"sidebarLink-large " + (location.pathname.includes('/explore') ? "chosen" : "")} onClick={() => navigate("/explore")}>
          <img src={location.pathname.includes('/explore') ? explore : explore}></img>
          <div>Explore</div>
        </div>
        <div className={"sidebarLink-large " + (location.pathname === '/liked' ? "chosen" : "")} onClick={() => navigate("/liked")}>
          <img src={location.pathname === '/liked' ? liked_active : liked}></img>
          <div>Liked videos</div>
        </div>
        <div className={"sidebarLink-large " + (location.pathname === '/favorite' ? "chosen" : "")} onClick={() => navigate("/favorite")}>
          <img src={location.pathname === '/favorite' ? star_active : star}></img>
          <div>Favorite</div>
        </div>
        <hr />
        {
          auth?.roles?.includes("channel_owner")
          &&
          <>
            <div className={"sidebarLink-large " + (isMyChannelSelected ? "chosen" : "")} onClick={(e) => {e.stopPropagation(); navigate(`/channel/${auth.channelId}`)}}>
              <img src={isMyChannelSelected ? mychannel_active : mychannel}></img>
              <div>My channel</div>
            </div>
            <hr />
          </>

        }

      </nav>

      <nav className="prevent-select sidebar">
        <div className="sidebarLink" onClick={() => navigate("/")}>
          <img src={location.pathname === '/' ? home_active : home}></img>
          <div>Home</div>
        </div>
        <div className="sidebarLink" onClick={() => navigate("/subscriptions")}>
          <img src={location.pathname === '/subscriptions' ? subscriptions_active : subscriptions}></img>
          <div>Subscriptions</div>
        </div>
        <div className="sidebarLink" onClick={() => navigate("/history")}>
          <img src={location.pathname === '/history' ? history_active : history}></img>
          <div>History</div>
        </div>
        <div className="sidebarLink" onClick={() => navigate("/playlists")}>
          <img src={location.pathname === '/playlists' ? playlists_active : playlists}></img>
          <div>Playlists</div>
        </div>
        <div className="sidebarLink" onClick={() => navigate("/watchlater")}>
          <img src={location.pathname === '/watchlater' ? watchlater_active : watchlater}></img>
          <div>Watch later</div>
        </div>
      </nav>

      <div className={"eclipse " + (props.menuOpen ? "" : "notactive")} onClick={() => props.setMenuOpen(false)}></div>
    </>
  );
}

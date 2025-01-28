import './Profile.css';
import { Outlet, useLocation, useParams } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import { useEffect, useState, useRef } from 'react';
export default function Profile() {
  useAxiosWithToken();
  const navigate = useNavigate();
  const location = useLocation();
  return (
    <>
      <div className="contentPanel">
        <h2>Profile management</h2>
        <div className='navDivs prevent-select'>
          <div
            className={"navDiv " + (location.pathname === `/profile` ? "selected" : "")}
            onClick={() => navigate(`/profile`)}
          >
            Profile
          </div>
          <div
            className={"navDiv " + (location.pathname === `/profile/statistics` ? "selected" : "")}
            onClick={() => navigate(`/profile/statistics`)}
          >
            Statistics
          </div>
        </div>
        <Outlet />
        
      </div>

    </>
  );
}

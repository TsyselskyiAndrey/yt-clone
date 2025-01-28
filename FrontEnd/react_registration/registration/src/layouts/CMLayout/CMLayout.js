import { Outlet } from "react-router-dom"
import './CMLayout.css';
import CMSidebar from '../../components/CMSidebar/CMSidebar';
import { useState } from 'react';
import CMHeader from "../../components/CMHeader/CMHeader";
import { VideoProvider } from '../../Contexts/VideoContext';
import { PlaylistProvider } from '../../Contexts/PlaylistContext';
import { PlaylistVideoProvider } from "../../Contexts/PlaylistVideoContext";
export default function CMLayout() {
  const [menuOpen, setMenuOpen] = useState(false)
  return (
    <VideoProvider>
      <PlaylistProvider>
        <div className='CMContainer'>
          <CMHeader menuOpen={menuOpen} setMenuOpen={setMenuOpen}></CMHeader>
          <CMSidebar menuOpen={menuOpen} setMenuOpen={setMenuOpen}></CMSidebar>
          <main>
            <Outlet></Outlet>
          </main>
        </div>
      </PlaylistProvider>
    </VideoProvider>

  )
}

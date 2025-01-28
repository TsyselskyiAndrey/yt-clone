import Auth from "./pages/Auth/Auth";
import Home from "./pages/Home/Home";
import ChannelManagement from "./pages/ChannelManagement/ChannelManagement";
import { IsSmallProvider } from "./Contexts/IsSmallContext";
import {
  createBrowserRouter,
  createRoutesFromElements,
  Route,
  RouterProvider,
} from "react-router-dom";
import "./App.css";
import RootLayout from "./layouts/RootLayout/RootLayout";
import NotFound from "./pages/NotFound.js/NotFound";
import CMLayout from "./layouts/CMLayout/CMLayout";
import ProtectedRoutes from "./utils/ProtectedRoutes";
import Statistics from "./pages/Statistics/Statistics";
import Customisation from "./pages/Customisation/Customisation";
import Subscriptions from "./pages/Subscriptions/Subscriptions";
import VideoStatistics from "./pages/VideoStatistics/VideoStatistics";
import VideoEdit from "./pages/VideoEdit/VideoEdit";
import History from "./pages/History/History";
import Playlists from "./pages/Playlists/Playlists";
import Watchlater from "./pages/Watchlater/Watchlater";
import Liked from "./pages/Liked/Liked";
import ContentVideos from "./pages/ContentVideos/ContentVideos";
import ContentPlaylists from "./pages/ContentPlaylists/ContentPlaylists";
import PlaylistManagement from "./pages/PlaylistManagement/PlaylistManagement";
import PlaylistEdit from "./pages/PlaylistEdit/PlaylistEdit";
import ContentPlaylistVideos from "./pages/ContentPlaylistVideos/ContentPlaylistVideos";
import Explore from "./pages/Explore/Explore";
import CategoryPage from "./pages/CategoryPage/CategoryPage";
import WatchVideo from "./pages/WatchVideo/WatchVideo";
import PlaylistPlayer from "./components/PlaylistPlayer/PlaylistPlayer";
import ManageSubs from "./pages/ManageSubs/ManageSubs";
import Favorite from "./pages/Favorite/Favorite";
import ChannelPage from "./pages/ChannelPage/ChannelPage";
import ChannelVideos from "./pages/ChannelVideos/ChannelVideos";
import ChannelPlaylists from "./pages/ChannelPlaylists/ChannelPlaylists";
import SearchResults from "./pages/SearchResults/SearchResults";
import Profile from "./pages/Profile/Profile";
import ProfileEdit from "./pages/ProfileEdit/ProfileEdit";
import ProfileStatistics from "./pages/ProfileStatistics/ProfileStatistics";

export default function App() {
  const router = createBrowserRouter(
    createRoutesFromElements(
      <Route path="/">
        <Route element={<RootLayout />}>
          <Route index element={<Home />} />
          <Route path="explore">
            <Route index element={<Explore />} />
            <Route path=":categoryId" element={<CategoryPage />} />
          </Route>
          <Route path="subscriptions" element={<Subscriptions />} />
          <Route path="history" element={<History />} />
          <Route path="favorite" element={<Favorite />} />
          <Route path="playlists" element={<Playlists />} />
          <Route path="watchlater" element={<Watchlater />} />
          <Route path="liked" element={<Liked />} />
          <Route path="managesubs" element={<ManageSubs />} />
          <Route path="search" element={<SearchResults />}></Route>
          <Route path="profile" element={<Profile />}>
            <Route index element={<ProfileEdit />}></Route>
            <Route path="statistics" element={<ProfileStatistics />}></Route>
          </Route>
          <Route path="channel/:channelId" element={<ChannelPage />}>
            <Route index element={<ChannelVideos />}></Route>
            <Route path="playlists" element={<ChannelPlaylists />}></Route>
          </Route>
          <Route path="watch">
            <Route path=":videoId" element={<WatchVideo />}>
              <Route
                path=":playlistId/:showPrivate"
                element={<PlaylistPlayer />}
              />
            </Route>
          </Route>
        </Route>
        <Route path="auth" element={<Auth />} />

        <Route element={<ProtectedRoutes />}>
          <Route path="channelmanagement" element={<CMLayout />}>
            <Route path="content" element={<ChannelManagement />}>
              <Route path="videos" element={<ContentVideos />}></Route>
              <Route path="playlists" element={<ContentPlaylists />} />
            </Route>
            <Route path="video">
              <Route path=":videoId/edit" element={<VideoEdit />} />
              <Route path=":videoId/statistics" element={<VideoStatistics />} />
            </Route>
            <Route path="playlist/:playlistId" element={<PlaylistManagement />}>
              <Route path="edit" element={<PlaylistEdit />} />
              <Route path="videos" element={<ContentPlaylistVideos />} />
            </Route>

            <Route path="statistics" element={<Statistics />} />
            <Route path="customisation" element={<Customisation />} />
          </Route>
        </Route>

        <Route path="*" element={<NotFound />}></Route>
      </Route>
    )
  );

  return (
    <IsSmallProvider>
      <RouterProvider router={router}></RouterProvider>
    </IsSmallProvider>
  );
}

import './ChannelPage.css';
import { useEffect, useState } from 'react';
import { Outlet, useLocation, useNavigate, useParams } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import { useMatch } from 'react-router-dom';
import bell from "../../components/resources/icons/bell.svg"
import bell_crossed from "../../components/resources/icons/bell-crossed.svg"
import useAuth from '../../hooks/useAuth';
import InfoWindow from '../../components/InfoWindow/InfoWindow';
export default function ChannelPage() {
    useAxiosWithToken();
    const [channel, setChannel] = useState();
    const { channelId } = useParams();
    const [message, setMessage] = useState("");
    const location = useLocation();
    const navigate = useNavigate();
    const { auth } = useAuth();
    useEffect(() => {
        GetChannel();
    }, [channelId])

    async function GetChannel() {
        try {
            const response = await axiosWithToken.get(
                `/api/media/getchannel/${channelId}`,
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setChannel(response.data)
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else {
                console.error('Error during retrieving video:', error.response?.data || error.message);
            }
        }
    }

    async function handleSubscribe() {
        try {
            const response = await axiosWithToken.post(
                `/api/media/subscribe/${channelId}`,
                null,
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setChannel(prevChannel => ({
                    ...prevChannel,
                    isSubscribed: response.data,
                    receiveNotifications: response.data ? false : true,
                    channelSubscriptions: (prevChannel?.channelSubscriptions || 0) + (prevChannel.isSubscribed ? -1 : 1)
                }));
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response.status) {
                setMessage("Want to subscribe to this channel? Sign in to subscribe to this channel.");
            }
            else {
                console.error('Error during subscribing:', error.response?.data || error.message);
            }
        }
    }

    async function handleNotifications() {
        try {
            const response = await axiosWithToken.patch(
                `/api/media/receivenotifications/${channelId}`,
                {},
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setChannel(prevChannel => ({
                    ...prevChannel,
                    receiveNotifications: response.data
                }));
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response.status) {
                setMessage("Want to receive notifications? Sign in to receive notifications from this channel.");
            }
            else {
                console.error('Error during managing notifications:', error.response?.data || error.message);
            }
        }
    }


    const isVideosSelected = useMatch('/channel/:channelId');
    const isPlaylistsSelected = useMatch('/channel/:channelId/playlists');
    return (
        <>
            <div className='centeredMain'>
                <div className='centeredMainContentContainer'>
                    <div className="channelContainer">
                        <img src={channel?.logoUrl + `?timestamp=${Date.now()}`} alt="" />
                        <div className="channelInformation">
                            <h2 className="channelTitle"> {channel?.title} </h2>
                            <p className="channelStats">
                                @{channel?.handle} &#183; {channel?.channelSubscriptions} subscribers &#183; {channel?.videoCount} videos
                            </p>
                            <p className="channelStats channelStatsDescription">
                                {channel?.description}
                            </p>
                            <div className="channelPageBtns">
                                {
                                    channel?.id == auth?.channelId
                                        ?
                                        <div className='channelManagementBtns'>
                                            <button className='submitBtn subscribeBtn subscribed' onClick={()=>navigate("/channelmanagement/customisation")}>Customize channel</button>
                                            <button className='submitBtn subscribeBtn subscribed' onClick={()=>navigate("/channelmanagement/content/videos")}>Manage videos</button>
                                        </div>
                                        :
                                        <button className={'submitBtn subscribeBtn ' + (channel?.isSubscribed ? "subscribed" : "")} onClick={() => { handleSubscribe() }} disabled={channel?.id == auth?.channelId}>{channel?.isSubscribed ? "Subscribed" : "Subscribe"}</button>
                                }
                                {
                                    channel?.isSubscribed && channel?.id != auth?.channelId
                                    &&
                                    <img className='bellImg' src={channel?.receiveNotifications ? bell : bell_crossed} alt="" onClick={() => { handleNotifications() }} />
                                }


                            </div>
                        </div>

                    </div>
                    <div className='navDivs prevent-select'>
                        <div className={"navDiv " + (isVideosSelected ? "selected" : "")} onClick={() => navigate(`/channel/${channelId}`)}>Videos</div>
                        <div className={"navDiv " + (isPlaylistsSelected ? "selected" : "")} onClick={() => navigate(`/channel/${channelId}/playlists`)}>Playlists</div>
                    </div>
                    <Outlet></Outlet>
                </div>
            </div>
            {
                message.length > 0
                    ?
                    <InfoWindow message={message} onOk={() => setMessage("")}></InfoWindow>
                    :
                    <></>
            }
        </>
    );
}

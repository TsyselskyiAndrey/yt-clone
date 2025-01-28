import './WatchVideo.css';
import { useEffect, useState } from 'react';
import { Outlet, useNavigate, useParams } from 'react-router-dom';
import { axiosWithToken } from "../../API/axioscfg"
import useAxiosWithToken from "../../hooks/useAxiosWithToken"
import like from "../../resources/like.png"
import like_active from "../../resources/like-active.png"
import dislike from "../../resources/dislike.png"
import dislike_active from "../../resources/dislike-active.png"
import tick from "../../resources/ok.png"
import bell from "../../components/resources/icons/bell.svg"
import bell_crossed from "../../components/resources/icons/bell-crossed.svg"
import TimeAgo from 'javascript-time-ago'
import useAuth from '../../hooks/useAuth';
import InfoWindow from '../../components/InfoWindow/InfoWindow';
import CommentInput from '../../components/CommentInput/CommentInput';
import arrow from "../../resources/arrow_purple.png"
import sort from "../../components/resources/icons/sorticon.svg"
import validateControl from '../Auth/Forms/Validations/GeneralValidation';
import ConfirmationWindow from '../../components/ConfirmationWindow/ConfirmationWindow';
export default function WatchVideo() {
    useAxiosWithToken();
    const { videoId } = useParams();
    const [deletedCommentId, setDeletedCommentId] = useState(false);
    const [video, setVideo] = useState({});
    const timeAgo = new TimeAgo('en-US')
    const { auth } = useAuth();
    const [playlists, setPlaylists] = useState([]);
    const [isAddingToPlaylist, setIsAddingToPlaylist] = useState(false)
    const [playlistUpdatingInProgress, setPlaylistUpdatingInProgress] = useState(false);
    const [message, setMessage] = useState("");
    const [isCommenting, setIsCommenting] = useState(false);
    const [isSortOpen, setIsSortOpen] = useState(false);
    const [sortOption, setSortOption] = useState("Newest")
    const [commentControl, setCommentControl] = useState({
        name: 'comment',
        errorMessage: '',
        value: '',
        valid: false,
        validation: {
            minLength: 1,
            maxLength: 1000,
            allowSymbols: true,
            allowSpaces: true
        },
        touched: false,
        shake: false
    });
    const [recommendedVideos, setRecommendedVideos] = useState();
    const [comments, setComments] = useState();
    const navigate = useNavigate()

    useEffect(() => {
        GetVideoDetails();
        GetRecommendedVideos();
    }, [videoId])

    useEffect(() => {
        GetPlaylists();
    }, []);

    useEffect(() => {
        GetComments();
    }, [sortOption, videoId]);

    function handleCommentChange(e) {
        const commentControlCopy = { ...commentControl }
        commentControlCopy.value = e.target.value
        const [isValid, newErrorMessage] = validateControl(e.target.value, commentControlCopy.validation, commentControlCopy.name)
        commentControlCopy.valid = isValid
        commentControlCopy.errorMessage = newErrorMessage
        commentControlCopy.touched = true;
        if (commentControlCopy.value == "") {
            commentControlCopy.touched = false
        }
        setCommentControl(commentControlCopy);
    }

    function handleReplyChange(e, commentId) {
        const replyControlCopy = { ...comments.find(comment => comment.id === commentId)?.replyControl };
        replyControlCopy.value = e.target.value
        const [isValid, newErrorMessage] = validateControl(e.target.value, replyControlCopy.validation, replyControlCopy.name)
        replyControlCopy.valid = isValid
        replyControlCopy.errorMessage = newErrorMessage
        replyControlCopy.touched = true;

        if (replyControlCopy.value == "") {
            replyControlCopy.touched = false
        }

        setComments(prevComments => (prevComments.map(comment => (
            comment.id === commentId
                ?
                {
                    ...comment,
                    replyControl: replyControlCopy
                }
                :
                comment
        ))))
    }

    function handleUpdatingCommentChange(e, commentId) {
        const updateControlCopy = { ...comments.find(comment => comment.id === commentId)?.updateControl };
        updateControlCopy.value = e.target.value
        const [isValid, newErrorMessage] = validateControl(e.target.value, updateControlCopy.validation, updateControlCopy.name)
        updateControlCopy.valid = isValid
        updateControlCopy.errorMessage = newErrorMessage
        updateControlCopy.touched = true;

        if (updateControlCopy.value == "") {
            updateControlCopy.touched = false
        }

        setComments(prevComments => (prevComments.map(comment => (
            comment.id === commentId
                ?
                {
                    ...comment,
                    updateControl: updateControlCopy
                }
                :
                comment
        ))))
    }

    function handleUpdatingReplyChange(e, commentId) {
        const parentComment = comments.find(comment => comment.childComments?.some(child => child.id === commentId));
        const childComment = parentComment?.childComments.find(child => child.id === commentId);
        const updateControlCopy = { ...childComment?.updateControl };
        updateControlCopy.value = e.target.value
        const [isValid, newErrorMessage] = validateControl(e.target.value, updateControlCopy.validation, updateControlCopy.name)
        updateControlCopy.valid = isValid
        updateControlCopy.errorMessage = newErrorMessage
        updateControlCopy.touched = true;

        if (updateControlCopy.value == "") {
            updateControlCopy.touched = false
        }

        setComments(prevComments =>
            prevComments.map(parentComment => {
                if (parentComment.childComments) {
                    return {
                        ...parentComment,
                        childComments: parentComment.childComments.map(childComment => {
                            if (childComment.id === commentId) {
                                return {
                                    ...childComment,
                                    updateControl: updateControlCopy
                                };
                            }
                            return childComment;
                        })
                    };
                }
                return parentComment;
            })
        );
    }

    async function GetPlaylists() {
        try {
            const response = await axiosWithToken.get(
                `/api/playlist/getvideoplaylists/${videoId}`,
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setPlaylists(response.data)
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else {
                console.error('Error during retrieving playlists:', error.response?.data || error.message);
            }
        }
    }

    async function GetRecommendedVideos() {
        if (videoId) {
            try {
                const response = await axiosWithToken.get(
                    `/api/media/recommendedvideos/${videoId}`,
                    {
                        withCredentials: true
                    }
                );
                if (response.status === 200) {
                    setRecommendedVideos(response.data)
                }
            } catch (error) {
                if (!error?.response) {
                    console.log("No Server Response");
                }
                else {
                    console.error('Error during retrieving recommendations:', error.response?.data || error.message);
                }
            }
        }
    }

    async function GetComments() {
        try {
            const response = await axiosWithToken.get(
                `/api/comments/getcomments/${videoId}/${sortOption}`,
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setComments(response.data.map(comment => ({
                    ...comment,
                    isRepliesOpen: false,
                    isReplying: false,
                    isUpdating: false,
                    replyControl: {
                        name: 'reply',
                        errorMessage: '',
                        value: '',
                        valid: false,
                        validation: {
                            minLength: 1,
                            maxLength: 1000,
                            allowSymbols: true,
                            allowSpaces: true
                        },
                        touched: false,
                        shake: false
                    },
                    updateControl: {
                        name: 'comment',
                        errorMessage: '',
                        value: '',
                        valid: false,
                        validation: {
                            minLength: 1,
                            maxLength: 1000,
                            allowSymbols: true,
                            allowSpaces: true
                        },
                        touched: false,
                        shake: false
                    },
                    childComments: comment.childComments?.map(child => ({
                        ...child,
                        isUpdating: false,
                        updateControl: {
                            name: 'comment',
                            errorMessage: '',
                            value: '',
                            valid: false,
                            validation: {
                                minLength: 1,
                                maxLength: 1000,
                                allowSymbols: true,
                                allowSpaces: true
                            },
                            touched: false,
                            shake: false
                        }
                    })) || []
                })))

            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else {
                console.error('Error during retrieving playlists:', error.response?.data || error.message);
            }
        }
    }

    async function handleAddRemovePlaylist(playlistId) {
        if (playlistUpdatingInProgress) {
            return;
        }
        try {
            setPlaylistUpdatingInProgress(true)
            const response = await axiosWithToken.post(
                `/api/playlist/addremovevideo/${playlistId}/${videoId}`,
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setPlaylists(playlists.map(item =>
                    item.id == playlistId
                        ? response.data
                        : item
                ))
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else {
                console.error('Error during request:', error.response || error.message);
            }
        } finally {
            setPlaylistUpdatingInProgress(false)
        }
    }

    async function GetVideoDetails() {
        try {
            const response = await axiosWithToken.get(
                `/api/media/getvideo/${videoId}`,
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setVideo(response.data)
            }
        } catch (error) {
            if (error?.response.status === 200) {
                setVideo(error?.response.data)
            }
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
                `/api/media/subscribe/${video?.channelId}`,
                null,
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setVideo(prevVideo => ({
                    ...prevVideo,
                    isSubscribed: response.data,
                    receiveNotifications: response.data ? false : true,
                    channelSubscriptions: (prevVideo?.channelSubscriptions || 0) + (prevVideo.isSubscribed ? -1 : 1)
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
                `/api/media/receivenotifications/${video?.channelId}`,
                {},
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setVideo(prevVideo => ({
                    ...prevVideo,
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

    async function handleLike() {
        try {
            const response = await axiosWithToken.patch(
                `/api/media/like/${videoId}`,
                {},
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setVideo(prevVideo => ({
                    ...prevVideo,
                    isLiked: response.data.isLiked,
                    numberOfLikes: (prevVideo?.numberOfLikes || 0) + (prevVideo.isLiked == true ? -1 : 1),
                    numberOfDislikes: (prevVideo?.numberOfDislikes || 0) + (prevVideo.isLiked == false ? -1 : 0)
                }));
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response.status) {
                setMessage("Like this video? You are not authenticated. Sign in to make your opinion count.");
            }
            else {
                console.error('Error during liking:', error.response?.data || error.message);
            }
        }
    }

    async function handleDislike() {
        try {
            const response = await axiosWithToken.patch(
                `/api/media/dislike/${videoId}`,
                {},
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setVideo(prevVideo => ({
                    ...prevVideo,
                    isLiked: response.data.isLiked,
                    numberOfLikes: (prevVideo?.numberOfLikes || 0) + (prevVideo.isLiked == true ? -1 : 0),
                    numberOfDislikes: (prevVideo?.numberOfDislikes || 0) + (prevVideo.isLiked == false ? -1 : 1)
                }));
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response.status) {
                setMessage("Don't like this video? You are not authenticated. Sign in to make your opinion count.");
            }
            else {
                console.error('Error during disliking:', error.response?.data || error.message);
            }
        }
    }

    async function handleFavorite() {
        try {
            const response = await axiosWithToken.patch(
                `/api/media/favorite/${videoId}`,
                {},
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setVideo(prevVideo => ({
                    ...prevVideo,
                    isFavorite: response.data.isFavorite
                }));
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response.status) {
                setMessage("Is this video special for you? You are not authenticated. Sign in to add the video to the favorite.");
            }
            else {
                console.error('Error during adding to the favorite:', error.response?.data || error.message);
            }
        }
    }

    async function handleWatchLater() {
        try {
            const response = await axiosWithToken.patch(
                `/api/media/watchlater/${videoId}`,
                {},
                {
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                setVideo(prevVideo => ({
                    ...prevVideo,
                    watchLater: response.data.watchLater
                }));
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response.status) {
                setMessage("Want to watch this again later? You are not authenticated. Sign in to reschedule watching this video.");
            }
            else {
                console.error('Error during adding to the "watch later" videos:', error.response?.data || error.message);
            }
        }
    }

    async function handleAddComment() {
        if (commentControl.valid == false) {
            setCommentControl({ ...commentControl, shake: true });

            setTimeout(() => {
                setCommentControl(prev => ({ ...prev, shake: false }));
            }, 300);
            return;
        }
        try {
            const response = await axiosWithToken.post(
                `/api/comments/addcomment/${videoId}`,
                {
                    message: commentControl.value
                },
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            if (response.status == 200) {
                await GetComments();
                handleCancelationComment()
                setVideo(prevVideo => ({
                    ...prevVideo,
                    numberOfComments: prevVideo.numberOfComments + 1
                }));
            }

        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response?.status === 400 || error.response?.status === 409) {
                const errorData = error.response.data;
                const commentControlCopy = { ...commentControl };

                for (const particularError of errorData) {
                    commentControlCopy.valid = false;
                    commentControlCopy.errorMessage = "* " + particularError.details;
                    commentControlCopy.touched = true;
                    setCommentControl({ ...commentControlCopy, shake: true });

                    setTimeout(() => {
                        setCommentControl(prev => ({ ...prev, shake: false }));
                    }, 300);
                }

            }
            else {
                console.error('Error during login:', error.response || error.message);
            }
        }
    }

    async function handleDeleteComment() {
        try {
            const response = await axiosWithToken.delete(
                `/api/comments/deletecomment/${deletedCommentId}`,
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            if (response.status === 200) {
                if (comments.find(comment => comment.id === deletedCommentId)) {
                    setVideo(prevVideo => ({
                        ...prevVideo,
                        numberOfComments: prevVideo.numberOfComments - 1
                    }));
                }
                else {
                    setComments(prevComments =>
                        prevComments.map(parentComment => {
                            if (parentComment.childComments?.some(comment => comment.id === deletedCommentId)) {
                                return {
                                    ...parentComment,
                                    numberOfReplies: parentComment.numberOfReplies - 1
                                };
                            }
                            return parentComment;
                        }
                        )
                    );
                }
                setComments(prevComments => prevComments.filter(comment => comment.id !== deletedCommentId).map(comment => ({
                    ...comment,
                    childComments: comment.childComments ? comment.childComments.filter(childComment => childComment.id !== deletedCommentId) : []
                })));
                setDeletedCommentId(null)
            }
        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else {
                console.error('Error during deleting comment:', error.response || error.message);
            }
        }
    }

    async function handleAddReply(commentId) {
        if (comments.find(comment => comment.id === commentId)?.replyControl.valid == false) {
            setComments(prevComments =>
                prevComments.map(comment =>
                    comment.id === commentId ? { ...comment, replyControl: { ...comment.replyControl, shake: true } } : comment
                )
            );
            setTimeout(() => {
                setComments(prevComments =>
                    prevComments.map(comment =>
                        comment.id === commentId ? { ...comment, replyControl: { ...comment.replyControl, shake: false } } : comment
                    )
                );
            }, 300);
            return;
        }
        try {
            const response = await axiosWithToken.post(
                `/api/comments/replytocomment/${commentId}`,
                {
                    message: comments.find(comment => comment.id === commentId)?.replyControl.value
                },
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            if (response.status == 200) {
                setComments(prevComments =>
                    prevComments.map(comment =>
                        comment.id === commentId ? { ...comment, childComments: [...comment.childComments, response.data] } : comment
                    )
                );
                setComments(prevComments =>
                    prevComments.map(comment =>
                        comment.id === commentId ? { ...comment, numberOfReplies: comment.numberOfReplies + 1 } : comment
                    )
                );
                changeIsReplying(commentId, false)
            }

        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response?.status === 400 || error.response?.status === 409) {
                const errorData = error.response.data;
                const replyControlCopy = { ...comments.find(comment => comment.id === commentId)?.replyControl };

                for (const particularError of errorData) {
                    replyControlCopy.valid = false;
                    replyControlCopy.errorMessage = "* " + particularError.details;
                    replyControlCopy.touched = true;
                    setComments(prevComments =>
                        prevComments.map(comment =>
                            comment.id === commentId ? { ...comment, replyControl: { ...replyControlCopy, shake: true } } : comment
                        )
                    );
                    setTimeout(() => {
                        setComments(prevComments =>
                            prevComments.map(comment =>
                                comment.id === commentId ? { ...comment, replyControl: { ...comment.replyControl, shake: false } } : comment
                            )
                        );
                    }, 300);
                }

            }
            else {
                console.error('Error during login:', error.response || error.message);
            }
        }
    }

    async function handleUpdateComment(commentId) {
        if (comments.find(comment => comment.id === commentId).updateControl.valid == false) {
            setComments(prevComments =>
                prevComments.map(comment =>
                    comment.id === commentId ? { ...comment, updateControl: { ...comment.updateControl, shake: true } } : comment
                )
            );
            setTimeout(() => {
                setComments(prevComments =>
                    prevComments.map(comment =>
                        comment.id === commentId ? { ...comment, updateControl: { ...comment.updateControl, shake: false } } : comment
                    )
                );
            }, 300);
            return;
        }

        try {
            const response = await axiosWithToken.patch(
                `/api/comments/updatecomment/${commentId}`,
                {
                    message: comments.find(comment => comment.id === commentId)?.updateControl.value
                },
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            if (response.status == 200) {
                setComments(prevComments =>
                    prevComments.map(comment =>
                        comment.id === commentId ? { ...comment, isUpdating: false, content: response.data } : comment
                    )
                );
            }

        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response?.status === 400 || error.response?.status === 409) {
                const errorData = error.response.data;
                const updateControlCopy = { ...comments.find(comment => comment.id === commentId)?.updateControl };

                for (const particularError of errorData) {
                    updateControlCopy.valid = false;
                    updateControlCopy.errorMessage = "* " + particularError.details;
                    updateControlCopy.touched = true;
                    setComments(prevComments =>
                        prevComments.map(comment =>
                            comment.id === commentId ? { ...comment, updateControl: { ...updateControlCopy, shake: true } } : comment
                        )
                    );
                    setTimeout(() => {
                        setComments(prevComments =>
                            prevComments.map(comment =>
                                comment.id === commentId ? { ...comment, updateControl: { ...comment.updateControlCopy, shake: false } } : comment
                            )
                        );
                    }, 300);
                }

            }
            else {
                console.error('Error during login:', error.response || error.message);
            }
        }
    }

    async function handleUpdateReply(commentId) {
        let flag = false;
        comments.map(parentComment => {
            if (parentComment.childComments?.some(comment => comment.id === commentId)) {
                if (parentComment.childComments?.find(comment => comment.id === commentId).updateControl.valid == false) {
                    setComments(prevComments =>
                        prevComments.map(parentComment => {
                            if (parentComment.childComments) {
                                return {
                                    ...parentComment,
                                    childComments: parentComment.childComments.map(childComment => {
                                        if (childComment.id === commentId) {
                                            return {
                                                ...childComment,
                                                updateControl: { ...childComment.updateControl, shake: true }
                                            };
                                        }
                                        return childComment;
                                    })
                                };
                            }
                            return parentComment;
                        })
                    );
                    setTimeout(() => {
                        setComments(prevComments =>
                            prevComments.map(parentComment => {
                                if (parentComment.childComments) {
                                    return {
                                        ...parentComment,
                                        childComments: parentComment.childComments.map(childComment => {
                                            if (childComment.id === commentId) {
                                                return {
                                                    ...childComment,
                                                    updateControl: { ...childComment.updateControl, shake: false }
                                                };
                                            }
                                            return childComment;
                                        })
                                    };
                                }
                                return parentComment;
                            })
                        );
                    }, 300);
                    flag = true
                }
            }
        })
        if (flag) {
            return;
        }

        try {
            const response = await axiosWithToken.patch(
                `/api/comments/updatecomment/${commentId}`,
                {
                    message: comments
                        .flatMap(comment => comment.childComments || [])
                        .find(childComment => childComment.id === commentId)?.updateControl.value
                },
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            if (response.status == 200) {
                setComments(prevComments =>
                    prevComments.map(parentComment => {
                        if (parentComment.childComments) {
                            return {
                                ...parentComment,
                                childComments: parentComment.childComments.map(childComment => {
                                    if (childComment.id === commentId) {
                                        return {
                                            ...childComment,
                                            isUpdating: false,
                                            content: response.data
                                        };
                                    }
                                    return childComment;
                                })
                            };
                        }
                        return parentComment;
                    })
                );
            }

        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response?.status === 400 || error.response?.status === 409) {
                const errorData = error.response.data;
                const updateControlCopy = {
                    ...comments
                        .flatMap(comment => comment.childComments || [])
                        .find(childComment => childComment.id === commentId)?.updateControl
                };

                for (const particularError of errorData) {
                    updateControlCopy.valid = false;
                    updateControlCopy.errorMessage = "* " + particularError.details;
                    updateControlCopy.touched = true;
                    setComments(prevComments =>
                        prevComments.map(parentComment => {
                            if (parentComment.childComments) {
                                return {
                                    ...parentComment,
                                    childComments: parentComment.childComments.map(childComment => {
                                        if (childComment.id === commentId) {
                                            return {
                                                ...childComment,
                                                updateControl: { ...updateControlCopy, shake: true }
                                            };
                                        }
                                        return childComment;
                                    })
                                };
                            }
                            return parentComment;
                        })
                    );
                    setTimeout(() => {
                        setComments(prevComments =>
                            prevComments.map(parentComment => {
                                if (parentComment.childComments) {
                                    return {
                                        ...parentComment,
                                        childComments: parentComment.childComments.map(childComment => {
                                            if (childComment.id === commentId) {
                                                return {
                                                    ...childComment,
                                                    updateControl: { ...childComment.updateControl, shake: false }
                                                };
                                            }
                                            return childComment;
                                        })
                                    };
                                }
                                return parentComment;
                            })
                        );
                    }, 300);
                }

            }
            else {
                console.error('Error during login:', error.response || error.message);
            }
        }

    }

    async function handleLikeComment(commentId) {
        try {
            const response = await axiosWithToken.patch(
                `/api/comments/like/${commentId}`,
                {},
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            if (response.status == 200) {
                setComments(prevComments =>
                    prevComments.map(comment =>
                        comment.id === commentId
                            ?
                            {
                                ...comment,
                                isLiked: response.data.isLiked,
                                numberOfLikes: (comment?.numberOfLikes || 0) + (comment.isLiked == true ? -1 : 1),
                                numberOfDislikes: (comment?.numberOfDislikes || 0) + (comment.isLiked == false ? -1 : 0)
                            }
                            :
                            comment
                    )
                );
            }

        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response.status) {
                setMessage("Like this comment? You are not authenticated. Sign in to make your opinion count.");
            }
            else {
                console.error('Error during login:', error.response || error.message);
            }
        }
    }

    async function handleDislikeComment(commentId) {
        try {
            const response = await axiosWithToken.patch(
                `/api/comments/dislike/${commentId}`,
                {},
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            if (response.status == 200) {
                setComments(prevComments =>
                    prevComments.map(comment =>
                        comment.id === commentId
                            ?
                            {
                                ...comment,
                                isLiked: response.data.isLiked,
                                numberOfLikes: (comment?.numberOfLikes || 0) + (comment.isLiked == true ? -1 : 0),
                                numberOfDislikes: (comment?.numberOfDislikes || 0) + (comment.isLiked == false ? -1 : 1)
                            }
                            :
                            comment
                    )
                );
            }

        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response.status) {
                setMessage("Don't like this comment? You are not authenticated. Sign in to make your opinion count.");
            }
            else {
                console.error('Error during login:', error.response || error.message);
            }
        }
    }

    async function handleLikeReply(commentId) {
        try {
            const response = await axiosWithToken.patch(
                `/api/comments/like/${commentId}`,
                {},
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            if (response.status == 200) {
                setComments(prevComments =>
                    prevComments.map(parentComment => {
                        if (parentComment.childComments) {
                            return {
                                ...parentComment,
                                childComments: parentComment.childComments.map(childComment => {
                                    if (childComment.id === commentId) {
                                        return {
                                            ...childComment,
                                            isLiked: response.data.isLiked,
                                            numberOfLikes: (childComment?.numberOfLikes || 0) + (childComment.isLiked == true ? -1 : 1),
                                            numberOfDislikes: (childComment?.numberOfDislikes || 0) + (childComment.isLiked == false ? -1 : 0)
                                        };
                                    }
                                    return childComment;
                                })
                            };
                        }
                        return parentComment;
                    })
                );
            }

        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response.status) {
                setMessage("Like this reply? You are not authenticated. Sign in to make your opinion count.");
            }
            else {
                console.error('Error during login:', error.response || error.message);
            }
        }
    }

    async function handleDislikeReply(commentId) {
        try {
            const response = await axiosWithToken.patch(
                `/api/comments/dislike/${commentId}`,
                {},
                {
                    headers: { 'Content-Type': 'application/json' },
                    withCredentials: true
                }
            );
            if (response.status == 200) {
                setComments(prevComments =>
                    prevComments.map(parentComment => {
                        if (parentComment.childComments) {
                            return {
                                ...parentComment,
                                childComments: parentComment.childComments.map(childComment => {
                                    if (childComment.id === commentId) {
                                        return {
                                            ...childComment,
                                            isLiked: response.data.isLiked,
                                            numberOfLikes: (childComment?.numberOfLikes || 0) + (childComment.isLiked == true ? -1 : 0),
                                            numberOfDislikes: (childComment?.numberOfDislikes || 0) + (childComment.isLiked == false ? -1 : 1)
                                        };
                                    }
                                    return childComment;
                                })
                            };
                        }
                        return parentComment;
                    })
                );
            }

        } catch (error) {
            if (!error?.response) {
                console.log("No Server Response");
            }
            else if (error.response.status) {
                setMessage("Don't like this reply? You are not authenticated. Sign in to make your opinion count.");
            }
            else {
                console.error('Error during login:', error.response || error.message);
            }
        }
    }

    function handleSortOption(option) {
        setSortOption(option);
        setIsSortOpen(false);
    }

    function changeIsReplying(commentId, newVal) {
        if(!auth || !auth?.roles?.includes("channel_owner")){
            setMessage("Want to comment? You are either not authenticated or don't have a channel. Sign in and create a channel to add a comment to the video.");
            return;
        }
        if (newVal == false) {
            setComments(prevComments =>
                prevComments.map(comment =>
                    comment.id === commentId ? { ...comment, isReplying: newVal, replyControl: { ...comment.replyControl, value: "", errorMessage: "", touched: false } } : comment
                )
            );
        }
        else {
            setComments(prevComments =>
                prevComments.map(comment =>
                    comment.id === commentId ? { ...comment, isReplying: newVal, replyControl: { ...comment.replyControl, value: "" } } : comment
                )
            );
        }

    }

    function changeIsRepliesOpen(commentId, newVal) {
        setComments(prevComments =>
            prevComments.map(comment =>
                comment.id === commentId ? { ...comment, isRepliesOpen: newVal } : comment
            )
        );
    }

    function handleCancelationComment() {
        setIsCommenting(false)
        setCommentControl(prevControl => ({
            ...prevControl,
            value: "",
            errorMessage: "",
            touched: false
        }))
    }

    function changeIsUpdatingComment(commentId, newVal) {
        if (newVal == false) {
            setComments(prevComments =>
                prevComments.map(comment =>
                    comment.id === commentId ? { ...comment, isUpdating: newVal, updateControl: { ...comment.updateControl, value: comment.content, errorMessage: "", touched: false } } : comment
                )
            );
        }
        else {
            setComments(prevComments =>
                prevComments.map(comment =>
                    comment.id === commentId ? { ...comment, isUpdating: newVal, updateControl: { ...comment.updateControl, value: comment.content } } : comment
                )
            );
        }
    }

    function changeIsUpdatingReply(commentId, newVal) {
        if (newVal == false) {
            setComments(prevComments =>
                prevComments.map(parentComment => {
                    if (parentComment.childComments) {
                        return {
                            ...parentComment,
                            childComments: parentComment.childComments.map(childComment => {
                                if (childComment.id === commentId) {
                                    return {
                                        ...childComment,
                                        isUpdating: newVal,
                                        updateControl: { ...childComment.updateControl, value: childComment.content, errorMessage: "", touched: false }
                                    };
                                }
                                return childComment;
                            })
                        };
                    }
                    return parentComment;
                })
            );
        }
        else {
            setComments(prevComments =>
                prevComments.map(parentComment => {
                    if (parentComment.childComments) {
                        return {
                            ...parentComment,
                            childComments: parentComment.childComments.map(childComment => {
                                if (childComment.id === commentId) {
                                    return {
                                        ...childComment,
                                        isUpdating: newVal,
                                        updateControl: { ...childComment.updateControl, value: childComment.content }
                                    };
                                }
                                return childComment;
                            })
                        };
                    }
                    return parentComment;
                })
            );
        }


    }

    const apiUrl = `https://localhost:44302/api/content/watch/${videoId}`;
    return (
        <div className='mainContainerParent'>
            <div className="mainContainer">
                <div className="videoWatchContainer">
                    <video id="videoWatchPlayer" key={videoId + "_player"} width="640" height="360" controls autoPlay={true}>
                        <source id="videoSource" src={apiUrl} type={"video/mp4"}></source>
                        Your browser does not support the video tag.
                    </video>
                    <h2>{video?.title}</h2>
                    <div className="channel-info prevent-select">
                        <div className="leftBlock">
                            <img src={video.channelLogo + `?timestamp=${Date.now()}`} alt="Channel Logo" className="channel-logo"></img>
                            <div className='subs'>
                                <span className="channel-name" onClick={(e) => {e.stopPropagation(); navigate(`/channel/${video?.channelId}`)}}>{video?.channelTitle}</span>
                                <span className="subscribers">{video?.channelSubscriptions} subscribers</span>
                            </div>
                            <button className={'submitBtn subscribeBtn ' + (video?.isSubscribed ? "subscribed" : "")} onClick={handleSubscribe} disabled={video?.channelId == auth?.channelId}>{video?.isSubscribed ? "Subscribed" : "Subscribe"}</button>
                            {
                                video?.isSubscribed && video?.channelId != auth?.channelId
                                &&
                                <img className='bellImg' src={video?.receiveNotifications ? bell : bell_crossed} alt="" onClick={handleNotifications} />
                            }
                        </div>
                        <div className="rightBlock">
                            <div className="videoReaction">
                                <div className="like" onClick={handleLike}>
                                    <img src={video?.isLiked == true ? like_active : like} alt="" />
                                    {video?.numberOfLikes}
                                </div>
                                <div className="dislike" onClick={handleDislike}>
                                    <img src={video?.isLiked == false ? dislike_active : dislike} alt="" />
                                    {video?.numberOfDislikes}
                                </div>
                            </div>
                            <div className={"actionDiv " + (video?.watchLater ? "active" : "")} onClick={handleWatchLater}>
                                {
                                    video?.watchLater &&
                                    <img src={tick} alt="" />
                                }
                                Watch later
                            </div>
                            <div className={"actionDiv " + (video?.isFavorite ? "active" : "")} onClick={handleFavorite}>
                                {
                                    video?.isFavorite &&
                                    <img src={tick} alt="" />
                                }
                                Favorite
                            </div>
                            <div className="actionDiv" onClick={() => {
                                if (auth) {
                                    setIsAddingToPlaylist(true)
                                }
                                else {
                                    setMessage("Want to save this video and add it to your collection? Sign in to add this video to a playlist.")
                                }
                            }}>
                                Save
                            </div>
                        </div>


                    </div>
                    <div className='descriptionContainer'>
                        <p className='generalInformation'>{video?.numberOfViews} views &#183; {video?.publicationDate && timeAgo.format(new Date(video.publicationDate))}
                            <span className='tags'>
                                {
                                    video?.tags
                                    &&
                                    video?.tags.map((tag, idx) => (
                                        <span key={tag.id}> {idx == 0 && <span> &#183;</span>} {tag.name} </span>
                                    ))
                                }
                            </span>
                        </p>
                        <p>{video?.description}</p>
                        <span className='categories'> Categories: &nbsp;
                            {
                                video?.categories
                                    &&
                                    video?.categories.length > 0
                                    ?
                                    video?.categories.map((category, idx) => (
                                        <span key={category.id}>{category.name}{idx != video?.categories.length - 1 ? <span>, </span> : <></>}</span>
                                    ))
                                    :
                                    <span>No categories</span>
                            }
                        </span>
                    </div>
                    <div className="commentsContainer">
                        <div className="commentHeader">
                            <h2>{video?.numberOfComments} Comments</h2>
                            <div className={'prevent-select sortBlock ' + (isSortOpen ? "sortopen" : "")} onClick={() => setIsSortOpen(!isSortOpen)}>
                                <img src={sort} alt="" />
                                Sort by
                                <div className="sortOptions">
                                    {["Most liked", "Most hated", "Longest", "Shortest", "Newest", "Oldest"].map(option => (
                                        <div
                                            key={option}
                                            className={`sortOption ${sortOption === option ? "chosenOption" : ""}`}
                                            onClick={() => handleSortOption(option)}
                                        >
                                            {option} comments first
                                        </div>
                                    ))}
                                </div>
                            </div>
                        </div>

                        <div className="addCommentBlock">
                            <img src={auth ? (auth.logoUrl ? auth.logoUrl + `?timestamp=${Date.now()}` : auth.avatarUrl + `?timestamp=${Date.now()}`) : "https://mystoragets.blob.core.windows.net/vibeoserver/user-avatars/default.png"} alt="" className="channel-logo" />
                            <div className="addComment">
                                <CommentInput label="Add a comment..." value={commentControl.value} onChange={handleCommentChange} OnFocus={() => {
                                            if(!auth || !auth?.roles?.includes("channel_owner")){
                                                setMessage("Want to comment? You are either not authenticated or don't have a channel. Sign in and create a channel to add a comment to the video.");
                                                return;
                                            }
                                            setIsCommenting(true)
                                }}></CommentInput>
                                <span className={"errorMessage " + (commentControl.shake ? "shake" : "")}>{(commentControl.valid || !commentControl.touched) ? <span>&nbsp;</span> : commentControl.errorMessage}</span>
                                {
                                    isCommenting
                                        ?
                                        <div className="commentBtns">
                                            <button className='submitBtn cancelCommentBtn' onClick={() => handleCancelationComment()}>Cancel</button>
                                            <button className='submitBtn commentBtn' onClick={handleAddComment}>Comment</button>
                                        </div>
                                        :
                                        <></>
                                }
                            </div>
                        </div>

                        {
                            comments
                            &&
                            Object.keys(comments).map((commentIdx) => {
                                return (

                                    <div className="commentBlock mainComment" key={comments[commentIdx]?.id}>
                                        <img src={comments[commentIdx]?.logoUrl + `?timestamp=${Date.now()}`} alt="" className="channel-logo" />
                                        {
                                            comments[commentIdx]?.isUpdating
                                                ?
                                                <div className="addComment updateComment">
                                                    <CommentInput label="Edit the comment..." value={comments[commentIdx]?.updateControl.value} onChange={(e) => handleUpdatingCommentChange(e, comments[commentIdx]?.id)}></CommentInput>
                                                    <span className={"errorMessage " + (comments[commentIdx]?.updateControl.shake ? "shake" : "")}>{(comments[commentIdx]?.updateControl.valid || !comments[commentIdx]?.updateControl.touched) ? <span>&nbsp;</span> : comments[commentIdx]?.updateControl.errorMessage}</span>
                                                    <div className="commentBtns">
                                                        <button className='submitBtn cancelCommentBtn' onClick={() => changeIsUpdatingComment(comments[commentIdx]?.id, false)}>Cancel</button>
                                                        <button className='submitBtn commentBtn' onClick={() => handleUpdateComment(comments[commentIdx]?.id)}>Edit</button>
                                                    </div>
                                                </div>
                                                :
                                                <div className="commentInfo">
                                                    <div className="commentGeneral video-stats-small">
                                                        <span className='channelHandleComment' onClick={(e) => {e.stopPropagation(); navigate(`/channel/${comments[commentIdx]?.channelId}`)}}>{"@" + comments[commentIdx]?.handle}</span> &#183; {comments[commentIdx]?.createdAt && timeAgo.format(new Date(comments[commentIdx]?.createdAt))}
                                                    </div>
                                                    <div className="commentContent">
                                                        <p className="comment">
                                                            {comments[commentIdx]?.content}
                                                        </p>
                                                        <div className="commentBtns">

                                                            {
                                                                auth?.channelId == comments[commentIdx]?.channelId || auth?.channelId == video?.channelId
                                                                    ?
                                                                    <button className='removeBtn' onClick={() => setDeletedCommentId(comments[commentIdx]?.id)}></button>
                                                                    :
                                                                    <></>
                                                            }

                                                            {
                                                                auth?.channelId == comments[commentIdx]?.channelId
                                                                    ?
                                                                    <button className='editBtn' onClick={() => changeIsUpdatingComment(comments[commentIdx]?.id, true)}></button>
                                                                    :
                                                                    <></>
                                                            }
                                                        </div>



                                                    </div>
                                                    <div className="commentReaction">
                                                        <div className="commentLike">
                                                            <img src={comments[commentIdx]?.isLiked == true ? like_active : like} alt="" onClick={() => handleLikeComment(comments[commentIdx]?.id)} />
                                                            {comments[commentIdx]?.numberOfLikes}
                                                        </div>
                                                        <div className="commentDislike">
                                                            <img src={comments[commentIdx]?.isLiked == false ? dislike_active : dislike} alt="" onClick={() => handleDislikeComment(comments[commentIdx]?.id)} />
                                                            {comments[commentIdx]?.numberOfDislikes}
                                                        </div>
                                                        <div className='replyBtn prevent-select' onClick={() => changeIsReplying(comments[commentIdx]?.id, !comments[commentIdx]?.isReplying)}>Reply</div>
                                                    </div>

                                                    {
                                                        comments[commentIdx]?.isReplying
                                                            ?
                                                            <div className="addCommentBlock">
                                                                <img src={auth && (auth.logoUrl ? auth.logoUrl + `?timestamp=${Date.now()}` : auth.avatarUrl + `?timestamp=${Date.now()}`)} alt="" className="channel-logo response-logo" />
                                                                <div className="addComment">
                                                                    <CommentInput label="Add a reply..." value={comments[commentIdx]?.replyControl.value} onChange={(e) => handleReplyChange(e, comments[commentIdx]?.id)}></CommentInput>
                                                                    <span className={"errorMessage " + (comments[commentIdx]?.replyControl.shake ? "shake" : "")}>{(comments[commentIdx]?.replyControl.valid || !comments[commentIdx]?.replyControl.touched) ? <span>&nbsp;</span> : comments[commentIdx]?.replyControl.errorMessage}</span>
                                                                    <div className="commentBtns">
                                                                        <button className='submitBtn cancelCommentBtn' onClick={() => changeIsReplying(comments[commentIdx]?.id, false)}>Cancel</button>
                                                                        <button className='submitBtn commentBtn' onClick={() => handleAddReply(comments[commentIdx]?.id)}>Reply</button>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            :
                                                            <></>
                                                    }

                                                    {
                                                        comments[commentIdx]?.childComments?.length > 0
                                                            ?
                                                            <div className="showComments prevent-select" onClick={() => changeIsRepliesOpen(comments[commentIdx]?.id, !comments[commentIdx]?.isRepliesOpen)}>
                                                                <img className={comments[commentIdx]?.isRepliesOpen ? "" : "selected"} src={arrow} alt="" />
                                                                {comments[commentIdx]?.numberOfReplies} replies
                                                            </div>
                                                            :
                                                            <></>
                                                    }

                                                    {
                                                        comments[commentIdx]?.isRepliesOpen
                                                            ?
                                                            <>
                                                                {
                                                                    comments[commentIdx].childComments
                                                                    &&
                                                                    Object.keys(comments[commentIdx].childComments).map((childCommentIdx) => {
                                                                        return (
                                                                            <div className="commentBlock replyBlock" key={comments[commentIdx]?.childComments[childCommentIdx]?.id}>

                                                                                <img src={comments[commentIdx]?.childComments[childCommentIdx]?.logoUrl + `?timestamp=${Date.now()}`} alt="" className="channel-logo response-logo" />
                                                                                {
                                                                                    comments[commentIdx]?.childComments[childCommentIdx]?.isUpdating
                                                                                        ?
                                                                                        <div className="addComment updateComment">
                                                                                            <CommentInput label="Edit the reply..." value={comments[commentIdx]?.childComments[childCommentIdx]?.updateControl.value} onChange={(e) => handleUpdatingReplyChange(e, comments[commentIdx]?.childComments[childCommentIdx]?.id)}></CommentInput>
                                                                                            <span className={"errorMessage " + (comments[commentIdx]?.childComments[childCommentIdx]?.updateControl.shake ? "shake" : "")}>{(comments[commentIdx]?.childComments[childCommentIdx]?.updateControl.valid || !comments[commentIdx]?.childComments[childCommentIdx]?.updateControl.touched) ? <span>&nbsp;</span> : comments[commentIdx]?.childComments[childCommentIdx]?.updateControl.errorMessage}</span>
                                                                                            <div className="commentBtns">
                                                                                                <button className='submitBtn cancelCommentBtn' onClick={() => changeIsUpdatingReply(comments[commentIdx]?.childComments[childCommentIdx]?.id, false)}>Cancel</button>
                                                                                                <button className='submitBtn commentBtn' onClick={() => handleUpdateReply(comments[commentIdx]?.childComments[childCommentIdx]?.id)}>Edit</button>
                                                                                            </div>
                                                                                        </div>
                                                                                        :
                                                                                        <div className="commentInfo">
                                                                                            <div className="commentGeneral video-stats-small">
                                                                                                <span className='channelHandleComment' onClick={(e) => {e.stopPropagation(); navigate(`/channel/${comments[commentIdx]?.childComments[childCommentIdx]?.channelId}`)}}>{"@" + comments[commentIdx]?.childComments[childCommentIdx]?.handle}</span> &#183; {comments[commentIdx]?.childComments[childCommentIdx]?.createdAt && timeAgo.format(new Date(comments[commentIdx]?.childComments[childCommentIdx]?.createdAt))}
                                                                                            </div>
                                                                                            <div className="commentContent">
                                                                                                <p className="comment">
                                                                                                    {comments[commentIdx]?.childComments[childCommentIdx]?.content}
                                                                                                </p>
                                                                                                <div className="commentBtns">
                                                                                                    {
                                                                                                        auth?.channelId == comments[commentIdx]?.childComments[childCommentIdx]?.channelId || auth?.channelId == video?.channelId
                                                                                                            ?
                                                                                                            <button className='removeBtn' onClick={() => setDeletedCommentId(comments[commentIdx]?.childComments[childCommentIdx]?.id)}></button>
                                                                                                            :
                                                                                                            <></>
                                                                                                    }

                                                                                                    {
                                                                                                        auth?.channelId == comments[commentIdx]?.childComments[childCommentIdx]?.channelId
                                                                                                            ?
                                                                                                            <button className='editBtn' onClick={() => changeIsUpdatingReply(comments[commentIdx]?.childComments[childCommentIdx]?.id, true)}></button>
                                                                                                            :
                                                                                                            <></>
                                                                                                    }
                                                                                                </div>
                                                                                            </div>
                                                                                            <div className="commentReaction">
                                                                                                <div className="commentLike">
                                                                                                    <img src={comments[commentIdx]?.childComments[childCommentIdx]?.isLiked == true ? like_active : like} alt="" onClick={() => handleLikeReply(comments[commentIdx]?.childComments[childCommentIdx]?.id)} />
                                                                                                    {comments[commentIdx]?.childComments[childCommentIdx]?.numberOfLikes}
                                                                                                </div>
                                                                                                <div className="commentDislike">
                                                                                                    <img src={comments[commentIdx]?.childComments[childCommentIdx]?.isLiked == false ? dislike_active : dislike} alt="" onClick={() => handleDislikeReply(comments[commentIdx]?.childComments[childCommentIdx]?.id)} />
                                                                                                    {comments[commentIdx]?.childComments[childCommentIdx]?.numberOfDislikes}
                                                                                                </div>
                                                                                            </div>
                                                                                        </div>
                                                                                }

                                                                            </div>
                                                                        )
                                                                    })
                                                                }
                                                            </>
                                                            :
                                                            <></>
                                                    }
                                                </div>
                                        }
                                    </div>
                                )
                            })
                        }

                    </div>
                </div>


                <div className="sidebarContainer">
                    <Outlet></Outlet>
                    <h2>Recommended Videos</h2>
                    {
                        recommendedVideos
                        &&
                        Object.keys(recommendedVideos).map((videoIdx) => {
                            return (
                                recommendedVideos[videoIdx]?.isPlaylist
                                    ?
                                    <div className="playlist-preview-small" onClick={() => navigate(`/watch/${recommendedVideos[videoIdx]?.id}/${recommendedVideos[videoIdx]?.playlistId}/${false}`)} key={recommendedVideos[videoIdx]?.id + "_playlist_" + recommendedVideos[videoIdx]?.playlistId}>
                                        <div className="thumbnail-row-small">
                                            <img className="thumbnail-small playlist-thumbnail-small" src={recommendedVideos[videoIdx]?.previewUrl + `?timestamp=${Date.now()}`} alt="Playlist Thumbnail"></img>
                                            <div className="video-count-small">{recommendedVideos[videoIdx]?.videoCount} videos</div>
                                        </div>

                                        <div className="playlist-details-small">
                                            <p className="playlist-title-small">{recommendedVideos[videoIdx]?.title}</p>
                                            <p className="playlist-meta-small">{recommendedVideos[videoIdx]?.channelTitle} · Updated {timeAgo.format(new Date(recommendedVideos[videoIdx]?.publicationDate))}</p>
                                            <a href="#" className="playlist-link-small">View full playlist</a>
                                        </div>
                                    </div>
                                    :
                                    <div className="video-preview-small" onClick={() => navigate(`/watch/${recommendedVideos[videoIdx]?.id}`)} key={recommendedVideos[videoIdx]?.id + "_video"}>
                                        <div className="thumbnail-row-small">
                                            <img className="thumbnail-small"  src={recommendedVideos[videoIdx]?.previewUrl + `?timestamp=${Date.now()}`}></img>
                                            <div className="video-time-small">{recommendedVideos[videoIdx]?.duration}</div>
                                        </div>
                                        <div className="video-info">
                                            <p className="video-title-small">{recommendedVideos[videoIdx]?.title}</p>
                                            <p className="video-author-small">{recommendedVideos[videoIdx]?.channelTitle}</p>
                                            <p className="video-stats-small">{recommendedVideos[videoIdx]?.numberOfViews} views &#183; {timeAgo.format(new Date(recommendedVideos[videoIdx]?.publicationDate))}</p>
                                        </div>
                                    </div>
                            );
                        })
                    }



                </div>
            </div>
            {
                isAddingToPlaylist
                    ?
                    <div className='blackbg'>
                        <div className='addingToPlaylistForm prevent-select'>
                            <h2>Choose playlists!</h2>
                            <div className="playlistContainer">
                                {
                                    Object.keys(playlists).map((playlistIdx) => {

                                        return (
                                            <label className="playlistItem" key={playlists[playlistIdx].id}>
                                                <input type="checkbox" checked={playlists[playlistIdx].hasVideo} onChange={() => handleAddRemovePlaylist(playlists[playlistIdx].id)} />
                                                <span>{playlists[playlistIdx].title}</span>
                                            </label>
                                        )
                                    })
                                }
                            </div>
                            <div className="buttonCont okBtnCont">
                                <button className='submitBtn' onClick={() => setIsAddingToPlaylist(false)}>Ok</button>
                            </div>
                        </div>
                    </div>
                    :
                    <></>
            }
            {
                message.length > 0
                    ?
                    <InfoWindow message={message} onOk={() => setMessage("")}></InfoWindow>
                    :
                    <></>
            }
            {
                deletedCommentId
                    ?
                    <ConfirmationWindow message="Are you sure you want to delete the comment with all corresponding comments and reactions?" onCancel={() => setDeletedCommentId(null)} onConfirm={handleDeleteComment}></ConfirmationWindow>
                    :
                    <></>
            }
        </div>
    );
}

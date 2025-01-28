using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Channels;
using VibeoServer.DTOs;
using VibeoServer.Helpers;
using VibeoServer.Models;
using VibeoServer.Services;
using VibeoServer.Services.Interfaces;
using static MimeDetective.Definitions.Default.FileTypes;

namespace VibeoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentsController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IChannelService _channelService;
        private readonly ICommentService _commentService;
        private readonly ICommentStatusService _commentStatusService;
        private readonly INotificationService _notificationService;
        private readonly INotificationTypeService _notificationTypeService;
        private readonly UserManager<User> _userManager;

        public CommentsController(IVideoService videoService,
            IChannelService channelService,
            UserManager<User> userManager,
            ICommentService commentService,
            ICommentStatusService commentStatusService,
            INotificationService notificationService,
            INotificationTypeService notificationTypeService)
        {
            _videoService = videoService;
            _channelService = channelService;
            _userManager = userManager;
            _commentService = commentService;
            _commentStatusService = commentStatusService;
            _notificationService = notificationService;
            _notificationTypeService = notificationTypeService;
        }

        [HttpPatch("dislike/{commentId}")]
        public async Task<IActionResult> DislikeComment(int commentId)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            var comment = await _commentService.GetByIdQuery(commentId).FirstOrDefaultAsync();
            if (comment == null)
            {
                return BadRequest("Comment not found");
            }

            var commentStatus = await _commentStatusService.GetAllQuery().Where(cs => cs.UserId == user.Id && cs.CommentId == comment.Id).FirstOrDefaultAsync();

            try
            {
                if (commentStatus == null)
                {
                    CommentStatus newCommentStatus = new CommentStatus
                    {
                        CommentId = comment.Id,
                        UserId = user.Id,
                        isLiked = false
                    };
                    await _commentStatusService.CreateAsync(newCommentStatus);
                    return Ok(new
                    {
                        isLiked = false
                    });
                }
                else
                {
                    commentStatus.isLiked = commentStatus.isLiked == null || commentStatus.isLiked == true ? false : null;
                    await _commentStatusService.UpdateAsync(commentStatus.Id, commentStatus);
                    return Ok(new
                    {
                        commentStatus.isLiked
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while subscribing");
            }


        }

        [HttpPatch("like/{commentId}")]
        public async Task<IActionResult> LikeComment(int commentId)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            var comment = await _commentService.GetByIdQuery(commentId).FirstOrDefaultAsync();
            if (comment == null)
            {
                return BadRequest("Comment not found");
            }

            var commentStatus = await _commentStatusService.GetAllQuery().Where(cs => cs.UserId == user.Id && cs.CommentId == comment.Id).FirstOrDefaultAsync();

            try
            {
                if (commentStatus == null)
                {
                    CommentStatus newCommentStatus = new CommentStatus
                    {
                        CommentId = comment.Id,
                        UserId = user.Id,
                        isLiked = true
                    };
                    await _commentStatusService.CreateAsync(newCommentStatus);
                    return Ok(new
                    {
                        isLiked = true
                    });
                }
                else
                {
                    commentStatus.isLiked = commentStatus.isLiked == null || commentStatus.isLiked == false ? true : null;
                    await _commentStatusService.UpdateAsync(commentStatus.Id, commentStatus);
                    return Ok(new
                    {
                        commentStatus.isLiked
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while subscribing");
            }


        }


        [HttpPatch("updatecomment/{commentId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> UpdateComment(int commentId, [FromBody] CommentModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(m => m.Value.Errors.Any())
                    .SelectMany(m => m.Value.Errors.Select(e => new CustomResponse
                    {
                        Field = m.Key,
                        Type = "ValidationError",
                        Details = e.ErrorMessage
                    }))
                    .ToList();

                return StatusCode(StatusCodes.Status400BadRequest, errors);
            }


            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.Users
                .Include(u => u.Channel)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            if (user.Channel == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The user doesn't have a channel");
            }

            try
            {
                var comment = await _commentService.GetByIdQuery(commentId).FirstOrDefaultAsync();
                if(comment == null)
                {
                    return BadRequest("No such comment");
                }
                if(comment.ChannelId != user.Channel.Id)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "Not your comment!");
                }
                comment.Content = model.Message;
                var updatedComment = await _commentService.UpdateAsync(commentId, comment);
                return Ok(updatedComment.Content);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }


        [HttpDelete("deletecomment/{commentId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.Users
                .Include(u => u.Channel)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            if (user.Channel == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The user doesn't have a channel");
            }
            var comment = await _commentService.GetByIdQuery(commentId).Include(v => v.Video).FirstOrDefaultAsync();
            if (comment.ChannelId != user.Channel.Id && user.Channel.Id != comment.Video.ChannelId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Not your comment!");
            }
            try
            {
                var res = await _commentService.DeleteAsync(commentId);
                return Ok(res);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }


        [HttpPost("addcomment/{videoId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> AddComment(int videoId, [FromBody] CommentModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(m => m.Value.Errors.Any())
                    .SelectMany(m => m.Value.Errors.Select(e => new CustomResponse
                    {
                        Field = m.Key,
                        Type = "ValidationError",
                        Details = e.ErrorMessage
                    }))
                    .ToList();

                return StatusCode(StatusCodes.Status400BadRequest, errors);
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.Users
                .Include(u => u.Channel)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            if (user.Channel == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The user doesn't have a channel");
            }


            var video = await _videoService.GetByIdQuery(videoId).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("There is no such video!");
            }
            if(video.IsPublic == false && video.ChannelId != user.Channel.Id)
            {
                return BadRequest("There is no such video!");
            }

            try
            {
                var comment = new Comment {
                    Content = model.Message,
                    CreatedAt = DateTime.Now,
                    ChannelId = user.Channel.Id,
                    VideoId = video.Id
                };

                var newComment = await _commentService.CreateAsync(comment);

                return Ok(new
                {
                    newComment.Content,
                    newComment.CreatedAt
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }


        [HttpPost("replytocomment/{commentId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> ReplyToComment(int commentId, [FromBody] CommentModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(m => m.Value.Errors.Any())
                    .SelectMany(m => m.Value.Errors.Select(e => new CustomResponse
                    {
                        Field = m.Key,
                        Type = "ValidationError",
                        Details = e.ErrorMessage
                    }))
                    .ToList();

                return StatusCode(StatusCodes.Status400BadRequest, errors);
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.Users
                .Include(u => u.Channel)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            if (user.Channel == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The user doesn't have a channel");
            }


            var parentComment = await _commentService.GetByIdQuery(commentId).Where(c => c.ParentComment == null).Include(c => c.ChildComments).Include(c => c.Channel).FirstOrDefaultAsync();

            if (parentComment == null) 
            {
                return BadRequest("No such comment.");
            }

            try
            {
                var comment = new Comment
                {
                    Content = model.Message,
                    CreatedAt = DateTime.Now,
                    ChannelId = user.Channel.Id,
                    VideoId = parentComment.VideoId,
                    ParentCommentId = parentComment.Id
                };

                var newComment = await _commentService.CreateAsync(comment);

                var newCommentRetrieved = await _commentService.GetByIdQuery(newComment.Id).Include(c => c.Channel).Include(c=>c.CommentStatuses).FirstOrDefaultAsync();

                if (user.Channel.Id != parentComment?.ChannelId)
                {
                    NotificationType? notificationType = await _notificationTypeService.GetByNameQuery("CommentReply").FirstOrDefaultAsync();
                    if (notificationType == null)
                    {
                        return BadRequest();
                    }
                    Notification notification = new Notification
                    {
                        UserId = parentComment.Channel.UserId,
                        CommentId = newCommentRetrieved?.Id,
                        CreatedAt = DateTime.Now,
                        IsRead = false,
                        NotificationType = notificationType,
                        Title = $"{newCommentRetrieved?.Channel.Title} replied to your comment",
                        Message = $@"""{newCommentRetrieved?.Content}"""
                    };
                    await _notificationService.CreateAsync(notification);
                }


                return Ok(new
                {
                    newCommentRetrieved.Id,
                    newCommentRetrieved.Content,
                    CreatedAt = newCommentRetrieved.CreatedAt.ToString("o"),
                    newCommentRetrieved.ChannelId,
                    newCommentRetrieved.Channel.LogoUrl,
                    newCommentRetrieved.Channel.Handle,
                    NumberOfLikes = newCommentRetrieved.CommentStatuses.Count(cs => cs.isLiked == true),
                    NumberOfDislikes = newCommentRetrieved.CommentStatuses.Count(cs => cs.isLiked == false)
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }


        [HttpGet("getcomments/{videoId}/{sortOption}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetComments(int videoId, string sortOption)
        {
            var commentsQuery = _commentService
                .GetAllQuery()
                .Include(c => c.Channel)
                .Include(c => c.ChildComments)
                    .ThenInclude(cc => cc.CommentStatuses)
                .Include(c => c.ChildComments)
                    .ThenInclude(cc => cc.Channel)
                .Include(c => c.CommentStatuses)
                .Where(c => c.VideoId == videoId && c.ParentComment == null);

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (!sortOption.IsNullOrEmpty())
            {
                switch (sortOption)
                {
                    case "Most liked":
                        commentsQuery = commentsQuery.OrderByDescending(c => c.CommentStatuses.Count(cs => cs.isLiked == true));
                        break;
                    case "Most hated":
                        commentsQuery = commentsQuery.OrderByDescending(c => c.CommentStatuses.Count(cs => cs.isLiked == false));
                        break;
                    case "Longest":
                        commentsQuery = commentsQuery.OrderByDescending(c => c.ChildComments.Count());
                        break;
                    case "Shortest":
                        commentsQuery = commentsQuery.OrderBy(c => c.ChildComments.Count());
                        break;
                    case "Newest":
                        commentsQuery = commentsQuery.OrderByDescending(c => c.CreatedAt);
                        break;
                    case "Oldest":
                        commentsQuery = commentsQuery.OrderBy(c => c.CreatedAt);
                        break;
                }
            }

            try
            {
                var comments = await commentsQuery.ToListAsync();

                if (user != null)
                {
                    var commentStatusQuery = _commentStatusService.GetAllQuery().Where(cs => cs.UserId == user.Id);
                    return Ok(comments.Select(c => new
                    {
                        c.Id,
                        c.Content,
                        CreatedAt = c.CreatedAt.ToString("o"),
                        c.ChannelId,
                        c.Channel.LogoUrl,
                        c.Channel.Handle,
                        IsLiked = commentStatusQuery.Where(cs => cs.CommentId == c.Id).Select(cs=>cs.isLiked).FirstOrDefault(),
                        NumberOfLikes = c.CommentStatuses.Count(cs => cs.isLiked == true),
                        NumberOfDislikes = c.CommentStatuses.Count(cs => cs.isLiked == false),
                        NumberOfReplies = c.ChildComments.Count(),
                        ChildComments = c.ChildComments.Select(cc => new {
                            cc.Id,
                            cc.Content,
                            CreatedAt = cc.CreatedAt.ToString("o"),
                            cc.ChannelId,
                            cc.Channel.LogoUrl,
                            cc.Channel.Handle,
                            IsLiked = commentStatusQuery.Where(cs => cs.CommentId == cc.Id).Select(cs => cs.isLiked).FirstOrDefault(),
                            NumberOfLikes = cc.CommentStatuses.Count(cs => cs.isLiked == true),
                            NumberOfDislikes = cc.CommentStatuses.Count(cs => cs.isLiked == false)
                        })
                    }));
                }

                Response.Headers.Add("X-Token-Expired", "true");
                return Ok(comments.Select(c => new
                {
                    c.Id,
                    c.Content,
                    CreatedAt = c.CreatedAt.ToString("o"),
                    c.ChannelId,
                    c.Channel.LogoUrl,
                    c.Channel.Handle,
                    NumberOfLikes = c.CommentStatuses.Count(cs => cs.isLiked == true),
                    NumberOfDislikes = c.CommentStatuses.Count(cs => cs.isLiked == false),
                    NumberOfReplies = c.ChildComments.Count(),
                    ChildComments = c.ChildComments.Select(cc => new { 
                        cc.Id, 
                        cc.Content, 
                        CreatedAt = cc.CreatedAt.ToString("o"),
                        cc.ChannelId,
                        cc.Channel.LogoUrl,
                        cc.Channel.Handle,
                        NumberOfLikes = cc.CommentStatuses.Count(cs => cs.isLiked == true),
                        NumberOfDislikes = cc.CommentStatuses.Count(cs => cs.isLiked == false)
                    })
                }));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }

    }
}

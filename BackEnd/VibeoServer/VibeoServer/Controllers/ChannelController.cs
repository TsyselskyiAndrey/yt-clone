using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using VibeoServer.DTOs;
using VibeoServer.Helpers;
using VibeoServer.Models;
using VibeoServer.Services;
using VibeoServer.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VibeoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChannelController : ControllerBase
    {
        private readonly IChannelService _channelService;
        private readonly UserManager<User> _userManager;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containerName = "vibeoserver";

        public ChannelController(IChannelService channelService, UserManager<User> userManager, BlobServiceClient blobServiceClient)
        {
            _channelService = channelService;
            _userManager = userManager; 
            _blobServiceClient = blobServiceClient;
        }

        [HttpGet("periodicchannelstats/{period}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> GetPeriodicStats(string period)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.Users
                .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Videos)
                        .ThenInclude(ch => ch.Comments)
                            .ThenInclude(c => c.ChildComments)
                .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Subscriptions)
                .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Videos)
                        .ThenInclude(v => v.Histories)
                            .ThenInclude(h => h.User)
                                .ThenInclude(u => u.Subscriptions)
                .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Comments)

                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            if (user.Channel == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The user doesn't have a channel");
            }

            switch (period)
            {
                case "all":
                    return Ok(new
                    {
                        ViewersWithoutSubscription = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .Select(h => h.User)
                            .Distinct()
                            .Count(u => !u.Subscriptions.Any(s => s.ChannelId == user.Channel.Id)),

                        Views = user.Channel.Videos
                            .Sum(v => v.Histories.Count()),

                        AvgViews = Math.Round(user.Channel.Videos
                            .Select(v => v.Histories.Count())
                            .DefaultIfEmpty(0)
                            .Average(), 2),

                        Comments = user.Channel.Videos
                            .Sum(v => v.Comments.Count()),

                        AvgComments = Math.Round(user.Channel.Videos
                            .Select(v => v.Comments.Count())
                            .DefaultIfEmpty(0)
                            .Average(), 2),

                        MaxComments = user.Channel.Videos
                            .Select(v => v.Comments.Count())
                            .DefaultIfEmpty(0)
                            .Max(),

                        Subscribers = user.Channel.Subscriptions.Count,

                        NewViewers = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .GroupBy(h => h.UserId)
                            .Count(),

                        NumberOfReplies = user.Channel.Videos
                            .SelectMany(v => v.Comments)
                            .SelectMany(c => c.ChildComments)
                            .Count(),

                        MyComments = user.Channel.Comments.Count(),

                        UniqueViews = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .GroupBy(h => new { h.VideoId, h.UserId })
                            .Count(),

                        UniqueViewers = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .Select(h => h.UserId)           
                            .Distinct()                      
                            .Count()

                    });
                case "year":
                    return Ok(new
                    {
                        ViewersWithoutSubscription = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .Where(h => h.WatchedAt >= DateTime.Now.AddYears(-1))
                            .Select(h => h.User)
                            .Distinct()
                            .Count(u => !u.Subscriptions.Any(s => s.ChannelId == user.Channel.Id)),

                        Views = user.Channel.Videos
                            .Sum(v => v.Histories.Count(h => h.WatchedAt >= DateTime.Now.AddYears(-1))),

                        AvgViews = Math.Round(user.Channel.Videos
                            .Select(v => v.Histories.Count(h => h.WatchedAt >= DateTime.Now.AddYears(-1)))
                            .DefaultIfEmpty(0)
                            .Average(), 2),

                        Comments = user.Channel.Videos
                            .Sum(v => v.Comments.Count(c => c.CreatedAt >= DateTime.Now.AddYears(-1))),

                        AvgComments = Math.Round(user.Channel.Videos
                            .Select(v => v.Comments.Count(c => c.CreatedAt >= DateTime.Now.AddYears(-1)))
                            .DefaultIfEmpty(0)
                            .Average(), 2),

                        MaxComments = user.Channel.Videos
                            .Select(v => v.Comments.Count(c => c.CreatedAt >= DateTime.Now.AddYears(-1)))
                            .DefaultIfEmpty(0)
                            .Max(),

                        Subscribers = user.Channel.Subscriptions.Count(s => s.SubscribedAt >= DateTime.Now.AddYears(-1)),

                        NewViewers = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .GroupBy(h => h.UserId)
                            .Count(g => g.Min(h => h.WatchedAt) >= DateTime.Now.AddYears(-1)),

                        NumberOfReplies = user.Channel.Videos
                            .SelectMany(v => v.Comments)
                            .SelectMany(c => c.ChildComments)
                            .Count(cc => cc.CreatedAt >= DateTime.Now.AddYears(-1)),

                        MyComments = user.Channel.Comments.Count(c => c.CreatedAt >= DateTime.Now.AddYears(-1)),

                        UniqueViews = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .Where(h => h.WatchedAt >= DateTime.Now.AddYears(-1))
                            .GroupBy(h => new { h.VideoId, h.UserId })
                            .Count(),

                        UniqueViewers = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .Where(h => h.WatchedAt >= DateTime.Now.AddYears(-1))
                            .Select(h => h.UserId)
                            .Distinct()
                            .Count()
                    });
                case "month":
                    return Ok(new
                    {
                        ViewersWithoutSubscription = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .Where(h => h.WatchedAt >= DateTime.Now.AddMonths(-1))
                            .Select(h => h.User)
                            .Distinct()
                            .Count(u => !u.Subscriptions.Any(s => s.ChannelId == user.Channel.Id)),

                        Views = user.Channel.Videos
                            .Sum(v => v.Histories.Count(h => h.WatchedAt >= DateTime.Now.AddMonths(-1))),

                        AvgViews = Math.Round(user.Channel.Videos
                            .Select(v => v.Histories.Count(h => h.WatchedAt >= DateTime.Now.AddMonths(-1)))
                            .DefaultIfEmpty(0)
                            .Average(), 2),

                        Comments = user.Channel.Videos
                            .Sum(v => v.Comments.Count(c => c.CreatedAt >= DateTime.Now.AddMonths(-1))),

                        AvgComments = Math.Round(user.Channel.Videos
                            .Select(v => v.Comments.Count(c => c.CreatedAt >= DateTime.Now.AddMonths(-1)))
                            .DefaultIfEmpty(0)
                            .Average(), 2),

                        MaxComments = user.Channel.Videos
                            .Select(v => v.Comments.Count(c => c.CreatedAt >= DateTime.Now.AddMonths(-1)))
                            .DefaultIfEmpty(0)
                            .Max(),

                        Subscribers = user.Channel.Subscriptions.Count(s => s.SubscribedAt >= DateTime.Now.AddMonths(-1)),

                        NewViewers = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .GroupBy(h => h.UserId)
                            .Count(g => g.Min(h => h.WatchedAt) >= DateTime.Now.AddMonths(-1)),

                        NumberOfReplies = user.Channel.Videos
                            .SelectMany(v => v.Comments)
                            .SelectMany(c => c.ChildComments)
                            .Count(cc => cc.CreatedAt >= DateTime.Now.AddMonths(-1)),

                        MyComments = user.Channel.Comments.Count(c => c.CreatedAt >= DateTime.Now.AddMonths(-1)),

                        UniqueViews = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .Where(h => h.WatchedAt >= DateTime.Now.AddMonths(-1))
                            .GroupBy(h => new { h.VideoId, h.UserId })
                            .Count(),

                        UniqueViewers = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .Where(h => h.WatchedAt >= DateTime.Now.AddMonths(-1))
                            .Select(h => h.UserId)
                            .Distinct()
                            .Count()
                    });
                case "day":
                    return Ok(new
                    {
                        ViewersWithoutSubscription = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .Where(h => h.WatchedAt >= DateTime.Now.AddDays(-1))
                            .Select(h => h.User)
                            .Distinct()
                            .Count(u => !u.Subscriptions.Any(s => s.ChannelId == user.Channel.Id)),

                        Views = user.Channel.Videos
                            .Sum(v => v.Histories.Count(h => h.WatchedAt >= DateTime.Now.AddDays(-1))),

                        AvgViews = Math.Round(user.Channel.Videos
                            .Select(v => v.Histories.Count(h => h.WatchedAt >= DateTime.Now.AddDays(-1)))
                            .DefaultIfEmpty(0)
                            .Average(), 2),

                        Comments = user.Channel.Videos
                            .Sum(v => v.Comments.Count(c => c.CreatedAt >= DateTime.Now.AddDays(-1))),

                        AvgComments = Math.Round(user.Channel.Videos
                            .Select(v => v.Comments.Count(c => c.CreatedAt >= DateTime.Now.AddDays(-1)))
                            .DefaultIfEmpty(0)
                            .Average(), 2),

                        MaxComments = user.Channel.Videos
                            .Select(v => v.Comments.Count(c => c.CreatedAt >= DateTime.Now.AddDays(-1)))
                            .DefaultIfEmpty(0)
                            .Max(),

                        Subscribers = user.Channel.Subscriptions.Count(s => s.SubscribedAt >= DateTime.Now.AddDays(-1)),

                        NewViewers = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .GroupBy(h => h.UserId)
                            .Count(g => g.Min(h => h.WatchedAt) >= DateTime.Now.AddDays(-1)),

                        NumberOfReplies = user.Channel.Videos
                            .SelectMany(v => v.Comments)
                            .SelectMany(c => c.ChildComments)
                            .Count(cc => cc.CreatedAt >= DateTime.Now.AddDays(-1)),

                        MyComments = user.Channel.Comments.Count(c => c.CreatedAt >= DateTime.Now.AddDays(-1)),

                        UniqueViews = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .Where(h => h.WatchedAt >= DateTime.Now.AddDays(-1))
                            .GroupBy(h => new { h.VideoId, h.UserId })
                            .Count(),

                        UniqueViewers = user.Channel.Videos
                            .SelectMany(v => v.Histories)
                            .Where(h => h.WatchedAt >= DateTime.Now.AddDays(-1))
                            .Select(h => h.UserId)
                            .Distinct()
                            .Count()
                    });
                default:
                    return BadRequest();
            }
        }

        [HttpGet("generalchannelstats")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> GetGeneralStats()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.Users
                .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Videos)
                        .ThenInclude(v => v.VideoStatuses)
                .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Videos)
                        .ThenInclude(ch => ch.Comments)
                            .ThenInclude(c => c.CommentStatuses)
                .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Videos)
                        .ThenInclude(ch => ch.Comments)
                            .ThenInclude(c => c.ChildComments)
                 .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Videos)
                        .ThenInclude(v => v.Tags)
                 .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Videos)
                        .ThenInclude(v => v.Categories)
                 .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Videos)
                        .ThenInclude(v => v.Histories)
                            .ThenInclude(h => h.User)
                 .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Videos)
                        .ThenInclude (v => v.Playlists)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            if (user.Channel == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The user doesn't have a channel");
            }

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            var MostPopularVideoTmp = user.Channel.Videos
                .SelectMany(v => v.Histories)
                .GroupBy(history => history.Video)
                .Select(g => new
                {
                    Video = g.Key,
                    WatchCount = g.Count()
                })
                .OrderByDescending(g => g.WatchCount)
                .FirstOrDefault();

            var MostPopularVideoResult = MostPopularVideoTmp != null
                ? $"{MostPopularVideoTmp.Video.Title} / {MostPopularVideoTmp.Video.PublicationDate:yyyy-MM-dd} / {MostPopularVideoTmp.Video.Histories.Count()}"
                : "Нет данных";

            return Ok(new
            {
                HighestRatingVideo = user.Channel.Videos.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == true) - v.VideoStatuses.Count(vs => vs.IsLiked == false)).FirstOrDefault()?.Title,
                NumberOfLikes = user.Channel.Videos.Sum(v => v.VideoStatuses.Count(vs => vs.IsLiked == true)),
                AvgNumberOfLikes = Math.Round(
                    user.Channel.Videos
                        .Select(v => v.VideoStatuses.Count(vs => vs.IsLiked == true))
                        .DefaultIfEmpty(0)
                        .Average(), 2
                ),
                MaxNumberOfLikes = user.Channel.Videos
                    .Select(v => v.VideoStatuses.Count(vs => vs.IsLiked == true))
                    .DefaultIfEmpty(0)
                    .Max(),
                NumberOfDislikes = user.Channel.Videos.Sum(v => v.VideoStatuses.Count(vs => vs.IsLiked == false)),
                AvgNumberOfDislikes = Math.Round(
                    user.Channel.Videos
                        .Select(v => v.VideoStatuses.Count(vs => vs.IsLiked == false))
                        .DefaultIfEmpty(0)
                        .Average(), 2
                ),
                MaxNumberOfDislikes = user.Channel.Videos
                    .Select(v => v.VideoStatuses.Count(vs => vs.IsLiked == false))
                    .DefaultIfEmpty(0)
                    .Max(),
                MostPopularTag = user.Channel.Videos
                    .SelectMany(v => v.Tags)
                    .GroupBy(tag => tag)
                    .Select(g => new
                    {
                        Tag = g.Key,
                        UsageCount = g.Count()
                    })
                    .OrderByDescending(t => t.UsageCount)
                    .FirstOrDefault()
                    ?.Tag.Name,
                MostPopularCategory = user.Channel.Videos
                    .SelectMany(v => v.Categories)
                    .GroupBy(category => category)
                    .Select(g => new
                    {
                        Category = g.Key,
                        UsageCount = g.Count()
                    })
                    .OrderByDescending(t => t.UsageCount)
                    .FirstOrDefault()
                    ?.Category.Name,
                MostPopularVideo = MostPopularVideoResult,
                MostLikedVideo = user.Channel.Videos
                    .SelectMany(v => v.VideoStatuses)
                    .Where(vs => vs.IsLiked == true)
                    .GroupBy(vs => vs.Video)
                    .Select(g => new
                    {
                        Video = g.Key,
                        LikeCount = g.Count()
                    })
                    .OrderByDescending(g => g.LikeCount)
                    .FirstOrDefault()
                    ?.Video.Title,
                PersistentViewers = user.Channel.Videos
                    .SelectMany(v => v.Histories)
                    .Where(h => h.WatchedAt >= DateTime.Now.AddYears(-1))
                    .GroupBy(history => history.User)
                    .Where(g =>
                    {
                        var uniqueVideosWatched = g.Select(h => h.VideoId).Distinct().Count();
                        var percentageWatched = (double)uniqueVideosWatched / user.Channel.Videos.Count() * 100;
                        return percentageWatched >= 80;
                    })
                    .Select(g => g.Key).Count(),

                MostDislikedVideo = user.Channel.Videos
                    .SelectMany(v => v.VideoStatuses)
                    .Where(vs => vs.IsLiked == false)
                    .GroupBy(vs => vs.Video)
                    .Select(g => new
                    {
                        Video = g.Key,
                        LikeCount = g.Count()
                    })
                    .OrderByDescending(g => g.LikeCount)
                    .FirstOrDefault()
                    ?.Video.Title,

               
                MostLikedComment = user.Channel.Videos
                    .SelectMany(v => v.Comments)
                    .SelectMany(v => v.CommentStatuses)
                    .Where(cs => cs.isLiked == true)
                    .GroupBy(cs => cs.Comment)
                    .Select(g => new
                    {
                        Comment = g.Key,
                        LikeCount = g.Count()
                    })
                    .OrderByDescending(g => g.LikeCount)
                    .FirstOrDefault()
                    ?.Comment.Content,

                NumberOfReplies = user.Channel.Videos
                    .SelectMany(v => v.Comments)              
                    .Select(c => new 
                    {
                        Comment = c,
                        RepliesCount = c.ChildComments.Count() 
                    })
                    .OrderByDescending(c => c.RepliesCount)  
                    .FirstOrDefault()?.RepliesCount,

                MostDiscussedVideo = user.Channel.Videos
                    .SelectMany(v => v.Comments)
                    .GroupBy(c => c.Video)
                    .Select(g => new
                    {
                        Video = g.Key,
                        CommentCount = g.Count()
                    })
                    .OrderByDescending(c => c.CommentCount)
                    .FirstOrDefault()?.Video.Title,

                NumberOfVideoAddedToPlaylists = user.Channel.Videos.Count(v => v.Playlists.Count > 0)

            });

        }


        [HttpPatch("uploadlogo")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> AddLogo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var allowedContentTypes = new[] { "image/png", "image/jpeg", "image/jpg" };

            if (!allowedContentTypes.Contains(file.ContentType.ToLower()))
            {
                return BadRequest("Unsupported file format.");
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
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var previewBlobName = $"channel-avatars/{user.Channel.Id}.jpeg";
                var blobClient = containerClient.GetBlobClient(previewBlobName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }
                user.Channel.LogoUrl = blobClient.Uri.ToString();

                var updatedChannel = await _channelService.UpdateAsync(user.Channel.Id, user.Channel);
                if (updatedChannel == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating the channel.");
                }

                return Ok(new
                {
                    updatedChannel.LogoUrl
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }


        [HttpPatch("updatechannel")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> UpdateChannel([FromBody] ChannelModel model)
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
                if ((await _channelService.IsHandleTakenAsync(model.Handle)) && model.Handle != user.Channel.Handle)
                {
                    return StatusCode(StatusCodes.Status409Conflict, new List<CustomResponse> {
                    new() {
                        Type="ValidationError",
                        Field = "Handle",
                        Details = "The handle is already taken!"
                    }
                });
                }
                user.Channel.Title = model.Title;
                user.Channel.Description = model.Description;
                user.Channel.Handle = model.Handle;

                var updatedChannel = await _channelService.UpdateAsync(user.Channel.Id, user.Channel);
                if (updatedChannel == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating the channel.");
                }

                return Ok(new
                {
                    updatedChannel.Id,
                    updatedChannel.Title,
                    updatedChannel.Description,
                    updatedChannel.Handle,
                    updatedChannel.LogoUrl,
                    CreatedAt = updatedChannel.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss"),
                    user.Email
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating the profile.");
            }

        }


        [HttpGet("getchannel")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> GetChannel()
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

            return Ok(new
            {
                user.Channel.Id,
                user.Channel.Title,
                user.Channel.Description,
                user.Channel.Handle,
                user.Channel.LogoUrl,
                CreatedAt = user.Channel.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss"),
                user.Email
            });

        }


        [HttpDelete("deletechannel")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> DeleteChannel()
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

            try
            {

                var res = await _channelService.DeleteAsync(user.Channel.Id);
                if (res)
                {
                    if (User.IsInRole("channel_owner"))
                    {
                        await _userManager.RemoveFromRoleAsync(user, "channel_owner");
                    }
                }
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while deleting video.");
            }

        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateChannel([FromBody] ChannelModel model)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (email == null)
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

            if (user.Channel != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, "The user already has a channel");
            }

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

            if (await _channelService.IsHandleTakenAsync(model.Handle))
            {
                return StatusCode(StatusCodes.Status409Conflict, new List<CustomResponse> {
                    new() {
                        Type="ValidationError",
                        Field = "Handle",
                        Details = "The handle is already taken!"
                    }
                });
            }

            var channel = new Channel
            {
                Title = model.Title,
                Description = model.Description,
                CreatedAt = DateTime.Now,
                Handle = model.Handle,
                IsDeleted = false,
                UserId = user.Id,
                User = user,
                LogoUrl = "https://mystoragets.blob.core.windows.net/vibeoserver/channel-avatars/default.png"
            };

            try
            {
                var newChannel = await _channelService.CreateAsync(channel);


                if (newChannel == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error while creating the channel");
                }
                else
                {
                    if (!User.IsInRole("channel_owner"))
                    {
                        await _userManager.AddToRoleAsync(user, "channel_owner");
                    }
                    return Ok(new
                    {
                        newChannel.Id,
                        newChannel.Handle,
                        newChannel.Title,
                        newChannel.LogoUrl,
                        newChannel.Description,
                        newChannel.CreatedAt,
                        newChannel.UserId
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while creating the channel");
            }

            
        }
    
    
        

    }
}

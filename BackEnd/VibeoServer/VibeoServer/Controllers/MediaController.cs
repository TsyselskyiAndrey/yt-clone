using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Channels;
using VibeoServer.DTOs;
using VibeoServer.Helpers;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;
using static MimeDetective.Definitions.Default.FileTypes;
using static VibeoServer.Controllers.ContentController;

namespace VibeoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MediaController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly ICategoryService _categoryService;
        private readonly IChannelService _channelService;
        private readonly ITagService _tagService;
        private readonly IPlaylistService _playlistService;
        private readonly ISubscriptionService _subscriptionService;
        private readonly IVideoStatusService _videoStatusService;
        private readonly IHistoryService _historyService;
        private readonly INotificationService _notificationService;
        private readonly UserManager<User> _userManager;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containerName = "vibeoserver";

        public MediaController(IVideoService videoService, 
            ICategoryService categoryService, 
            IChannelService channelService, 
            IPlaylistService playlistService, 
            ISubscriptionService subscriptionService, 
            IVideoStatusService videoStatusService,
            IHistoryService historyService,
            UserManager<User> userManager, 
            ITagService tagService,
            INotificationService notificationService,
            BlobServiceClient blobServiceClient)
        {
            _videoService = videoService;
            _categoryService = categoryService;
            _playlistService = playlistService;
            _channelService = channelService;
            _subscriptionService = subscriptionService;
            _videoStatusService = videoStatusService;
            _historyService = historyService;
            _userManager = userManager;
            _tagService = tagService;
            _notificationService = notificationService;
            _blobServiceClient = blobServiceClient;
        }


        [HttpGet("periodicprofilestats/{period}")]
        public async Task<IActionResult> GetPeriodicStats(string period)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }
            
            var user = await _userManager.Users
                .Include(u => u.Subscriptions)
                .Include(u => u.Histories)
                    .ThenInclude(h => h.Video)
                        .ThenInclude(v => v.Channel)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            switch (period)
            {
                case "all":
                    return Ok(new
                    {
                        NumberOfFollowedChannels = user.Subscriptions.Count(),
                        FavoriteChannel = user.Histories
                            .GroupBy(h => h.Video.Channel)
                            .Select(g => new
                            {
                                Channel = g.Key,
                                WatchCount = g.Count()
                            })
                            .OrderByDescending(v => v.WatchCount)
                            .FirstOrDefault()?.Channel.Title,
                        NumberOfWathedVideos = user.Histories
                            .GroupBy(h => h.VideoId)
                            .Select(g => g.First())
                            .Count(),
                        NumberOfViews = user.Histories.Count()
                    });
                case "year":
                    return Ok(new
                    {
                        NumberOfFollowedChannels = user.Subscriptions.Count(s => DateTime.Now.AddYears(-1) <= s.SubscribedAt),
                        FavoriteChannel = user.Histories
                            .GroupBy(h => h.Video.Channel)
                            .Select(g => new
                            {
                                Channel = g.Key,
                                WatchCount = g.Count(h => DateTime.Now.AddYears(-1) <= h.WatchedAt)
                            })
                            .OrderByDescending(v => v.WatchCount)
                            .FirstOrDefault()?.Channel.Title,
                        NumberOfWathedVideos = user.Histories
                            .Where(h => h.WatchedAt >= DateTime.Now.AddYears(-1)) 
                            .GroupBy(h => h.VideoId)                            
                            .Select(g => g.First())                             
                            .Count(),
                        NumberOfViews = user.Histories.Count(h => DateTime.Now.AddYears(-1) <= h.WatchedAt)
                    });
                case "month":
                    return Ok(new
                    {
                        NumberOfFollowedChannels = user.Subscriptions.Count(s => DateTime.Now.AddMonths(-1) <= s.SubscribedAt),
                        FavoriteChannel = user.Histories
                            .GroupBy(h => h.Video.Channel)
                            .Select(g => new
                            {
                                Channel = g.Key,
                                WatchCount = g.Count(h => DateTime.Now.AddMonths(-1) <= h.WatchedAt)
                            })
                            .OrderByDescending(v => v.WatchCount)
                            .FirstOrDefault()?.Channel.Title,
                        NumberOfWathedVideos = user.Histories
                            .Where(h => h.WatchedAt >= DateTime.Now.AddMonths(-1))
                            .GroupBy(h => h.VideoId)
                            .Select(g => g.First())
                            .Count(),
                        NumberOfViews = user.Histories.Count(h => DateTime.Now.AddMonths(-1) <= h.WatchedAt)
                    });
                case "day":
                    return Ok(new
                    {
                        NumberOfFollowedChannels = user.Subscriptions.Count(s => DateTime.Now.AddDays(-1) <= s.SubscribedAt),
                        FavoriteChannel = user.Histories
                            .GroupBy(h => h.Video.Channel)
                            .Select(g => new
                            {
                                Channel = g.Key,
                                WatchCount = g.Count(h => DateTime.Now.AddDays(-1) <= h.WatchedAt)
                            })
                            .OrderByDescending(v => v.WatchCount)
                            .FirstOrDefault()?.Channel.Title,
                        NumberOfWathedVideos = user.Histories
                            .Where(h => h.WatchedAt >= DateTime.Now.AddDays(-1))
                            .GroupBy(h => h.VideoId)
                            .Select(g => g.First())
                            .Count(),
                        NumberOfViews = user.Histories.Count(h => DateTime.Now.AddDays(-1) <= h.WatchedAt)
                    });
                default:
                    return BadRequest();
            }
        }

        [HttpGet("generalprofilestats")]
        public async Task<IActionResult> GetGeneralStats()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.Users
                .Include(u => u.Histories)
                    .ThenInclude(h => h.Video)
                .Include(u => u.CommentStatuses)
                .Include(u => u.VideoStatuses)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            return Ok(new
            {
                MostWatchedVideo = user.Histories
                    .GroupBy(h => h.Video)               
                    .Select(g => new
                    {
                        Video = g.Key,                  
                        WatchCount = g.Count()             
                    })
                    .OrderByDescending(v => v.WatchCount) 
                    .FirstOrDefault()?.Video.Title,
                NumberOfLikedComments = user.CommentStatuses.Count(cs => cs.isLiked == true),
                NumberOfDislikedComments = user.CommentStatuses.Count(cs => cs.isLiked == false),
                NumberOfLikedVideos = user.VideoStatuses.Count(vs => vs.IsLiked == true),
                NumberOfDislikedVideos = user.VideoStatuses.Count(vs => vs.IsLiked == false),
                NumberOfWatchLaterVideos = user.VideoStatuses.Count(vs => vs.WatchLater == true),
                NumberOfFavoriteVideos = user.VideoStatuses.Count(vs => vs.IsFavorite == true)
            });

        }

        [HttpPatch("uploadavatar")]
        public async Task<IActionResult> AddAvatar(IFormFile file)
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

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }


            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var previewBlobName = $"user-avatars/{user.Id}.jpeg";
                var blobClient = containerClient.GetBlobClient(previewBlobName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }
                user.AvatarUrl = blobClient.Uri.ToString();
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new
                    {
                        user.AvatarUrl
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating the profile.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }


        [HttpPatch("updateprofile")]
        public async Task<IActionResult> UpdateProfile([FromBody] RegisterStep2Model model)
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
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            try
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.BirthDate = DateTime.ParseExact(model.BirthDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new {
                        user.Id,
                        user.FirstName,
                        user.LastName,
                        user.Email,
                        user.AvatarUrl,
                        CreatedAt = user.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss"),
                        BirthDate = user.BirthDate?.ToString("MM / dd / yyyy") ?? "N/A"
                    });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating the profile.");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating the profile.");
            }

        }


        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
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

            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.AvatarUrl,
                CreatedAt = user.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss"),
                BirthDate = user.BirthDate?.ToString("MM / dd / yyyy") ?? "N/A"
            });
            
        }


        [HttpPatch("readnotification/{notificationId}")]
        public async Task<IActionResult> ReadNotification(int notificationId)
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


            var notification = await _notificationService.GetByIdQuery(notificationId).FirstOrDefaultAsync();

            if(notification == null)
            {
                return BadRequest("No such notification!");
            }

            try
            {
                notification.IsRead = true;
                var updatedNotification = await _notificationService.UpdateAsync(notification.Id, notification);
                return Ok(new
                {
                    updatedNotification.IsRead
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while reading the notification");
            }
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
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


            var notifications = _notificationService.GetAllQuery()
                .Include(n => n.Comment)
                    .ThenInclude(c => c.Channel)
                .Include(n => n.Video)
                    .ThenInclude(v => v.Channel)
                .Where(n => n.UserId == user.Id)
                .OrderByDescending(n => n.CreatedAt);

            return Ok(new
            {
                NumberOfNotifications = notifications.Count(n => n.IsRead == false),
                Notifications = await notifications.Select(n => new
                {
                    n.Id,
                    n.Title,
                    NotificationType = n.NotificationType.Name,
                    n.Message,
                    n.IsRead,
                    CreatedAt = n.CreatedAt.ToString("o"),
                    ChannelLogo = n.Video.Channel.LogoUrl ?? n.Comment.Channel.LogoUrl ?? "https://mystoragets.blob.core.windows.net/vibeoserver/user-avatars/default.png",
                    VideoId = n.VideoId ?? -1,
                    CommentVideoId = n.Comment == null ? -1 : n.Comment.VideoId
                }).ToListAsync()
            });
        }

        [HttpGet("searchresults/{searchValue}/{searchType}/{sortOption}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSearchResults(string searchValue, string searchType, string sortOption, [FromQuery] SearchFiltersModel filters)
        {

            if(searchType == "Videos")
            {
                var videosQuery = _videoService.GetAllQuery()
                .Include(v => v.Histories)
                .Include(v => v.Channel)
                .Include(v => v.Categories)
                .Where(v => v.IsPublic);

                if (!string.IsNullOrEmpty(searchValue))
                {
                    videosQuery = videosQuery.Where(v => v.Title.ToLower().Contains(searchValue.ToLower()) ||
                        v.Channel.Title.ToLower().Contains(searchValue.ToLower()) ||
                        v.Categories.Any(c => c.Name.ToLower().Contains(searchValue.ToLower())) ||
                        v.Tags.Any(t => t.Name.ToLower().Contains(searchValue.ToLower())));
                }


                if (!filters.PublicationDate.IsNullOrEmpty())
                {
                    switch (filters.PublicationDate)
                    {
                        case "last_hour":
                            videosQuery = videosQuery.Where(v => DateTime.Now.AddHours(-1) <= v.PublicationDate);
                            break;
                        case "today":
                            videosQuery = videosQuery.Where(v => DateTime.Now.AddDays(-1) <= v.PublicationDate);
                            break;
                        case "this_week":
                            videosQuery = videosQuery.Where(v => DateTime.Now.AddDays(-7) <= v.PublicationDate);
                            break;
                        case "this_month":
                            videosQuery = videosQuery.Where(v => DateTime.Now.AddMonths(-1) <= v.PublicationDate);
                            break;
                        case "this_year":
                            videosQuery = videosQuery.Where(v => DateTime.Now.AddYears(-1) <= v.PublicationDate);
                            break;
                    }
                }

                if (!filters.Duration.IsNullOrEmpty())
                {
                    switch (filters.Duration)
                    {
                        case "under_4_minutes":
                            videosQuery = videosQuery.Where(v => v.Duration <= TimeSpan.FromMinutes(4));
                            break;
                        case "4_20_minutes":
                            videosQuery = videosQuery.Where(v => v.Duration >= TimeSpan.FromMinutes(4)
                            && v.Duration <= TimeSpan.FromMinutes(20));
                            break;
                        case "20_60_minutes":
                            videosQuery = videosQuery.Where(v => v.Duration >= TimeSpan.FromMinutes(20)
                            && v.Duration <= TimeSpan.FromMinutes(60));
                            break;
                        case "over_1_hour":
                            videosQuery = videosQuery.Where(v => v.Duration >= TimeSpan.FromHours(1));
                            break;
                    }
                }

                if (!filters.Likes.IsNullOrEmpty())
                {
                    switch (filters.Likes)
                    {
                        case "under_1_thousand":
                            videosQuery = videosQuery.Where(v => v.VideoStatuses.Count(vs => vs.IsLiked == true) <= 1000);
                            break;
                        case "1_10_thousand":
                            videosQuery = videosQuery.Where(v => v.VideoStatuses.Count(vs => vs.IsLiked == true) >= 1000 && v.VideoStatuses.Count(vs => vs.IsLiked == true) <= 10000);
                            break;
                        case "10_100_thousand":
                            videosQuery = videosQuery.Where(v => v.VideoStatuses.Count(vs => vs.IsLiked == true) >= 10000 && v.VideoStatuses.Count(vs => vs.IsLiked == true) <= 100000);
                            break;
                        case "over_100_thousand":
                            videosQuery = videosQuery.Where(v => v.VideoStatuses.Count(vs => vs.IsLiked == true) >= 100000);
                            break;
                    }
                }

                if (!filters.Views.IsNullOrEmpty())
                {
                    switch (filters.Views)
                    {
                        case "under_1_thousand":
                            videosQuery = videosQuery.Where(v => v.Histories.Count() <= 1000);
                            break;
                        case "1_10_thousand":
                            videosQuery = videosQuery.Where(v => v.Histories.Count() >= 1000 && v.Histories.Count() <= 10000);
                            break;
                        case "10_100_thousand":
                            videosQuery = videosQuery.Where(v => v.Histories.Count() >= 10000 && v.Histories.Count() <= 100000);
                            break;
                        case "100_500_thousand":
                            videosQuery = videosQuery.Where(v => v.Histories.Count() >= 100000 && v.Histories.Count() <= 500000);
                            break;
                        case "over_500_thousand":
                            videosQuery = videosQuery.Where(v => v.Histories.Count() >= 500000);
                            break;
                    }
                }

                if (!filters.Category.IsNullOrEmpty())
                {
                    videosQuery = videosQuery.Where(v => v.Categories.Any(c => c.Name == filters.Category));
                }

                if (!sortOption.IsNullOrEmpty())
                {
                    switch (sortOption)
                    {
                        case "Most liked":
                            videosQuery = videosQuery.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == true));
                            break;
                        case "Most hated":
                            videosQuery = videosQuery.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == false));
                            break;
                        case "Longest":
                            var sortRes1 = (await videosQuery.ToListAsync()).OrderByDescending(v => v.Duration.TotalSeconds).Select(v => new HomeContentModel
                            {
                                Id = v.Id,
                                Title = v.Title,
                                PublicationDate = v.PublicationDate.ToString("o"),
                                PreviewUrl = v.PreviewUrl,
                                Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                                NumberOfViews = v.Histories.Count(),
                                ChannelLogo = v.Channel.LogoUrl,
                                ChannelId = v.Channel.Id,
                                ChannelTitle = v.Channel.Title,
                                PlaylistId = 0,
                                LastUpdated = DateTime.Now.ToString("o"),
                                VideoCount = 0,
                                IsPlaylist = false
                            });
                            return Ok(sortRes1);

                        case "Shortest":
                            var sortRes2 = (await videosQuery.ToListAsync()).OrderBy(v => v.Duration.TotalSeconds).Select(v => new HomeContentModel
                            {
                                Id = v.Id,
                                Title = v.Title,
                                PublicationDate = v.PublicationDate.ToString("o"),
                                PreviewUrl = v.PreviewUrl,
                                Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                                NumberOfViews = v.Histories.Count(),
                                ChannelLogo = v.Channel.LogoUrl,
                                ChannelId = v.Channel.Id,
                                ChannelTitle = v.Channel.Title,
                                PlaylistId = 0,
                                LastUpdated = DateTime.Now.ToString("o"),
                                VideoCount = 0,
                                IsPlaylist = false
                            });
                            return Ok(sortRes2);
                        case "Popular":
                            videosQuery = videosQuery.OrderByDescending(v => v.Histories.Count);
                            break;
                        case "Newest":
                            videosQuery = videosQuery.OrderByDescending(v => v.PublicationDate);
                            break;
                        case "Oldest":
                            videosQuery = videosQuery.OrderBy(v => v.PublicationDate);
                            break;
                    }
                }


                var videoResult = await videosQuery.Select(v => new
                {
                    v.Id,
                    v.Title,
                    v.Description,
                    PublicationDate = v.PublicationDate.ToString("o"),
                    v.PreviewUrl,
                    Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                    NumberOfViews = v.Histories.Count(),
                    ChannelId = v.Channel.Id,
                    ChannelTitle = v.Channel.Title
                }).ToListAsync();

                return Ok(videoResult);
            }
            else if(searchType == "Playlists")
            {
                var playlistQuery = _playlistService.GetAllQuery()
               .Include(p => p.Videos)
               .Include(p => p.User)
                .ThenInclude(u => u.Channel)
               .AsQueryable();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    playlistQuery = playlistQuery.Where(v => v.Title.ToLower().Contains(searchValue.ToLower()) ||
                        v.User.Channel.Title.ToLower().Contains(searchValue.ToLower()));
                }

                if (!filters.PublicationDate.IsNullOrEmpty())
                {
                    switch (filters.PublicationDate)
                    {
                        case "last_hour":
                            playlistQuery = playlistQuery.Where(v => DateTime.Now.AddHours(-1) <= v.CreatedAt);
                            break;
                        case "today":
                            playlistQuery = playlistQuery.Where(v => DateTime.Now.AddDays(-1) <= v.CreatedAt);
                            break;
                        case "this_week":
                            playlistQuery = playlistQuery.Where(v => DateTime.Now.AddDays(-7) <= v.CreatedAt);
                            break;
                        case "this_month":
                            playlistQuery = playlistQuery.Where(v => DateTime.Now.AddMonths(-1) <= v.CreatedAt);
                            break;
                        case "this_year":
                            playlistQuery = playlistQuery.Where(v => DateTime.Now.AddYears(-1) <= v.CreatedAt);
                            break;
                    }
                }

                if (!filters.LastUpdated.IsNullOrEmpty())
                {
                    switch (filters.LastUpdated)
                    {
                        case "last_hour":
                            playlistQuery = playlistQuery.Where(v => DateTime.Now.AddHours(-1) <= v.LastUpdated);
                            break;
                        case "today":
                            playlistQuery = playlistQuery.Where(v => DateTime.Now.AddDays(-1) <= v.LastUpdated);
                            break;
                        case "this_week":
                            playlistQuery = playlistQuery.Where(v => DateTime.Now.AddDays(-7) <= v.LastUpdated);
                            break;
                        case "this_month":
                            playlistQuery = playlistQuery.Where(v => DateTime.Now.AddMonths(-1) <= v.LastUpdated);
                            break;
                        case "this_year":
                            playlistQuery = playlistQuery.Where(v => DateTime.Now.AddYears(-1) <= v.LastUpdated);
                            break;
                    }
                }

                if (!filters.VideoCount.IsNullOrEmpty())
                {
                    switch (filters.VideoCount)
                    {
                        case "under_10":
                            playlistQuery = playlistQuery.Where(v => v.Videos.Count() <= 10);
                            break;
                        case "10_50":
                            playlistQuery = playlistQuery.Where(v => v.Videos.Count() >= 10 && v.Videos.Count() <= 50);
                            break;
                        case "50_100":
                            playlistQuery = playlistQuery.Where(v => v.Videos.Count() >= 50 && v.Videos.Count() <= 100);
                            break;
                        case "100_500":
                            playlistQuery = playlistQuery.Where(v => v.Videos.Count() >= 100 && v.Videos.Count() <= 500);
                            break;
                        case "over_500":
                            playlistQuery = playlistQuery.Where(v => v.Videos.Count() >= 500);
                            break;
                    }
                }

                if (!sortOption.IsNullOrEmpty())
                {
                    switch (sortOption)
                    {
                        case "A-Z":
                            playlistQuery = playlistQuery.OrderBy(p => p.Title);
                            break;
                        case "Z-A":
                            playlistQuery = playlistQuery.OrderByDescending(p => p.Title);
                            break;
                        case "Longest":
                            playlistQuery = playlistQuery.OrderByDescending(p => p.Videos.Count());
                            break;
                        case "Shortest":
                            playlistQuery = playlistQuery.OrderBy(p => p.Videos.Count());
                            break;
                        case "Recently updated":
                            playlistQuery = playlistQuery.OrderByDescending(p => p.LastUpdated);
                            break;
                        case "Long untouched":
                            playlistQuery = playlistQuery.OrderBy(p => p.LastUpdated);
                            break;
                        case "Newest":
                            playlistQuery = playlistQuery.OrderByDescending(p => p.CreatedAt);
                            break;
                        case "Oldest":
                            playlistQuery = playlistQuery.OrderBy(p => p.CreatedAt);
                            break;
                    }
                }


                var playlistResult = await playlistQuery.Select(p => new
                {
                    Playlist = p,
                    LatestVideo = p.Videos.Where(v => v.IsPublic).OrderByDescending(v => v.PublicationDate).FirstOrDefault()
                })
                .Select(p => new
                {
                    Id = p.LatestVideo != null ? p.LatestVideo.Id : 0,
                    Title = p.Playlist.Title,
                    p.Playlist.Description,
                    PublicationDate = p.Playlist.CreatedAt.ToString("o"),
                    PreviewUrl = p.LatestVideo != null ? p.LatestVideo.PreviewUrl : "https://mystoragets.blob.core.windows.net/vibeoserver/previews/default.png",
                    ChannelId = p.Playlist.User.Channel.Id,
                    ChannelTitle = p.Playlist.User.Channel.Title,
                    PlaylistId = p.Playlist.Id,
                    LastUpdated = p.Playlist.LastUpdated.ToString("o"),
                    VideoCount = p.Playlist.Videos.Count(v => v.IsPublic),
                }).ToListAsync();

                return Ok(playlistResult);
            }
            else if(searchType == "Channels")
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                var user = await _userManager.Users.Include(u => u.Channel).FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    Response.Headers["X-Token-Expired"] = "true";
                }


                var channelQuery = _channelService.GetAllQuery()
                  .Include(ch => ch.Videos)
                  .Include(ch => ch.Subscriptions)
                  .AsQueryable();

                if (!string.IsNullOrEmpty(searchValue))
                {
                    channelQuery = channelQuery.Where(v => v.Title.ToLower().Contains(searchValue.ToLower()));
                }

                if (!filters.CreationDate.IsNullOrEmpty())
                {
                    switch (filters.CreationDate)
                    {
                        case "last_hour":
                            channelQuery = channelQuery.Where(v => DateTime.Now.AddHours(-1) <= v.CreatedAt);
                            break;
                        case "today":
                            channelQuery = channelQuery.Where(v => DateTime.Now.AddDays(-1) <= v.CreatedAt);
                            break;
                        case "this_week":
                            channelQuery = channelQuery.Where(v => DateTime.Now.AddDays(-7) <= v.CreatedAt);
                            break;
                        case "this_month":
                            channelQuery = channelQuery.Where(v => DateTime.Now.AddMonths(-1) <= v.CreatedAt);
                            break;
                        case "this_year":
                            channelQuery = channelQuery.Where(v => DateTime.Now.AddYears(-1) <= v.CreatedAt);
                            break;
                    }
                }

                if (!filters.Subscribers.IsNullOrEmpty())
                {
                    switch (filters.Subscribers)
                    {
                        case "under_1_thousand":
                            channelQuery = channelQuery.Where(v => v.Subscriptions.Count() <= 1000);
                            break;
                        case "1_10_thousand":
                            channelQuery = channelQuery.Where(v => v.Subscriptions.Count() >= 1000 && v.Subscriptions.Count() <= 10000);
                            break;
                        case "10_100_thousand":
                            channelQuery = channelQuery.Where(v => v.Subscriptions.Count() >= 10000 && v.Subscriptions.Count() <= 100000);
                            break;
                        case "100_500_thousand":
                            channelQuery = channelQuery.Where(v => v.Subscriptions.Count() >= 100000 && v.Subscriptions.Count() <= 500000);
                            break;
                        case "over_500_thousand":
                            channelQuery = channelQuery.Where(v => v.Subscriptions.Count() >= 500000);
                            break;
                    }
                }

                if (!filters.VideoCount.IsNullOrEmpty())
                {
                    switch (filters.VideoCount)
                    {
                        case "under_10":
                            channelQuery = channelQuery.Where(v => v.Videos.Count() <= 10);
                            break;
                        case "10_50":
                            channelQuery = channelQuery.Where(v => v.Videos.Count() >= 10 && v.Videos.Count() <= 50);
                            break;
                        case "50_100":
                            channelQuery = channelQuery.Where(v => v.Videos.Count() >= 50 && v.Videos.Count() <= 100);
                            break;
                        case "100_500":
                            channelQuery = channelQuery.Where(v => v.Videos.Count() >= 100 && v.Videos.Count() <= 500);
                            break;
                        case "over_500":
                            channelQuery = channelQuery.Where(v => v.Videos.Count() >= 500);
                            break;
                    }
                }

                if (!sortOption.IsNullOrEmpty())
                {
                    switch (sortOption)
                    {
                        case "A-Z":
                            channelQuery = channelQuery.OrderBy(p => p.Title);
                            break;
                        case "Z-A":
                            channelQuery = channelQuery.OrderByDescending(p => p.Title);
                            break;
                        case "Most famous":
                            channelQuery = channelQuery.OrderByDescending(c => c.Subscriptions.Count());
                            break;
                        case "Most unknown":
                            channelQuery = channelQuery.OrderBy(c => c.Subscriptions.Count());
                            break;
                        case "Recently active":
                            channelQuery = channelQuery.OrderByDescending(ch => ch.Videos.Max(v => v.PublicationDate));
                            break;

                        case "Most inactive":
                            channelQuery = channelQuery.OrderBy(ch => ch.Videos.Max(v => v.PublicationDate));
                            break;
                        case "Newest":
                            channelQuery = channelQuery.OrderByDescending(p => p.CreatedAt);
                            break;
                        case "Oldest":
                            channelQuery = channelQuery.OrderBy(p => p.CreatedAt);
                            break;
                    }
                }


                var channelResult = await channelQuery
                .Select(ch => new
                {
                    ch.Id,
                    ch.Title,
                    ch.Handle,
                    ch.Description,
                    ch.LogoUrl,
                    NumberOfSubscribers = ch.Subscriptions.Count(),
                    IsSubscribed = user == null ? false : ch.Subscriptions.Any(s => s.User == user),
                    ReceiveNotifications = user == null ? false : (ch.Subscriptions.Where(s => s.UserId == user.Id).FirstOrDefault() == null ? false : ch.Subscriptions.Where(s => s.UserId == user.Id).FirstOrDefault().ReceiveNotifications)
                }).ToListAsync();

                return Ok(channelResult);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("getchannel/{channelId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetChannel(int channelId)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.Users.Include(u => u.Channel).FirstOrDefaultAsync(u => u.Email == email);


            var channelQuery = _channelService.GetByIdQuery(channelId).Include(ch => ch.Subscriptions).Include(ch => ch.Videos);

            if (user == null)
            {
                Response.Headers["X-Token-Expired"] = "true";
            }

            var channel = await channelQuery.Select(ch => new
            {
                ch.Id,
                ch.Title,
                ch.Description,
                ch.Handle,
                ch.LogoUrl,
                ch.CreatedAt,
                ChannelSubscriptions = ch.Subscriptions.Count(),
                IsSubscribed = user == null ? false : ch.Subscriptions.Any(s => s.User == user),
                ReceiveNotifications = user == null ? false : (ch.Subscriptions.Where(s => s.UserId == user.Id).FirstOrDefault() == null ? false : ch.Subscriptions.Where(s => s.UserId == user.Id).FirstOrDefault().ReceiveNotifications),
                VideoCount = user == null || user.Channel == null || user.Channel.Id != channelId ? ch.Videos.Count(v => v.IsPublic) : ch.Videos.Count()
            })
            .FirstOrDefaultAsync();


            return Ok(channel);
        }

        [HttpPatch("removefromwatchlater/{videoId}")]
        public async Task<IActionResult> RemoveWatchLaterVideo(int videoId)
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

            var video = await _videoService.GetByIdQuery(videoId).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("Video not found");
            }

            var videoStatus = await _videoStatusService.GetAllQuery().Where(vs => vs.UserId == user.Id && vs.VideoId == video.Id).FirstOrDefaultAsync();

            try
            {
                if (videoStatus == null)
                {
                    return BadRequest("Something is wrong!");
                }
                else
                {
                    videoStatus.WatchLater = false;
                    await _videoStatusService.UpdateAsync(videoStatus.Id, videoStatus);
                    return Ok(new
                    {
                        videoStatus.WatchLater
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while subscribing");
            }


        }

        [HttpPatch("removefromfavorite/{videoId}")]
        public async Task<IActionResult> RemoveFavoriteVideo(int videoId)
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

            var video = await _videoService.GetByIdQuery(videoId).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("Video not found");
            }

            var videoStatus = await _videoStatusService.GetAllQuery().Where(vs => vs.UserId == user.Id && vs.VideoId == video.Id).FirstOrDefaultAsync();

            try
            {
                if (videoStatus == null)
                {
                    return BadRequest("Something is wrong!");
                }
                else
                {
                    videoStatus.IsFavorite = false;
                    await _videoStatusService.UpdateAsync(videoStatus.Id, videoStatus);
                    return Ok(new
                    {
                        videoStatus.IsFavorite
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while subscribing");
            }


        }

        [HttpGet("likedvideos/{sortOption}")]
        public async Task<IActionResult> GetLikedVideos(string sortOption)
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

            var videoStatuses = _videoStatusService.GetAllQuery().Where(vs => vs.UserId == user.Id && vs.IsLiked == true)
                .Include(h => h.Video)
                .ThenInclude(v => v.Histories)
                .Include(h => h.Video)
                .ThenInclude(v => v.Channel);

            var videos = videoStatuses.Select(vs => vs.Video).Where(v => v.IsPublic);

            if (!sortOption.IsNullOrEmpty())
            {
                switch (sortOption)
                {
                    case "Most liked":
                        videos = videos.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == true));
                        break;
                    case "Most hated":
                        videos = videos.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == false));
                        break;
                    case "Longest":
                        var sortRes1 = (await videos.ToListAsync()).OrderByDescending(v => v.Duration.TotalSeconds).Select(v => new
                        {
                            v.Id,
                            v.Title,
                            PublicationDate = v.PublicationDate.ToString("o"),
                            v.PreviewUrl,
                            Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                            NumberOfViews = v.Histories.Count(),
                            ChannelId = v.Channel.Id,
                            ChannelTitle = v.Channel.Title
                        });
                        return Ok(sortRes1);

                    case "Shortest":
                        var sortRes2 = (await videos.ToListAsync()).OrderBy(v => v.Duration.TotalSeconds).Select(v => new
                        {
                            v.Id,
                            v.Title,
                            PublicationDate = v.PublicationDate.ToString("o"),
                            v.PreviewUrl,
                            Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                            NumberOfViews = v.Histories.Count(),
                            ChannelId = v.Channel.Id,
                            ChannelTitle = v.Channel.Title
                        });
                        return Ok(sortRes2);
                    case "Popular":
                        videos = videos.OrderByDescending(v => v.Histories.Count);
                        break;
                    case "Newest":
                        videos = videos.OrderByDescending(v => v.PublicationDate);
                        break;
                    case "Oldest":
                        videos = videos.OrderBy(v => v.PublicationDate);
                        break;
                }
            }

            var videosRes = await videos.Select(v => new
            {
                v.Id,
                v.Title,
                v.Description,
                PublicationDate = v.PublicationDate.ToString("o"),
                v.PreviewUrl,
                Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                NumberOfViews = v.Histories.Count(),
                ChannelId = v.Channel.Id,
                ChannelTitle = v.Channel.Title
            }).ToListAsync();

            return Ok(videosRes);
        }

        [HttpGet("watchlatervideos/{sortOption}")]
        public async Task<IActionResult> GetWatchLaterVideos(string sortOption)
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

            var videoStatuses = _videoStatusService.GetAllQuery().Where(vs => vs.UserId == user.Id && vs.WatchLater == true)
                .Include(h => h.Video)
                .ThenInclude(v => v.Histories)
                .Include(h => h.Video)
                .ThenInclude(v => v.Channel);

            var videos = videoStatuses.Select(vs => vs.Video).Where(v => v.IsPublic);

            if (!sortOption.IsNullOrEmpty())
            {
                switch (sortOption)
                {
                    case "Most liked":
                        videos = videos.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == true));
                        break;
                    case "Most hated":
                        videos = videos.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == false));
                        break;
                    case "Longest":
                        var sortRes1 = (await videos.ToListAsync()).OrderByDescending(v => v.Duration.TotalSeconds).Select(v => new
                        {
                            v.Id,
                            v.Title,
                            PublicationDate = v.PublicationDate.ToString("o"),
                            v.PreviewUrl,
                            Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                            NumberOfViews = v.Histories.Count(),
                            ChannelId = v.Channel.Id,
                            ChannelTitle = v.Channel.Title
                        });
                        return Ok(sortRes1);

                    case "Shortest":
                        var sortRes2 = (await videos.ToListAsync()).OrderBy(v => v.Duration.TotalSeconds).Select(v => new
                        {
                            v.Id,
                            v.Title,
                            PublicationDate = v.PublicationDate.ToString("o"),
                            v.PreviewUrl,
                            Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                            NumberOfViews = v.Histories.Count(),
                            ChannelId = v.Channel.Id,
                            ChannelTitle = v.Channel.Title
                        });
                        return Ok(sortRes2);
                    case "Popular":
                        videos = videos.OrderByDescending(v => v.Histories.Count);
                        break;
                    case "Newest":
                        videos = videos.OrderByDescending(v => v.PublicationDate);
                        break;
                    case "Oldest":
                        videos = videos.OrderBy(v => v.PublicationDate);
                        break;
                }
            }

            var videosRes = await videos.Select(v => new
            {
                v.Id,
                v.Title,
                v.Description,
                PublicationDate = v.PublicationDate.ToString("o"),
                v.PreviewUrl,
                Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                NumberOfViews = v.Histories.Count(),
                ChannelId = v.Channel.Id,
                ChannelTitle = v.Channel.Title
            }).ToListAsync();

            return Ok(videosRes);
        }

        [HttpGet("favoritevideos/{sortOption}")]
        public async Task<IActionResult> GetFavoriteVideos(string sortOption)
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

            var videoStatuses = _videoStatusService.GetAllQuery().Where(vs => vs.UserId == user.Id && vs.IsFavorite == true)
                .Include(h => h.Video)
                .ThenInclude(v => v.Histories)
                .Include(h => h.Video)
                .ThenInclude(v => v.Channel);

            var videos = videoStatuses.Select(vs => vs.Video).Where(v => v.IsPublic);

            if (!sortOption.IsNullOrEmpty())
            {
                switch (sortOption)
                {
                    case "Most liked":
                        videos = videos.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == true));
                        break;
                    case "Most hated":
                        videos = videos.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == false));
                        break;
                    case "Longest":
                        var sortRes1 = (await videos.ToListAsync()).OrderByDescending(v => v.Duration.TotalSeconds).Select(v => new
                        {
                            v.Id,
                            v.Title,
                            PublicationDate = v.PublicationDate.ToString("o"),
                            v.PreviewUrl,
                            Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                            NumberOfViews = v.Histories.Count(),
                            ChannelId = v.Channel.Id,
                            ChannelTitle = v.Channel.Title
                        });
                        return Ok(sortRes1);

                    case "Shortest":
                        var sortRes2 = (await videos.ToListAsync()).OrderBy(v => v.Duration.TotalSeconds).Select(v => new
                        {
                            v.Id,
                            v.Title,
                            PublicationDate = v.PublicationDate.ToString("o"),
                            v.PreviewUrl,
                            Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                            NumberOfViews = v.Histories.Count(),
                            ChannelId = v.Channel.Id,
                            ChannelTitle = v.Channel.Title
                        });
                        return Ok(sortRes2);
                    case "Popular":
                        videos = videos.OrderByDescending(v => v.Histories.Count);
                        break;
                    case "Newest":
                        videos = videos.OrderByDescending(v => v.PublicationDate);
                        break;
                    case "Oldest":
                        videos = videos.OrderBy(v => v.PublicationDate);
                        break;
                }
            }

            var videosRes = await videos.Select(v => new
            {
                v.Id,
                v.Title,
                v.Description,
                PublicationDate = v.PublicationDate.ToString("o"),
                v.PreviewUrl,
                Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                NumberOfViews = v.Histories.Count(),
                ChannelId = v.Channel.Id,
                ChannelTitle = v.Channel.Title
            }).ToListAsync();

            return Ok(videosRes);
        }

        [HttpGet("historyvideos")]
        public async Task<IActionResult> GetHistoryVideos()
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

            var histories = await _historyService.GetAllQuery().Where(h => h.UserId == user.Id)
                .Include(h => h.Video)
                .ThenInclude(v => v.Histories)
                .Include(h => h.Video)
                .ThenInclude(v => v.Channel)
                .OrderByDescending(h => h.WatchedAt)
                .ToListAsync();

            var latestHistories = histories.Where(h => h.Video.IsPublic).GroupBy(h => h.Video.Id).Select(g => g.OrderByDescending(h => h.WatchedAt).FirstOrDefault());

            var videos = latestHistories.Select(h => new
            {
                h.Video.Id,
                h.Video.Title,
                h.Video.Description,
                PublicationDate = h.Video.PublicationDate.ToString("o"),
                h.Video.PreviewUrl,
                Duration = new DateTime(h.Video.Duration.Ticks).ToString("HH:mm:ss"),
                NumberOfViews = h.Video.Histories.Count(),
                ChannelId = h.Video.Channel.Id,
                ChannelTitle = h.Video.Channel.Title,
                h.WatchedAt
            });

            return Ok(videos);
        }

        [HttpGet("subscriptionchannels/{sortOption}")]
        public async Task<IActionResult> GetSubChannels(string sortOption)
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

            var subscribedChannelsQuery = _channelService.GetAllQuery().Include(ch => ch.Subscriptions).Where(ch => ch.Subscriptions.Any(s => s.UserId == user.Id));

            if (!sortOption.IsNullOrEmpty())
            {
                switch (sortOption)
                {
                    case "Most popular":
                        subscribedChannelsQuery = subscribedChannelsQuery.OrderByDescending(c => c.Subscriptions.Count());
                        break;
                    case "Most unknown":
                        subscribedChannelsQuery = subscribedChannelsQuery.OrderBy(c => c.Subscriptions.Count());
                        break;
                    case "New activity":
                        subscribedChannelsQuery = subscribedChannelsQuery
                            .Include(ch => ch.Videos)
                            .OrderByDescending(ch => ch.Videos.Max(v => v.PublicationDate));
                        break;

                    case "Most inactive":
                        subscribedChannelsQuery = subscribedChannelsQuery
                            .Include(ch => ch.Videos)
                            .OrderBy(ch => ch.Videos.Max(v => v.PublicationDate));
                        break;
                    case "A-Z":
                        subscribedChannelsQuery = subscribedChannelsQuery.OrderBy(c => c.Title);
                        break;
                    case "Z-A":
                        subscribedChannelsQuery = subscribedChannelsQuery.OrderByDescending(c => c.Title);
                        break;
                }
            }

            var subscribedChannels = await subscribedChannelsQuery
                .Select(ch => new
                {
                    ch.Id,
                    ch.Title,
                    ch.Handle,
                    ch.Description,
                    ch.LogoUrl,
                    NumberOfSubscribers = ch.Subscriptions.Count(),
                    IsSubscribed = true,
                    ReceiveNotifications = ch.Subscriptions
                        .Where(s => s.UserId == user.Id)
                        .Select(s => s.ReceiveNotifications)
                        .FirstOrDefault()
                })
                .ToListAsync();


            return Ok(subscribedChannels);
        }

        [HttpGet("subscriptionvideos")]
        public async Task<IActionResult> GetSubVideos()
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

            var subscribedChannelIds = await _subscriptionService.GetAllQuery().Where(s => s.UserId == user.Id).Select(s => s.ChannelId).ToListAsync();

            var videos = await _videoService.GetAllQuery()
                .Include(v => v.Histories)
                .Include(v => v.Channel)
                .Where(v => v.IsPublic && subscribedChannelIds.Contains(v.ChannelId))
                .OrderByDescending(c => c.PublicationDate)
                .Select(v => new HomeContentModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    PublicationDate = v.PublicationDate.ToString("o"),
                    PreviewUrl = v.PreviewUrl,
                    Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                    NumberOfViews = v.Histories.Count(),
                    ChannelLogo = v.Channel.LogoUrl,
                    ChannelId = v.Channel.Id,
                    ChannelTitle = v.Channel.Title,
                    PlaylistId = 0,
                    LastUpdated = DateTime.Now.ToString("o"),
                    VideoCount = 0,
                    IsPlaylist = false
                }).ToListAsync();

            return Ok(videos);
        }

        [HttpGet("recommendedvideos/{videoId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRecommendedVideos(int videoId)
        {
            var video = await _videoService.GetByIdQuery(videoId)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .FirstOrDefaultAsync();

            if(video == null)
            {
                return BadRequest("No such video");
            }

            var videos = await _videoService.GetAllQuery()
                .Include(v => v.Histories)
                .Include(v => v.Channel)
                .Where(v => v.IsPublic && v.Id != video.Id)
                .Where(v => v.Categories.Any(c => video.Categories.Contains(c)) || v.Tags.Any(t => video.Tags.Contains(t)) || v.ChannelId == video.ChannelId)
                .Select(v => new HomeContentModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    PublicationDate = v.PublicationDate.ToString("o"),
                    PreviewUrl = v.PreviewUrl,
                    Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                    NumberOfViews = v.Histories.Count(),
                    ChannelLogo = v.Channel.LogoUrl,
                    ChannelId = v.Channel.Id,
                    ChannelTitle = v.Channel.Title,
                    PlaylistId = 0,
                    LastUpdated = DateTime.Now.ToString("o"),
                    VideoCount = 0,
                    IsPlaylist = false
                }).ToListAsync();

            var playlists = await _playlistService.GetAllQuery()
                .Include(p => p.Videos)
                .Include(p => p.User)
                .ThenInclude(u => u.Channel)
                .Where(p => p.IsPublic && p.Videos.Any(v => v.IsPublic))
                .Where(p =>p.Videos.Any(v => (v.Id == videoId && p.Videos.Count(vi => vi.IsPublic) > 1 ) || v.Categories.Any(c => video.Categories.Contains(c)) || v.Tags.Any(t => video.Tags.Contains(t)) || v.ChannelId == video.ChannelId))
                .Select(p => new
                {
                    Playlist = p,
                    LatestVideo = p.Videos.Where(v => v.IsPublic).OrderByDescending(v => v.PublicationDate).FirstOrDefault()
                })
                .Select(p => new HomeContentModel
                {
                    Id = p.LatestVideo != null ? p.LatestVideo.Id : 0,
                    Title = p.Playlist.Title,
                    PublicationDate = p.Playlist.CreatedAt.ToString("o"),
                    PreviewUrl = p.LatestVideo != null ? p.LatestVideo.PreviewUrl : "https://mystoragets.blob.core.windows.net/vibeoserver/previews/default.png",
                    Duration = "0",
                    NumberOfViews = 0,
                    ChannelLogo = p.Playlist.User.Channel.LogoUrl,
                    ChannelId = p.Playlist.User.Channel.Id,
                    ChannelTitle = p.Playlist.User.Channel.Title,
                    PlaylistId = p.Playlist.Id,
                    LastUpdated = p.Playlist.LastUpdated.ToString("o"),
                    VideoCount = p.Playlist.Videos.Count(v => v.IsPublic),
                    IsPlaylist = true
                }).ToListAsync();

            var mixedContent = videos.Union(playlists).OrderByDescending(c => c.PublicationDate).Take(20);

            return Ok(mixedContent);
        }

        [HttpPatch("watchlater/{videoId}")]
        public async Task<IActionResult> WatchLaterVideo(int videoId)
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

            var video = await _videoService.GetByIdQuery(videoId).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("Video not found");
            }

            var videoStatus = await _videoStatusService.GetAllQuery().Where(vs => vs.UserId == user.Id && vs.VideoId == video.Id).FirstOrDefaultAsync();

            try
            {
                if (videoStatus == null)
                {
                    VideoStatus newVideoStatus = new VideoStatus
                    {
                        VideoId = video.Id,
                        UserId = user.Id,
                        IsLiked = null,
                        IsFavorite = false,
                        WatchLater = true
                    };
                    await _videoStatusService.CreateAsync(newVideoStatus);
                    return Ok(new
                    {
                        WatchLater = true
                    });
                }
                else
                {
                    videoStatus.WatchLater = !videoStatus.WatchLater;
                    await _videoStatusService.UpdateAsync(videoStatus.Id, videoStatus);
                    return Ok(new
                    {
                        videoStatus.WatchLater
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while subscribing");
            }


        }

        [HttpPatch("favorite/{videoId}")]
        public async Task<IActionResult> FavoriteVideo(int videoId)
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

            var video = await _videoService.GetByIdQuery(videoId).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("Video not found");
            }

            var videoStatus = await _videoStatusService.GetAllQuery().Where(vs => vs.UserId == user.Id && vs.VideoId == video.Id).FirstOrDefaultAsync();

            try
            {
                if (videoStatus == null)
                {
                    VideoStatus newVideoStatus = new VideoStatus
                    {
                        VideoId = video.Id,
                        UserId = user.Id,
                        IsLiked = null,
                        IsFavorite = true,
                        WatchLater = false
                    };
                    await _videoStatusService.CreateAsync(newVideoStatus);
                    return Ok(new
                    {
                        IsFavorite = true
                    });
                }
                else
                {
                    videoStatus.IsFavorite = !videoStatus.IsFavorite;
                    await _videoStatusService.UpdateAsync(videoStatus.Id, videoStatus);
                    return Ok(new
                    {
                        videoStatus.IsFavorite
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while subscribing");
            }


        }

        [HttpPatch("dislike/{videoId}")]
        public async Task<IActionResult> DislikeVideo(int videoId)
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

            var video = await _videoService.GetByIdQuery(videoId).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("Video not found");
            }

            var videoStatus = await _videoStatusService.GetAllQuery().Where(vs => vs.UserId == user.Id && vs.VideoId == video.Id).FirstOrDefaultAsync();

            try
            {
                if (videoStatus == null)
                {
                    VideoStatus newVideoStatus = new VideoStatus
                    {
                        VideoId = video.Id,
                        UserId = user.Id,
                        IsLiked = false,
                        IsFavorite = false,
                        WatchLater = false
                    };
                    await _videoStatusService.CreateAsync(newVideoStatus);
                    return Ok(new
                    {
                        isLiked = false
                    });
                }
                else
                {
                    videoStatus.IsLiked = videoStatus.IsLiked == null || videoStatus.IsLiked == true ? false : null;
                    await _videoStatusService.UpdateAsync(videoStatus.Id, videoStatus);
                    return Ok(new
                    {
                        isLiked = videoStatus.IsLiked
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while subscribing");
            }


        }

        [HttpPatch("like/{videoId}")]
        public async Task<IActionResult> LikeVideo(int videoId)
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

            var video = await _videoService.GetByIdQuery(videoId).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("Video not found");
            }

            var videoStatus = await _videoStatusService.GetAllQuery().Where(vs => vs.UserId == user.Id && vs.VideoId == video.Id).FirstOrDefaultAsync();

            try
            {
                if (videoStatus == null)
                {
                    VideoStatus newVideoStatus = new VideoStatus
                    {
                        VideoId = video.Id,
                        UserId = user.Id,
                        IsLiked = true,
                        IsFavorite = false,
                        WatchLater = false
                    };
                    await _videoStatusService.CreateAsync(newVideoStatus);
                    return Ok(new
                    {
                        isLiked = true
                    });
                }
                else
                {
                    videoStatus.IsLiked = videoStatus.IsLiked == null || videoStatus.IsLiked == false ? true : null;
                    await _videoStatusService.UpdateAsync(videoStatus.Id, videoStatus);
                    return Ok(new
                    {
                        isLiked = videoStatus.IsLiked
                    });
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while subscribing");
            }


        }


        [HttpPatch("receivenotifications/{channelId}")]
        public async Task<IActionResult> ReceiveNotifications(int channelId)
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

            var subscription = await _subscriptionService.GetAllQuery().Where(s => s.UserId == user.Id && s.ChannelId == channelId).FirstOrDefaultAsync();
            if (subscription == null)
            {
                return BadRequest("You are not subscribed");
            }
            else
            {
                try
                {
                    subscription.ReceiveNotifications = !subscription.ReceiveNotifications;
                    var res = await _subscriptionService.UpdateAsync(subscription.Id, subscription);
                    return Ok(res?.ReceiveNotifications);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error while subscribing");
                }
            }


        }


        [HttpPost("subscribe/{channelId}")]
        public async Task<IActionResult> Subscribe(int channelId)
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

            
            var channel = await _channelService.GetByIdQuery(channelId).FirstOrDefaultAsync();
            if (channel == null) 
            {
                return BadRequest("No such channel.");
            }
            if (channel.UserId == user.Id)
            {
                return BadRequest("You can't subscribe to your own channel.");
            }

            var subscription = _subscriptionService.GetAllQuery().Where(s => s.ChannelId == channelId && s.UserId == user.Id).FirstOrDefault();
            if (subscription == null) 
            {
                try
                {
                    subscription = new Subscription
                    {
                        Channel = channel,
                        User = user,
                        SubscribedAt = DateTime.Now,
                        ReceiveNotifications = false
                    };
                    await _subscriptionService.CreateAsync(subscription);
                    return Ok(true);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error while subscribing");
                }
                
                
            }
            else
            {
                try
                {
                    await _subscriptionService.DeleteAsync(subscription.Id);
                    return Ok(false);
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error while subscribing");
                }
            }


        }


        [HttpGet("getplaylistplayerinfo/{playlistId}/{sortOption}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPlaylistPlayerInfo(int playlistId, string sortOption, [FromQuery] bool? getPrivate)
        {
            var playlist = await _playlistService.GetByIdQuery(playlistId)
                .Include(p => p.User)
                .ThenInclude(u => u.Channel)
                .Include(p => p.Videos)
                .ThenInclude(v => v.VideoStatuses)
                .Include(p => p.Videos)
                .ThenInclude(v => v.Histories)
                .Include(p => p.Videos)
                .ThenInclude(v => v.Channel)
                .FirstOrDefaultAsync();

            if(playlist == null)
            {
                return BadRequest("No such playlist");
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);

            var videos = playlist.Videos;

            if (playlist.IsPublic == false || getPrivate == true)
            {
                if (email == null)
                {
                    return Unauthorized("You need to be logged in to access this playlist");
                }

                if (user == null || playlist.UserId != user.Id)
                {
                    return Forbid();
                }

                videos = videos.Where(v => v.IsPublic || v.Channel.UserId == user.Id).ToList();
            }
            else
            {
                videos = videos.Where(v => v.IsPublic).ToList();
            }

            if (!sortOption.IsNullOrEmpty())
            {
                switch (sortOption)
                {
                    case "Most liked":
                        videos = videos.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == true)).ToList();
                        break;
                    case "Most hated":
                        videos = videos.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == false)).ToList();
                        break;
                    case "Longest":
                        videos = videos.OrderByDescending(v => v.Duration.TotalSeconds).ToList();
                        break;
                    case "Shortest":
                        videos = videos.OrderBy(v => v.Duration.TotalSeconds).ToList();
                        break;
                    case "Popular":
                        videos = videos.OrderByDescending(v => v.Histories.Count).ToList();
                        break;
                    case "Newest":
                        videos = videos.OrderByDescending(v => v.PublicationDate).ToList();
                        break;
                    case "Oldest":
                        videos = videos.OrderBy(v => v.PublicationDate).ToList();
                        break;
                }
            }

            return Ok(new {
                playlist.Id,
                playlist.Title,
                ChannelTitle = playlist.User.Channel.Title,
                Videos = videos.Select(v => new
                {
                    v.Id,
                    v.Title,
                    PublicationDate = v.PublicationDate.ToString("o"),
                    v.PreviewUrl,
                    Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                    NumberOfViews = v.Histories.Count(),
                    ChannelId = v.Channel.Id,
                    ChannelTitle = v.Channel.Title,
                })
            });
        }

        [HttpGet("getvideo/{videoId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVideo(int videoId)
        {
            var video = await _videoService.GetByIdQuery(videoId)
                .Include(v => v.Tags)
                .Include(v => v.Categories)
                .Include(v => v.Histories)
                .Include(v => v.Comments)
                .Include(v => v.Channel)
                .ThenInclude(ch => ch.Subscriptions)
                .Include(v => v.VideoStatuses)
                .FirstOrDefaultAsync();

            if (video == null)
            {
                return BadRequest("No such video");
            }


            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);


            if (video.IsPublic == false)
            {
                if (email == null)
                {
                    return Unauthorized("You need to be logged in to access this video");
                }
                
                if (user == null || video.Channel.UserId != user.Id)
                {
                    return Forbid("You do not have access to this video");
                }
            }

            if (user != null)
            {
                var videoStatus = await _videoStatusService.GetAllQuery().Where(vs => vs.VideoId == video.Id && user.Id == vs.UserId).FirstOrDefaultAsync();

                try
                {
                    var lastHistory = await _historyService.GetAllQuery().Where(h => h.UserId == user.Id && h.VideoId == video.Id).OrderByDescending(h => h.WatchedAt).FirstOrDefaultAsync();
                    if (lastHistory == null)
                    {
                        History newHistory = new History
                        {
                            UserId = user.Id,
                            VideoId = video.Id,
                            WatchedAt = DateTime.Now
                        };
                        await _historyService.CreateAsync(newHistory);
                    }
                    else
                    {
                        if (lastHistory.WatchedAt + video.Duration * 2 < DateTime.Now)
                        {
                            History newHistory = new History
                            {
                                UserId = user.Id,
                                VideoId = video.Id,
                                WatchedAt = DateTime.Now
                            };
                            await _historyService.CreateAsync(newHistory);
                        }
                        else
                        {
                            lastHistory.WatchedAt = DateTime.Now;
                            await _historyService.UpdateAsync(lastHistory.Id, lastHistory);
                        }

                    }
                }
                catch (Exception)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error while getting the video");
                }


                return Ok(new
                {
                    video.Id,
                    video.Title,
                    video.Description,
                    Tags = video.Tags.Select(t => new { t.Id, t.Name }),
                    video.PublicationDate,
                    Categories = video.Categories.Select(c => new { c.Id, c.Name }),
                    NumberOfViews = video.Histories.Count(),
                    NumberOfComments = video.Comments.Count(c => c.ParentComment == null),
                    video.ChannelId,
                    ChannelLogo = video.Channel.LogoUrl,
                    ChannelSubscriptions = video.Channel.Subscriptions.Count(),
                    ChannelTitle = video.Channel.Title,
                    IsSubscribed = video.Channel.Subscriptions.Any(s => s.User == user),
                    videoStatus?.IsLiked,
                    NumberOfLikes = video.VideoStatuses.Count(vs => vs.IsLiked == true),
                    NumberOfDislikes = video.VideoStatuses.Count(vs => vs.IsLiked == false),
                    videoStatus?.WatchLater,
                    videoStatus?.IsFavorite,
                    video.Channel.Subscriptions.Where(s => s.UserId == user.Id).FirstOrDefault()?.ReceiveNotifications
                });
            }

            Response.Headers["X-Token-Expired"] = "true";
            return Ok(new
            {
                video.Id,
                video.Title,
                video.Description,
                Tags = video.Tags.Select(t => new { t.Id, t.Name }),
                video.PublicationDate,
                Categories = video.Categories.Select(c => new { c.Id, c.Name }),
                NumberOfViews = video.Histories.Count(),
                NumberOfComments = video.Comments.Count(c => c.ParentComment == null),
                video.ChannelId,
                ChannelLogo = video.Channel.LogoUrl,
                ChannelSubscriptions = video.Channel.Subscriptions.Count(),
                ChannelTitle = video.Channel.Title,
                IsSubscribed = false,
                NumberOfLikes = video.VideoStatuses.Count(vs => vs.IsLiked == true),
                NumberOfDislikes = video.VideoStatuses.Count(vs => vs.IsLiked == false),
                WatchLater = false,
                IsFavorite = false,
                ReceiveNotifications = false
            });

        }

        [HttpGet("channelplaylists/{channelId}/{sortOption}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetChannelPlaylists(int channelId, string sortOption, [FromQuery] PlaylistFiltersModel filters)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.Users.Include(u => u.Channel).FirstOrDefaultAsync(u => u.Email == email);

            var playlistQuery = _playlistService.GetAllQuery()
                .Include(p => p.Videos)
                .Include(p => p.User)
                .ThenInclude(u => u.Channel)
                .Where(p => p.User.Channel.Id == channelId)
                .AsQueryable();

            if (user == null || user.Channel == null || user.Channel.Id != channelId)
            {
                playlistQuery = playlistQuery.Where(p => p.IsPublic && p.Videos.Any(v => v.IsPublic));
            }

            if (user == null)
            {
                Response.Headers["X-Token-Expired"] = "true";
            }

            if (!filters.PublicationDate.IsNullOrEmpty())
            {
                switch (filters.PublicationDate)
                {
                    case "last_hour":
                        playlistQuery = playlistQuery.Where(v => DateTime.Now.AddHours(-1) <= v.CreatedAt);
                        break;
                    case "today":
                        playlistQuery = playlistQuery.Where(v => DateTime.Now.AddDays(-1) <= v.CreatedAt);
                        break;
                    case "this_week":
                        playlistQuery = playlistQuery.Where(v => DateTime.Now.AddDays(-7) <= v.CreatedAt);
                        break;
                    case "this_month":
                        playlistQuery = playlistQuery.Where(v => DateTime.Now.AddMonths(-1) <= v.CreatedAt);
                        break;
                    case "this_year":
                        playlistQuery = playlistQuery.Where(v => DateTime.Now.AddYears(-1) <= v.CreatedAt);
                        break;
                }
            }

            if (!filters.LastUpdated.IsNullOrEmpty())
            {
                switch (filters.LastUpdated)
                {
                    case "last_hour":
                        playlistQuery = playlistQuery.Where(v => DateTime.Now.AddHours(-1) <= v.LastUpdated);
                        break;
                    case "today":
                        playlistQuery = playlistQuery.Where(v => DateTime.Now.AddDays(-1) <= v.LastUpdated);
                        break;
                    case "this_week":
                        playlistQuery = playlistQuery.Where(v => DateTime.Now.AddDays(-7) <= v.LastUpdated);
                        break;
                    case "this_month":
                        playlistQuery = playlistQuery.Where(v => DateTime.Now.AddMonths(-1) <= v.LastUpdated);
                        break;
                    case "this_year":
                        playlistQuery = playlistQuery.Where(v => DateTime.Now.AddYears(-1) <= v.LastUpdated);
                        break;
                }
            }

            if (!filters.VideoCount.IsNullOrEmpty())
            {
                switch (filters.VideoCount)
                {
                    case "under_10":
                        playlistQuery = playlistQuery.Where(v => v.Videos.Count() <= 10);
                        break;
                    case "10_50":
                        playlistQuery = playlistQuery.Where(v => v.Videos.Count() >= 10 && v.Videos.Count() <= 50);
                        break;
                    case "50_100":
                        playlistQuery = playlistQuery.Where(v => v.Videos.Count() >= 50 && v.Videos.Count() <= 100);
                        break;
                    case "100_500":
                        playlistQuery = playlistQuery.Where(v => v.Videos.Count() >= 100 && v.Videos.Count() <= 500);
                        break;
                    case "over_500":
                        playlistQuery = playlistQuery.Where(v => v.Videos.Count() >= 500);
                        break;
                }
            }

            if (!sortOption.IsNullOrEmpty())
            {
                switch (sortOption)
                {
                    case "A-Z":
                        playlistQuery = playlistQuery.OrderBy(p => p.Title);
                        break;
                    case "Z-A":
                        playlistQuery = playlistQuery.OrderByDescending(p => p.Title);
                        break;
                    case "Longest":
                        playlistQuery = playlistQuery.OrderByDescending(p => p.Videos.Count());
                        break;
                    case "Shortest":
                        playlistQuery = playlistQuery.OrderBy(p => p.Videos.Count());
                        break;
                    case "Recently updated":
                        playlistQuery = playlistQuery.OrderByDescending(p => p.LastUpdated);
                        break;
                    case "Long untouched":
                        playlistQuery = playlistQuery.OrderBy(p => p.LastUpdated);
                        break;
                    case "Newest":
                        playlistQuery = playlistQuery.OrderByDescending(p => p.CreatedAt);
                        break;
                    case "Oldest":
                        playlistQuery = playlistQuery.OrderBy(p => p.CreatedAt);
                        break;
                }
            }


            if(user == null || user.Channel == null || user.Channel.Id != channelId)
            {
                var playlistResult = await playlistQuery.Select(p => new
                {
                    Playlist = p,
                    LatestVideo = p.Videos.Where(v => v.IsPublic).OrderByDescending(v => v.PublicationDate).FirstOrDefault()
                })
                .Select(p => new HomeContentModel
                {
                    Id = p.LatestVideo != null ? p.LatestVideo.Id : 0,
                    Title = p.Playlist.Title,
                    PublicationDate = p.Playlist.CreatedAt.ToString("o"),
                    PreviewUrl = p.LatestVideo != null ? p.LatestVideo.PreviewUrl : "https://mystoragets.blob.core.windows.net/vibeoserver/previews/default.png",
                    Duration = "0",
                    NumberOfViews = 0,
                    ChannelLogo = p.Playlist.User.Channel.LogoUrl,
                    ChannelId = p.Playlist.User.Channel.Id,
                    ChannelTitle = p.Playlist.User.Channel.Title,
                    PlaylistId = p.Playlist.Id,
                    LastUpdated = p.Playlist.LastUpdated.ToString("o"),
                    VideoCount = p.Playlist.Videos.Count(v => v.IsPublic),
                    IsPlaylist = true
                }).ToListAsync();

                return Ok(playlistResult);
            }
            
            var playlistResultOwner = await playlistQuery.Select(p => new
            {
                Playlist = p,
                LatestVideo = p.Videos.OrderByDescending(v => v.PublicationDate).FirstOrDefault()
            })
            .Select(p => new HomeContentModel
            {
                Id = p.LatestVideo != null ? p.LatestVideo.Id : 0,
                Title = p.Playlist.Title,
                PublicationDate = p.Playlist.CreatedAt.ToString("o"),
                PreviewUrl = p.LatestVideo != null ? p.LatestVideo.PreviewUrl : "https://mystoragets.blob.core.windows.net/vibeoserver/previews/default.png",
                Duration = "0",
                NumberOfViews = 0,
                ChannelLogo = p.Playlist.User.Channel.LogoUrl,
                ChannelId = p.Playlist.User.Channel.Id,
                ChannelTitle = p.Playlist.User.Channel.Title,
                PlaylistId = p.Playlist.Id,
                LastUpdated = p.Playlist.LastUpdated.ToString("o"),
                VideoCount = p.Playlist.Videos.Count(),
                IsPlaylist = true
            }).ToListAsync();

            return Ok(playlistResultOwner);
        }

        [HttpGet("channelvideos/{channelId}/{sortOption}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetChannelVideos(int channelId, string sortOption, [FromQuery] VideoFiltersModel filters)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.Users.Include(u => u.Channel).FirstOrDefaultAsync(u => u.Email == email);

            var videosQuery = _videoService.GetAllQuery()
                .Include(v => v.Histories)
                .Include(v => v.Channel)
                .Include(v => v.Categories)
                .Where(v => v.ChannelId == channelId);

            if ( user == null || user.Channel == null || user.Channel.Id != channelId) 
            {
                videosQuery = videosQuery.Where(v => v.IsPublic);
            }

            if(user == null)
            {
                Response.Headers["X-Token-Expired"] = "true";
            }

            if (!filters.PublicationDate.IsNullOrEmpty())
            {
                switch (filters.PublicationDate)
                {
                    case "last_hour":
                        videosQuery = videosQuery.Where(v => DateTime.Now.AddHours(-1) <= v.PublicationDate);
                        break;
                    case "today":
                        videosQuery = videosQuery.Where(v => DateTime.Now.AddDays(-1) <= v.PublicationDate);
                        break;
                    case "this_week":
                        videosQuery = videosQuery.Where(v => DateTime.Now.AddDays(-7) <= v.PublicationDate);
                        break;
                    case "this_month":
                        videosQuery = videosQuery.Where(v => DateTime.Now.AddMonths(-1) <= v.PublicationDate);
                        break;
                    case "this_year":
                        videosQuery = videosQuery.Where(v => DateTime.Now.AddYears(-1) <= v.PublicationDate);
                        break;
                }
            }

            if (!filters.Duration.IsNullOrEmpty())
            {
                switch (filters.Duration)
                {
                    case "under_4_minutes":
                        videosQuery = videosQuery.Where(v => v.Duration <= TimeSpan.FromMinutes(4));
                        break;
                    case "4_20_minutes":
                        videosQuery = videosQuery.Where(v => v.Duration >= TimeSpan.FromMinutes(4)
                        && v.Duration <= TimeSpan.FromMinutes(20));
                        break;
                    case "20_60_minutes":
                        videosQuery = videosQuery.Where(v => v.Duration >= TimeSpan.FromMinutes(20)
                        && v.Duration <= TimeSpan.FromMinutes(60));
                        break;
                    case "over_1_hour":
                        videosQuery = videosQuery.Where(v => v.Duration >= TimeSpan.FromHours(1));
                        break;
                }
            }

            if (!filters.Likes.IsNullOrEmpty())
            {
                switch (filters.Likes)
                {
                    case "under_1_thousand":
                        videosQuery = videosQuery.Where(v => v.VideoStatuses.Count(vs => vs.IsLiked == true) <= 1000);
                        break;
                    case "1_10_thousand":
                        videosQuery = videosQuery.Where(v => v.VideoStatuses.Count(vs => vs.IsLiked == true) >= 1000 && v.VideoStatuses.Count(vs => vs.IsLiked == true) <= 10000);
                        break;
                    case "10_100_thousand":
                        videosQuery = videosQuery.Where(v => v.VideoStatuses.Count(vs => vs.IsLiked == true) >= 10000 && v.VideoStatuses.Count(vs => vs.IsLiked == true) <= 100000);
                        break;
                    case "over_100_thousand":
                        videosQuery = videosQuery.Where(v => v.VideoStatuses.Count(vs => vs.IsLiked == true) >= 100000);
                        break;
                }
            }

            if (!filters.Views.IsNullOrEmpty())
            {
                switch (filters.Views)
                {
                    case "under_1_thousand":
                        videosQuery = videosQuery.Where(v => v.Histories.Count() <= 1000);
                        break;
                    case "1_10_thousand":
                        videosQuery = videosQuery.Where(v => v.Histories.Count() >= 1000 && v.Histories.Count() <= 10000);
                        break;
                    case "10_100_thousand":
                        videosQuery = videosQuery.Where(v => v.Histories.Count() >= 10000 && v.Histories.Count() <= 100000);
                        break;
                    case "100_500_thousand":
                        videosQuery = videosQuery.Where(v => v.Histories.Count() >= 100000 && v.Histories.Count() <= 500000);
                        break;
                    case "over_500_thousand":
                        videosQuery = videosQuery.Where(v => v.Histories.Count() >= 500000);
                        break;
                }
            }

            if (!filters.Category.IsNullOrEmpty())
            {
                videosQuery = videosQuery.Where(v => v.Categories.Any(c => c.Name == filters.Category));
            }

            if (!sortOption.IsNullOrEmpty())
            {
                switch (sortOption)
                {
                    case "Most liked":
                        videosQuery = videosQuery.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == true));
                        break;
                    case "Most hated":
                        videosQuery = videosQuery.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == false));
                        break;
                    case "Longest":
                        var sortRes1 = (await videosQuery.ToListAsync()).OrderByDescending(v => v.Duration.TotalSeconds).Select(v => new HomeContentModel
                        {
                            Id = v.Id,
                            Title = v.Title,
                            PublicationDate = v.PublicationDate.ToString("o"),
                            PreviewUrl = v.PreviewUrl,
                            Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                            NumberOfViews = v.Histories.Count(),
                            ChannelLogo = v.Channel.LogoUrl,
                            ChannelId = v.Channel.Id,
                            ChannelTitle = v.Channel.Title,
                            PlaylistId = 0,
                            LastUpdated = DateTime.Now.ToString("o"),
                            VideoCount = 0,
                            IsPlaylist = false
                        });
                        return Ok(sortRes1);

                    case "Shortest":
                        var sortRes2 = (await videosQuery.ToListAsync()).OrderBy(v => v.Duration.TotalSeconds).Select(v => new HomeContentModel
                        {
                            Id = v.Id,
                            Title = v.Title,
                            PublicationDate = v.PublicationDate.ToString("o"),
                            PreviewUrl = v.PreviewUrl,
                            Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                            NumberOfViews = v.Histories.Count(),
                            ChannelLogo = v.Channel.LogoUrl,
                            ChannelId = v.Channel.Id,
                            ChannelTitle = v.Channel.Title,
                            PlaylistId = 0,
                            LastUpdated = DateTime.Now.ToString("o"),
                            VideoCount = 0,
                            IsPlaylist = false
                        });
                        return Ok(sortRes2);
                    case "Popular":
                        videosQuery = videosQuery.OrderByDescending(v => v.Histories.Count);
                        break;
                    case "Newest":
                        videosQuery = videosQuery.OrderByDescending(v => v.PublicationDate);
                        break;
                    case "Oldest":
                        videosQuery = videosQuery.OrderBy(v => v.PublicationDate);
                        break;
                }
            }

            var videoResult = await videosQuery.Select(v => new HomeContentModel
            {
                Id = v.Id,
                Title = v.Title,
                PublicationDate = v.PublicationDate.ToString("o"),
                PreviewUrl = v.PreviewUrl,
                Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                NumberOfViews = v.Histories.Count(),
                ChannelLogo = v.Channel.LogoUrl,
                ChannelId = v.Channel.Id,
                ChannelTitle = v.Channel.Title,
                PlaylistId = 0,
                LastUpdated = DateTime.Now.ToString("o"),
                VideoCount = 0,
                IsPlaylist = false
            }).ToListAsync();

            return Ok(videoResult);
        }

        [HttpGet("categoryvideos/{categoryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryVideos(int categoryId)
        {
            var videos = await _videoService.GetAllQuery()
                .Include(v => v.Histories)
                .Include(v => v.Channel)
                .Include(v => v.Categories)
                .Where(v => v.IsPublic && v.Categories.Any(c => c.Id == categoryId))
                .Select(v => new
                {
                    Id = v.Id,
                    Title = v.Title,
                    PublicationDate = v.PublicationDate.ToString("o"),
                    PreviewUrl = v.PreviewUrl,
                    Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                    NumberOfViews = v.Histories.Count(),
                    ChannelLogo = v.Channel.LogoUrl,
                    ChannelId = v.Channel.Id,
                    ChannelTitle = v.Channel.Title,
                    PlaylistId = 0,
                    LastUpdated = DateTime.Now.ToString("o"),
                    VideoCount = 0,
                    IsPlaylist = false,
                    CategoryName = v.Categories.Where(c => c.Id == categoryId)
                }).ToListAsync();

            var resultContent = videos.OrderByDescending(c => c.PublicationDate);

            return Ok(resultContent);
        }

        [HttpGet("homeplaylists")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHomePlaylists()
        {
            var playlists = await _playlistService.GetAllQuery()
                .Include(p => p.Videos)
                .Include(p => p.User)
                .ThenInclude(u => u.Channel)
                .Where(p => p.IsPublic && p.Videos.Any(v => v.IsPublic))
                .Select(p => new
                {
                    Playlist = p,
                    LatestVideo = p.Videos.Where(v => v.IsPublic).OrderByDescending(v => v.PublicationDate).FirstOrDefault()
                })
                .Select(p => new HomeContentModel
                {
                    Id = p.LatestVideo != null ? p.LatestVideo.Id : 0,
                    Title = p.Playlist.Title,
                    PublicationDate = p.Playlist.CreatedAt.ToString("o"),
                    PreviewUrl = p.LatestVideo != null ? p.LatestVideo.PreviewUrl : "https://mystoragets.blob.core.windows.net/vibeoserver/previews/default.png",
                    Duration = "0",
                    NumberOfViews = 0,
                    ChannelLogo = p.Playlist.User.Channel.LogoUrl,
                    ChannelId = p.Playlist.User.Channel.Id,
                    ChannelTitle = p.Playlist.User.Channel.Title,
                    PlaylistId = p.Playlist.Id,
                    LastUpdated = p.Playlist.LastUpdated.ToString("o"),
                    VideoCount = p.Playlist.Videos.Count(v => v.IsPublic),
                    IsPlaylist = true
                }).ToListAsync();

            var resultContent = playlists.OrderByDescending(c => c.PublicationDate);

            return Ok(resultContent);
        }

        [HttpGet("homevideos")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHomeVideos()
        {
            var videos= await _videoService.GetAllQuery()
                .Include(v => v.Histories)
                .Include(v => v.Channel)
                .Where(v => v.IsPublic)
                .Select(v => new HomeContentModel
                {
                    Id = v.Id,
                    Title = v.Title,
                    PublicationDate = v.PublicationDate.ToString("o"),
                    PreviewUrl = v.PreviewUrl,
                    Duration = new DateTime(v.Duration.Ticks).ToString("HH:mm:ss"),
                    NumberOfViews = v.Histories.Count(),
                    ChannelLogo = v.Channel.LogoUrl,
                    ChannelId = v.Channel.Id,
                    ChannelTitle = v.Channel.Title,
                    PlaylistId = 0,
                    LastUpdated = DateTime.Now.ToString("o"),
                    VideoCount = 0,
                    IsPlaylist = false
                }).ToListAsync();

            var playlists = await _playlistService.GetAllQuery()
                .Include(p => p.Videos)
                .Include(p => p.User)
                .ThenInclude(u=>u.Channel)
                .Where(p => p.IsPublic && p.Videos.Any(v => v.IsPublic))
                .Select(p => new
                {
                    Playlist = p,
                    LatestVideo = p.Videos.Where(v => v.IsPublic).OrderByDescending(v => v.PublicationDate).FirstOrDefault()
                })
                .Select(p => new HomeContentModel
                {
                    Id = p.LatestVideo != null ? p.LatestVideo.Id : 0,
                    Title = p.Playlist.Title,
                    PublicationDate = p.Playlist.CreatedAt.ToString("o"),
                    PreviewUrl = p.LatestVideo != null ? p.LatestVideo.PreviewUrl : "https://mystoragets.blob.core.windows.net/vibeoserver/previews/default.png",
                    Duration = "0",
                    NumberOfViews = 0,
                    ChannelLogo = p.Playlist.User.Channel.LogoUrl,
                    ChannelId = p.Playlist.User.Channel.Id,
                    ChannelTitle = p.Playlist.User.Channel.Title,
                    PlaylistId = p.Playlist.Id,
                    LastUpdated = p.Playlist.LastUpdated.ToString("o"),
                    VideoCount = p.Playlist.Videos.Count(v => v.IsPublic),
                    IsPlaylist = true
                }).ToListAsync();

            var mixedContent = videos.Union(playlists).OrderByDescending(c => c.PublicationDate);

            return Ok(mixedContent);
        }

    }
}

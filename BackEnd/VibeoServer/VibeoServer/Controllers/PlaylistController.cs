using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Security.Claims;
using VibeoServer.DTOs;
using VibeoServer.Helpers;
using VibeoServer.Models;
using VibeoServer.Services;
using VibeoServer.Services.Interfaces;
using static VibeoServer.Controllers.ContentController;

namespace VibeoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PlaylistController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IPlaylistService _playlistService;
        private readonly UserManager<User> _userManager;

        public PlaylistController(IVideoService videoService, IPlaylistService playlistService, UserManager<User> userManager)
        {
            _videoService = videoService;
            _playlistService = playlistService;
            _userManager = userManager;
        }

        [HttpDelete("removevideofromplaylist/{playlistId}/{videoId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> RemoveVideo(int playlistId, int videoId)
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

            var playlist = await _playlistService.GetByIdQuery(playlistId).Include(p => p.Videos).FirstOrDefaultAsync();
            if (playlist == null)
            {
                return BadRequest("No such playlist");
            }
            var video = await _videoService.GetByIdQuery(videoId).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("No such video");
            }
            try
            {
                if (playlist.Videos.Any(v => v.Id == videoId))
                {
                    playlist.Videos.Remove(video);
                    playlist.LastUpdated = DateTime.Now;
                    await _playlistService.UpdateAsync(playlistId, playlist);
                    return Ok(true);
                }
                else
                {
                    return BadRequest("No such video in the playlist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while adding the video");
            }


        }

        [HttpPost("addremovevideo/{playlistId}/{videoId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> AddRemoveVideo(int playlistId, int videoId)
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

            var playlist = await _playlistService.GetByIdQuery(playlistId).Include(p => p.Videos).FirstOrDefaultAsync();
            if (playlist == null)
            {
                return BadRequest("No such playlist");
            }
            var video = await _videoService.GetByIdQuery(videoId).FirstOrDefaultAsync();
            if(video == null)
            {
                return BadRequest("No such video");
            }

            try
            {
                var newPlaylist = new Playlist();
                if (playlist.Videos.Any(v => v.Id == videoId))
                {
                    playlist.Videos.Remove(video);
                    playlist.LastUpdated = DateTime.Now;
                    newPlaylist = await _playlistService.UpdateAsync(playlistId, playlist);
                    return Ok(new
                    {
                        newPlaylist.Id,
                        newPlaylist.Title,
                        HasVideo = newPlaylist.Videos.Any(v => v.Id == videoId)
                    });
                }
                
                playlist.Videos.Add(video);
                playlist.LastUpdated = DateTime.Now;
                newPlaylist = await _playlistService.UpdateAsync(playlistId, playlist);
                return Ok(new
                {
                    newPlaylist.Id,
                    newPlaylist.Title,
                    HasVideo = newPlaylist.Videos.Any(v => v.Id == videoId)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while adding the video");
            }

            
        }

        [HttpGet("getvideoplaylists/{videoId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> GetVideoPlaylists(int videoId)
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

            var playlists = await _playlistService.GetAllQuery().Include(p => p.Videos).Where(p => p.UserId == user.Id).ToListAsync();

            return Ok(playlists.Select(p => new
            {
                p.Id,
                p.Title,
                HasVideo = p.Videos.Any(v => v.Id == videoId)
            }));
        }

        [HttpGet("getplaylistvideos/{playlistId}/{sortmethod:alpha}/{isAscending?}/{searchValue?}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> GetPlaylistVideos(int playlistId, SortMethod sortMethod, bool isAscending, string searchValue, [FromQuery] VideoFiltersModel filters)
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

            var videosQuery = _videoService.GetAllQuery()
                .Include(v => v.VideoStatuses)
                .Include(v => v.Histories)
                .Include(v => v.Comments)
                .Include(v => v.Playlists)
                .Include(v => v.Categories)
                .Include(v => v.Tags)
                .Where(v => v.Playlists.Any(p => p.Id == playlistId));

            //Search
            if (!string.IsNullOrEmpty(searchValue))
            {
                videosQuery = videosQuery.Where(v => v.Title.ToLower().Contains(searchValue.ToLower()) ||
                    v.Format.ToLower().Contains(searchValue.ToLower()) ||
                    v.Categories.Any(c => c.Name.ToLower().Contains(searchValue.ToLower())) ||
                    v.Tags.Any(t => t.Name.ToLower().Contains(searchValue.ToLower())));
            }



            //Filter

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

            if (filters.VideoFormat != null && filters.VideoFormat.Any())
            {
                videosQuery = videosQuery.Where(v => filters.VideoFormat.Contains(v.Format));
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

            if (!filters.Size.IsNullOrEmpty())
            {
                switch (filters.Size)
                {
                    case "0_50_mb":
                        videosQuery = videosQuery.Where(v => v.Size / 1024 / 1024 <= 50);
                        break;
                    case "50_200_mb":
                        videosQuery = videosQuery.Where(v => v.Size / 1024 / 1024 >= 50 && v.Size / 1024 / 1024 <= 200);
                        break;
                    case "200_500_mb":
                        videosQuery = videosQuery.Where(v => v.Size / 1024 / 1024 >= 200 && v.Size / 1024 / 1024 <= 500);
                        break;
                    case "over_500_mb":
                        videosQuery = videosQuery.Where(v => v.Size / 1024 / 1024 >= 500);
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

            if (!filters.Visibility.IsNullOrEmpty())
            {
                switch (filters.Visibility)
                {
                    case "private":
                        videosQuery = videosQuery.Where(v => !v.IsPublic);
                        break;
                    case "public":
                        videosQuery = videosQuery.Where(v => v.IsPublic);
                        break;
                }
            }

            if (!filters.Category.IsNullOrEmpty())
            {
                videosQuery = videosQuery.Where(v => v.Categories.Any(c => c.Name == filters.Category));
            }

            //Sorting

            switch (sortMethod)
            {
                case SortMethod.Title:
                    videosQuery = isAscending ? videosQuery.OrderBy(v => v.Title.ToLower()) : videosQuery.OrderByDescending(v => v.Title.ToLower());
                    break;
                case SortMethod.Publication:
                    videosQuery = isAscending ? videosQuery.OrderBy(v => v.PublicationDate) : videosQuery.OrderByDescending(v => v.PublicationDate);
                    break;
                case SortMethod.Format:
                    videosQuery = isAscending ? videosQuery.OrderBy(v => v.Format) : videosQuery.OrderByDescending(v => v.Format);
                    break;
                case SortMethod.Views:
                    videosQuery = isAscending ? videosQuery.OrderBy(v => v.Histories.Count()) : videosQuery.OrderByDescending(v => v.Histories.Count());
                    break;
                case SortMethod.Rating:
                    videosQuery = isAscending ? videosQuery.OrderBy(v => v.VideoStatuses.Count(vs => vs.IsLiked == true) - v.VideoStatuses.Count(vs => vs.IsLiked == false)) : videosQuery.OrderByDescending(v => v.VideoStatuses.Count(vs => vs.IsLiked == true) - v.VideoStatuses.Count(vs => vs.IsLiked == false));
                    break;
                case SortMethod.Duration:
                    videosQuery = isAscending ? videosQuery.OrderBy(v => v.Duration) : videosQuery.OrderByDescending(v => v.Duration);
                    break;
                case SortMethod.Comments:
                    videosQuery = isAscending ? videosQuery.OrderBy(v => v.Comments.Count()) : videosQuery.OrderByDescending(v => v.Comments.Count());
                    break;
                case SortMethod.Size:
                    videosQuery = isAscending ? videosQuery.OrderBy(v => v.Size) : videosQuery.OrderByDescending(v => v.Size);
                    break;
            }

            var videos = await videosQuery
               .Select(v => new
               {
                   v.Id,
                   v.Title,
                   PublicationDate = v.PublicationDate.ToString("MM/dd/yyyy HH:mm:ss"),
                   v.Format,
                   v.Size,
                   v.PreviewUrl,
                   v.Description,
                   Duration = v.Duration.ToString(@"hh\:mm\:ss"),
                   v.IsPublic,
                   NumberOfLikes = v.VideoStatuses.Count(vs => vs.IsLiked == true),
                   NumberOfDisLikes = v.VideoStatuses.Count(vs => vs.IsLiked == false),
                   NumberOfViews = v.Histories.Count(),
                   NumberOfComments = v.Comments.Count(),
                   NumberOfPlaylists = v.Playlists.Count(),
                   Categories = v.Categories.Select(c => c.Name),
                   Tags = v.Tags.Select(t => t.Name)
               })
               .ToListAsync();

            return Ok(videos);
        }

        [HttpPatch("updateplaylist/{playlistId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> UpdatePlaylist(int playlistId, [FromBody] PlaylistModel model)
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


            var playlist = await _playlistService.GetByIdQuery(playlistId).FirstOrDefaultAsync();
            if (playlist == null)
            {
                return BadRequest("There is no such video!");
            }

            try
            {
                playlist.Title = model.Title;
                playlist.Description = model.Description;
                playlist.LastUpdated = DateTime.Now;

                playlist = await _playlistService.UpdateAsync(playlistId, playlist);

                return Ok(new
                {
                    playlist.Title,
                    playlist.Description
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }

        [HttpGet("getplaylist/{playlistId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> GetPlaylist(int playlistId)
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


            var playlist = await _playlistService.GetByIdQuery(playlistId).FirstOrDefaultAsync();
            if (playlist == null)
            {
                return BadRequest("There is no such playlist!");
            }
            else
            {
                return Ok(new
                {
                    playlist.Title,
                    playlist.Description,
                    playlist.IsPublic
                });
            }
        }

        [HttpDelete("deleteplaylist/{playlistId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> DeletePlaylist(int playlistId)
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


            try
            {
                var playlist = await _playlistService.GetByIdQuery(playlistId).FirstOrDefaultAsync();
                if (playlist == null)
                {
                    return NotFound("Playlist not found");
                }
                var res = await _playlistService.DeleteAsync(playlistId);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while deleting video.");
            }

        }

        [HttpPatch("changevisibility/{playlistId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> ChangeVideoVisibility(int playlistId)
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


            var playlist = await _playlistService.GetByIdQuery(playlistId).FirstOrDefaultAsync();
            if (playlist == null)
            {
                return BadRequest("There is no such video!");
            }

            try
            {
                playlist.IsPublic = !playlist.IsPublic;

                playlist = await _playlistService.UpdateAsync(playlistId, playlist);

                return Ok(playlist.IsPublic);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }

        [HttpPost("create")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> CreatePlaylist([FromBody] PlaylistModel model)
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


            var playlist = new Playlist
            {
                Title = model.Title,
                Description = model.Description,
                CreatedAt = DateTime.Now,
                LastUpdated = DateTime.Now,
                IsPublic = false,
                UserId = user.Id,
                User = user
            };

            try
            {
                var newPlaylist = await _playlistService.CreateAsync(playlist);


                if (newPlaylist == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error while creating the channel");
                }
                else
                {
                    return Ok(newPlaylist);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while creating the channel");
            }


        }

        [HttpGet("mychannelplaylists/{sortmethod:alpha}/{isAscending?}/{searchValuePlaylist?}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> GetPlaylistsByChannel(SortMethod sortMethod, bool isAscending, string searchValuePlaylist, [FromQuery] PlaylistFiltersModel filters)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.Users.Include(u => u.Channel).FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            if (user.Channel == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The user doesn't have a channel");
            }

            var playlistQuery = _playlistService.GetAllQuery()
                .Include(p => p.Videos)
                .Where(v => v.UserId == user.Id);


            //Search
            if (!searchValuePlaylist.IsNullOrEmpty())
            {
                playlistQuery = playlistQuery.Where(p => p.Title.ToLower().Contains(searchValuePlaylist.ToLower()));
            }


            //Filter

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

            if (!filters.Visibility.IsNullOrEmpty())
            {
                switch (filters.Visibility)
                {
                    case "private":
                        playlistQuery = playlistQuery.Where(v => !v.IsPublic);
                        break;
                    case "public":
                        playlistQuery = playlistQuery.Where(v => v.IsPublic);
                        break;
                }
            }

            //Sorting

            switch (sortMethod)
            {
                case SortMethod.Title:
                    playlistQuery = isAscending ? playlistQuery.OrderBy(v => v.Title.ToLower()) : playlistQuery.OrderByDescending(v => v.Title.ToLower());
                    break;
                case SortMethod.Publication:
                    playlistQuery = isAscending ? playlistQuery.OrderBy(v => v.CreatedAt) : playlistQuery.OrderByDescending(v => v.CreatedAt);
                    break;
                case SortMethod.LastUpdated:
                    playlistQuery = isAscending ? playlistQuery.OrderBy(v => v.LastUpdated) : playlistQuery.OrderByDescending(v => v.LastUpdated);
                    break;
                case SortMethod.VideoCount:
                    playlistQuery = isAscending ? playlistQuery.OrderBy(v => v.Videos.Count()) : playlistQuery.OrderByDescending(v => v.Videos.Count());
                    break;
            }


            var playlists = await playlistQuery
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    PublicationDate = p.CreatedAt.ToString("MM/dd/yyyy HH:mm:ss"),
                    LastUpdated = p.LastUpdated.ToString("MM/dd/yyyy HH:mm:ss"),
                    p.IsPublic,
                    VideoCount = p.Videos.Count(),
                    PreviewUrl = p.Videos.Where(v => v.IsPublic).Count() > 0 ? p.Videos.Where(v => v.IsPublic).OrderByDescending(v => v.PublicationDate).FirstOrDefault().PreviewUrl : "https://mystoragets.blob.core.windows.net/vibeoserver/previews/default.png"
                }).ToListAsync();

            return Ok(playlists);
        }
    }
}

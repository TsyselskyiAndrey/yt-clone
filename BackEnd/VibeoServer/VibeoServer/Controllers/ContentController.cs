using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using MimeDetective;
using VibeoServer.Models;
using VibeoServer.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Xabe.FFmpeg;
using System;
using System.Globalization;
using VibeoServer.DTOs;
using Microsoft.IdentityModel.Tokens;
using VibeoServer.Helpers;
using System.Reflection.Metadata;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace VibeoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContentController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;
        private readonly INotificationService _notificationService;
        private readonly IChannelService _channelService;
        private readonly INotificationTypeService _notificationTypeService;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly UserManager<User> _userManager;
        private readonly string containerName = "vibeoserver";
        public ContentController(IVideoService videoService, ICategoryService categoryService, INotificationService notificationService, INotificationTypeService notificationTypeService, IChannelService channelService, BlobServiceClient blobServiceClient, UserManager<User> userManager, ITagService tagService)
        {
            _videoService = videoService;
            _categoryService = categoryService;
            _notificationService = notificationService;
            _notificationTypeService = notificationTypeService;
            _channelService = channelService;
            _blobServiceClient = blobServiceClient;
            _userManager = userManager;
            _tagService = tagService;
        }

        public enum SortMethod
        {
            Title,
            Publication,
            Format,
            Views,
            Rating,
            Duration,
            Comments,
            Size,
            VideoCount,
            LastUpdated
        }

        [HttpGet("videocommentsreport/{videoId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> GetVideoCommentsReport(int videoId)
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


            var video = await _videoService.GetByIdQuery(videoId)
                .Include(v => v.Channel)
                .Include(v => v.Comments)
                    .ThenInclude(c => c.Channel)
                .Include(v => v.Comments)
                    .ThenInclude(c => c.CommentStatuses)
                .Include(v => v.Comments)
                    .ThenInclude(c => c.ChildComments)
                        .ThenInclude(cc => cc.Channel)
                .FirstOrDefaultAsync();

            if (video == null)
            {
                return BadRequest();
            }

            var videoInfo = new
            {
                video.Title,
                Owner = video.Channel.Title,
                Comments = video.Comments.Select((c, index) => new
                {
                    Index = index + 1,
                    c.CreatedAt,
                    c.Content,
                    ChannelTitle = c.Channel.Title,
                    Likes = c.CommentStatuses.Count(cs => cs.isLiked == true),
                    Dislikes = c.CommentStatuses.Count(cs => cs.isLiked == false),
                    ChildComments = c.ChildComments.Select((cc, indexCC) => new
                    {
                        Index = indexCC + 1,
                        cc.CreatedAt,
                        cc.Content,
                        ChannelTitle = cc.Channel.Title,
                        Likes = cc.CommentStatuses.Count(cs => cs.isLiked == true),
                        Dislikes = cc.CommentStatuses.Count(cs => cs.isLiked == false)
                    })
                }).ToList()
            };

            var pdf = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);

                    page.Content().Column(column =>
                    {
                        var currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                        column.Item().Text($"Video: {videoInfo.Title}").FontSize(16).Bold();
                        column.Item().Text($"Owner: {videoInfo.Owner}").FontSize(14).Italic();
                        column.Item().Text($"Report Date: {currentDate}").FontSize(12).Italic().AlignRight();
                        column.Spacing(10);

                        column.Item().Text("Comments:").FontSize(14).Bold();

                        foreach (var comment in videoInfo.Comments)
                        {
                            column.Item().Text($"{comment.Index}. {comment.ChannelTitle} ({comment.CreatedAt:yyyy-MM-dd HH:mm}) - Likes: {comment.Likes}, Dislikes: {comment.Dislikes}").FontSize(12);
                            column.Item().Text($"    Content: {comment.Content}").FontSize(12).Italic();

                            foreach (var childComment in comment.ChildComments)
                            {
                                column.Item().Text($"    {comment.Index}.{childComment.Index} {childComment.ChannelTitle} ({childComment.CreatedAt:yyyy-MM-dd HH:mm}) - Likes: {childComment.Likes}, Dislikes: {childComment.Dislikes}").FontSize(11);
                                column.Item().Text($"        Content: {childComment.Content}").FontSize(11).Italic();
                            }
                        }
                    });
                });
            });

            var pdfBytes = pdf.GeneratePdf();
            Response.Headers.Add("Content-Disposition", "inline; filename=VideoCommentsReport.pdf");
            Response.ContentType = "application/pdf";
            return File(pdfBytes, "application/pdf");
        }


        [HttpGet("channelvideosreport")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> GetChannelVideosReport()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.Users
                .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Videos)
                         .ThenInclude(v => v.Histories)
                 .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Videos)
                         .ThenInclude(v => v.VideoStatuses)
                 .Include(u => u.Channel)
                    .ThenInclude(ch => ch.Videos)
                         .ThenInclude(v => v.Comments)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            if (user.Channel == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The user doesn't have a channel");
            }

            var channel = new
            {
                user.Channel.Title,
                Owner = user.FirstName + " " + user.LastName+" (" + user.Email+")",
                Videos = user.Channel.Videos.Select((v, index) => new
                {
                    Index = index + 1,
                    v.Title,
                    v.PublicationDate,
                    Views = v.Histories.Count,
                    Likes = v.VideoStatuses.Count(vs => vs.IsLiked == true),
                    Dislikes = v.VideoStatuses.Count(vs => vs.IsLiked == false),
                    CommentCount = v.Comments.Count
                }).ToList()
            };


            var pdf = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    var currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);

                    page.Content().Column(column =>
                    {
                        column.Item().Text($"Channel: {channel.Title}").FontSize(16).Bold();
                        column.Item().Text($"Owner: {channel.Owner}").FontSize(14).Italic();
                        column.Item().Text($"Report Date: {currentDate}").FontSize(12).Italic().AlignRight();
                        column.Spacing(10);

                        column.Item().Text("Videos:").FontSize(14).Bold();

                        foreach (var video in channel.Videos)
                        {
                            column.Item().Text($"{video.Index}. {video.Title} ({video.PublicationDate:yyyy-MM-dd}) - Views: {video.Views}, Likes: {video.Likes}, Dislikes: {video.Dislikes}, Comments: {video.CommentCount}").FontSize(12);
                        }
                    });
                });
            });

            var pdfBytes = pdf.GeneratePdf();
            Response.Headers.Add("Content-Disposition", "inline; filename=ChannelVideosReport.pdf");
            Response.ContentType = "application/pdf";
            return File(pdfBytes, "application/pdf");
        }


        [AllowAnonymous]
        [HttpGet("watch/{id}")]
        public async Task<IActionResult> StreamVideo(int id)
        {
            var video = await _videoService.GetByIdQuery(id).FirstOrDefaultAsync();

            if (video == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "The video wasn't found!");
            }

            try
            {
                var blobClient = new BlobClient(new Uri(video.Url));
                var stream = await blobClient.OpenReadAsync();
                return File(stream, $"video/{video.Format.Replace(".", string.Empty)}", enableRangeProcessing: true);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while streaming video.");
            }
           
        }


        [HttpGet("periodicvideostats/{videoId}/{period}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> GetPeriodicStats(int videoId, string period)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.Users.Include(u => u.Channel)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("Invalid user.");
            }

            if (user.Channel == null)
            {
                return StatusCode(StatusCodes.Status403Forbidden, "The user doesn't have a channel");
            }

            var video = await _videoService.GetByIdQuery(videoId)
                .Include(v => v.Comments)
                    .ThenInclude(c => c.ChildComments)
                .Include(v => v.Comments)
                    .ThenInclude(c => c.CommentStatuses)
                .Include(c => c.Histories)
                    .ThenInclude(h => h.User)
                        .ThenInclude(u => u.Subscriptions)
                .FirstOrDefaultAsync();

            if (video == null)
            {
                return BadRequest();
            }

            switch (period)
            {
                case "all":
                    return Ok(new
                    {
                        NumberOfComments = video.Comments.Count(),

                        NumberOfViews = video.Histories.Count(),

                        UniqueViews = video.Histories
                            .GroupBy(h => new { h.VideoId, h.UserId })
                            .Count(),

                        TotalReplies = video.Comments.Sum(c => c.ChildComments.Count()),

                        ViewersWithoutSubscription = video.Histories
                            .Where(h => !h.User.Subscriptions.Any(s => s.ChannelId == video.ChannelId))
                            .GroupBy(h => h.User).Count(),

                        NewViewersForVideo = video.Histories
                            .Where(h => !h.User.Histories.Any(uh => uh.Video.ChannelId == video.ChannelId && uh.UserId == h.UserId))
                            .GroupBy(h => h.User)
                            .Count()
                    });
                case "year":
                    return Ok(new
                    {
                        NumberOfComments = video.Comments.Count(c => DateTime.Now.AddYears(-1) <= c.CreatedAt),

                        NumberOfViews = video.Histories.Count(h => DateTime.Now.AddYears(-1) <= h.WatchedAt),

                        UniqueViews = video.Histories.Where(h => DateTime.Now.AddYears(-1) <= h.WatchedAt)
                            .GroupBy(h => new { h.VideoId, h.UserId })
                            .Count(),

                        TotalReplies = video.Comments.Sum(c => c.ChildComments.Count(cc => DateTime.Now.AddYears(-1) <= cc.CreatedAt)),

                        ViewersWithoutSubscription = video.Histories.Where(h => DateTime.Now.AddYears(-1) <= h.WatchedAt)
                            .Where(h => !h.User.Subscriptions.Any(s => s.ChannelId == video.ChannelId))
                            .GroupBy(h => h.User).Count(),

                        NewViewersForVideo = video.Histories.Where(h => DateTime.Now.AddYears(-1) <= h.WatchedAt)
                            .Where(h => !h.User.Histories.Any(uh => uh.Video.ChannelId == video.ChannelId && uh.UserId == h.UserId))
                            .GroupBy(h => h.User)
                            .Count()
                    });
                case "month":
                    return Ok(new
                    {
                        NumberOfComments = video.Comments.Count(c => DateTime.Now.AddMonths(-1) <= c.CreatedAt),

                        NumberOfViews = video.Histories.Count(h => DateTime.Now.AddMonths(-1) <= h.WatchedAt),

                        UniqueViews = video.Histories.Where(h => DateTime.Now.AddMonths(-1) <= h.WatchedAt)
                            .GroupBy(h => new { h.VideoId, h.UserId })
                            .Count(),

                        TotalReplies = video.Comments.Sum(c => c.ChildComments.Count(cc => DateTime.Now.AddMonths(-1) <= cc.CreatedAt)),

                        ViewersWithoutSubscription = video.Histories.Where(h => DateTime.Now.AddMonths(-1) <= h.WatchedAt)
                            .Where(h => !h.User.Subscriptions.Any(s => s.ChannelId == video.ChannelId))
                            .GroupBy(h => h.User).Count(),

                        NewViewersForVideo = video.Histories.Where(h => DateTime.Now.AddMonths(-1) <= h.WatchedAt)
                            .Where(h => !h.User.Histories.Any(uh => uh.Video.ChannelId == video.ChannelId && uh.UserId == h.UserId))
                            .GroupBy(h => h.User)
                            .Count()
                    });
                case "day":
                    return Ok(new
                    {
                        NumberOfComments = video.Comments.Count(c => DateTime.Now.AddDays(-1) <= c.CreatedAt),

                        NumberOfViews = video.Histories.Count(h => DateTime.Now.AddDays(-1) <= h.WatchedAt),

                        UniqueViews = video.Histories.Where(h => DateTime.Now.AddDays(-1) <= h.WatchedAt)
                            .GroupBy(h => new { h.VideoId, h.UserId })
                            .Count(),

                        TotalReplies = video.Comments.Sum(c => c.ChildComments.Count(cc => DateTime.Now.AddDays(-1) <= cc.CreatedAt)),

                        ViewersWithoutSubscription = video.Histories.Where(h => DateTime.Now.AddDays(-1) <= h.WatchedAt)
                            .Where(h => !h.User.Subscriptions.Any(s => s.ChannelId == video.ChannelId))
                            .GroupBy(h => h.User).Count(),

                        NewViewersForVideo = video.Histories.Where(h => DateTime.Now.AddDays(-1) <= h.WatchedAt)
                            .Where(h => !h.User.Histories.Any(uh => uh.Video.ChannelId == video.ChannelId && uh.UserId == h.UserId))
                            .GroupBy(h => h.User)
                            .Count()
                    });
                default:
                    return BadRequest();
            }
        }

        [HttpGet("generalvideostats/{videoId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> GetGeneralStats(int videoId)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized("User is not logged in.");
            }

            var user = await _userManager.Users.Include(u => u.Channel)
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

            var video = await _videoService.GetByIdQuery(videoId)
                .Include(v => v.Comments)
                    .ThenInclude(c => c.ChildComments)
                .Include(v => v.Comments)
                    .ThenInclude(c => c.CommentStatuses)
                .Include(v => v.Histories)
                .FirstOrDefaultAsync();

            if (video == null)
            {
                return BadRequest();
            }

            return Ok(new
            {
                NumberOfReplies = video.Comments.Sum(c => c.ChildComments.Count()),

                UniqueViews = video.Histories
                            .GroupBy(h => new { h.VideoId, h.UserId })
                            .Count(),

                TotalLikes = video.Comments.Sum(c => c.CommentStatuses.Count(cs => cs.isLiked == true)),
                TotalDislikes = video.Comments.Sum(c => c.CommentStatuses.Count(cs => cs.isLiked == false))

            });

        }


        [HttpPatch("uploadpreview/{videoId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> AddPreview(int videoId, IFormFile file)
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


            var video = await _videoService.GetByIdQuery(videoId).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("There is no such video!");
            }

            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var previewBlobName = $"previews/{videoId}.jpeg";
                var blobClient = containerClient.GetBlobClient(previewBlobName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }
                video.PreviewUrl = blobClient.Uri.ToString();
                video = await _videoService.UpdateAsync(videoId, video);

                return Ok(new
                {
                    video.PreviewUrl
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }


        [HttpPatch("addtag/{videoId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> AddTag(int videoId, [FromBody] TagModel model)
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


            var video = await _videoService.GetByIdQuery(videoId).Include(v => v.Tags).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("There is no such video!");
            }
            if (video.Tags.Any(c => c.Name == "#" + model.Name))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new List<CustomResponse> {
                    new() {
                        Type="ValidationError",
                        Field = "Tag",
                        Details = "This tag is already added."
                    }
                });
            }

            try
            {
                video.Tags.Add(new Tag { Name = "#" + model.Name });
                video = await _videoService.UpdateAsync(videoId, video);

                return Ok(new
                {
                    Tags = video.Tags.Select(t => new {t.Id, t.Name})
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }

        [HttpDelete("deletetag/{videoId}/{tagId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> DeleteTag(int videoId, int tagId)
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


            var video = await _videoService.GetByIdQuery(videoId).Include(v => v.Tags).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("There is no such video!");
            }
            var selectedTag = video.Tags.FirstOrDefault(c => c.Id == tagId);
            if (selectedTag == null)
            {
                return BadRequest("The video doesn't have the tag!");
            }
            try
            {
                video.Tags.Remove(selectedTag);
                video = await _videoService.UpdateAsync(videoId, video);

                return Ok(new
                {
                    Tags = video.Tags.Select(t => new { t.Id, t.Name })
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }


        [HttpPatch("addcategory/{videoId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> AddCategory(int videoId, [FromBody] CategoryModel model)
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

            var categories = await _categoryService.GetAllQuery().ToListAsync();
            if (!categories.Any(c => c.Name == model.Name))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new List<CustomResponse> {
                    new() {
                        Type="ValidationError",
                        Field = "Category",
                        Details = "The category doesn't exist."
                    }
                });
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


            var video = await _videoService.GetByIdQuery(videoId).Include(v => v.Categories).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("There is no such video!");
            }
            if(video.Categories.Any(c=>c.Name == model.Name))
            {
                return StatusCode(StatusCodes.Status400BadRequest, new List<CustomResponse> {
                    new() {
                        Type="ValidationError",
                        Field = "Category",
                        Details = "The category is already added."
                    }
                });
            }
            var selectedCategory = categories.FirstOrDefault(c => c.Name == model.Name);
            if(selectedCategory == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new List<CustomResponse> {
                    new() {
                        Type="ValidationError",
                        Field = "Category",
                        Details = "The category doesn't exist."
                    }
                });
            }
            try
            {
                video.Categories.Add(selectedCategory);
                video = await _videoService.UpdateAsync(videoId, video);

                return Ok(new
                {
                    Categories = video.Categories.Select(c => new {c.Id, c.Name})
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }

        [HttpDelete("deletecategory/{videoId}/{categoryId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> DeleteCategory(int videoId, int categoryId)
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


            var video = await _videoService.GetByIdQuery(videoId).Include(v => v.Categories).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("There is no such video!");
            }
            var selectedCategory = video.Categories.FirstOrDefault(c => c.Id == categoryId);
            if (selectedCategory == null) {
                return BadRequest("The video is not in the category!");
            }
            try
            {
                video.Categories.Remove(selectedCategory);
                video = await _videoService.UpdateAsync(videoId, video);

                return Ok(new
                {
                    Categories = video.Categories.Select(c => new { c.Id, c.Name })
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }

        [HttpGet("getvideo/{videoId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> GetVideo(int videoId)
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
            

            var video = await _videoService.GetByIdQuery(videoId).Include(v => v.Categories).Include(v => v.Tags).Include(v => v.Playlists).FirstOrDefaultAsync();
            if (video == null)
            {
                return BadRequest("There is no such video!");
            }
            else
            {
                return Ok(new
                {
                    video.Title,
                    video.Description,
                    video.Format,
                    video.Size,
                    video.IsPublic,
                    video.PreviewUrl,
                    Categories = video.Categories.Select(c => new { c.Id, c.Name }),
                    Tags = video.Tags.Select(c => new { c.Id, c.Name }),
                    Playlists = video.Playlists.Select(p => p.Id)
                });
            }

        }

        [HttpPatch("updatevideo/{videoId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> UpdateVideo(int videoId, [FromBody] VideoModel model)
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

            try
            {
                video.Title = model.Title;
                video.Description = model.Description;

                video = await _videoService.UpdateAsync(videoId, video);

                return Ok(new
                {
                    video.Title,
                    video.Description
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }

        }

        [HttpGet("getcategories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllQuery().Include(c => c.Videos).ThenInclude(v => v.Histories).ToListAsync();
                return Ok(categories.Select(c => new { 
                    c.Id,
                    c.Name,
                    NumberOfVideos = c.Videos.Where(v => v.IsPublic).Count(),
                    NumberOfViews = c.Videos.Sum(v => v.Histories.Count)
                }));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while getting categories.");
            }
            

        }

        [HttpDelete("deletevideo/{videoId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> DeleteVideo(int videoId)
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
                var video = await _videoService.GetByIdQuery(videoId).FirstOrDefaultAsync();
                if (video == null)
                {
                    return NotFound("Video not found");
                }

                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient($"videos/{videoId}{video.Format}");

                if (await blobClient.ExistsAsync())
                {
                    await blobClient.DeleteAsync();
                }

                var previewBlobClient = containerClient.GetBlobClient($"previews/{videoId}.jpeg");

                if (await previewBlobClient.ExistsAsync())
                {
                    await previewBlobClient.DeleteAsync();
                }

                var res = await _videoService.DeleteAsync(videoId);

                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while deleting video.");
            }

        }

        [HttpPatch("changevisibility/{videoId}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> ChangeVideoVisibility(int videoId)
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


            var video = await _videoService.GetByIdQuery(videoId).FirstOrDefaultAsync();
            if (video == null) {
                return BadRequest("There is no such video!");
            }

            try
            {
                video.IsPublic = !video.IsPublic;

                video = await _videoService.UpdateAsync(videoId, video);

                if (video.IsPublic)
                {
                    var notifications = await _notificationService.GetAllQuery().Where(n => n.VideoId == video.Id).ToListAsync();
                    if (notifications.Count > 0)
                    {
                        foreach (var notification in notifications)
                        {
                            notification.CreatedAt = DateTime.Now;
                            notification.IsRead = false;
                            await _notificationService.UpdateAsync(notification.Id, notification);
                        }
                    }
                    else
                    {
                        var channel = await _channelService.GetByIdQuery(video.ChannelId).Include(ch => ch.Subscriptions).FirstOrDefaultAsync();
                        if (channel != null)
                        {
                            foreach (var sub in channel.Subscriptions)
                            {
                                if (sub.ReceiveNotifications)
                                {
                                    NotificationType? notificationType = await _notificationTypeService.GetByNameQuery("NewVideo").FirstOrDefaultAsync();
                                    if (notificationType == null)
                                    {
                                        return BadRequest();
                                    }
                                    Notification notification = new Notification
                                    {
                                        UserId = sub.UserId,
                                        VideoId = video.Id,
                                        CreatedAt = DateTime.Now,
                                        IsRead = false,
                                        NotificationType = notificationType,
                                        Title = $"New Video from {video.Channel.Title}",
                                        Message = $@"Check out our latest video ""{video.Title}""!"

                                    };
                                    await _notificationService.CreateAsync(notification);
                                }
                            }
                        }
                    }
                }
                else
                {
                    var notifications = await _notificationService.GetAllQuery().Where(n => n.VideoId == video.Id).ToListAsync();
                    if (notifications.Count > 0)
                    {
                        foreach (var notification in notifications)
                        {
                            await _notificationService.DeleteAsync(notification.Id);
                        }
                    }
                }

                return Ok(video.IsPublic);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while updating video.");
            }
            
        }


        [HttpGet("mychannelvideos/{sortmethod:alpha}/{isAscending?}/{searchValue?}")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> GetVideosByChannel(SortMethod sortMethod, bool isAscending, string searchValue, [FromQuery] VideoFiltersModel filters)
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
                .Where(v => v.ChannelId == user.Channel.Id);


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
                        videosQuery = videosQuery.Where(v => v.Size/1024/1024 <= 50);
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


        [HttpPost("upload")]
        [Authorize(Roles = "channel_owner")]
        public async Task<IActionResult> UploadVideo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file");

            var allowedContentTypes = new[] { "video/mp4", "video/avi", "video/x-matroska", "video/quicktime" };

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
            

            var tempFilePath = Path.Combine(Path.GetTempPath(), file.FileName);
            var convertedFilePath = string.Empty;
            try
            {
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var ffmpegPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ffmpeg");
                FFmpeg.SetExecutablesPath(ffmpegPath);


                if (Path.GetExtension(file.FileName).ToLower() == ".avi")
                {
                    convertedFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.mp4");

                    await FFmpeg.Conversions.New()
                        .AddParameter($"-i \"{tempFilePath}\"")
                        .AddParameter($"-c:v libx264 -preset fast -crf 23")
                        .AddParameter($"-c:a aac -b:a 128k")
                        .AddParameter($"\"{convertedFilePath}\"")
                        .Start();

                    tempFilePath = convertedFilePath;
                }
 
                var mediaInfo = await FFmpeg.GetMediaInfo(tempFilePath);

                Video video = new Video()
                {
                    Title = Path.GetFileNameWithoutExtension(file.FileName).Length < 2 ? "Default Title" : Path.GetFileNameWithoutExtension(file.FileName),
                    PublicationDate = DateTime.Now,
                    Format = Path.GetExtension(file.FileName),
                    Duration = mediaInfo.Duration,
                    Size = mediaInfo.Size,
                    IsPublic = false,
                    IsUploaded = false,
                    Url = "",
                    ChannelId = user.Channel.Id
                };
                await _videoService.CreateAsync(video);

                var videoId = video.Id;
                var blobName = $"videos/{videoId}{Path.GetExtension(file.FileName)}";


                var random = new Random();
                var randomTime = TimeSpan.FromSeconds(random.Next(0, (int)mediaInfo.Duration.TotalSeconds));

                var previewFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}_{Path.GetFileNameWithoutExtension(file.FileName)}.jpeg");
                await FFmpeg.Conversions.New()
                        .AddParameter($"-i \"{tempFilePath}\"")
                        .AddParameter($"-ss {randomTime}")
                        .AddParameter("-vframes 1")
                        .AddParameter($"\"{previewFilePath}\"")
                        .Start();


                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                var blobClient = containerClient.GetBlobClient(blobName);

                using (var stream = System.IO.File.OpenRead(tempFilePath))
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                if (System.IO.File.Exists(previewFilePath))
                {
                    var previewBlobName = $"previews/{videoId}.jpeg";
                    var previewBlobClient = containerClient.GetBlobClient(previewBlobName);

                    using (var previewStream = System.IO.File.OpenRead(previewFilePath))
                    {
                        await previewBlobClient.UploadAsync(previewStream, overwrite: true);
                    }

                    video.PreviewUrl = previewBlobClient.Uri.ToString();
                    System.IO.File.Delete(previewFilePath);
                }

                var fileUrl = blobClient.Uri.ToString();
                video.Url = fileUrl;
                video.IsUploaded = true;
                var videoRes = await _videoService.UpdateAsync(videoId, video);

                return Ok(new {
                    videoRes.Id,
                    videoRes.Title,
                    videoRes.PublicationDate,
                    videoRes.Format,
                    videoRes.Size,
                    videoRes.PreviewUrl,
                    videoRes.Description,
                    videoRes.Duration,
                    videoRes.IsPublic
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error while uploading video.");
            }
            finally
            {
                if (System.IO.File.Exists(tempFilePath))
                {
                    System.IO.File.Delete(tempFilePath);
                }

            }
        }
    }
}

using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VibeoServer.Data;
using VibeoServer.Helpers;
using VibeoServer.Models;
using VibeoServer.Services;
using VibeoServer.Services.Interfaces;

namespace VibeoServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDataProtection();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
           
            builder.Services.AddDbContext<SqlDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<User, IdentityRole>(options => {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
            })
           .AddEntityFrameworkStores<SqlDbContext>()
           .AddDefaultTokenProviders();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            }).AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["GoogleAuthentication:ClientId"];
                googleOptions.ClientSecret = builder.Configuration["GoogleAuthentication:ClientSecret"];
                googleOptions.CallbackPath = "/signin-google";
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("X-Token-Expired"); ;
                });
            });


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 2147483648;
            });

            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Limits.MaxRequestBodySize = 2147483648;
            });

            //builder.Services.AddSingleton<BlobServiceClient>((serviceProvider) => {
            //    var config = serviceProvider.GetRequiredService<IConfiguration>();
            //    var storageAccountUri = config["StorageAccount:Uri"];
            //    var accountUri = new Uri(storageAccountUri);
            //    var credential = new DefaultAzureCredential();
            //    var blobServiceClient = new BlobServiceClient(accountUri, credential);
            //    return blobServiceClient;
            //});

            builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("StorageAccount")));

            builder.Services.AddScoped<IJWTService, JWTService>();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<IVideoService, VideoService>();
            builder.Services.AddTransient<IPlaylistService, PlaylistService>();
            builder.Services.AddTransient<IChannelService, ChannelService>();
            builder.Services.AddTransient<ICommentService, CommentService>();
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddTransient<ISubscriptionService, SubscriptionService>();
            builder.Services.AddTransient<IVideoStatusService, VideoStatusService>();
            builder.Services.AddTransient<ICommentStatusService, CommentStatusService>();
            builder.Services.AddTransient<IHistoryService, HistoryService>();
            builder.Services.AddTransient<INotificationTypeService, NotificationTypeService>();
            builder.Services.AddTransient<ITagService, TagService>();
            builder.Services.AddTransient<INotificationService, NotificationService>();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var app = builder.Build();


            app.UseHttpsRedirection();

            app.UseCors("AllowSpecificOrigins");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();

            app.Run();
        }
    }
}

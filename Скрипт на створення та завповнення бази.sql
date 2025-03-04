USE [master]
GO
/****** Object:  Database [VibeoServerMain]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE DATABASE [VibeoServerMain]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'VibeoServerMain', FILENAME = N'E:\Databases\VibeoServerMain.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'VibeoServerMain_log', FILENAME = N'E:\Databases\VibeoServerMain_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [VibeoServerMain] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [VibeoServerMain].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [VibeoServerMain] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [VibeoServerMain] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [VibeoServerMain] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [VibeoServerMain] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [VibeoServerMain] SET ARITHABORT OFF 
GO
ALTER DATABASE [VibeoServerMain] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [VibeoServerMain] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [VibeoServerMain] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [VibeoServerMain] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [VibeoServerMain] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [VibeoServerMain] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [VibeoServerMain] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [VibeoServerMain] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [VibeoServerMain] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [VibeoServerMain] SET  ENABLE_BROKER 
GO
ALTER DATABASE [VibeoServerMain] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [VibeoServerMain] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [VibeoServerMain] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [VibeoServerMain] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [VibeoServerMain] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [VibeoServerMain] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [VibeoServerMain] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [VibeoServerMain] SET RECOVERY FULL 
GO
ALTER DATABASE [VibeoServerMain] SET  MULTI_USER 
GO
ALTER DATABASE [VibeoServerMain] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [VibeoServerMain] SET DB_CHAINING OFF 
GO
ALTER DATABASE [VibeoServerMain] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [VibeoServerMain] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [VibeoServerMain] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [VibeoServerMain] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'VibeoServerMain', N'ON'
GO
ALTER DATABASE [VibeoServerMain] SET QUERY_STORE = ON
GO
ALTER DATABASE [VibeoServerMain] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [VibeoServerMain]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](255) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[AvatarUrl] [nvarchar](512) NULL,
	[BirthDate] [datetime2](7) NULL,
	[RefreshToken] [nvarchar](max) NULL,
	[RefreshTokenExpiryTime] [datetime2](7) NOT NULL,
	[EmailConfirmationCode] [nvarchar](max) NULL,
	[EmailConfirmationCodeExpiryTime] [datetime2](7) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoriesVideos]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoriesVideos](
	[CategoryId] [int] NOT NULL,
	[VideoId] [int] NOT NULL,
 CONSTRAINT [PK_CategoriesVideos] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC,
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Channels]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Channels](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[LogoUrl] [nvarchar](512) NULL,
	[Handle] [nvarchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_Channels] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](1000) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ChannelId] [int] NOT NULL,
	[VideoId] [int] NOT NULL,
	[ParentCommentId] [int] NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CommentStatuses]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommentStatuses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[isLiked] [bit] NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[CommentId] [int] NOT NULL,
 CONSTRAINT [PK_CommentStatuses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Histories]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Histories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[VideoId] [int] NOT NULL,
	[WatchedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Histories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](256) NOT NULL,
	[Message] [nvarchar](1000) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[IsRead] [bit] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[NotificationTypeId] [int] NOT NULL,
	[VideoId] [int] NULL,
	[CommentId] [int] NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationTypes]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_NotificationTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Playlists]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Playlists](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[IsPublic] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Playlists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlaylistsVideos]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlaylistsVideos](
	[PlaylistId] [int] NOT NULL,
	[VideoId] [int] NOT NULL,
 CONSTRAINT [PK_PlaylistsVideos] PRIMARY KEY CLUSTERED 
(
	[PlaylistId] ASC,
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subscriptions]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subscriptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ChannelId] [int] NOT NULL,
	[SubscribedAt] [datetime2](7) NOT NULL,
	[ReceiveNotifications] [bit] NOT NULL,
 CONSTRAINT [PK_Subscriptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tags]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tags](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TagsVideos]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagsVideos](
	[TagId] [int] NOT NULL,
	[VideoId] [int] NOT NULL,
 CONSTRAINT [PK_TagsVideos] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC,
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Videos]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Videos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[PublicationDate] [datetime2](7) NOT NULL,
	[Url] [nvarchar](512) NOT NULL,
	[PreviewUrl] [nvarchar](512) NULL,
	[Description] [nvarchar](1000) NULL,
	[Duration] [time](7) NOT NULL,
	[IsPublic] [bit] NOT NULL,
	[ChannelId] [int] NOT NULL,
	[Format] [nvarchar](50) NOT NULL,
	[IsUploaded] [bit] NOT NULL,
	[Size] [bigint] NOT NULL,
 CONSTRAINT [PK_Videos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VideoStatuses]    Script Date: 1/8/2025 12:12:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VideoStatuses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsLiked] [bit] NULL,
	[IsFavorite] [bit] NOT NULL,
	[WatchLater] [bit] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[VideoId] [int] NOT NULL,
 CONSTRAINT [PK_VideoStatuses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241112101034_Initial-Migration', N'8.0.10')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241112104524_AddFormat-Migration', N'8.0.10')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241116130349_IsUploadedMigration', N'8.0.10')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241116134857_SizeMigration', N'8.0.10')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241123145454_PlaylistLastUpdatedMigration', N'8.0.10')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'38985dfe-f11b-4c21-8b78-b3e0a835b5bb', N'admin', N'admin', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'f38f67a0-0419-42c1-bea2-5f60a13168df', N'channel_owner', N'channel_owner', NULL)
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', N'f38f67a0-0419-42c1-bea2-5f60a13168df')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'77f01ccf-396e-418d-ba05-69ad7f7ee736', N'f38f67a0-0419-42c1-bea2-5f60a13168df')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', N'f38f67a0-0419-42c1-bea2-5f60a13168df')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'eb81d759-e897-4178-8a40-5990eb19b39e', N'f38f67a0-0419-42c1-bea2-5f60a13168df')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'f744397e-cc0c-45d0-b407-162c9418232a', N'f38f67a0-0419-42c1-bea2-5f60a13168df')
GO
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [CreatedAt], [AvatarUrl], [BirthDate], [RefreshToken], [RefreshTokenExpiryTime], [EmailConfirmationCode], [EmailConfirmationCodeExpiryTime], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', N'Andrew', N'Tsyselskyi', CAST(N'2024-11-17T11:55:23.8559948' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/user-avatars/39e0a996-1386-45d9-9492-4d4d44d0f2a3.jpeg', CAST(N'2008-09-03T00:00:00.0000000' AS DateTime2), N'nwMtetH0o7SA9m9BZ6o0mzUTdAUelSytmcaEnnD4F3RlybwfYdIdLfMm17A+t+h5hfBa7DOqEsUoronRyTPrZQ==', CAST(N'2025-01-13T22:39:53.2284704' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'tsyselskyiandrey@gmail.com', N'TSYSELSKYIANDREY@GMAIL.COM', N'tsyselskyiandrey@gmail.com', N'TSYSELSKYIANDREY@GMAIL.COM', 1, NULL, N'LBUQWGYDL2WWWBVBXOTWVG75CU5TNKXA', N'b52f1065-8cfc-4fb7-bb26-e566c84aed4e', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [CreatedAt], [AvatarUrl], [BirthDate], [RefreshToken], [RefreshTokenExpiryTime], [EmailConfirmationCode], [EmailConfirmationCodeExpiryTime], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'673ec326-c93d-48a7-918f-8d745b31e06c', N'Kaktusl', N'TS', CAST(N'2025-01-06T21:23:48.0499263' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/user-avatars/default.png', NULL, NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'kaktuslts@gmail.com', N'KAKTUSLTS@GMAIL.COM', N'kaktuslts@gmail.com', N'KAKTUSLTS@GMAIL.COM', 1, NULL, N'626IZPLQWPSEE4Q2I3NNPPHHHELYQPAI', N'523bcc30-e99d-4d4f-9452-958aa313e5ce', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [CreatedAt], [AvatarUrl], [BirthDate], [RefreshToken], [RefreshTokenExpiryTime], [EmailConfirmationCode], [EmailConfirmationCodeExpiryTime], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'77f01ccf-396e-418d-ba05-69ad7f7ee736', N'Max', N'Fosh', CAST(N'2024-11-27T22:59:28.4088180' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/user-avatars/77f01ccf-396e-418d-ba05-69ad7f7ee736.jpeg', CAST(N'2001-12-03T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'tsyselskyiandrey2@gmail.com', N'TSYSELSKYIANDREY2@GMAIL.COM', N'tsyselskyiandrey2@gmail.com', N'TSYSELSKYIANDREY2@GMAIL.COM', 1, NULL, N'GF3ZTY75TI2DLMJ5IUUV32ZQJOD267JY', N'e6a6d206-6741-454d-b7a3-cdf7fa177c1f', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [CreatedAt], [AvatarUrl], [BirthDate], [RefreshToken], [RefreshTokenExpiryTime], [EmailConfirmationCode], [EmailConfirmationCodeExpiryTime], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', N'Max', N'Verstappen', CAST(N'2024-12-08T02:08:23.6715724' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/user-avatars/cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7.jpeg', CAST(N'1991-10-19T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'tsyselskyi@gmail.com', N'TSYSELSKYI@GMAIL.COM', N'tsyselskyi@gmail.com', N'TSYSELSKYI@GMAIL.COM', 1, NULL, N'W5XKMFUA3CZ4KL5TPQ23QKO4V7VWQKKQ', N'7c41df67-2c33-4462-bdb3-874d099ed605', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [CreatedAt], [AvatarUrl], [BirthDate], [RefreshToken], [RefreshTokenExpiryTime], [EmailConfirmationCode], [EmailConfirmationCodeExpiryTime], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'eb81d759-e897-4178-8a40-5990eb19b39e', N'Net', N'Ninja', CAST(N'2024-11-19T21:29:41.6561997' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/user-avatars/eb81d759-e897-4178-8a40-5990eb19b39e.jpeg', CAST(N'2006-09-03T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'andrey.tsyselskyi@gmail.com', N'ANDREY.TSYSELSKYI@GMAIL.COM', N'andrey.tsyselskyi@gmail.com', N'ANDREY.TSYSELSKYI@GMAIL.COM', 1, NULL, N'NJTIJXUWP76ARSFAVTEKJ3WD3U6O2AGM', N'8be027df-a4d1-4543-ab16-f46c903e05b9', NULL, 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [CreatedAt], [AvatarUrl], [BirthDate], [RefreshToken], [RefreshTokenExpiryTime], [EmailConfirmationCode], [EmailConfirmationCodeExpiryTime], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'f744397e-cc0c-45d0-b407-162c9418232a', N'Nick', N'DiGiovanni', CAST(N'2024-11-27T22:49:31.4037233' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/user-avatars/f744397e-cc0c-45d0-b407-162c9418232a.jpeg', CAST(N'1998-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), N'andrii.tsyselskyi@nure.ua', N'ANDRII.TSYSELSKYI@NURE.UA', N'andrii.tsyselskyi@nure.ua', N'ANDRII.TSYSELSKYI@NURE.UA', 1, NULL, N'VO3SURJOPVUZOJDZIYXEETQP6X6UV5DM', N'd779e960-6a9d-413f-85b9-1d5c895c2903', NULL, 0, 0, NULL, 1, 0)
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [Name]) VALUES (1, N'Education')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (2, N'Entertainment')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (3, N'Music')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (4, N'Gaming')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (5, N'Technology')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (6, N'Sports')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (7, N'News')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (8, N'Travel')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (9, N'Lifestyle')
INSERT [dbo].[Categories] ([Id], [Name]) VALUES (15, N'Cooking')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (1, 1031)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (2, 1034)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (4, 1034)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (1, 1035)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (5, 1035)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (7, 1035)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (2, 1055)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (4, 1055)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (3, 2055)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (8, 2055)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (9, 2055)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (2, 2056)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (9, 2056)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (2, 3063)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (9, 3063)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (2, 4064)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (1, 6063)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (5, 6063)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (1, 6064)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (5, 6064)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (1, 6065)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (5, 6065)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (1, 6066)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (5, 6066)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (1, 6067)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (5, 6067)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (1, 6068)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (5, 6068)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (1, 6069)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (5, 6069)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (1, 6070)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (5, 6070)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (1, 6071)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (5, 6071)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (1, 6072)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (5, 6072)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (1, 6073)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (5, 6073)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6074)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6075)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6076)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6077)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6078)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6079)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6080)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6081)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (2, 6082)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (9, 6082)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (2, 6083)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (9, 6083)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (2, 6084)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (9, 6084)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (2, 6085)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (9, 6085)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (2, 6086)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (9, 6086)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (2, 6087)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (9, 6087)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (2, 6088)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (9, 6088)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (2, 6089)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (9, 6089)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (9, 6090)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6090)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6091)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6092)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6093)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6094)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6095)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6096)
INSERT [dbo].[CategoriesVideos] ([CategoryId], [VideoId]) VALUES (15, 6097)
GO
SET IDENTITY_INSERT [dbo].[Channels] ON 

INSERT [dbo].[Channels] ([Id], [Title], [Description], [CreatedAt], [LogoUrl], [Handle], [IsDeleted], [UserId]) VALUES (1005, N'Andrew Tsyselskyi', N'My name is Andrew. I am 18 y.o.', CAST(N'2024-11-17T11:55:51.4050782' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/channel-avatars/1005.jpeg', N'AndrewTsyselskyi', 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3')
INSERT [dbo].[Channels] ([Id], [Title], [Description], [CreatedAt], [LogoUrl], [Handle], [IsDeleted], [UserId]) VALUES (1006, N'Net Ninja', N'Black-belt your web development skills. Over 2000 free programming tutorial videos about:

- Modern JavaScript (beginner to advanced)
- Node.js
- React
- Vue.js
- Firebase
- MongoDB
- HTML & CSS
- PHP & MySQL
- Laravel
- React Native
- Flutter
- Open AI
- SolidJS

...And many more topics as well :)', CAST(N'2024-11-27T22:37:51.7489885' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/channel-avatars/1006.jpeg', N'NetNinja', 0, N'eb81d759-e897-4178-8a40-5990eb19b39e')
INSERT [dbo].[Channels] ([Id], [Title], [Description], [CreatedAt], [LogoUrl], [Handle], [IsDeleted], [UserId]) VALUES (1007, N'Nick DiGiovanni', N'Subscribe to get to 30M!', CAST(N'2024-11-27T22:50:05.3266742' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/channel-avatars/1007.jpeg', N'NickDiGiovanni', 0, N'f744397e-cc0c-45d0-b407-162c9418232a')
INSERT [dbo].[Channels] ([Id], [Title], [Description], [CreatedAt], [LogoUrl], [Handle], [IsDeleted], [UserId]) VALUES (1010, N'Max the Meat Guy', N'Check out MaxJerky! 🥩
https://www.maxjerky.com

—————
SUBSCRIBE and RING THE BELL to get notified when I post a video!

PLATFORMS
-Snapchat: https://www.snapchat.com/add/maxthemeatguy?share_id=Nzg2RjE4&locale=en_US
-Instagram: https://www.instagram.com/maxthemeatguy/
-TikTok: https://www.tiktok.com/@maxthemeatguy
-Facebook: https://www.facebook.com/MaxTheMeatGuy
-YouTube: https://youtube.com/c/maxthemeatguy

DISCOUNTS
https://www.beacons.ai/maxthemeatguy

WEBSITE
https://www.maxthemeatguy.com/', CAST(N'2024-12-08T02:08:46.3019064' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/channel-avatars/1010.jpeg', N'MaxtheMeatGuy', 0, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7')
INSERT [dbo].[Channels] ([Id], [Title], [Description], [CreatedAt], [LogoUrl], [Handle], [IsDeleted], [UserId]) VALUES (4009, N'Max Fosh', N'subscribe if you''re a silly billy', CAST(N'2024-12-31T16:37:01.2576868' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/channel-avatars/4009.jpeg', N'MaxFosh', 0, N'77f01ccf-396e-418d-ba05-69ad7f7ee736')
SET IDENTITY_INSERT [dbo].[Channels] OFF
GO
SET IDENTITY_INSERT [dbo].[Comments] ON 

INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (1136, N'The video quality is great', CAST(N'2024-12-04T14:26:38.8739609' AS DateTime2), 1005, 2056, NULL)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (1137, N'I don''t even know what to say... Just awesome ', CAST(N'2024-12-04T14:32:00.7508700' AS DateTime2), 1005, 2056, NULL)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (1139, N'A very good video!', CAST(N'2024-12-04T14:32:03.9141937' AS DateTime2), 1005, 2056, NULL)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (1140, N'Agree with you!', CAST(N'2024-12-04T14:39:59.6591164' AS DateTime2), 1005, 2056, 1139)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (1141, N'fact', CAST(N'2024-12-04T14:41:31.6799989' AS DateTime2), 1005, 2056, 1139)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (1142, N'you have a point', CAST(N'2024-12-04T14:41:34.4141577' AS DateTime2), 1005, 2056, 1139)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (1143, N'true', CAST(N'2024-12-04T14:46:54.4643691' AS DateTime2), 1005, 2056, 1139)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (1147, N'That a bad video', CAST(N'2024-12-05T16:59:17.0233756' AS DateTime2), 1005, 2056, NULL)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (2154, N'asdada', CAST(N'2024-12-08T02:12:49.1052306' AS DateTime2), 1010, 3063, NULL)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (2155, N'asdada', CAST(N'2024-12-08T02:17:33.6265732' AS DateTime2), 1010, 3063, NULL)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (2156, N'asdada', CAST(N'2024-12-08T02:17:37.2896747' AS DateTime2), 1010, 3063, 2155)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (3144, N'The greatest video I have seen', CAST(N'2024-12-09T00:27:33.1385899' AS DateTime2), 1005, 1031, NULL)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (3146, N'thank you for your reactions', CAST(N'2024-12-10T00:42:24.4556677' AS DateTime2), 1005, 2056, 1147)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (3159, N'I completely agree with you', CAST(N'2024-12-10T03:42:21.6916307' AS DateTime2), 1006, 2056, 1147)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (3160, N'You are right!', CAST(N'2024-12-10T03:42:28.9964865' AS DateTime2), 1006, 2056, 1147)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (3162, N'thank you for your comment', CAST(N'2024-12-10T20:33:08.3951382' AS DateTime2), 1005, 2056, 1147)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (6145, N'for sure', CAST(N'2024-12-28T17:28:25.9384410' AS DateTime2), 1005, 2056, NULL)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (6146, N'sda', CAST(N'2024-12-28T17:28:30.4909367' AS DateTime2), 1005, 2056, 6145)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (7145, N'He definitely doesn''t know how to cook!', CAST(N'2024-12-31T16:08:07.8923042' AS DateTime2), 1007, 6074, NULL)
INSERT [dbo].[Comments] ([Id], [Content], [CreatedAt], [ChannelId], [VideoId], [ParentCommentId]) VALUES (7146, N'for sure', CAST(N'2024-12-31T16:08:20.8593836' AS DateTime2), 1007, 6074, 7145)
SET IDENTITY_INSERT [dbo].[Comments] OFF
GO
SET IDENTITY_INSERT [dbo].[CommentStatuses] ON 

INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (40, 1, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1136)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (41, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1137)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (43, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1139)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (44, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1140)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (45, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1141)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (46, 1, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1142)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (47, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1143)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (49, 0, N'eb81d759-e897-4178-8a40-5990eb19b39e', 1139)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (53, 0, N'eb81d759-e897-4178-8a40-5990eb19b39e', 1143)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (55, 1, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1147)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (2048, NULL, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2156)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (2049, 1, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3144)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (4050, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6145)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (4051, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3160)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (4052, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3159)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (4053, 1, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3162)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (5050, 1, N'f744397e-cc0c-45d0-b407-162c9418232a', 7145)
INSERT [dbo].[CommentStatuses] ([Id], [isLiked], [UserId], [CommentId]) VALUES (5051, 1, N'f744397e-cc0c-45d0-b407-162c9418232a', 7146)
SET IDENTITY_INSERT [dbo].[CommentStatuses] OFF
GO
SET IDENTITY_INSERT [dbo].[Histories] ON 

INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-05T23:33:16.2919604' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (6, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-05T23:47:51.1905260' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-05T23:46:25.7940371' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1031, CAST(N'2024-12-06T01:59:10.7582595' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (10, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-05T23:55:20.5861392' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (11, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-05T23:55:46.2325335' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (13, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-06T00:08:10.9481535' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (15, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T00:00:26.0724914' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (16, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T00:02:18.7202080' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (18, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T00:02:38.5777056' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (20, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T00:08:04.5896920' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (23, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T00:08:15.9983554' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (1002, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-06T00:15:00.5973427' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (1003, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T00:15:03.9830252' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (1005, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T00:14:51.8462264' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (1007, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T00:17:16.3070249' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2002, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T00:29:54.8885300' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2003, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-06T00:37:42.9296426' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2006, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T00:29:57.5084697' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2009, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T00:37:56.5277797' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2012, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-06T00:46:07.6156027' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2013, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T00:46:26.5007292' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2014, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T00:46:29.0682567' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2015, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-06T01:18:50.7394187' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2017, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T00:53:00.1580735' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2018, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T00:53:15.6939819' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2020, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T00:58:48.5694649' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2024, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T01:03:24.4096817' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2025, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:05:50.4327723' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2028, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:09:12.7130923' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2030, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:12:28.5623016' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2032, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T01:14:04.2546862' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2034, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:16:23.7198687' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (2035, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:18:03.1266447' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3002, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T01:37:42.8385083' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3004, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:38:20.8803464' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3005, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:39:48.3734006' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3006, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T01:39:51.9888565' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3009, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-06T01:42:22.3285031' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3010, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:42:16.0825118' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3012, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:48:27.2215733' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3014, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-06T01:59:03.5379442' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3015, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:50:34.6981624' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3017, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T01:51:17.3295009' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3019, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:55:11.1891862' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3020, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-06T01:55:31.4315181' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3021, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:56:31.7152880' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3023, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:57:47.6750711' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3024, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T01:59:11.9303200' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3026, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-06T16:17:28.0535562' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3028, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T16:17:52.8115497' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3029, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T16:20:24.9901698' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3030, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-06T18:34:01.6781212' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3032, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T18:34:45.8408442' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3033, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1031, CAST(N'2024-12-07T00:45:05.2970642' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3034, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-06T21:52:08.5447626' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3037, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-07T00:30:45.5285068' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3039, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-07T00:45:02.3202784' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3041, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-07T08:20:15.3367910' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3048, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-07T09:46:07.9635620' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3068, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 3063, CAST(N'2024-12-08T02:08:29.2258533' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3069, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 3063, CAST(N'2024-12-08T02:17:30.5134718' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3070, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 1031, CAST(N'2024-12-08T02:31:13.6865567' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3071, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 3063, CAST(N'2024-12-08T02:30:55.2275225' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3073, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 1034, CAST(N'2024-12-08T02:31:22.1380518' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3074, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 1055, CAST(N'2024-12-08T02:31:25.7663315' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3075, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 2056, CAST(N'2024-12-08T02:31:36.3430129' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3076, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-08T02:32:00.4060011' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3077, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1031, CAST(N'2024-12-08T03:35:09.4406756' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3079, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-08T02:33:43.5786524' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3080, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-08T02:34:49.0770775' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3082, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T02:49:04.4968541' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3084, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T02:59:38.9895576' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3085, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-08T03:03:16.6867074' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3087, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-08T03:03:28.9228380' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3088, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T03:10:29.1439760' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3089, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-08T03:33:51.1770223' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3090, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-08T03:33:36.0272363' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3091, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T03:33:56.1056092' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3093, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-08T03:38:21.3782871' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (3094, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T03:38:43.1018976' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4002, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T13:50:52.1469356' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4003, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1031, CAST(N'2024-12-09T01:57:22.1789372' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4005, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T14:17:07.2820036' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4006, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-08T14:17:33.3826958' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4007, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T15:04:33.4350875' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4009, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-08T15:54:01.1060146' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4010, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-08T15:54:00.4205633' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4011, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-08T15:53:32.0296718' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4012, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-08T15:56:28.9515145' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4013, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-08T16:08:48.2950762' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4014, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-08T16:27:47.0363602' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4015, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T16:27:55.3168801' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4016, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-08T16:45:36.6328891' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4017, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-08T16:49:29.7415499' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4018, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T16:54:00.0016378' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4020, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1035, CAST(N'2024-12-08T18:19:23.0717540' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4021, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T18:18:49.8820610' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4022, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T19:14:05.5958073' AS DateTime2))
GO
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4023, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-08T19:15:01.5054435' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4024, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T19:21:41.3894413' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4025, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T20:01:44.4448788' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4026, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-08T20:01:43.9940440' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4027, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-08T20:01:43.4951716' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4028, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-08T20:17:40.2196048' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4029, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-08T20:17:47.2450921' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4030, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-08T20:17:50.6422974' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4031, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1035, CAST(N'2024-12-08T20:18:39.5980979' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4032, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-08T20:19:54.8811960' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4033, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-08T20:18:47.4548991' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4034, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-09T00:16:14.2035330' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4036, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T00:16:12.8362526' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4037, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-09T00:16:17.2861819' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4038, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T00:19:31.9070639' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4039, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T00:19:31.7256621' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4040, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-09T00:24:01.0508497' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4041, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T00:23:56.9488912' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4042, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T00:27:16.8144760' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4043, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T00:27:15.4682121' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4044, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-09T00:29:23.7568734' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4045, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-09T00:31:21.2169381' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4046, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T00:31:07.2847205' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4047, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T00:31:08.3771777' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4048, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T00:32:10.1047257' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4049, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-09T00:36:12.1761253' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4050, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T00:36:08.1022252' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4051, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-09T00:41:58.7738122' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4052, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T00:41:46.8632691' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4053, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T00:41:27.3358780' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4055, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T01:22:49.5331812' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4057, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T01:24:07.5625056' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4058, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-09T01:24:30.9746616' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4060, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-09T01:30:05.4988418' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4061, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T01:29:54.0884790' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4062, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-09T01:29:56.5572364' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4063, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-09T01:32:46.5960418' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4064, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T01:31:04.3410252' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4065, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T01:30:59.8152574' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4066, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T01:33:49.0853244' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4067, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T01:35:41.0242011' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4068, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T01:36:42.7962805' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4069, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T01:40:43.8920510' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4070, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-09T01:52:43.0171858' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4071, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1035, CAST(N'2024-12-09T01:52:32.1432061' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4072, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T01:52:36.6011819' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4073, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-09T01:54:01.9225048' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4074, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T01:52:45.2034740' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4075, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-09T01:52:52.5736400' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4078, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T01:57:20.6148913' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4079, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T01:57:21.3878479' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4080, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T12:53:25.5512826' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4081, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T12:53:26.7654175' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4082, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1031, CAST(N'2024-12-09T12:53:27.6758838' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4083, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-09T14:05:13.1675131' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4084, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T18:06:27.8809112' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4085, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-09T19:00:24.2343432' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4087, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T19:00:10.1887362' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4089, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T19:03:12.7486557' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4090, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-09T19:05:14.4313778' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4091, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T19:08:18.6730008' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4093, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T19:07:25.4105585' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4094, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1031, CAST(N'2024-12-10T00:39:54.8606664' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4095, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-09T19:07:37.4136525' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4097, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-09T19:12:28.7527017' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4098, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T19:35:01.0140786' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4100, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-09T19:36:03.4568684' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4101, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T19:36:17.3799647' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4102, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T20:13:32.9746320' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4103, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T20:13:33.8025873' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4104, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-09T20:18:50.9369914' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4105, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T20:17:01.5148898' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4107, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T20:19:04.1962759' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4108, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-09T20:19:06.7735095' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4109, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-09T20:50:38.6803507' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4110, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T21:04:24.6937864' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4112, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T21:26:26.9122988' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4113, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-09T21:26:32.5482832' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4116, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T21:59:59.3365803' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4117, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-09T22:04:57.6464440' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4118, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-09T22:11:53.4747158' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4119, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-09T22:20:48.1564020' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4120, N'eb81d759-e897-4178-8a40-5990eb19b39e', 2056, CAST(N'2024-12-10T00:34:39.2849307' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4123, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-10T00:39:46.9306713' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4124, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-10T00:39:50.6191382' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4125, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-10T00:48:25.7131879' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4126, N'eb81d759-e897-4178-8a40-5990eb19b39e', 2056, CAST(N'2024-12-10T00:58:26.0229302' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4127, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-10T01:07:42.7025729' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4128, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-10T01:21:57.1498603' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4129, N'eb81d759-e897-4178-8a40-5990eb19b39e', 2056, CAST(N'2024-12-10T01:23:27.9780555' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4130, N'eb81d759-e897-4178-8a40-5990eb19b39e', 2056, CAST(N'2024-12-10T02:53:57.7076053' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4131, N'eb81d759-e897-4178-8a40-5990eb19b39e', 2056, CAST(N'2024-12-10T03:42:04.0456420' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4132, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-10T13:30:20.2399359' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4134, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-10T13:25:35.0453006' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4136, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-10T13:46:53.2456709' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4138, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-10T19:02:00.0868058' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4139, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-10T19:08:16.8938604' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4140, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-10T19:26:37.9719741' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4142, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-10T19:24:49.6651965' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4143, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-10T20:33:10.1178642' AS DateTime2))
GO
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4144, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-10T20:33:44.2408470' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4145, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-10T20:33:45.7340496' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4146, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1031, CAST(N'2024-12-10T20:33:46.6786555' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4147, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-10T23:03:01.0607758' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4148, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-11T02:14:04.7867784' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4149, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-11T02:40:52.0984283' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4150, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-11T02:44:49.2141034' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4152, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-11T04:22:01.4051741' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4160, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-11T05:39:14.4919647' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4161, N'eb81d759-e897-4178-8a40-5990eb19b39e', 4064, CAST(N'2024-12-11T05:39:36.6198713' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4163, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-11T11:15:46.7731973' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (4164, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-11T11:17:34.3705108' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (5120, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-13T22:22:25.6117996' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (5122, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-13T22:23:54.9385259' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (6120, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 4064, CAST(N'2024-12-18T22:45:42.1837613' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (6121, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 4064, CAST(N'2024-12-18T22:50:37.5643629' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (6122, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 4064, CAST(N'2024-12-18T22:54:07.4959390' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (6126, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-18T22:58:12.4739686' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7121, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-28T11:53:33.1412228' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7122, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-28T11:53:38.7769224' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7123, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-28T16:36:09.1555774' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7124, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-28T16:38:15.2401847' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7125, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-28T16:38:37.2993835' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7126, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-28T16:44:24.2342612' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7128, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-28T16:41:32.3964152' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7129, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-28T16:41:49.5807108' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7130, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-28T16:41:50.1372934' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7131, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1031, CAST(N'2024-12-28T16:41:50.6581299' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7133, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-28T16:47:56.5973434' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7135, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-28T16:52:40.2439523' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7136, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-28T16:54:10.9712368' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7138, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-28T17:05:06.9503599' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7140, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-28T17:05:06.2058211' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7141, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-28T17:16:28.6815993' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7143, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-28T17:28:14.8429966' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7144, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-28T17:29:03.8469186' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7145, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-28T17:28:02.9337590' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7146, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-28T17:27:58.8469056' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7147, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-28T17:27:58.6489180' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (7148, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-28T17:29:06.4395918' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8120, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6063, CAST(N'2024-12-30T23:02:27.2738754' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8121, N'eb81d759-e897-4178-8a40-5990eb19b39e', 4064, CAST(N'2024-12-30T22:56:46.6303261' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8122, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6071, CAST(N'2024-12-30T23:30:10.1147109' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8123, N'eb81d759-e897-4178-8a40-5990eb19b39e', 4064, CAST(N'2024-12-30T23:30:20.1138391' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8124, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6072, CAST(N'2024-12-30T23:43:25.2753823' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8125, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6070, CAST(N'2024-12-30T23:37:57.9728224' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8126, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6073, CAST(N'2024-12-30T23:50:59.4172893' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8127, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6063, CAST(N'2024-12-30T23:36:30.2617171' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8128, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6064, CAST(N'2024-12-30T23:36:11.8704799' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8129, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6065, CAST(N'2024-12-30T23:36:31.1430882' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8130, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6067, CAST(N'2024-12-30T23:37:14.7846489' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8131, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6066, CAST(N'2024-12-30T23:36:43.0747590' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8132, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6069, CAST(N'2024-12-30T23:37:36.1424462' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8133, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6068, CAST(N'2024-12-30T23:37:39.7545339' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8134, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034, CAST(N'2024-12-31T00:00:56.4352958' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8135, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6073, CAST(N'2024-12-31T00:23:30.7123732' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8136, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-31T00:00:42.0434956' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8137, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-31T00:01:02.0672451' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8138, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1031, CAST(N'2024-12-31T00:54:43.0440025' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8139, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-31T00:01:05.1066802' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8140, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-31T00:15:34.3037805' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8141, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-31T00:15:31.9819828' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8142, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6073, CAST(N'2024-12-31T00:16:07.6677263' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8143, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6072, CAST(N'2024-12-31T00:16:13.2944116' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8144, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6072, CAST(N'2024-12-31T00:16:26.8410714' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8145, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6067, CAST(N'2024-12-31T00:16:33.3676112' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8156, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6073, CAST(N'2024-12-31T00:43:28.7750083' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8157, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6069, CAST(N'2024-12-31T00:43:30.2779728' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8158, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6067, CAST(N'2024-12-31T00:43:33.3436053' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8159, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063, CAST(N'2024-12-31T00:54:41.2501922' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8160, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 4064, CAST(N'2024-12-31T00:54:32.4501878' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8161, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2024-12-31T00:56:43.4355071' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8162, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055, CAST(N'2024-12-31T00:54:42.1933667' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8163, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1035, CAST(N'2024-12-31T00:55:21.0679789' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8164, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6073, CAST(N'2024-12-31T00:56:58.5983587' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8165, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6067, CAST(N'2024-12-31T00:55:47.6468066' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8166, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055, CAST(N'2024-12-31T00:56:45.1673901' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8167, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6066, CAST(N'2024-12-31T15:34:16.1219933' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8168, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6073, CAST(N'2024-12-31T15:34:17.6295213' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8169, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6068, CAST(N'2024-12-31T15:34:19.2525208' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8170, N'f744397e-cc0c-45d0-b407-162c9418232a', 6074, CAST(N'2024-12-31T16:08:34.6358937' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8171, N'f744397e-cc0c-45d0-b407-162c9418232a', 6069, CAST(N'2024-12-31T16:04:30.7919108' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8172, N'f744397e-cc0c-45d0-b407-162c9418232a', 6081, CAST(N'2024-12-31T16:08:28.2508360' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8173, N'f744397e-cc0c-45d0-b407-162c9418232a', 6079, CAST(N'2024-12-31T15:56:58.5504813' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8174, N'f744397e-cc0c-45d0-b407-162c9418232a', 6073, CAST(N'2024-12-31T16:01:59.5222951' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8175, N'f744397e-cc0c-45d0-b407-162c9418232a', 2055, CAST(N'2024-12-31T15:57:38.9610352' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8176, N'f744397e-cc0c-45d0-b407-162c9418232a', 4064, CAST(N'2024-12-31T15:57:50.8811142' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8177, N'f744397e-cc0c-45d0-b407-162c9418232a', 1035, CAST(N'2024-12-31T15:58:01.2828733' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8178, N'f744397e-cc0c-45d0-b407-162c9418232a', 6072, CAST(N'2024-12-31T15:57:58.8631041' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8179, N'f744397e-cc0c-45d0-b407-162c9418232a', 3063, CAST(N'2024-12-31T15:58:11.5736910' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8180, N'f744397e-cc0c-45d0-b407-162c9418232a', 1034, CAST(N'2024-12-31T15:58:18.4554786' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8181, N'f744397e-cc0c-45d0-b407-162c9418232a', 6080, CAST(N'2024-12-31T15:58:36.7162985' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8182, N'f744397e-cc0c-45d0-b407-162c9418232a', 3063, CAST(N'2024-12-31T16:02:12.2909928' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8183, N'f744397e-cc0c-45d0-b407-162c9418232a', 1055, CAST(N'2024-12-31T16:02:20.9375092' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8184, N'f744397e-cc0c-45d0-b407-162c9418232a', 1034, CAST(N'2024-12-31T16:02:19.1477606' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8185, N'f744397e-cc0c-45d0-b407-162c9418232a', 1031, CAST(N'2024-12-31T16:02:16.9411242' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8186, N'f744397e-cc0c-45d0-b407-162c9418232a', 6078, CAST(N'2024-12-31T16:08:30.7431639' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8187, N'f744397e-cc0c-45d0-b407-162c9418232a', 1035, CAST(N'2024-12-31T16:04:29.5570376' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8188, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 6069, CAST(N'2024-12-31T18:40:38.2767468' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8189, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 6089, CAST(N'2024-12-31T18:40:41.6282104' AS DateTime2))
GO
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8190, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 6088, CAST(N'2024-12-31T18:40:44.7699375' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8191, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 4064, CAST(N'2024-12-31T19:44:00.3221286' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8192, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 6088, CAST(N'2024-12-31T19:44:05.3671143' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8193, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 6079, CAST(N'2024-12-31T19:44:09.7224642' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8194, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 6077, CAST(N'2024-12-31T19:44:13.0245165' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8195, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 6068, CAST(N'2024-12-31T19:44:16.3823542' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8196, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 1035, CAST(N'2024-12-31T19:44:21.0248002' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8197, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 6096, CAST(N'2024-12-31T19:56:20.0067568' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8198, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 6094, CAST(N'2024-12-31T19:56:24.6322665' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8199, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 6097, CAST(N'2025-01-01T14:42:05.6124517' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8200, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 6086, CAST(N'2025-01-01T14:44:01.7407783' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8201, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 6085, CAST(N'2025-01-01T14:43:56.3427704' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8202, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 6084, CAST(N'2025-01-01T14:44:21.5786978' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (8203, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 6068, CAST(N'2025-01-01T14:44:37.0736759' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (9120, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6096, CAST(N'2025-01-06T17:52:14.2331391' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (9121, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6073, CAST(N'2025-01-06T17:52:23.6238781' AS DateTime2))
INSERT [dbo].[Histories] ([Id], [UserId], [VideoId], [WatchedAt]) VALUES (9122, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056, CAST(N'2025-01-06T18:08:20.4098114' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Histories] OFF
GO
SET IDENTITY_INSERT [dbo].[Notifications] ON 

INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (10, N'aaaaa replied to your comment', N'"I completely agree with you"', CAST(N'2024-12-10T03:42:21.7077313' AS DateTime2), 1, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2, NULL, 3159)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (11, N'aaaaa replied to your comment', N'"You are right!"', CAST(N'2024-12-10T03:42:29.0023000' AS DateTime2), 1, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2, NULL, 3160)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (19, N'New Video from asdada2', N'Check out our latest video "sample_1280x720_surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing surfing"!', CAST(N'2024-12-11T11:09:45.4310723' AS DateTime2), 0, N'eb81d759-e897-4178-8a40-5990eb19b39e', 1, 2056, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (1004, N'New Video from asdada2', N'Check out our latest video "A very happy wolf"!', CAST(N'2024-12-13T22:23:18.1890398' AS DateTime2), 0, N'eb81d759-e897-4178-8a40-5990eb19b39e', 1, 4064, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3004, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #1 - Introduction"!', CAST(N'2024-12-30T22:56:08.1824169' AS DateTime2), 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1, 6063, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3005, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #1 - Introduction"!', CAST(N'2024-12-30T22:56:08.1947723' AS DateTime2), 0, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 1, 6063, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3008, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #10 - Forms & Actions"!', CAST(N'2024-12-30T23:28:48.9905780' AS DateTime2), 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1, 6072, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3009, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #10 - Forms & Actions"!', CAST(N'2024-12-30T23:28:48.9944837' AS DateTime2), 0, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 1, 6072, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3010, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #9 - Making Breadcrumbs (useLocation hook)"!', CAST(N'2024-12-30T23:28:49.9736742' AS DateTime2), 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1, 6071, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3011, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #9 - Making Breadcrumbs (useLocation hook)"!', CAST(N'2024-12-30T23:28:49.9775604' AS DateTime2), 0, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 1, 6071, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3012, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #8 - Error Elements"!', CAST(N'2024-12-30T23:28:50.5687663' AS DateTime2), 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1, 6070, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3013, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #8 - Error Elements"!', CAST(N'2024-12-30T23:28:50.5729458' AS DateTime2), 0, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 1, 6070, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3014, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #7 - Route Parameters"!', CAST(N'2024-12-30T23:28:51.1563933' AS DateTime2), 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1, 6069, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3015, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #7 - Route Parameters"!', CAST(N'2024-12-30T23:28:51.1603724' AS DateTime2), 0, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 1, 6069, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3016, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #6 - Loaders"!', CAST(N'2024-12-30T23:28:51.7419062' AS DateTime2), 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1, 6068, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3017, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #6 - Loaders"!', CAST(N'2024-12-30T23:28:51.7458459' AS DateTime2), 0, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 1, 6068, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3018, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #5 - Custom 404 Page"!', CAST(N'2024-12-30T23:28:52.7369308' AS DateTime2), 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1, 6067, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3019, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #5 - Custom 404 Page"!', CAST(N'2024-12-30T23:28:52.7411663' AS DateTime2), 0, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 1, 6067, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3020, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #4 - Nested Routes & Layouts"!', CAST(N'2024-12-30T23:28:53.2666650' AS DateTime2), 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1, 6066, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3021, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #4 - Nested Routes & Layouts"!', CAST(N'2024-12-30T23:28:53.2707252' AS DateTime2), 0, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 1, 6066, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3022, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #3 - Router Provider, createBrowserRouter & Outlet"!', CAST(N'2024-12-30T23:28:53.8240979' AS DateTime2), 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1, 6065, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3023, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #3 - Router Provider, createBrowserRouter & Outlet"!', CAST(N'2024-12-30T23:28:53.8282752' AS DateTime2), 0, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 1, 6065, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3024, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #2 - React Router Basics"!', CAST(N'2024-12-30T23:28:54.8520141' AS DateTime2), 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1, 6064, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3025, N'New Video from  Net Ninja', N'Check out our latest video "React Router in Depth #2 - React Router Basics"!', CAST(N'2024-12-30T23:28:54.8559249' AS DateTime2), 0, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 1, 6064, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3028, N'New Video from Andrew Tsyselskyi', N'Check out our latest video "A very happy wolf part 2"!', CAST(N'2024-12-31T00:04:24.2371987' AS DateTime2), 0, N'eb81d759-e897-4178-8a40-5990eb19b39e', 1, 3063, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3031, N'New Video from Andrew Tsyselskyi', N'Check out our latest video "F-22 Raptor the fighter plane"!', CAST(N'2024-12-31T00:55:08.0735159' AS DateTime2), 0, N'eb81d759-e897-4178-8a40-5990eb19b39e', 1, 1035, NULL)
INSERT [dbo].[Notifications] ([Id], [Title], [Message], [CreatedAt], [IsRead], [UserId], [NotificationTypeId], [VideoId], [CommentId]) VALUES (3032, N'New Video from Net Ninja', N'Check out our latest video "React Router in Depth #11 - Navigate Component"!', CAST(N'2024-12-31T19:38:34.3432934' AS DateTime2), 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1, 6073, NULL)
SET IDENTITY_INSERT [dbo].[Notifications] OFF
GO
SET IDENTITY_INSERT [dbo].[NotificationTypes] ON 

INSERT [dbo].[NotificationTypes] ([Id], [Name]) VALUES (1, N'NewVideo')
INSERT [dbo].[NotificationTypes] ([Id], [Name]) VALUES (2, N'CommentReply')
SET IDENTITY_INSERT [dbo].[NotificationTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[Playlists] ON 

INSERT [dbo].[Playlists] ([Id], [Title], [Description], [IsPublic], [CreatedAt], [UserId], [LastUpdated]) VALUES (1002, N'My favourite playlist', N'dada12', 1, CAST(N'2024-11-25T14:11:24.9760489' AS DateTime2), N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', CAST(N'2024-12-31T00:04:45.4764383' AS DateTime2))
INSERT [dbo].[Playlists] ([Id], [Title], [Description], [IsPublic], [CreatedAt], [UserId], [LastUpdated]) VALUES (1004, N'My playlist number 1', N'MY5555555', 1, CAST(N'2024-11-29T17:02:44.0019705' AS DateTime2), N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', CAST(N'2024-12-31T00:04:30.9989986' AS DateTime2))
INSERT [dbo].[Playlists] ([Id], [Title], [Description], [IsPublic], [CreatedAt], [UserId], [LastUpdated]) VALUES (2005, N'The Best Pranks', N'', 1, CAST(N'2024-12-11T05:09:05.3196303' AS DateTime2), N'77f01ccf-396e-418d-ba05-69ad7f7ee736', CAST(N'2024-12-31T17:13:14.1325312' AS DateTime2))
INSERT [dbo].[Playlists] ([Id], [Title], [Description], [IsPublic], [CreatedAt], [UserId], [LastUpdated]) VALUES (3005, N'React Router in Depth', N'', 1, CAST(N'2024-12-30T23:31:23.3340970' AS DateTime2), N'eb81d759-e897-4178-8a40-5990eb19b39e', CAST(N'2024-12-30T23:35:48.1063212' AS DateTime2))
INSERT [dbo].[Playlists] ([Id], [Title], [Description], [IsPublic], [CreatedAt], [UserId], [LastUpdated]) VALUES (3006, N'The Best Cooking Videos', N'', 1, CAST(N'2024-12-31T16:03:51.9877997' AS DateTime2), N'f744397e-cc0c-45d0-b407-162c9418232a', CAST(N'2024-12-31T16:04:04.2713825' AS DateTime2))
INSERT [dbo].[Playlists] ([Id], [Title], [Description], [IsPublic], [CreatedAt], [UserId], [LastUpdated]) VALUES (3007, N'Meat playlist', N'', 1, CAST(N'2024-12-31T19:55:23.2797602' AS DateTime2), N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', CAST(N'2024-12-31T19:55:52.8577143' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Playlists] OFF
GO
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (1002, 1031)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (1002, 1034)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (1002, 1055)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (1004, 2056)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (1002, 3063)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (1004, 4064)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3005, 6063)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3005, 6064)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3005, 6065)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3005, 6066)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3005, 6067)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3005, 6068)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3005, 6069)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3005, 6070)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3005, 6071)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3005, 6072)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3005, 6073)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3006, 6078)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3006, 6081)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (2005, 6082)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (2005, 6084)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (2005, 6085)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (2005, 6086)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3007, 6094)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3007, 6095)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3007, 6096)
INSERT [dbo].[PlaylistsVideos] ([PlaylistId], [VideoId]) VALUES (3007, 6097)
GO
SET IDENTITY_INSERT [dbo].[Subscriptions] ON 

INSERT [dbo].[Subscriptions] ([Id], [UserId], [ChannelId], [SubscribedAt], [ReceiveNotifications]) VALUES (2120, N'eb81d759-e897-4178-8a40-5990eb19b39e', 1005, CAST(N'2024-11-30T17:15:49.6892288' AS DateTime2), 1)
INSERT [dbo].[Subscriptions] ([Id], [UserId], [ChannelId], [SubscribedAt], [ReceiveNotifications]) VALUES (7140, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1006, CAST(N'2024-12-09T21:26:05.9550775' AS DateTime2), 1)
INSERT [dbo].[Subscriptions] ([Id], [UserId], [ChannelId], [SubscribedAt], [ReceiveNotifications]) VALUES (9142, N'f744397e-cc0c-45d0-b407-162c9418232a', 1006, CAST(N'2024-12-31T19:38:17.3563762' AS DateTime2), 0)
INSERT [dbo].[Subscriptions] ([Id], [UserId], [ChannelId], [SubscribedAt], [ReceiveNotifications]) VALUES (9143, N'77f01ccf-396e-418d-ba05-69ad7f7ee736', 1006, CAST(N'2024-12-31T19:40:48.7698232' AS DateTime2), 1)
SET IDENTITY_INSERT [dbo].[Subscriptions] OFF
GO
SET IDENTITY_INSERT [dbo].[Tags] ON 

INSERT [dbo].[Tags] ([Id], [Name]) VALUES (11, N'#freeurkraine')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (12, N'#freeurkraine1')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (13, N'#freeurkraine12')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (14, N'#freeurkraine123')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (15, N'#asdadsa')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (16, N'#asdadsa')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (17, N'#asdadsa')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (18, N'#asdadada')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (19, N'#asda')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (20, N'#sad')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (21, N'#sad11')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (22, N'#asd')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (23, N'#asda')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (24, N'#asdaa')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (25, N'#asdaaa')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (26, N'#asdaaaa')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (27, N'#asdaaaaa')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (28, N'#asdaaaaaa')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (1022, N'#ukraine')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (1023, N'#france')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (1024, N'#zxczczx')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (1025, N'#zxczc')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (1026, N'#ukraine')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (2026, N'#France')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (3026, N'#zcxxzcxz')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (3027, N'#zcxxzcxz1')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (4026, N'#France1')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (4027, N'#France12')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (5026, N'#asdsad')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (6026, N'#sds')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7026, N'#react')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7027, N'#webdev')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7028, N'#react')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7029, N'#webdev')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7030, N'#react')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7031, N'#webdev')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7032, N'#react')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7033, N'#webdev')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7034, N'#react')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7035, N'#webdev')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7036, N'#react')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7037, N'#webdev')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7038, N'#react')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7039, N'#webdev')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7040, N'#react')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7041, N'#webdev')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7042, N'#react')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7043, N'#webdev')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7044, N'#react')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7045, N'#webdev')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7046, N'#react')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7047, N'#webdev')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7048, N'#wolf')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7049, N'#wolf')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7050, N'#surfing')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7051, N'#ocean')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7052, N'#fish')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7053, N'#gaming')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7054, N'#fornight')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7055, N'#f22')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7056, N'#airforce')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7057, N'#fortnight')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7058, N'#games')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7059, N'#itstep')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7060, N'#lesson')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7061, N'#angular')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7062, N'#cook')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7063, N'#cook')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7064, N'#cook')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7065, N'#cook')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7066, N'#cook')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7067, N'#cook')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7068, N'#cook')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7069, N'#cook')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7070, N'#maxfosh')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7071, N'#uk')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7072, N'#uk')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7073, N'#maxfosh')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7074, N'#uk')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7075, N'#maxfosh')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7076, N'#maxfosh')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7077, N'#uk')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7078, N'#maxfosh')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7079, N'#uk')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7080, N'#maxfosh')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7081, N'#uk')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7082, N'#uk')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7083, N'#maxfosh')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7084, N'#maxfosh')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7085, N'#uk')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7086, N'#meat')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7087, N'#meat')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7088, N'#fish')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7089, N'#meat')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7090, N'#meat')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7091, N'#meat')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7092, N'#meat')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7093, N'#hotdog')
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7094, N'#meat')
GO
INSERT [dbo].[Tags] ([Id], [Name]) VALUES (7095, N'#meat')
SET IDENTITY_INSERT [dbo].[Tags] OFF
GO
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7059, 1031)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7060, 1031)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7061, 1031)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7057, 1034)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7058, 1034)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7055, 1035)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7056, 1035)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7053, 1055)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7054, 1055)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7051, 2055)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7052, 2055)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (2026, 2056)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7050, 2056)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7049, 3063)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7048, 4064)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7026, 6063)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7027, 6063)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7028, 6064)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7029, 6064)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7030, 6065)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7031, 6065)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7032, 6066)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7033, 6066)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7034, 6067)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7035, 6067)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7036, 6068)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7037, 6068)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7038, 6069)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7039, 6069)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7040, 6070)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7041, 6070)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7042, 6071)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7043, 6071)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7044, 6072)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7045, 6072)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7046, 6073)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7047, 6073)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7062, 6074)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7063, 6075)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7064, 6076)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7065, 6077)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7066, 6078)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7067, 6079)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7068, 6080)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7069, 6081)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7084, 6082)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7085, 6082)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7082, 6083)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7083, 6083)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7080, 6084)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7081, 6084)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7078, 6085)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7079, 6085)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7076, 6086)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7077, 6086)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7074, 6087)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7075, 6087)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7072, 6088)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7073, 6088)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7070, 6089)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7071, 6089)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7086, 6090)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7087, 6091)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7088, 6091)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7089, 6092)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7090, 6093)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7091, 6094)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7092, 6095)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7093, 6095)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7094, 6096)
INSERT [dbo].[TagsVideos] ([TagId], [VideoId]) VALUES (7095, 6097)
GO
SET IDENTITY_INSERT [dbo].[Videos] ON 

INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (1031, N'Angular и React-20241005_090135-Meeting Recording', CAST(N'2024-11-19T23:41:05.5380752' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/1031.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/1031.jpeg', NULL, CAST(N'02:48:40.6400000' AS Time), 1, 1005, N'.mp4', 1, 632854344)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (1034, N'Fortnight videoplayback part 2', CAST(N'2024-11-19T23:44:46.7484276' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/1034.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/1034.jpeg', N'', CAST(N'00:00:29.0713830' AS Time), 1, 1005, N'.mp4', 1, 2498882)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (1035, N'F-22 Raptor the fighter plane', CAST(N'2024-11-19T23:46:41.3522356' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/1035.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/1035.jpeg', N'Some facts about F-22 Raptor', CAST(N'00:01:07.8951470' AS Time), 1, 1005, N'.mp4', 1, 6504229)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (1055, N'Fortnight videoplayback part 1', CAST(N'2024-11-25T19:20:34.1471359' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/1055.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/1055.jpeg', N'Hi everyone! Today in this video we will play some Fortnight', CAST(N'00:00:29.0713830' AS Time), 1, 1005, N'.mp4', 1, 2498882)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (2055, N'Ocean with fishes', CAST(N'2024-11-27T22:18:40.2752086' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/2055.mkv', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/2055.jpeg', N'Some description', CAST(N'00:00:46.6160000' AS Time), 1, 1005, N'.mkv', 1, 17344861)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (2056, N'Just some surfing', CAST(N'2024-11-27T22:18:56.1070515' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/2056.avi', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/2056.jpeg', N'My description', CAST(N'00:03:03.1680000' AS Time), 1, 1005, N'.avi', 1, 37473285)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (3063, N'A very happy wolf part 2', CAST(N'2024-12-07T09:24:46.0874499' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/3063.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/3063.jpeg', N'Some description', CAST(N'00:01:07.3146490' AS Time), 1, 1005, N'.mp4', 1, 17231552)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (4064, N'A very happy wolf part 1', CAST(N'2024-12-11T05:39:05.7041514' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/4064.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/4064.jpeg', N'Some description', CAST(N'00:01:07.3146490' AS Time), 1, 1005, N'.mp4', 1, 17231552)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6063, N'React Router in Depth #1 - Introduction', CAST(N'2024-12-30T22:54:36.1644511' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6063.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6063.jpeg', N'In this React Router tutorial series, we''ll take a close look at some of the different ways we can work with it - including etting up different pages & layouts, nesting routes, using loaders & actions - and much more.

⭐⭐ Watch the whole course now (without ads) on Net Ninja Pro:
https://netninja.dev/p/react-router-i...

🐱‍💻 Access the course files on GitHub:
https://github.com/iamshaunjp/react-r...

🐱‍💻 React Tutorial:
On Net Ninja Pro - https://netninja.dev/p/build-websites...
On YouTube -    • Full React Tutorial #1 - Introduction  

🐱‍💻 React Router Docs - https://reactrouter.com/en/main
🐱‍💻 VS Code - https://code.visualstudio.com/', CAST(N'00:07:10.6372790' AS Time), 1, 1006, N'.mp4', 1, 20450337)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6064, N'React Router in Depth #2 - React Router Basics', CAST(N'2024-12-30T23:14:49.3217352' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6064.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6064.jpeg', N'In this lesson we''ll learn the basics of the React Router - how to set up routes & pages and how to use Link components to link between those pages.

⭐⭐ Watch the whole course now (without ads) on Net Ninja Pro:
https://netninja.dev/p/react-router-i...

🐱‍💻 Access the course files on GitHub:
https://github.com/iamshaunjp/react-r...

🐱‍💻 React Tutorial:
On Net Ninja Pro - https://netninja.dev/p/build-websites...
On YouTube -    • Full React Tutorial #1 - Introduction  

🐱‍💻 React Router Docs - https://reactrouter.com/en/main
🐱‍💻 VS Code - https://code.visualstudio.com/', CAST(N'00:12:50.8096150' AS Time), 1, 1006, N'.mp4', 1, 35181220)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6065, N'React Router in Depth #3 - Router Provider, createBrowserRouter & Outlet', CAST(N'2024-12-30T23:15:15.9340311' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6065.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6065.jpeg', N'In this React Router tutorial, you''ll learn how to create a browser router th "new way", using the createBrowserRouter function. You''ll also learn about the Router Provider & Outlet components.

⭐⭐ Watch the whole course now (without ads) on Net Ninja Pro:
https://netninja.dev/p/react-router-i...

🐱‍💻 Access the course files on GitHub:
https://github.com/iamshaunjp/react-r...

🐱‍💻 React Tutorial:
On Net Ninja Pro - https://netninja.dev/p/build-websites...
On YouTube -    • Full React Tutorial #1 - Introduction  

🐱‍💻 React Router Docs - https://reactrouter.com/en/main
🐱‍💻 VS Code - https://code.visualstudio.com/', CAST(N'00:10:21.4124260' AS Time), 1, 1006, N'.mp4', 1, 28001695)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6066, N'React Router in Depth #4 - Nested Routes & Layouts', CAST(N'2024-12-30T23:15:24.9365126' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6066.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6066.jpeg', N'In this React Router tutorial, you''ll learn how to create nested routes and nested layouts.

⭐⭐ Watch the whole course now (without ads) on Net Ninja Pro:
https://netninja.dev/p/react-router-i...

🐱‍💻 Access the course files on GitHub:
https://github.com/iamshaunjp/react-r...

🐱‍💻 React Tutorial:
On Net Ninja Pro - https://netninja.dev/p/build-websites...
On YouTube -    • Full React Tutorial #1 - Introduction  

🐱‍💻 React Router Docs - https://reactrouter.com/en/main
🐱‍💻 VS Code - https://code.visualstudio.com/', CAST(N'00:11:21.0877110' AS Time), 1, 1006, N'.mp4', 1, 34590032)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6067, N'React Router in Depth #5 - Custom 404 Page', CAST(N'2024-12-30T23:15:31.6049625' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6067.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6067.jpeg', N'⭐⭐ Watch the whole course now (without ads) on Net Ninja Pro:
https://netninja.dev/p/react-router-i...

🐱‍💻 Access the course files on GitHub:
https://github.com/iamshaunjp/react-r...

🐱‍💻 React Tutorial:
On Net Ninja Pro - https://netninja.dev/p/build-websites...
On YouTube -    • Full React Tutorial #1 - Introduction  

🐱‍💻 React Router Docs - https://reactrouter.com/en/main
🐱‍💻 VS Code - https://code.visualstudio.com/', CAST(N'00:03:42.3310670' AS Time), 1, 1006, N'.mp4', 1, 11780216)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6068, N'React Router in Depth #6 - Loaders', CAST(N'2024-12-30T23:15:38.9897864' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6068.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6068.jpeg', N'In this React Router tutorial you''ll learn about loaders - a newer way to fetch component data.

⭐⭐ Watch the whole course now (without ads) on Net Ninja Pro:
https://netninja.dev/p/react-router-i...

🐱‍💻 Access the course files on GitHub:
https://github.com/iamshaunjp/react-r...

🐱‍💻 React Tutorial:
On Net Ninja Pro - https://netninja.dev/p/build-websites...
On YouTube -    • Full React Tutorial #1 - Introduction  

🐱‍💻 React Router Docs - https://reactrouter.com/en/main
🐱‍💻 VS Code - https://code.visualstudio.com/', CAST(N'00:12:06.4130610' AS Time), 1, 1006, N'.mp4', 1, 32925409)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6069, N'React Router in Depth #7 - Route Parameters', CAST(N'2024-12-30T23:15:47.2233178' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6069.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6069.jpeg', N'⭐⭐ Watch the whole course now (without ads) on Net Ninja Pro:
https://netninja.dev/p/react-router-i...

🐱‍💻 Access the course files on GitHub:
https://github.com/iamshaunjp/react-r...

🐱‍💻 React Tutorial:
On Net Ninja Pro - https://netninja.dev/p/build-websites...
On YouTube -    • Full React Tutorial #1 - Introduction  

🐱‍💻 React Router Docs - https://reactrouter.com/en/main
🐱‍💻 VS Code - https://code.visualstudio.com/', CAST(N'00:10:00.3983670' AS Time), 1, 1006, N'.mp4', 1, 28349696)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6070, N'React Router in Depth #8 - Error Elements', CAST(N'2024-12-30T23:15:58.3832078' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6070.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6070.jpeg', N'⭐⭐ Watch the whole course now (without ads) on Net Ninja Pro:
https://netninja.dev/p/react-router-i...

🐱‍💻 Access the course files on GitHub:
https://github.com/iamshaunjp/react-r...

🐱‍💻 React Tutorial:
On Net Ninja Pro - https://netninja.dev/p/build-websites...
On YouTube -    • Full React Tutorial #1 - Introduction  

🐱‍💻 React Router Docs - https://reactrouter.com/en/main
🐱‍💻 VS Code - https://code.visualstudio.com/', CAST(N'00:09:02.6039000' AS Time), 1, 1006, N'.mp4', 1, 23543824)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6071, N'React Router in Depth #9 - Making Breadcrumbs (useLocation hook)', CAST(N'2024-12-30T23:16:09.9504693' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6071.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6071.jpeg', N'In this React Router tutorial, you''ll learn how to put the useLocation hook to ood use, to make a Breadcrumbs component.

⭐⭐ Watch the whole course now (without ads) on Net Ninja Pro:
https://netninja.dev/p/react-router-i...

🐱‍💻 Access the course files on GitHub:
https://github.com/iamshaunjp/react-r...

🐱‍💻 React Tutorial:
On Net Ninja Pro - https://netninja.dev/p/build-websites...
On YouTube -    • Full React Tutorial #1 - Introduction  

🐱‍💻 React Router Docs - https://reactrouter.com/en/main
🐱‍💻 VS Code - https://code.visualstudio.com/', CAST(N'00:10:51.4358330' AS Time), 1, 1006, N'.mp4', 1, 29466713)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6072, N'React Router in Depth #10 - Forms & Actions', CAST(N'2024-12-30T23:16:15.5736440' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6072.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6072.jpeg', N'In this React Router tutorial you''ll learn about the new Form component and how to use actions to react to form submissions.

⭐⭐ Watch the whole course now (without ads) on Net Ninja Pro:
https://netninja.dev/p/react-router-i...

🐱‍💻 Access the course files on GitHub:
https://github.com/iamshaunjp/react-r...

🐱‍💻 React Tutorial:
On Net Ninja Pro - https://netninja.dev/p/build-websites...
On YouTube -    • Full React Tutorial #1 - Introduction  

🐱‍💻 React Router Docs - https://reactrouter.com/en/main
🐱‍💻 VS Code - https://code.visualstudio.com/', CAST(N'00:11:16.2347440' AS Time), 1, 1006, N'.mp4', 1, 34328652)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6073, N'React Router in Depth #11 - Navigate Component', CAST(N'2024-12-30T23:16:20.8334816' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6073.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6073.jpeg', N'Hey all, in this React Router tutorial you''ll learn how to use the Navigate component to redirect users to other pages.

⭐⭐ Watch the whole course now (without ads) on Net Ninja Pro:
https://netninja.dev/p/react-router-i...

🐱‍💻 Access the course files on GitHub:
https://github.com/iamshaunjp/react-r...

🐱‍💻 React Tutorial:
On Net Ninja Pro - https://netninja.dev/p/build-websites...
On YouTube -    • Full React Tutorial #1 - Introduction  

🐱‍💻 React Router Docs - https://reactrouter.com/en/main
🐱‍💻 VS Code - https://code.visualstudio.com/
', CAST(N'00:05:42.0995919' AS Time), 1, 1006, N'.mp4', 1, 19989711)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6074, N'Can I Turn Ryan Trahan Into A MasterChef', CAST(N'2024-12-31T15:43:18.3070946' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6074.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6074.jpeg', N'Comment below to give ‪@ryan‬ a rating out of 10!

SUBSCRIBE FOR A WORLD RECORD!

Huge thanks to the following:
• If you hadn''t already noticed...we have a new studio kitchen! Shoutout to Emily''s Interiors for designing our new kitchen and RemodelWerks for building it. Visit their website for more information: www.remodelwerksllc.com
• Zack Jonas for making Ryan’s awesome knife: www.jonasblade.com
• The Wagyu Shop (www.wagyushop.com) and San Francisco Meat Co. (www.sfmeatco.com) for the steak
• La Tienda for the prosciutto: www.latienda.com', CAST(N'00:50:48' AS Time), 1, 1007, N'.mp4', 1, 284542388)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6075, N'Cooking Challenge vs Logan Paul', CAST(N'2024-12-31T15:44:46.9747918' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6075.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6075.jpeg', N'Comment below who you think won!

Thanks to chef ‪@loganpaulvlogs‬ 

SUBSCRIBE FOR A WORLD RECORD!', CAST(N'00:19:40.6185940' AS Time), 1, 1007, N'.mp4', 1, 126400012)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6076, N'I Trained A Rat To Cook Ratatouille', CAST(N'2024-12-31T15:45:16.9963706' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6076.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6076.jpeg', N'Anyone can cook :) 

This is one of my favorite videos we''ve ever made

SUBSCRIBE FOR A WORLD RECORD!', CAST(N'00:12:44' AS Time), 1, 1007, N'.mp4', 1, 58215975)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6077, N'I Cooked Against Robots', CAST(N'2024-12-31T15:45:49.3897435' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6077.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6077.jpeg', N'The last robot was CRAZY...

Enjoy 20% off the NEW Happy Potato merch drop from November 9th to 14th! First come, first serve, while supplies last. Discount is applied at check-out solely for select products and purchases driven by the YouTube Shopping buttons below.

Thanks to The Wagyu Shop (www.wagyushop.com) and San Francisco Meat Co. (www.sfmeatco.com) for the steak! 

Thanks to ‪@MarkRober‬ and ‪@HarrisonWebb97‬ for helping make this video with me! And, a big thanks to the robots (and their teams) too.

Neo
www.1x.tech

Moley
www.moley.com

xRobotics
www.xRobotics.io

Neo is a REAL robot! As a disclaimer, it can operate and do many actions on its own but some of the specialized actions in this video were ''remotely controlled'' by a robot using a VR headset (since it is not yet specifically trained to cook), and the robot''s voice works using Open AI software. ', CAST(N'00:16:56.2213150' AS Time), 1, 1007, N'.mp4', 1, 90733668)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6078, N'I Hatched A Chicken, Then Cooked It', CAST(N'2024-12-31T15:46:19.5671588' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6078.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6078.jpeg', N'Rest in peace, Bob. 

SUBSCRIBE TO CATCH GORDON RAMSAY!', CAST(N'00:15:23.8291160' AS Time), 1, 1007, N'.mp4', 1, 106933940)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6079, N'World''s Spiciest Cooking Challenge', CAST(N'2024-12-31T15:46:43.6249505' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6079.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6079.jpeg', N'Loser has to eat the world''s spiciest pepper. Watch to the end to see who it is!

Thanks to Sean + Esther + ‪@FirstWeFeast‬ for making this happen :)

And thanks to all the following for helping out:
• Hexclad for the pots and pans
• Smokin'' Ed for the pepper X
• Anthony Christian for being our EMT
• Mike''s Hot Honey for the hot honey

SUBSCRIBE TO CATCH GORDON RAMSAY ', CAST(N'00:25:37.4628570' AS Time), 1, 1007, N'.mp4', 1, 155294960)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6080, N'I Ate The World’s Rarest Foods', CAST(N'2024-12-31T15:47:08.5073019' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6080.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6080.jpeg', N'Check out my newest merch drop! Every piece of clothing feeds people in need: https://www.happypotato.com

SUBSCRIBE FOR $10,000!

Thank you to the following for the help!
Amazon Air Water for the $350 water (www.oamazonwater.com)
Giusti for the $1000 Giusti 100 Reserve (www.giusti.com)
Elvish Honey for the $1500 honey (www.elvishoney.com)
La Tienda for the $2000 Cinco Jotas jamón ibérico (www.latienda.com)
San Francisco Meat Co. for the $5000 A5 wagyu (www.sfmeatco.com)
Marky''s for the $15,000 Almas caviar (www.markys.com)', CAST(N'00:23:59.5210890' AS Time), 1, 1007, N'.mp4', 1, 105359526)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6081, N'I Cooked YouTubers Their Favorite Foods', CAST(N'2024-12-31T15:47:29.8487298' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6081.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6081.jpeg', N'You won''t believe what the last creator wanted to eat...

Thank you to the following for the help!
The Wagyu Shop for the wagyu jenga! (www.wagyushop.com)
Chef Gonzo Jimenez for helping with the chocolate sculpture! (@chef.gonzo)
New City Microcreamery for providing the cake batter ice cream!
Sally + Smiles By The Mile ice cream truck! (www.smilesbythemileicecream.com)
Nathan Wyburn for helping with the noodle art!
Autec for providing the sushi robots! (www.sushimachines.com)
311 Omakase for hosting us to make sushi!
Frank Pepe''s for making pizza with us!

SUBSCRIBE FOR A PAIR OF APPLE VISION PROS!

Thanks to all of these YouTubers for letting me cook for them :)  @MrBeast   @ksi   @GameTheory   @loganpaulvlogs   @mkbhd   @rug   @ryan   @KaiCenat   @Jesser  ', CAST(N'00:16:18' AS Time), 1, 1007, N'.mp4', 1, 104153424)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6082, N'I Hired A Church Choir To Start Offensive Football Chants', CAST(N'2024-12-31T16:37:35.3472493' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6082.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6082.jpeg', N'go check out shopify - https://www.shopify.co.uk/maxfosh', CAST(N'00:12:41.0340140' AS Time), 1, 4009, N'.mp4', 1, 82386954)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6083, N'I Hired A F1 Driver To Beat My Friends At Go Karting', CAST(N'2024-12-31T16:37:51.2419543' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6083.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6083.jpeg', N'Get AirUp for your friends and fam this Christmas with their fantastic bundle deals! - https://airup.link/maxgifting', CAST(N'00:14:23.5268930' AS Time), 1, 4009, N'.mp4', 1, 108492123)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6084, N'I Broke Into YouTubers Houses To Roast Them', CAST(N'2024-12-31T16:39:07.8761230' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6084.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6084.jpeg', N'Sit in the front row and I''ll roast you too! https://www.maxfosh.co', CAST(N'00:12:54.4087110' AS Time), 1, 4009, N'.mp4', 1, 69898754)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6085, N'I Made A Sign More Famous Than The Hollywood Sign', CAST(N'2024-12-31T16:39:23.9332963' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6085.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6085.jpeg', N'If you wanna hear all about these antics, come to my live show! www.maxfosh.co', CAST(N'00:08:41.1718820' AS Time), 1, 4009, N'.mp4', 1, 50656316)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6086, N'I Got Revenge On My Best Mate…By Performing To His Mum', CAST(N'2024-12-31T16:39:32.6701846' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6086.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6086.jpeg', N'Get tickets to LOOPHOLE now! - https://www.maxfosh.co', CAST(N'00:10:05.2281220' AS Time), 1, 4009, N'.mp4', 1, 65506277)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6087, N'Professional Eater vs. Grizzly Bear_ Hot Dog Eating Contest', CAST(N'2024-12-31T16:39:41.8600999' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6087.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6087.jpeg', N'Get your AirUp now! Got to https://Airup.link/fosh and use code FOSH10 for 10% off', CAST(N'00:08:02.9518370' AS Time), 1, 4009, N'.mp4', 1, 63696633)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6088, N'I Tricked America That I Was A British Royal', CAST(N'2024-12-31T16:39:53.7558285' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6088.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6088.jpeg', N'Go to http://hostinger.com/maxfosh and use code MAXFOSH for 10% off your own website!', CAST(N'00:11:25.3601810' AS Time), 1, 4009, N'.mp4', 1, 76359131)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6089, N'responding to the allegations', CAST(N'2024-12-31T16:40:02.0364327' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6089.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6089.jpeg', N'more responding to allegations at https://www.maxfosh.co', CAST(N'00:02:22.9420440' AS Time), 1, 4009, N'.mp4', 1, 8413360)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6090, N'I Cooked EVERY Exotic Egg', CAST(N'2024-12-31T19:47:32.3087313' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6090.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6090.jpeg', N'From sturgeon, to snake, to balut, to ostrich and what might just be the rarest egg in the world... today we''re going on a crazy egg journey while we potentially set a new world record!

Check out MaxJerky here! https://www.maxjerky.com
—————
SUBSCRIBE and RING THE BELL to get notified when I post a video!

PLATFORMS
-Snapchat: https://www.snapchat.com/add/maxtheme...
-Instagram:   / maxthemeatguy  
-TikTok:   / maxthemeatguy  
-Facebook:   / maxthemeatguy  
-YouTube:    / maxthemeatguy  

DISCOUNTS
https://www.beacons.ai/maxthemeatguy

WEBSITE
https://www.maxthemeatguy.com/

FILMED BY
Sophia Greb    / sophiagreb  ', CAST(N'00:22:30.4725670' AS Time), 1, 1010, N'.mp4', 1, 86664366)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6091, N'Can Wasabi Dry Age Save the World’s WORST Tuna', CAST(N'2024-12-31T19:47:47.5900662' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6091.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6091.jpeg', N'Don’t miss out on all the action this week at DraftKings! Download the DraftKings app today! Sign-up using https://dkng.co/max100 or through my promo code MAX100

Can wasabi and dry aging make the worst tasting tuna taste... good? Today we are putting this to the test. The results shocked us all!

Gambling problem? Call 1-800-GAMBLER (MI/NJ/PA/WV).
Help is available for problem gambling, call (888) 789-7777 or visit ccpg.org (CT). 
21+. Physically present in CT/MI/NJ/PA/WV only. Void in ONT. Eligibility restrictions apply. 1 per new Casino customer. Min. $10 in wagers req. Max. $100 issued in Casino Credits for select games that are non-withdrawable and expire in 7 days (168 hours). Terms: casino.draftkings.com/promos. Ends 1/5/25 at 11:59PM ET. Sponsored by DK.

Based on combined U.S. Gross Gaming Revenue for DraftKings Casino and Golden Nugget Online Gaming as outlined in Eilers & Krejcik Gaming U.S. Online Casino Monthly Monitor as of July 2024.
', CAST(N'00:16:44.7274380' AS Time), 1, 1010, N'.mp4', 1, 81075077)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6092, N'I Hosted 2024’s BIGGEST YouTube Food Award Show', CAST(N'2024-12-31T19:48:09.6695812' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6092.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6092.jpeg', N'I present to you… THE MEATIES! It has been a crazy culinary year on YouTube and today we are celebrating it all! From the juiciest shot, to the best Gordon Ramsay roast and finally the GREATEST video posted in 2024… who will take home the ultimate prize?', CAST(N'00:21:09.5278000' AS Time), 1, 1010, N'.mp4', 1, 99410203)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6093, N'I Soaked EVERY Meat in Syrup for 7 Days and Cooked Them', CAST(N'2024-12-31T19:48:28.5296661' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6093.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6093.jpeg', N'40% off MAXJERKY ends tomorrow night! Our half pound bags are the best value and perfect holiday gift… https://www.maxjerky.com/products/meg...

Using a technique from the beginning of time… we are on a mission to see if soaking meat in syrups, sugars and jams for 7 days then cooking it is worth the wait, or if it ends in complete disaster. This one gets sticky!
—————
SUBSCRIBE and RING THE BELL to get notified when I post a video!', CAST(N'00:20:55.0385490' AS Time), 1, 1010, N'.mp4', 1, 104711863)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6094, N'I Cooked a $1,000 Brisket For 3 Days', CAST(N'2024-12-31T19:48:40.8634313' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6094.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6094.jpeg', N'Cook a brisket to medium rare? It almost always will result in a tough, chewy and disaster of an end product. But what happens when we use meat science and 3 full days to cook it to medium rare? The results were shocking!

MaxJerky is 40% off until December 2nd! No code necessary: https://www.maxjerky.com 

Shout out to ‪@alpinebutcher‬ for the wagyu A5 brisket! Check them out here: https://alpinebutchershop.com/?ref=L9...

Thank you ‪@JoshsPremiumMeats‬ for the Australian Wagyu Brisket! 
—————
SUBSCRIBE and RING THE BELL to get notified when I post a video!', CAST(N'00:12:51.2972340' AS Time), 1, 1010, N'.mp4', 1, 54008317)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6095, N'I Made Hot Dogs From EVERY Animal', CAST(N'2024-12-31T19:48:49.6252714' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6095.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6095.jpeg', N'Thank you to Bombas for sponsoring this video! One Item Purchased = One Item Donated, so head to https://bombas.com/max and use code MAX20 at checkout for 20% off your first purchase.

#hotdog #dryagedmeat #lobster #camel #venison 
—————
SUBSCRIBE and RING THE BELL to get notified when I post a video!', CAST(N'00:20:40.1313380' AS Time), 1, 1010, N'.mp4', 1, 100615955)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6096, N'I Cooked Steak from EVERY State in America', CAST(N'2024-12-31T19:49:05.6342993' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6096.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6096.jpeg', N'Steak from ALL 50 STATES… you heard that right… today we are cooking a steak dish from each state in the United States. Fried steak, cheese steak, clam chowder topped steak, surf & turfs and more… This one gets crazy.

Use code STATE30 for 30% off MaxJerky! Valid until 12/8/24 at 11:59PM PST. https://www.maxjerky.com

Check out my 3 seasonings from McCormick! https://www.mccormick.com/grill-mates...', CAST(N'00:35:46.8705670' AS Time), 1, 1010, N'.mp4', 1, 163648508)
INSERT [dbo].[Videos] ([Id], [Title], [PublicationDate], [Url], [PreviewUrl], [Description], [Duration], [IsPublic], [ChannelId], [Format], [IsUploaded], [Size]) VALUES (6097, N'The World’s SPICIEST Dry Aged Steak (6M Scoville)', CAST(N'2024-12-31T19:49:29.2728606' AS DateTime2), N'https://mystoragets.blob.core.windows.net/vibeoserver/videos/6097.mp4', N'https://mystoragets.blob.core.windows.net/vibeoserver/previews/6097.jpeg', N'6 million scoville units worth of hot peppers, 10 pounds of honey and a piece of meat worth $1,000… this is our craziest dry age to date!

Check out MaxJerky here! https://www.maxjerky.com

Shout out to ‪@alpinebutcher‬ for the meat! Check them out here: https://alpinebutchershop.com/?ref=L9...
—————
SUBSCRIBE and RING THE BELL to get notified when I post a video!', CAST(N'00:14:47.0951470' AS Time), 0, 1010, N'.mp4', 1, 69586430)
SET IDENTITY_INSERT [dbo].[Videos] OFF
GO
SET IDENTITY_INSERT [dbo].[VideoStatuses] ON 

INSERT [dbo].[VideoStatuses] ([Id], [IsLiked], [IsFavorite], [WatchLater], [UserId], [VideoId]) VALUES (2, 1, 1, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2056)
INSERT [dbo].[VideoStatuses] ([Id], [IsLiked], [IsFavorite], [WatchLater], [UserId], [VideoId]) VALUES (4, 0, 0, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1034)
INSERT [dbo].[VideoStatuses] ([Id], [IsLiked], [IsFavorite], [WatchLater], [UserId], [VideoId]) VALUES (2002, 1, 1, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1031)
INSERT [dbo].[VideoStatuses] ([Id], [IsLiked], [IsFavorite], [WatchLater], [UserId], [VideoId]) VALUES (2010, NULL, 0, 1, N'cb57a1d3-304e-4b7b-897a-3d5e0c7d96e7', 3063)
INSERT [dbo].[VideoStatuses] ([Id], [IsLiked], [IsFavorite], [WatchLater], [UserId], [VideoId]) VALUES (2011, NULL, 0, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 3063)
INSERT [dbo].[VideoStatuses] ([Id], [IsLiked], [IsFavorite], [WatchLater], [UserId], [VideoId]) VALUES (2012, 1, 1, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 2055)
INSERT [dbo].[VideoStatuses] ([Id], [IsLiked], [IsFavorite], [WatchLater], [UserId], [VideoId]) VALUES (3003, 1, 1, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1055)
INSERT [dbo].[VideoStatuses] ([Id], [IsLiked], [IsFavorite], [WatchLater], [UserId], [VideoId]) VALUES (3004, 0, 0, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 1035)
INSERT [dbo].[VideoStatuses] ([Id], [IsLiked], [IsFavorite], [WatchLater], [UserId], [VideoId]) VALUES (3005, 1, 0, 0, N'eb81d759-e897-4178-8a40-5990eb19b39e', 2056)
INSERT [dbo].[VideoStatuses] ([Id], [IsLiked], [IsFavorite], [WatchLater], [UserId], [VideoId]) VALUES (5005, NULL, 0, 0, N'eb81d759-e897-4178-8a40-5990eb19b39e', 6070)
INSERT [dbo].[VideoStatuses] ([Id], [IsLiked], [IsFavorite], [WatchLater], [UserId], [VideoId]) VALUES (5006, NULL, 1, 1, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6073)
INSERT [dbo].[VideoStatuses] ([Id], [IsLiked], [IsFavorite], [WatchLater], [UserId], [VideoId]) VALUES (5007, NULL, 0, 0, N'39e0a996-1386-45d9-9492-4d4d44d0f2a3', 6067)
SET IDENTITY_INSERT [dbo].[VideoStatuses] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CategoriesVideos_VideoId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_CategoriesVideos_VideoId] ON [dbo].[CategoriesVideos]
(
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Channels_Handle]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Channels_Handle] ON [dbo].[Channels]
(
	[Handle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Channels_UserId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Channels_UserId] ON [dbo].[Channels]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Comments_ChannelId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_Comments_ChannelId] ON [dbo].[Comments]
(
	[ChannelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Comments_ParentCommentId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_Comments_ParentCommentId] ON [dbo].[Comments]
(
	[ParentCommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Comments_VideoId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_Comments_VideoId] ON [dbo].[Comments]
(
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CommentStatuses_CommentId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_CommentStatuses_CommentId] ON [dbo].[CommentStatuses]
(
	[CommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_CommentStatuses_UserId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_CommentStatuses_UserId] ON [dbo].[CommentStatuses]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Histories_UserId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_Histories_UserId] ON [dbo].[Histories]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Histories_VideoId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_Histories_VideoId] ON [dbo].[Histories]
(
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notifications_CommentId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_Notifications_CommentId] ON [dbo].[Notifications]
(
	[CommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notifications_NotificationTypeId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_Notifications_NotificationTypeId] ON [dbo].[Notifications]
(
	[NotificationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Notifications_UserId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_Notifications_UserId] ON [dbo].[Notifications]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Notifications_VideoId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_Notifications_VideoId] ON [dbo].[Notifications]
(
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Playlists_UserId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_Playlists_UserId] ON [dbo].[Playlists]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PlaylistsVideos_VideoId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_PlaylistsVideos_VideoId] ON [dbo].[PlaylistsVideos]
(
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Subscriptions_ChannelId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_Subscriptions_ChannelId] ON [dbo].[Subscriptions]
(
	[ChannelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Subscriptions_UserId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_Subscriptions_UserId] ON [dbo].[Subscriptions]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TagsVideos_VideoId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_TagsVideos_VideoId] ON [dbo].[TagsVideos]
(
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Videos_ChannelId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_Videos_ChannelId] ON [dbo].[Videos]
(
	[ChannelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_VideoStatuses_UserId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_VideoStatuses_UserId] ON [dbo].[VideoStatuses]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_VideoStatuses_VideoId]    Script Date: 1/8/2025 12:12:25 AM ******/
CREATE NONCLUSTERED INDEX [IX_VideoStatuses_VideoId] ON [dbo].[VideoStatuses]
(
	[VideoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Channels] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Channels] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Comments] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Histories] ADD  DEFAULT (getdate()) FOR [WatchedAt]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Notifications] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsRead]
GO
ALTER TABLE [dbo].[Playlists] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsPublic]
GO
ALTER TABLE [dbo].[Playlists] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Playlists] ADD  DEFAULT (getdate()) FOR [LastUpdated]
GO
ALTER TABLE [dbo].[Subscriptions] ADD  DEFAULT (getdate()) FOR [SubscribedAt]
GO
ALTER TABLE [dbo].[Subscriptions] ADD  DEFAULT (CONVERT([bit],(0))) FOR [ReceiveNotifications]
GO
ALTER TABLE [dbo].[Videos] ADD  DEFAULT (getdate()) FOR [PublicationDate]
GO
ALTER TABLE [dbo].[Videos] ADD  DEFAULT (CONVERT([bit],(1))) FOR [IsPublic]
GO
ALTER TABLE [dbo].[Videos] ADD  DEFAULT (N'') FOR [Format]
GO
ALTER TABLE [dbo].[Videos] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsUploaded]
GO
ALTER TABLE [dbo].[Videos] ADD  DEFAULT (CONVERT([bigint],(0))) FOR [Size]
GO
ALTER TABLE [dbo].[VideoStatuses] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsFavorite]
GO
ALTER TABLE [dbo].[VideoStatuses] ADD  DEFAULT (CONVERT([bit],(0))) FOR [WatchLater]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[CategoriesVideos]  WITH CHECK ADD  CONSTRAINT [FK_CategoriesVideos_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CategoriesVideos] CHECK CONSTRAINT [FK_CategoriesVideos_Categories_CategoryId]
GO
ALTER TABLE [dbo].[CategoriesVideos]  WITH CHECK ADD  CONSTRAINT [FK_CategoriesVideos_Videos_VideoId] FOREIGN KEY([VideoId])
REFERENCES [dbo].[Videos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CategoriesVideos] CHECK CONSTRAINT [FK_CategoriesVideos_Videos_VideoId]
GO
ALTER TABLE [dbo].[Channels]  WITH CHECK ADD  CONSTRAINT [FK_Channels_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Channels] CHECK CONSTRAINT [FK_Channels_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Channels_ChannelId] FOREIGN KEY([ChannelId])
REFERENCES [dbo].[Channels] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Channels_ChannelId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Comments_ParentCommentId] FOREIGN KEY([ParentCommentId])
REFERENCES [dbo].[Comments] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Comments_ParentCommentId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Videos_VideoId] FOREIGN KEY([VideoId])
REFERENCES [dbo].[Videos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Videos_VideoId]
GO
ALTER TABLE [dbo].[CommentStatuses]  WITH CHECK ADD  CONSTRAINT [FK_CommentStatuses_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CommentStatuses] CHECK CONSTRAINT [FK_CommentStatuses_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[CommentStatuses]  WITH CHECK ADD  CONSTRAINT [FK_CommentStatuses_Comments_CommentId] FOREIGN KEY([CommentId])
REFERENCES [dbo].[Comments] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CommentStatuses] CHECK CONSTRAINT [FK_CommentStatuses_Comments_CommentId]
GO
ALTER TABLE [dbo].[Histories]  WITH CHECK ADD  CONSTRAINT [FK_Histories_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Histories] CHECK CONSTRAINT [FK_Histories_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Histories]  WITH CHECK ADD  CONSTRAINT [FK_Histories_Videos_VideoId] FOREIGN KEY([VideoId])
REFERENCES [dbo].[Videos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Histories] CHECK CONSTRAINT [FK_Histories_Videos_VideoId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Comments_CommentId] FOREIGN KEY([CommentId])
REFERENCES [dbo].[Comments] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Comments_CommentId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_NotificationTypes_NotificationTypeId] FOREIGN KEY([NotificationTypeId])
REFERENCES [dbo].[NotificationTypes] ([Id])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_NotificationTypes_NotificationTypeId]
GO
ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_Notifications_Videos_VideoId] FOREIGN KEY([VideoId])
REFERENCES [dbo].[Videos] ([Id])
GO
ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_Notifications_Videos_VideoId]
GO
ALTER TABLE [dbo].[Playlists]  WITH CHECK ADD  CONSTRAINT [FK_Playlists_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Playlists] CHECK CONSTRAINT [FK_Playlists_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[PlaylistsVideos]  WITH CHECK ADD  CONSTRAINT [FK_PlaylistsVideos_Playlists_PlaylistId] FOREIGN KEY([PlaylistId])
REFERENCES [dbo].[Playlists] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PlaylistsVideos] CHECK CONSTRAINT [FK_PlaylistsVideos_Playlists_PlaylistId]
GO
ALTER TABLE [dbo].[PlaylistsVideos]  WITH CHECK ADD  CONSTRAINT [FK_PlaylistsVideos_Videos_VideoId] FOREIGN KEY([VideoId])
REFERENCES [dbo].[Videos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PlaylistsVideos] CHECK CONSTRAINT [FK_PlaylistsVideos_Videos_VideoId]
GO
ALTER TABLE [dbo].[Subscriptions]  WITH CHECK ADD  CONSTRAINT [FK_Subscriptions_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_Subscriptions_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Subscriptions]  WITH CHECK ADD  CONSTRAINT [FK_Subscriptions_Channels_ChannelId] FOREIGN KEY([ChannelId])
REFERENCES [dbo].[Channels] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_Subscriptions_Channels_ChannelId]
GO
ALTER TABLE [dbo].[TagsVideos]  WITH CHECK ADD  CONSTRAINT [FK_TagsVideos_Tags_TagId] FOREIGN KEY([TagId])
REFERENCES [dbo].[Tags] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TagsVideos] CHECK CONSTRAINT [FK_TagsVideos_Tags_TagId]
GO
ALTER TABLE [dbo].[TagsVideos]  WITH CHECK ADD  CONSTRAINT [FK_TagsVideos_Videos_VideoId] FOREIGN KEY([VideoId])
REFERENCES [dbo].[Videos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TagsVideos] CHECK CONSTRAINT [FK_TagsVideos_Videos_VideoId]
GO
ALTER TABLE [dbo].[Videos]  WITH CHECK ADD  CONSTRAINT [FK_Videos_Channels_ChannelId] FOREIGN KEY([ChannelId])
REFERENCES [dbo].[Channels] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Videos] CHECK CONSTRAINT [FK_Videos_Channels_ChannelId]
GO
ALTER TABLE [dbo].[VideoStatuses]  WITH CHECK ADD  CONSTRAINT [FK_VideoStatuses_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VideoStatuses] CHECK CONSTRAINT [FK_VideoStatuses_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[VideoStatuses]  WITH CHECK ADD  CONSTRAINT [FK_VideoStatuses_Videos_VideoId] FOREIGN KEY([VideoId])
REFERENCES [dbo].[Videos] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VideoStatuses] CHECK CONSTRAINT [FK_VideoStatuses_Videos_VideoId]
GO
USE [master]
GO
ALTER DATABASE [VibeoServerMain] SET  READ_WRITE 
GO

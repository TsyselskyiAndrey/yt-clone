using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VibeoServer.Models;
using System.Reflection.Emit;

namespace VibeoServer.Data
{
    public class SqlDbContext : IdentityDbContext<User>
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options)
            : base(options)
        {

        }

        public DbSet<Video> Videos { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<VideoStatus> VideoStatuses { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentStatus> CommentStatuses { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(u => u.LastName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(u => u.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("getdate()");

                entity.Property(c => c.AvatarUrl)
                    .HasMaxLength(512);

            });

            modelBuilder.Entity<Video>(entity =>
            {
                entity.HasKey(v => v.Id);

                entity.Property(v => v.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(v => v.Url)
                    .IsRequired()
                    .HasMaxLength(512);

                entity.Property(v => v.Format)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(v => v.Size)
                    .IsRequired();

                entity.Property(v => v.PreviewUrl)
                    .HasMaxLength(512);

                entity.Property(v => v.Description)
                    .HasMaxLength(1000);

                entity.Property(v => v.PublicationDate)
                    .IsRequired()
                    .HasDefaultValueSql("getdate()");

                entity.Property(v => v.Duration)
                    .IsRequired();

                entity.Property(v => v.IsPublic)
                    .IsRequired()
                    .HasDefaultValue(true);

                entity.Property(v => v.IsUploaded)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(c => c.ChannelId)
                    .IsRequired();

                entity.HasOne(v => v.Channel)
                    .WithMany(c => c.Videos)
                    .HasForeignKey(v => v.ChannelId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(v => v.Categories)
                    .WithMany(c => c.Videos)
                    .UsingEntity<Dictionary<string, object>>(
                        "CategoriesVideos",
                        j => j
                            .HasOne<Category>()
                            .WithMany()
                            .HasForeignKey("CategoryId")
                            .OnDelete(DeleteBehavior.Cascade),
                        j => j
                            .HasOne<Video>()
                            .WithMany()
                            .HasForeignKey("VideoId")
                            .OnDelete(DeleteBehavior.Cascade)
                    ).HasKey("CategoryId", "VideoId");

                entity.HasMany(v => v.Tags)
                  .WithMany(t => t.Videos)
                  .UsingEntity<Dictionary<string, object>>(
                      "TagsVideos",
                      j => j
                          .HasOne<Tag>()
                          .WithMany()
                          .HasForeignKey("TagId")
                          .OnDelete(DeleteBehavior.Cascade),
                      j => j
                          .HasOne<Video>()
                          .WithMany()
                          .HasForeignKey("VideoId")
                          .OnDelete(DeleteBehavior.Cascade)
                  ).HasKey("TagId", "VideoId");

                entity.HasMany(v => v.Playlists)
                  .WithMany(p => p.Videos)
                  .UsingEntity<Dictionary<string, object>>(
                      "PlaylistsVideos",
                      j => j
                          .HasOne<Playlist>()
                          .WithMany()
                          .HasForeignKey("PlaylistId")
                          .OnDelete(DeleteBehavior.Cascade),
                      j => j
                          .HasOne<Video>()
                          .WithMany()
                          .HasForeignKey("VideoId")
                          .OnDelete(DeleteBehavior.Cascade)
                  ).HasKey("PlaylistId", "VideoId");

            });

            modelBuilder.Entity<Channel>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(c => c.Description)
                    .HasMaxLength(1000);

                entity.Property(c => c.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("getdate()");

                entity.Property(c => c.LogoUrl)
                    .HasMaxLength(512);

                entity.Property(c => c.Handle)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(c => c.IsDeleted)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.HasIndex(c => c.Handle, "IX_Channels_Handle")
                    .IsUnique();

                entity.Property(c => c.UserId)
                    .IsRequired();

                entity.HasIndex(c => c.UserId, "IX_Channels_UserId")
                    .IsUnique();

                entity.HasOne(c => c.User)
                    .WithOne(u => u.Channel)
                    .HasForeignKey<Channel>(c => c.UserId)
                    .OnDelete(DeleteBehavior.Restrict);  // ----------- Attention
            });

            modelBuilder.Entity<VideoStatus>(entity =>
            {
                entity.HasKey(vs => vs.Id);

                entity.Property(vs => vs.IsFavorite)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(vs => vs.WatchLater)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(vs => vs.VideoId)
                    .IsRequired();

                entity.HasOne(vs => vs.Video)
                    .WithMany(v => v.VideoStatuses)
                    .HasForeignKey(vs => vs.VideoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(vs => vs.UserId)
                    .IsRequired();

                entity.HasOne(vs => vs.User)
                    .WithMany(u => u.VideoStatuses)
                    .HasForeignKey(vs => vs.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<History>(entity =>
            {
                entity.HasKey(h => h.Id);

                entity.Property(h => h.UserId)
                    .IsRequired();

                entity.HasOne(h => h.User)
                    .WithMany(u => u.Histories)
                    .HasForeignKey(h => h.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(h => h.VideoId)
                    .IsRequired();

                entity.HasOne(h => h.Video)
                    .WithMany(v => v.Histories)
                    .HasForeignKey(h => h.VideoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(h => h.WatchedAt)
                    .IsRequired()
                    .HasDefaultValueSql("getdate()");
            });

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.SubscribedAt)
                .IsRequired()
                .HasDefaultValueSql("getdate()");

                entity.Property(s => s.ReceiveNotifications)
                .IsRequired()
                .HasDefaultValue(false);


                entity.Property(s => s.UserId)
                      .IsRequired();

                entity.HasOne(s => s.User)
                    .WithMany(u => u.Subscriptions)
                    .HasForeignKey(s => s.UserId)
                    .OnDelete(DeleteBehavior.Cascade);


                entity.Property(s => s.ChannelId)
                      .IsRequired();

                entity.HasOne(s => s.Channel)
                    .WithMany(c => c.Subscriptions)
                    .HasForeignKey(s => s.ChannelId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Title)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(p => p.Description)
                    .HasMaxLength(1000);

                entity.Property(p => p.IsPublic)
                     .IsRequired()
                     .HasDefaultValue(false);

                entity.Property(p => p.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("getdate()");

                entity.Property(p => p.LastUpdated)
                    .IsRequired()
                    .HasDefaultValueSql("getdate()");

                entity.Property(p => p.UserId)
                    .IsRequired();

                entity.HasOne(p => p.User)
                      .WithMany(u => u.Playlists)
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Content)
                      .IsRequired()
                      .HasMaxLength(1000);

                entity.Property(c => c.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("getdate()");

                entity.Property(c => c.ChannelId)
                      .IsRequired();

                entity.HasOne(c => c.Channel)
                      .WithMany(ch => ch.Comments)
                      .HasForeignKey(c => c.ChannelId)
                      .OnDelete(DeleteBehavior.Restrict); // ----------- Attention

                entity.Property(c => c.VideoId)
                      .IsRequired();

                entity.HasOne(c => c.Video)
                      .WithMany(v => v.Comments)
                      .HasForeignKey(c => c.VideoId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.ParentComment)
                      .WithMany(c => c.ChildComments)
                      .HasForeignKey(c => c.ParentCommentId)
                      .OnDelete(DeleteBehavior.Restrict); // ----------- Attention

            });

            modelBuilder.Entity<CommentStatus>(entity =>
            {
                entity.HasKey(cs => cs.Id);

                entity.Property(cs => cs.UserId)
                      .IsRequired();

                entity.HasOne(cs => cs.User)
                      .WithMany(u => u.CommentStatuses)
                      .HasForeignKey(cs => cs.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(cs => cs.CommentId)
                      .IsRequired();

                entity.HasOne(cs => cs.Comment)
                      .WithMany(c => c.CommentStatuses)
                      .HasForeignKey(cs => cs.CommentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<NotificationType>(entity =>
            {
                entity.HasKey(nt => nt.Id);

                entity.Property(nt => nt.Name)
                      .IsRequired()
                      .HasMaxLength(256);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.Id);

                entity.Property(n => n.Title)
                      .IsRequired()
                      .HasMaxLength(256);

                entity.Property(n => n.Message)
                      .IsRequired()
                      .HasMaxLength(1000);

                entity.Property(n => n.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("getdate()");

                entity.Property(n => n.IsRead)
                      .IsRequired()
                      .HasDefaultValue(false);

                entity.Property(n => n.UserId)
                      .IsRequired();

                entity.HasOne(n => n.User)
                      .WithMany(u => u.Notifications)
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(n => n.NotificationTypeId)
                      .IsRequired();

                entity.HasOne(n => n.NotificationType)
                      .WithMany(nt => nt.Notifications)
                      .HasForeignKey(n => n.NotificationTypeId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(n => n.Video)
                      .WithMany(v => v.Notifications)
                      .HasForeignKey(n => n.VideoId)
                      .OnDelete(DeleteBehavior.Restrict); // ----------- Attention

                entity.HasOne(n => n.Comment)
                      .WithMany(c => c.Notifications)
                      .HasForeignKey(n => n.CommentId)
                      .OnDelete(DeleteBehavior.Cascade);

            });




            var admin = new IdentityRole("admin");
            admin.NormalizedName = "admin";

            var channelOwner = new IdentityRole("channel_owner");
            channelOwner.NormalizedName = "channel_owner";

            modelBuilder.Entity<IdentityRole>().HasData(admin, channelOwner);
        }

    }
}

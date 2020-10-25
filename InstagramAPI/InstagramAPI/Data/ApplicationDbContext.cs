using InstagramAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstagramAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<UserFollow> UserFollows { get; set; }
        public DbSet<UserLikeMedia> UserLikeMedias { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserFollow>().HasKey(uf => new { uf.AppUserFollowedId, uf.AppUserFollowId });
            builder.Entity<UserLikeMedia>().HasKey(um => new { um.AppUserId, um.MediaId });
            base.OnModelCreating(builder);
        }
    }
}

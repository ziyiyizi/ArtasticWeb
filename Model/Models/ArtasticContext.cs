using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Models
{
    public class ArtasticContext : DbContext
    {
        public ArtasticContext(DbContextOptions<ArtasticContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<artdata>()
                .Property(e => e.Artdata1)
                .IsUnicode(false);

            modelBuilder.Entity<artworks>()
                .Property(e => e.Artwork_name)
                .IsUnicode(false);

            modelBuilder.Entity<artworks>()
                .Property(e => e.Artwork_description)
                .IsUnicode(false);

            modelBuilder.Entity<artworks>()
                .Property(e => e.Artwork_dir)
                .IsUnicode(false);

            modelBuilder.Entity<artworks>()
                .Property(e => e.Artdata1)
                .IsUnicode(false);

            modelBuilder.Entity<comments>()
                .Property(e => e.User_Name)
                .IsUnicode(false);

            modelBuilder.Entity<comments>()
                .Property(e => e.Commentor_Name)
                .IsUnicode(false);

            modelBuilder.Entity<comments>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<comments>()
                .HasKey(e => new { e.Artwork_ID, e.Commentor_Name, e.User_Name, e.Comment_time });

            modelBuilder.Entity<notification>()
                .Property(e => e.Sender_Name)
                .IsUnicode(false);

            modelBuilder.Entity<notification>()
                .Property(e => e.Receiver_name)
                .IsUnicode(false);

            modelBuilder.Entity<notification>()
                .Property(e => e.Noti_State)
                .IsUnicode(false);

            modelBuilder.Entity<notification>()
                .Property(e => e.Noti_Content)
                .IsUnicode(false);

            modelBuilder.Entity<notification>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<notification>()
                .Property(e => e.Work_Name)
                .IsUnicode(false);

            modelBuilder.Entity<follow>()
                .HasKey(e => new { e.Artist_ID, e.Follower_ID });

            modelBuilder.Entity<likes>()
                .HasKey(e => new { e.Artwork_ID, e.User_ID });

            modelBuilder.Entity<tags>()
                .Property(e => e.Tag_name)
                .IsUnicode(false);

            modelBuilder.Entity<tags>()
                .HasKey(e => new { e.Artwork_ID, e.Tag_name });

            modelBuilder.Entity<users>()
                .Property(e => e.User_name)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.User_password)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.User_sex)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.User_age)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.User_description)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.User_software)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.User_job)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.User_address)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.User_mail)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.User_phone)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.User_icon)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.User_state)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.User_token)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.User_salt)
                .IsUnicode(false);
        }

        public DbSet<artdata> artdata { get; set; }
        public DbSet<artworks> artworks { get; set; }
        public DbSet<clicks> clicks { get; set; }
        public DbSet<comments> comments { get; set; }
        public DbSet<follow> follow { get; set; }
        public DbSet<likes> likes { get; set; }
        public DbSet<notification> notification { get; set; }
        public DbSet<tags> tags { get; set; }
        public DbSet<users> users { get; set; }
    }


}

using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Model.Models;

namespace Model.Context
{
    public class BlogContext : DbContext
    {
        public BlogContext() : this("name=BlogContext") { }

        public BlogContext(string connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserCategory> UserCategories { get; set; }

        public DbSet<Rubric> Rubrics { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<Link> Links { get; set; }

        public DbSet<Kommentar> Kommentars { get; set; }

        public DbSet<HashTag> HashTags { get; set; }

        public DbSet<GroupContact> GroupContacts { get; set; }

        public DbSet<FileType> FileTypes { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<AttachFile> AttachFiles { get; set; }

        public DbSet<FileCategory> FileCategories { get; set; }

        public DbSet<RatingPost> RatingPosts { get; set; }

        public DbSet<RatingKommentar> RatingKommentars { get; set; }

        public DbSet<BlackList> BlackLists { get; set; }

        public DbSet<Answer> Answers { get; set; }
    }
}

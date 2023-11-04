
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RecipeBookAPI.Data;

namespace RecipeBookAPI.Data
{
    public class AppDBContext: DbContext
    {
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}RecipeBookDb.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Post[] postsToSeed = new Post[6];
            for (int i=1; i<=6; i++)
            {
                postsToSeed[i-1] = new Post
                {
                    PostId = i,
                    Title = $"Post {i}",
                    Content = $"This is post {i} and it is a special recipe."
                };
            }
            modelBuilder.Entity<Post>().HasData(postsToSeed);
        }
    }
}

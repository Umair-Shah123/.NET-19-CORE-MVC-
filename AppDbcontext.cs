using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
namespace WebApplication2.Data
    
{
    public class AppDbcontext : IdentityDbContext<IdentityUser>
    {
        public AppDbcontext(DbContextOptions<AppDbcontext>options):base (options )
        {

        }
        public DbSet<Post>Posts { get; set; }
        public DbSet<Category>Categories { get; set; }

        public DbSet<Comment> Comments {  get; set; }


        // <--- seeding and defining data --->
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Technology" },
                new Category { Id = 2, Name = "Health" },
                new Category { Id = 3, Name = "Lifestyle" }

                );
            modelBuilder.Entity<Post>().HasData(

                new Post
                {   Id=1,
                    Title = "Tech Post 1",
                    Content = "Content of Tech Post 1...",
                    Author = "John Smith",
                    FeatureImagePath = "/images/tech_image.png",
                    PublishedDate = new DateTime(2026,1,1),
                    CategoryId = 1 


                },

                new Post
                {   Id=2,
                    Title = "Health Post 1",
                    Content = "Content of Health Post 1...",
                    Author = "John Doe",
                    FeatureImagePath = "/images/health_image.png",
                    PublishedDate = new DateTime(2026,1,1),
                    CategoryId = 2
                },

                new Post
                {   Id=3,
                    Title = "Lifestyle Post 1",
                    Content = "Content of Lifestyle Post 1...",
                    Author = "Alex Smith",
                    FeatureImagePath = "/images/lifestyle_image.png",
                    PublishedDate = new DateTime(2026, 1, 1),
                    CategoryId =3
                }
                );
        }

    }
}

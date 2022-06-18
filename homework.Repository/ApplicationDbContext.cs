using homework.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace homework.Repository
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Screaning> Screanings { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Movie>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .HasOne(z => z.User)
                .WithMany(z => z.ShoppingCarts)
                .HasForeignKey(z => z.UserId);

            builder.Entity<OrderItem>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            //builder.Entity<OrderItem>()
            //    .HasOne(z => z.ShoppingCart)
            //    .WithMany(z => z.OrderItems)
            //    .HasForeignKey(z => z.ShoppingCartId);

            //builder.Entity<Screaning>()
            //    .Property(z => z.Id)
            //    .ValueGeneratedOnAdd();

            //builder.Entity<Screaning>()
            //    .HasOne(z => z.Movie)
            //    .WithMany(z => z.Screanings)
            //    .HasForeignKey(z => z.MovieId);

            //builder.Entity<Ticket>()
            //    .Property(z => z.Id)
            //    .ValueGeneratedOnAdd();

            //builder.Entity<Ticket>()
            //    .HasOne(z => z.Screaning)
            //    .WithMany(z => z.Tickets)
            //    .HasForeignKey(z => z.ScreaningId);
        }
    }
}

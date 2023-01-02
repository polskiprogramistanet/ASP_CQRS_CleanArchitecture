using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ASP_CQRS.Domain.Entities;
using ASP_CQRS.Domain.Common;
using ASP_CQRS.Persistence.FF.DummyData;
using System.Threading;

namespace ASP_CQRS.Persistence.FF
{
    public class ASP_CQRSContext : DbContext
    {
        public ASP_CQRSContext(DbContextOptions<ASP_CQRSContext> options) : base(options)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Webinar> Webinars { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreateDate = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifyDate = DateTime.Now;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.
                ApplyConfigurationsFromAssembly
                (typeof(ASP_CQRSContext).Assembly);



            foreach (var item in DummyCategories.Get())
            {
                modelBuilder.Entity<Category>().HasData(item);
            }

            foreach (var item in DummyPosts.Get())
            {
                modelBuilder.Entity<Post>(b =>
                {
                    b.HasData(item);
                    //b.OwnsOne(e => e.Category).HasData(item.Category);
                });

                //modelBuilder.Entity<Post>().HasData(item);
                //modelBuilder.owns
            }

            foreach (var item in DummyWebinars.Get())
            {
                modelBuilder.Entity<Webinar>().HasData(item);
            }
        }
    }
}

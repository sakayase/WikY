using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WikYModels.Interface;
using WikYModels.Models;

namespace WikYModels.DbContexts
{
    public class WikYDbContext : IdentityDbContext<User>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Theme> Themes { get; set; }

        public WikYDbContext(DbContextOptions<WikYDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


        //Overrided to implement automatic timestamps on save
        public override int SaveChanges()
        {
            IEnumerable<ITimeStampedModel?>? newEntities = this.ChangeTracker.Entries()
        .Where(
            x => x.State == EntityState.Added &&
            x.Entity != null &&
            x.Entity as ITimeStampedModel != null
            )
        .Select(x => x.Entity as ITimeStampedModel);

            IEnumerable<ITimeStampedModel?>? modifiedEntities = this.ChangeTracker.Entries()
                .Where(
                    x => x.State == EntityState.Modified &&
                    x.Entity != null &&
                    x.Entity as ITimeStampedModel != null
                    )
                .Select(x => x.Entity as ITimeStampedModel);

            foreach (var newEntity in newEntities)
            {
                if (newEntity != null)
                {
                    newEntity.CreatedAt = DateTime.UtcNow;
                    newEntity.UpdatedAt = DateTime.UtcNow;
                }
            }

            foreach (var modifiedEntity in modifiedEntities)
            {
                if (modifiedEntity != null)
                {
                    modifiedEntity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return base.SaveChanges();

        }

    }
}

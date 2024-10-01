using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LimbusIdentityApi.Data
{
    public class IdentityDbContext(DbContextOptions<IdentityDbContext> options) : DbContext(options)
    {
        public DbSet<Identity> Identities => Set<Identity>();
        public DbSet<Passive> Passives => Set<Passive>();
        public DbSet<Skill> Skills => Set<Skill>();

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Identity>()
        //        .HasMany(i => i.Passives)
        //        .WithOne(i => i.);
        //    modelBuilder.Entity<Identity>()
        //        .HasMany(i => i.Skills)
        //        .WithOne(i => i.Identity);
        //}
    }

    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
            var conn = config.GetConnectionString("IdentityDbConnectionString");
            optionsBuilder.UseSqlServer(conn);
            return new IdentityDbContext(optionsBuilder.Options);
        }
    }
}

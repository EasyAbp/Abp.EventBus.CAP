using CapSample.SecondBooks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace CapSample.EntityFrameworkCore
{
    [ConnectionStringName("Default")]
    public class SecondDbContext : AbpDbContext<SecondDbContext>
    {
        /* Add DbSet properties for your Aggregate Roots / Entities here. */
        
        public DbSet<SecondBook> SecondBooks { get; set; }

        public SecondDbContext(DbContextOptions<SecondDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /* Include modules to your migration db context */

            /* Configure your own tables/entities inside here */

            builder.Entity<SecondBook>(b =>
            {
                b.ToTable(CapSampleConsts.DbTablePrefix + "SecondBooks", CapSampleConsts.DbSchema);
                b.ConfigureByConvention();
            });
        }
    }
}

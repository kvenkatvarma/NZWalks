using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Models.Domain;

namespace NZWalks.Api.Data
{
    public class NZWalksDbContext : DbContext
    {
        //Here we will be passing the dbContextoptions from program.cs file using DI
        public NZWalksDbContext(DbContextOptions<NZWalksDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<Difficulty> Difficulties { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var difficulties = new List<Difficulty>
            {
                new Difficulty
                {
                    Id=Guid.Parse("b0b9074d-c6b5-4f79-bdec-e0d13639e355"),
                    Name="Easy"
                },
                 new Difficulty
                {
                    Id=Guid.Parse("93e4436b-23f8-4167-85e4-6e7dcc4e32b6"),
                    Name="Medium"
                },
                  new Difficulty
                {
                    Id=Guid.Parse("62e78ed5-6701-44c3-82cf-2cddca20a667"),
                    Name="Low"
                }
            };
            //Here we are seeding the Difficulties data
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            var region = new List<Region>
            {
                new Region
                {
                    Code="AKL",
                    Name="Auckland",
                    RegionImageUrl="Auckland.jpg",
                    Id=Guid.Parse("3f6ae914-aef5-4a90-901a-e00709360c47")
                },
                 new Region
                {
                    Code="QL",
                    Name="Queensland",
                    RegionImageUrl="Queensland.jpg",
                    Id=Guid.Parse("aaec2f37-867b-457a-bcfd-b797f098522c")
                },
                  new Region
                {
                    Code="WL",
                    Name="Wellington",
                    RegionImageUrl="Wellington.jpg",
                    Id=Guid.Parse("e3941adc-b940-483e-9f84-dac0fa39c7b4")
                }
            };

            modelBuilder.Entity<Region>().HasData(region);
        }
    }
}

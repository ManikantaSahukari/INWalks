﻿using INWalksAPI.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace INWalksAPI.Data
{
    public class INWalksDbContext:DbContext
    {
        public INWalksDbContext(DbContextOptions<INWalksDbContext> dbContextOptions):base(dbContextOptions) 
        {
            
        }

        public DbSet<Difficulty> Difficulties { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //seed the data for difficulty
            //Easy,Medium,Hard
            var difficulties = new List<Difficulty>
            {
                new Difficulty()
                {
                    Id=Guid.Parse("0290b25b-2392-4a7f-8254-fc1fe44a017b"),
                    Name="Easy",
                },
                new Difficulty()
                {
                    Id=Guid.Parse("6eb28147-17f7-4dbf-9bec-6002a1c15732"),
                    Name="Medium",
                },
                new Difficulty()
                {
                    Id=Guid.Parse("3d9e01cc-9d6e-49ec-b35e-14d7093d7e56"),
                    Name="Hard",
                }

            };
            //Seed difficulties to Database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);


            //Seed Data For Reagion
            var regions = new List<Region>
            {
                new Region()
                {
                    Id= Guid.Parse("49a61654-04f4-4e4b-a929-c81e32b50b01"),
                    Name= "Srikakulam",
                    Code="SKLM",
                    RegionImageUrl="https://images.app.goo.gl/kA5NALdFwX6watDY6"
                },   
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = null
                },
            };

            modelBuilder.Entity<Region>().HasData(regions);


        }

    }
}

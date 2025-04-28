using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResourceServer.Models;

namespace ResourceServer.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
    

    public DbSet<Phone> Phones { get; set; }
    public DbSet<PhoneBrand> PhoneBrands { get; set; }
    public DbSet<PhoneSpecification> PhoneSpecification { get; set; }
    public DbSet<PhoneImage> PhoneImages { get; set; }
    public DbSet<PhoneVariant> PhoneVariants { get; set; }
    public DbSet<PhoneRating> PhoneRatings { get; set; }

    // 💥 Đây nè!
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var today = new DateTime(2025, 4, 27);
            // ====== FluentAPI bắt đầu từ đây ======

            // PhoneBrand 1 - * Phone
            modelBuilder.Entity<Phone>()
                .HasOne(p => p.PhoneBrand)
                .WithMany(b => b.Phones)
                .OnDelete(DeleteBehavior.Restrict);

            // Phone 1 - 1 PhoneSpecification
            modelBuilder.Entity<Phone>()
                .HasOne(p => p.Specification)
                .WithOne(s => s.Phone)
                .OnDelete(DeleteBehavior.Cascade);

            // Phone 1 - * PhoneRating
            modelBuilder.Entity<PhoneRating>()
                .HasOne(r => r.Phone)
                .WithMany(p => p.Ratings)
                .OnDelete(DeleteBehavior.Cascade);

            // Phone 1 - * PhoneVariant
            modelBuilder.Entity<PhoneVariant>()
                .HasOne(v => v.Phone)
                .WithMany(p => p.Variants)
                .OnDelete(DeleteBehavior.Cascade);

            // Phone 1 - * PhoneImage
            modelBuilder.Entity<PhoneImage>()
                .HasOne(i => i.Phone)
                .WithMany(p => p.Images)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PhoneBrand>().HasData(
                new PhoneBrand { Id = 1, Name = "Apple", CreatedAt = today },
                new PhoneBrand { Id = 2, Name = "Samsung", CreatedAt = today },
                new PhoneBrand { Id = 3, Name = "Xiaomi", CreatedAt = today },
                new PhoneBrand { Id = 4, Name = "OnePlus", CreatedAt = today }
            );
            modelBuilder.Entity<Phone>().HasData(
                new Phone { Id = 1, Name = "iPhone 16 Pro Max", Description = "Latest iPhone with A17 chip", CreatedAt = today, PhoneBrandId = 1 },
                new Phone { Id = 2, Name = "iPhone 16 Plus", Description = "Latest iPhone with A17 chip", CreatedAt = today, PhoneBrandId = 1 },
                new Phone { Id = 3, Name = "Xiaomi Redmi Note 14 5G", Description = "Latest Xiaomi flagship", CreatedAt = today, PhoneBrandId = 3 },
                new Phone { Id = 4, Name = "Xiaomi Redmi Note 14 Pro Plus", Description = "Latest Xiaomi flagship", CreatedAt = today, PhoneBrandId = 3 },
                new Phone { Id = 5, Name = "Xiaomi Redmi Note 14 Pro", Description = "Latest Xiaomi flagship", CreatedAt = today, PhoneBrandId = 3 },
                new Phone { Id = 6, Name = "Samsung Galaxy S25 Ultra 5G 12GB", Description = "Latest Samsung flagship", CreatedAt = today, PhoneBrandId = 2 },
                new Phone { Id = 7, Name = "Samsung Galaxy S25 Plus 5G 12GB", Description = "Latest Samsung flagship", CreatedAt = today, PhoneBrandId = 2 },
                new Phone { Id = 8, Name = "OnePlus 12 Pro", Description = "Latest OnePlus flagship", CreatedAt = today, PhoneBrandId = 4 },
                new Phone { Id = 9, Name = "OnePlus 12", Description = "Latest OnePlus flagship", CreatedAt = today, PhoneBrandId = 4 }
            );
            modelBuilder.Entity<PhoneSpecification>().HasData(
                new PhoneSpecification { 
                    Id = 1, 
                    PhoneId = 1, 
                    Origin = "China", 
                    ReleaseDate = new DateTime(2023, 9, 1), 
                    WarrantyPeriodMonths = 24, Dimensions = "163 x 77.6 x 8.25 mm", 
                    Weight = "218g", 
                    WaterResistance = "IP68", 
                    FrameMaterial = "Titanium", 
                    BackMaterial = "Tempered glass", 
                    Processor = "Apple A18 Pro",
                    Cores = 6,
                    RAM = "8 GB",
                    ScreenSize = "6.9 inch",
                    ScreenTechnology = "OLED",
                    ScreenStandard = "Super Retina XDR",
                    ScreenResolution = "2886 x 1320 Pixel",
                    RefreshRate = "120 Hz",
                    GlassMaterial = "Ceramic Shield",
                    Brightness = "2000 nits",

                    ContactStorage = "No limit",
                    ExternalMemoryCard = "Not available",

                    SimSlots = 2,
                    SimType = "Nano SIM, eSIM",
                    SupportedNetworks = "5G, 4G, 3G, 2G",
                    Port = "1 Type C",
                    Wifi = "Wi-Fi 6",
                    Gps = "GPS, A-GPS, GLONASS, BDS, GALILEO",
                    Bluetooth = "Bluetooth 5.3",
                    OtherConnections = "NFC, Infrared, USB OTG",

                    BatteryType = "Lithium-ion",
                    BatteryCapacity = "5000 mAh"
                },
                new PhoneSpecification { 
                    Id = 2, 
                    PhoneId = 2, 
                    Origin = "China", 
                    ReleaseDate = new DateTime(2023, 9, 1), 
                    WarrantyPeriodMonths = 24, Dimensions = "163 x 77.6 x 8.25 mm", 
                    Weight = "218g", 
                    WaterResistance = "IP68", 
                    FrameMaterial = "Titanium", 
                    BackMaterial = "Tempered glass", 
                    Processor = "Apple A18 Pro",
                    Cores = 6,
                    RAM = "8 GB",
                    ScreenSize = "6.9 inch",
                    ScreenTechnology = "OLED",
                    ScreenStandard = "Super Retina XDR",
                    ScreenResolution = "2886 x 1320 Pixel",
                    RefreshRate = "120 Hz",
                    GlassMaterial = "Ceramic Shield",
                    Brightness = "2000 nits",

                    ContactStorage = "No limit",
                    ExternalMemoryCard = "Not available",

                    SimSlots = 2,
                    SimType = "Nano SIM, eSIM",
                    SupportedNetworks = "5G, 4G, 3G, 2G",
                    Port = "1 Type C",
                    Wifi = "Wi-Fi 6",
                    Gps = "GPS, A-GPS, GLONASS, BDS, GALILEO",
                    Bluetooth = "Bluetooth 5.3",
                    OtherConnections = "NFC, Infrared, USB OTG",

                    BatteryType = "Lithium-ion",
                    BatteryCapacity = "5000 mAh"
                }
            );
            modelBuilder.Entity<PhoneVariant>().HasData(
                new PhoneVariant { Id = 2, PhoneId = 1, Color = "Titan Sa Mạc", Storage = "256 GB", Price = 39990000 },
                new PhoneVariant { Id = 3, PhoneId = 1, Color = "Titan Sa Mạc", Storage = "512 GB", Price = 41990000 },
                new PhoneVariant { Id = 4, PhoneId = 1, Color = "Titan Sa Mạc", Storage = "1 TB", Price = 43990000 },
                new PhoneVariant { Id = 5, PhoneId = 1, Color = "Titan Tự Nhiên", Storage = "256 GB", Price = 39990000 },
                new PhoneVariant { Id = 6, PhoneId = 1, Color = "Titan Tự Nhiên", Storage = "512 GB", Price = 41990000 },
                new PhoneVariant { Id = 7, PhoneId = 1, Color = "Titan Tự Nhiên", Storage = "1 TB", Price = 43990000 },
                new PhoneVariant { Id = 8, PhoneId = 1, Color = "Titan Trắng", Storage = "256 GB", Price = 39990000 },
                new PhoneVariant { Id = 9, PhoneId = 1, Color = "Titan Trắng", Storage = "512 GB", Price = 41990000 },
                new PhoneVariant { Id = 10, PhoneId = 1, Color = "Titan Trắng", Storage = "1 TB", Price = 43990000 },
                new PhoneVariant { Id = 11, PhoneId = 1, Color = "Titan Đen", Storage = "256 GB", Price = 39990000 },
                new PhoneVariant { Id = 12, PhoneId = 1, Color = "Titan Đen", Storage = "512 GB", Price = 41990000 },
                new PhoneVariant { Id = 13, PhoneId = 1, Color = "Titan Đen", Storage = "1 TB", Price = 43990000 }
            ); 
        }
    }
}
using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ResourceServer.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhoneBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Phones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneBrandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Phones_PhoneBrands_PhoneBrandId",
                        column: x => x.PhoneBrandId,
                        principalTable: "PhoneBrands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhoneImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMain = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneImages_Phones_PhoneId",
                        column: x => x.PhoneId,
                        principalTable: "Phones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneId = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneRatings_Phones_PhoneId",
                        column: x => x.PhoneId,
                        principalTable: "Phones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneSpecification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneId = table.Column<int>(type: "int", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WarrantyPeriodMonths = table.Column<int>(type: "int", nullable: false),
                    Dimensions = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Weight = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WaterResistance = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FrameMaterial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackMaterial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cores = table.Column<int>(type: "int", nullable: false),
                    RAM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScreenSize = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScreenTechnology = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScreenStandard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScreenResolution = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GlassMaterial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brightness = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactStorage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExternalMemoryCard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SimSlots = table.Column<int>(type: "int", nullable: false),
                    SimType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupportedNetworks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Port = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Wifi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bluetooth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OtherConnections = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatteryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BatteryCapacity = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneSpecification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneSpecification_Phones_PhoneId",
                        column: x => x.PhoneId,
                        principalTable: "Phones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneId = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Storage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneVariants_Phones_PhoneId",
                        column: x => x.PhoneId,
                        principalTable: "Phones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PhoneBrands",
                columns: new[] { "Id", "CreatedAt", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Apple" },
                    { 2, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Samsung" },
                    { 3, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Xiaomi" },
                    { 4, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "OnePlus" }
                });

            migrationBuilder.InsertData(
                table: "Phones",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "PhoneBrandId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Latest iPhone with A17 chip", "iPhone 16 Pro Max", 1 },
                    { 2, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Latest iPhone with A17 chip", "iPhone 16 Plus", 1 },
                    { 3, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Latest Xiaomi flagship", "Xiaomi Redmi Note 14 5G", 3 },
                    { 4, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Latest Xiaomi flagship", "Xiaomi Redmi Note 14 Pro Plus", 3 },
                    { 5, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Latest Xiaomi flagship", "Xiaomi Redmi Note 14 Pro", 3 },
                    { 6, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Latest Samsung flagship", "Samsung Galaxy S25 Ultra 5G 12GB", 2 },
                    { 7, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Latest Samsung flagship", "Samsung Galaxy S25 Plus 5G 12GB", 2 },
                    { 8, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Latest OnePlus flagship", "OnePlus 12 Pro", 4 },
                    { 9, new DateTime(2025, 4, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Latest OnePlus flagship", "OnePlus 12", 4 }
                });

            migrationBuilder.InsertData(
                table: "PhoneSpecification",
                columns: new[] { "Id", "BackMaterial", "BatteryCapacity", "BatteryType", "Bluetooth", "Brightness", "ContactStorage", "Cores", "Dimensions", "ExternalMemoryCard", "FrameMaterial", "GlassMaterial", "Gps", "Origin", "OtherConnections", "PhoneId", "Port", "Processor", "RAM", "RefreshRate", "ReleaseDate", "ScreenResolution", "ScreenSize", "ScreenStandard", "ScreenTechnology", "SimSlots", "SimType", "SupportedNetworks", "WarrantyPeriodMonths", "WaterResistance", "Weight", "Wifi" },
                values: new object[,]
                {
                    { 1, "Tempered glass", "5000 mAh", "Lithium-ion", "Bluetooth 5.3", "2000 nits", "No limit", 6, "163 x 77.6 x 8.25 mm", "Not available", "Titanium", "Ceramic Shield", "GPS, A-GPS, GLONASS, BDS, GALILEO", "China", "NFC, Infrared, USB OTG", 1, "1 Type C", "Apple A18 Pro", "8 GB", "120 Hz", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2886 x 1320 Pixel", "6.9 inch", "Super Retina XDR", "OLED", 2, "Nano SIM, eSIM", "5G, 4G, 3G, 2G", 24, "IP68", "218g", "Wi-Fi 6" },
                    { 2, "Tempered glass", "5000 mAh", "Lithium-ion", "Bluetooth 5.3", "2000 nits", "No limit", 6, "163 x 77.6 x 8.25 mm", "Not available", "Titanium", "Ceramic Shield", "GPS, A-GPS, GLONASS, BDS, GALILEO", "China", "NFC, Infrared, USB OTG", 2, "1 Type C", "Apple A18 Pro", "8 GB", "120 Hz", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "2886 x 1320 Pixel", "6.9 inch", "Super Retina XDR", "OLED", 2, "Nano SIM, eSIM", "5G, 4G, 3G, 2G", 24, "IP68", "218g", "Wi-Fi 6" }
                });

            migrationBuilder.InsertData(
                table: "PhoneVariants",
                columns: new[] { "Id", "Color", "PhoneId", "Price", "Storage" },
                values: new object[,]
                {
                    { 2, "Titan Sa Mạc", 1, 39990000m, "256 GB" },
                    { 3, "Titan Sa Mạc", 1, 41990000m, "512 GB" },
                    { 4, "Titan Sa Mạc", 1, 43990000m, "1 TB" },
                    { 5, "Titan Tự Nhiên", 1, 39990000m, "256 GB" },
                    { 6, "Titan Tự Nhiên", 1, 41990000m, "512 GB" },
                    { 7, "Titan Tự Nhiên", 1, 43990000m, "1 TB" },
                    { 8, "Titan Trắng", 1, 39990000m, "256 GB" },
                    { 9, "Titan Trắng", 1, 41990000m, "512 GB" },
                    { 10, "Titan Trắng", 1, 43990000m, "1 TB" },
                    { 11, "Titan Đen", 1, 39990000m, "256 GB" },
                    { 12, "Titan Đen", 1, 41990000m, "512 GB" },
                    { 13, "Titan Đen", 1, 43990000m, "1 TB" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhoneImages_PhoneId",
                table: "PhoneImages",
                column: "PhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneRatings_PhoneId",
                table: "PhoneRatings",
                column: "PhoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Phones_PhoneBrandId",
                table: "Phones",
                column: "PhoneBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneSpecification_PhoneId",
                table: "PhoneSpecification",
                column: "PhoneId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhoneVariants_PhoneId",
                table: "PhoneVariants",
                column: "PhoneId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhoneImages");

            migrationBuilder.DropTable(
                name: "PhoneRatings");

            migrationBuilder.DropTable(
                name: "PhoneSpecification");

            migrationBuilder.DropTable(
                name: "PhoneVariants");

            migrationBuilder.DropTable(
                name: "Phones");

            migrationBuilder.DropTable(
                name: "PhoneBrands");
        }
    }
}

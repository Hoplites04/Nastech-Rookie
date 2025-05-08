using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceServer.Controllers;
using ResourceServer.Database;
using Xunit;
using FluentAssertions;

namespace MyTestProject
{
    public class PhoneControllerTests
    {
        private ApplicationDbContext GetRealDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=LAPTOP-6CDHHU50\\SQLEXPRESS;Database=ResourceServer;User Id=sa;Password=12345678;Trusted_Connection=True;TrustServerCertificate=True;") // <-- Change as needed
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetPhones_ShouldReturnDataFromDatabase()
        {
            // Arrange
            var context = GetRealDbContext();
            var controller = new PhoneController(context);

            // Act
            var result = await controller.GetPhones(null, null);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();

            var data = okResult!.Value as IEnumerable<object>;
            data.Should().NotBeNull();
            data!.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetPhoneById_ShouldReturnOk_WhenPhoneExists()
        {
            // Arrange
            var context = GetRealDbContext();
            var controller = new PhoneController(context);

            int testPhoneId = 1; // 🔧 Ensure this ID exists in your database

            // Act
            var result = await controller.GetPhoneById(testPhoneId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull("Expected 200 OK for existing phone");
            var value = okResult!.Value;

            value.Should().NotBeNull();
            value.ToString().Should().Contain("Id").And.Contain("BrandName");
        }

        [Fact]
        public async Task GetPhoneDetail_ShouldReturnOk_WithPhoneDetailDto()
        {
            // Arrange
            var context = GetRealDbContext();
            var controller = new PhoneController(context);

            int existingPhoneId = 1; // ✅ Replace with a known valid Phone ID in your DB

            // Act
            var result = await controller.GetPhoneDetail(existingPhoneId);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull("Expected 200 OK for existing phone");
            okResult!.Value.Should().NotBeNull();

            // Optional: Strongly validate expected keys
            var json = System.Text.Json.JsonSerializer.Serialize(okResult.Value);
            var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            dict.Should().ContainKey("PhoneId");
            dict.Should().ContainKey("Name");
            dict.Should().ContainKey("BrandName");
            dict.Should().ContainKey("AvailableColors");
            dict.Should().ContainKey("AvailableStorages");
            dict.Should().ContainKey("Variants");
        }

        [Fact]
        public async Task GetPhoneDetail_ShouldReturnNotFound_WhenIdInvalid()
        {
            // Arrange
            var context = GetRealDbContext();
            var controller = new PhoneController(context);

            int invalidId = -999; // ✅ This ID should not exist

            // Act
            var result = await controller.GetPhoneDetail(invalidId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

    }
}

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using MSClubInsights.Application.ServiceInterfaces;
using MSClubInsights.Application.Services;
using MSClubInsights.Domain.Entities.Identity;
using MSClubInsights.Domain.RepoInterfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MSClubInsights.Test.Services
{
	[TestFixture]
	public class TokenServiceTests
	{
		private Mock<IConfiguration> _mockConfig;
		private Mock<IUserRepository> _mockUserRepository;
		private TokenService _tokenService;

		[SetUp]
		public void SetUp()
		{
			_mockConfig = new Mock<IConfiguration>();
			_mockUserRepository = new Mock<IUserRepository>();
			var randomKey = Convert.ToBase64String(Guid.NewGuid().ToByteArray()); // This will give a 16-byte key, but you can make it longer.
			if (randomKey.Length < 32)
			{
				randomKey = randomKey.PadRight(32, 'X'); // Ensure the key is at least 32 bytes long
			}
			_mockConfig.Setup(config => config["JWT:Key"]).Returns(randomKey);
			_tokenService = new TokenService(_mockConfig.Object, _mockUserRepository.Object);
		}

		[Test]
		public async Task GenerateJwtToken_ShouldReturnValidToken_WhenUserIsValid()
		{
			var user = new AppUser
			{
				Id = "1",
				Email = "user@example.com",
				FirstName = "John",
				LastName = "Doe",
				CityId = 1,
				gender = true,
				DateOfBirth = new DateOnly(1990, 1, 1),
				PhoneNumber = "1234567890",
			};
			_mockUserRepository.Setup(repo => repo.GetUserRoles(It.IsAny<AppUser>())).ReturnsAsync(new List<string> { "Admin" });


			// Act
			var token = await _tokenService.GenerateJwtToken(user);

			// Assert
			token.Should().NotBeNullOrEmpty();  // FluentAssertions
		}

            [Test]
		public async Task GenerateJwtToken_ShouldThrowArgumentNullException_WhenUserIsInvalid()
		{
			Assert.ThrowsAsync<ArgumentNullException>(async () => await _tokenService.GenerateJwtToken(null));// FluentAssertions
        }

        [Test]
        public async Task GenerateJwtToken_ShouldThrowArgumentNullException_WhenJwtKeyIsMissing()
        {
            // Arrange
            var config = new Mock<IConfiguration>();
            config.Setup(c => c["JWT:Key"]).Returns((string)null);
            var user = new AppUser
            {
                Id = "1",
                Email = "user@example.com",
                FirstName = "John",
                LastName = "Doe",
                CityId = 1,
                gender = true,
                DateOfBirth = new DateOnly(1990, 1, 1),
                PhoneNumber = "1234567890",
            };
            _mockUserRepository.Setup(repo => repo.GetUserRoles(It.IsAny<AppUser>())).ReturnsAsync(new List<string> { "Admin" });

            var tokenService = new TokenService(config.Object, _mockUserRepository.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => tokenService.GenerateJwtToken(user).GetAwaiter().GetResult());
        }
	}
}
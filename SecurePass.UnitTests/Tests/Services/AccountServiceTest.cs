using Moq;
using AutoMapper;
using SecurePass.Services;
using System.Linq.Expressions;
using SecurePass.DAL.Contracts;
using SecurePass.Data.Entities;
using SecurePass.Common.Models;
using SecurePass.Services.Contracts;
using SecurePass.UnitTests.TestData.Services;
using SecurePass.Common.Models.MappingProfiles;

namespace SecurePass.UnitTests.Tests.Services
{
    public class AccountServiceTest : TestBase
    {
        private readonly Mock<IJwtService> _mockJwtService;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ITempUserRepository> _mockTempUserRepository;
        private readonly Mock<ICryptographyService> _mockCryptographyService;
        private readonly Mock<IBackgroundJobService> _mockBackgroundJobService;
        private readonly Mock<IRefreshTokenEntryRepository> _mockRefreshTokenEntryRepository;

        private readonly AccountService _accountService;

        public AccountServiceTest()
        {
            _mockJwtService = new Mock<IJwtService>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTempUserRepository = new Mock<ITempUserRepository>();
            _mockCryptographyService = new Mock<ICryptographyService>();
            _mockBackgroundJobService = new Mock<IBackgroundJobService>();
            _mockRefreshTokenEntryRepository = new Mock<IRefreshTokenEntryRepository>();

            List<Profile> profiles =
            [
                new UserMappingProfile(),
            ];
            this.ConfigureMapper(profiles);

            _accountService = new AccountService(
                this.Mapper,
                _mockJwtService.Object,
                _mockUserRepository.Object,
                _mockTempUserRepository.Object,
                _mockCryptographyService.Object,
                _mockBackgroundJobService.Object,
                _mockRefreshTokenEntryRepository.Object
            );
        }

        [Fact]
        public async Task GetAllUsers_ReturnsEmptyList_WhenNoUserExists()
        {
            _mockUserRepository.Setup(repo => repo.GetList(null, It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync([]);

            var result = await _accountService.GetAllUsers(It.IsAny<int>());

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [MemberData(nameof(AccountServiceTestData.GetAllUsersTestData), MemberType = typeof(AccountServiceTestData))]
        public async Task GetAllUsers_ReturnsMappedUserModels_WhenUsersExists(IEnumerable<User> users)
        {
            var expectedUserModels = this.Mapper.Map<List<UserModel>>(users);
            _mockUserRepository.Setup(repo => repo.GetList(null, It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(users);

            var result = await _accountService.GetAllUsers(It.IsAny<int>());

            Assert.NotNull(result);
            Assert.Equal(expectedUserModels, result);
        }

        [Theory]
        [MemberData(nameof(AccountServiceTestData.GetAllUsersTestData), MemberType = typeof(AccountServiceTestData))]
        public async Task GetAllUsers_FiltersCurrentUser_WhenUsersExists(IEnumerable<User> users)
        {
            _mockUserRepository.Setup(repo => repo.GetList(null, It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync(users.Where(u => u.Id != AccountServiceTestData.ValidUserId));
            var expectedUserModels = this.Mapper.Map<List<UserModel>>(users.Where(u => u.Id != AccountServiceTestData.ValidUserId));

            var result = await _accountService.GetAllUsers(AccountServiceTestData.ValidUserId);

            Assert.NotNull(result);
            Assert.Equal(expectedUserModels, result);
        }
    }
}

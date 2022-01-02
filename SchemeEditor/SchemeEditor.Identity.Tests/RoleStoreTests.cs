using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;
using SchemeEditor.Identity.Infrastructure;

namespace SchemeEditor.Identity.Tests
{
    [TestFixture]
    public class RoleStoreTests
    {
        private CancellationToken _cancellationToken;
        private RoleStore<User, Role> _roleStore;
        private Mock<IAccountService<User, Role>> _accountServiceMock;
        private Role _role;
        
        [SetUp]
        public void Setup()
        {
            _cancellationToken = new CancellationToken();
            _accountServiceMock = new Mock<IAccountService<User, Role>>();
            _roleStore = new RoleStore<User, Role>(_accountServiceMock.Object);
            _role = new Role
            {
                Id = 1,
                Name = "Administrator",
                NormalizedName = "Administrator"
            };
        }

        [Test]
        public async Task WhenCreateRole_ReturnedStatus()
        {
            // arrange
            var expectedStatus = IdentityResult.Success;
            
            // act
            var actual = await _roleStore.CreateAsync(_role, _cancellationToken);
            
            // assert
            Assert.AreEqual(expectedStatus, actual);
        }

        [Test]
        public async Task WhenUpdateRole_ReturnedStatus()
        {
            // arrange
            var expectedStatus = IdentityResult.Success;

            // act
            var actual = await _roleStore.UpdateAsync(_role, _cancellationToken);

            // assert
            Assert.AreEqual(expectedStatus, actual);
        }

        [Test]
        public async Task WhenDeleteRole_ReturnedStatus()
        {
            // arrange
            var expectedStatus = IdentityResult.Success;

            // act
            var actual = await _roleStore.DeleteAsync(_role, _cancellationToken);

            // assert
            Assert.AreEqual(expectedStatus, actual);
        }

        [Test]
        public async Task WhenGetRoleId_ReturnedRoleId()
        {
            // arrange
            var expectedRoleId = "1";

            // act
            var actual = await _roleStore.GetRoleIdAsync(_role, _cancellationToken);

            // assert
            Assert.AreEqual(expectedRoleId, actual);
        }

        [Test]
        public async Task WhenGetRoleName_ReturnedRoleName()
        {
            // arrange
            var expectedName = "Administrator";

            // act
            var actual = await _roleStore.GetRoleNameAsync(_role, _cancellationToken);

            // assert
            Assert.AreEqual(expectedName, actual);
        }

        [Test]
        public async Task WhenSetRoleName_SetRoleName()
        {
            // arrange

            // act
            await _roleStore.SetRoleNameAsync(_role, "Administrator", _cancellationToken);

            // assert
            _accountServiceMock.Verify(v => v.UpdateRoleAsync(_role));
        }

        [Test]
        public async Task WhenGetNormalizedRoleName_ReturnedNormalizedRoleName()
        {
            // arrange 
            var expectedNormalizedRoleName = "Administrator";

            // act
            var actual = await _roleStore.GetNormalizedRoleNameAsync(_role, _cancellationToken);

            // assert
            Assert.AreEqual(expectedNormalizedRoleName, actual);
        }
    }
}
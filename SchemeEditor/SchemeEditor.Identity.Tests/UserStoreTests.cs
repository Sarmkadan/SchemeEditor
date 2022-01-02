using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;
using SchemeEditor.Identity.Infrastructure;


namespace SchemeEditor.Identity.Tests
{
    [TestFixture]
    public class UserStoreTests
    {
        private CancellationToken _cancellationToken;
        private UserStore<User, Role> _userStore;
        private Mock<IAccountService<User, Role>> _accountServiceMock;
        private User _user;

        [SetUp]
        public void Setup()
        {
            _cancellationToken = new CancellationToken();
            _accountServiceMock = new Mock<IAccountService<User, Role>>();
            _userStore = new UserStore<User, Role>(_accountServiceMock.Object);
            _user = new User
            {
                Id = 1,
                PasswordHash = "098f6bcd4621d373cade4e832627b4f6",
                Email = "test@test.ru",
                EmailConfirmed = true,
                NormalizedEmail = "test@test.ru",
                Phone = "+79001234567",
                PhoneConfirmed = true,
                Name = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                City = "Москва",
                Company = "Газпром",
                Position = "Генеральный директор",
                IsBlocked = false,
                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        Id = 1
                    }
                }
            };
        }

        [Test]
        public async Task WhenGetUserId_ReturnedUserId()
        {
            // arrange
            var expectedId = "1";

            // act
            var actual = await _userStore.GetUserIdAsync(_user, _cancellationToken);

            // assert
            Assert.AreEqual(expectedId, actual);
        }

        [Test]
        public async Task WhenGetUserName_ReturnedUserName()
        {
            // arrange
            var expectedUserName = "test@test.ru";

            // act
            var actual = await _userStore.GetUserNameAsync(_user, _cancellationToken);

            // assert
            Assert.AreEqual(expectedUserName, actual);
        }

        [Test]
        public async Task WhenSetUserName_SetUserName()
        {
            // arrange

            // act
            await _userStore.SetUserNameAsync(_user, "test1@test.ru", _cancellationToken);

            // assert
            _accountServiceMock.Verify(v => v.UpdateAsync(_user.Id, _user, null));
        }

        [Test]
        public async Task WhenGetNormalizedUserName_ReturnedNormalizedUserName()
        {
            // arrange
            var expectedNormalizedUserName = "test@test.ru";

            // act
            var actual = await _userStore.GetNormalizedUserNameAsync(_user, _cancellationToken);

            // assert
            Assert.AreEqual(expectedNormalizedUserName, actual);
        }

        [Test]
        public async Task WhenSetNormalizedUserName_SetNormalizedUserName()
        {
            // arrange

            // act
            await _userStore.SetNormalizedUserNameAsync(_user, "test1@test.ru", _cancellationToken);

            // assert
            _accountServiceMock.Verify(v => v.UpdateAsync(_user.Id, _user, null));
        }

        [Test]
        public async Task WhenCreate_ReturnedSuccessResult()
        {
            // arrange
            var expectedResult = IdentityResult.Success;

            // act
            var actual = await _userStore.CreateAsync(_user, _cancellationToken);

            // assert
            Assert.AreEqual(expectedResult, actual);
        }

        [Test]
        public async Task WhenUpdate_ReturnedSuccessResult()
        {
            // arrange
            var expectedResult = IdentityResult.Success;

            // act
            var actual = await _userStore.UpdateAsync(_user, _cancellationToken);

            // assert
            Assert.AreEqual(expectedResult, actual);
        }

        [Test]
        public async Task WhenDelete_ReturnedSuccessResult()
        {
            // arrange
            var expectedResult = IdentityResult.Success;

            // act
            var actual = await _userStore.DeleteAsync(_user, _cancellationToken);

            // assert
            Assert.AreEqual(expectedResult, actual);
        }

        [Test]
        public async Task WhenFindById_ReturnedUser()
        {
            // arrange
            var expectedUser = new User
            {
                Id = 1,
                PasswordHash = "098f6bcd4621d373cade4e832627b4f6",
                Email = "test@test.ru",
                EmailConfirmed = true,
                NormalizedEmail = "test@test.ru",
                Phone = "+79001234567",
                PhoneConfirmed = true,
                Name = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                City = "Москва",
                Company = "Газпром",
                Position = "Генеральный директор",
                IsBlocked = false,
                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        Id = 1
                    }
                }
            };

            _accountServiceMock.Setup(arg => arg.Get(_user.Id))
                .ReturnsAsync(_user);

            // act
            var actual = await _userStore.FindByIdAsync(_user.Id.ToString(), _cancellationToken);

            // assert
            actual.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public async Task WhenFindByName_ReturnedUser()
        {
            // arrange
            var expectedUser = new User
            {
                Id = 1,
                PasswordHash = "098f6bcd4621d373cade4e832627b4f6",
                Email = "test@test.ru",
                EmailConfirmed = true,
                NormalizedEmail = "test@test.ru",
                Phone = "+79001234567",
                PhoneConfirmed = true,
                Name = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                City = "Москва",
                Company = "Газпром",
                Position = "Генеральный директор",
                IsBlocked = false,
                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        Id = 1
                    }
                }
            };

            _accountServiceMock.Setup(arg => arg.Find(_user.NormalizedEmail))
                .ReturnsAsync(_user);

            // act
            var actual = await _userStore.FindByNameAsync(_user.NormalizedEmail, _cancellationToken);

            // assert
            actual.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public void WhenSetEmailWithoutUser_ThrewNullReferenceException()
        {
            // arrange

            // act
            _accountServiceMock.Setup(arg => arg.UpdateAsync(_user.Id, null, null))
                .Returns(Task.FromException<User>(new NullReferenceException()));

            // assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await _userStore.SetEmailAsync(null, "test@test.ru", _cancellationToken));
        }

        [Test]
        public async Task WhenGetEmail_ReturnedEmail()
        {
            // arrange
            var expectedEmail = "test@test.ru";

            // act
            var actual = await _userStore.GetEmailAsync(_user, _cancellationToken);

            // assert
            Assert.AreEqual(expectedEmail, actual);
        }

        [Test]
        public async Task WhenGetEmailConfirmed_ReturnedEmailConfirmed()
        {
            // arrange

            // act
            var actual = await _userStore.GetEmailConfirmedAsync(_user, _cancellationToken);

            // assert
            Assert.AreEqual(true, actual);
        }

        [Test]
        public async Task WhenSetEmailConfirmed_SetEmailConfirmed()
        {
            // arrange

            // act
            await _userStore.SetEmailConfirmedAsync(_user, false, _cancellationToken);

            // assert
            _accountServiceMock.Verify(v => v.UpdateAsync(_user.Id, _user, null));
        }

        [Test]
        public async Task WhenFindByEmail_ReturnedUser()
        {
            // arrange
            var expectedUser = new User
            {
                Id = 1,
                PasswordHash = "098f6bcd4621d373cade4e832627b4f6",
                Email = "test@test.ru",
                EmailConfirmed = true,
                NormalizedEmail = "test@test.ru",
                Phone = "+79001234567",
                PhoneConfirmed = true,
                Name = "Иван",
                LastName = "Иванов",
                MiddleName = "Иванович",
                City = "Москва",
                Company = "Газпром",
                Position = "Генеральный директор",
                IsBlocked = false,
                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        Id = 1
                    }
                }
            };

            _accountServiceMock.Setup(arg => arg.Find(_user.Email))
                .ReturnsAsync(_user);

            // act
            var actual = await _userStore.FindByEmailAsync(_user.Email, _cancellationToken);

            // assert
            actual.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public async Task WhenGetNormalizedEmail_ReturnedNormalizedEmail()
        {
            // arrange
            var expectedNormalizedEmail = "test@test.ru";

            // act
            var actual = await _userStore.GetNormalizedEmailAsync(_user, _cancellationToken);

            // assert
            Assert.AreEqual(expectedNormalizedEmail, actual);
        }

        [Test]
        public async Task WhenSetNormalizedEmail_SetNormalizedEmail()
        {
            // arrange

            // act
            await _userStore.SetNormalizedEmailAsync(_user, "test1@test.ru", _cancellationToken);

            // assert
            _accountServiceMock.Verify(v => v.UpdateAsync(_user.Id, _user, null));
        }

        [Test]
        public async Task WhenSetPhoneNumber_SetPhoneNumber()
        {
            // arrange

            // act
            await _userStore.SetPhoneNumberAsync(_user, "+79001234567", _cancellationToken);

            // assert
            _accountServiceMock.Verify(v => v.UpdateAsync(_user.Id, _user, null));
        }

        [Test]
        public async Task WhenGetPhoneNumber_ReturnedPhoneNumber()
        {
            // arrange
            var expectedPhoneNumber = "+79001234567";

            // act
            var actual = await _userStore.GetPhoneNumberAsync(_user, _cancellationToken);

            // assert
            Assert.AreEqual(expectedPhoneNumber, actual);
        }

        [Test]
        public async Task WhenGetPhoneNumberConfirmed_ReturnedPhoneNumberConfirmed()
        {
            // arrange

            // act
            var actual = await _userStore.GetPhoneNumberConfirmedAsync(_user, _cancellationToken);

            // assert
            Assert.AreEqual(true, actual);
        }

        [Test]
        public async Task WhenSetPhoneNumberConfirmed_SetPhoneNumberConfirmed()
        {
            // arrange

            // act
            await _userStore.SetPhoneNumberConfirmedAsync(_user, false, _cancellationToken);

            // assert
            _accountServiceMock.Verify(v => v.UpdateAsync(_user.Id, _user, null));
        }

        [Test]
        public async Task WhenHasPassword_ReturnedResult()
        {
            // arrange

            // act 
            var actual = await _userStore.HasPasswordAsync(_user, _cancellationToken);

            // assert
            Assert.AreEqual(true, actual);
        }

        [Test]
        public async Task WhenAddToRole_AddedToRole()
        {
            // arrange

            // act
            await _userStore.AddToRoleAsync(_user, "administrator", _cancellationToken);

            // assert
            _accountServiceMock.Verify(v => v.AddToRoleAsync(_user, "administrator"));
        }

        [Test]
        public async Task WhenRemoveFromRole_RemovedFromRole()
        {
            // arrange

            // act
            await _userStore.RemoveFromRoleAsync(_user, "administrator", _cancellationToken);

            // assert
            _accountServiceMock.Verify(v => v.RemoveFromRoleAsync(_user, "administrator"));
        }

        [Test]
        public async Task WhenGetRoles_GotRoles()
        {
            // arrange
            var expectedRoles = new List<string>
            {
                "administrator"
            };

            _accountServiceMock.Setup(arg => arg.GetRolesAsync(_user))
                .ReturnsAsync(expectedRoles);

            // act 
            var actual = await _userStore.GetRolesAsync(_user, _cancellationToken);

            // assert
            Assert.AreEqual(expectedRoles, actual);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SchemeEditor.Abstraction.Application.Models;
using SchemeEditor.Abstraction.Application.Services;
using SchemeEditor.Abstraction.DAL.Repositories;
using SchemeEditor.Application.Services;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.Application.Tests
{
    [TestFixture]
    public class SchemeServiceTests
    {
        private Mock<ISchemeRepository> _schemeRepositoryMock;
        private Mock<IExecutionContext<User>> _executionContextMock;
        private Mock<IAccountService<User, Role>> _accountServiceMock;
        private ISchemeService _schemeService;

        [SetUp]
        public void Setup()
        {
            _schemeRepositoryMock = new Mock<ISchemeRepository>();
            _executionContextMock = new Mock<IExecutionContext<User>>();
            _accountServiceMock = new Mock<IAccountService<User, Role>>();
            _schemeService = new SchemeService(_schemeRepositoryMock.Object, _executionContextMock.Object, _accountServiceMock.Object);
        }

        [Test]
        public async Task WhenGetList_GotSchemesList()
        {
            // arrange
            var currentDate = DateTime.UtcNow;
            var userId = 1;
            var schemeId = 1;

            var expectedSchemeView = new SchemeView
            {
                Id = schemeId,
                Name = "GetSchemesList",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = null,
                DeletedBy = null
            };
            var expectedSchemeViewsList = new List<SchemeView> { expectedSchemeView };

            var schemeMock = new Scheme
            {
                Id = schemeId,
                Name = "GetSchemesList",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = null,
                DeletedBy = null
            };
            var schemesListMock = new List<Scheme> { schemeMock };

            _schemeRepositoryMock.Setup(arg => arg.List())
                            .ReturnsAsync(schemesListMock);

            // act
            var actualValue = await _schemeService.List();

            // assert
            actualValue.Should().BeEquivalentTo(expectedSchemeViewsList);
        }

        [Test]
        public async Task WhenGetListById_GotSchemesList()
        {
            // arrange
            var currentDate = DateTime.UtcNow;
            var userId = 1;
            var schemeId = 1;

            var expectedSchemeView = new SchemeView
            {
                Id = schemeId,
                Name = "GetSchemesListById",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = null,
                DeletedBy = null
            };
            var expectedSchemeViewsList = new List<SchemeView> { expectedSchemeView };

            var schemeMock = new Scheme
            {
                Id = schemeId,
                Name = "GetSchemesListById",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = null,
                DeletedBy = null
            };
            var schemesListMock = new List<Scheme> { schemeMock };

            _schemeRepositoryMock.Setup(arg => arg.List(schemeId))
                            .ReturnsAsync(schemesListMock);

            // act
            var actualValue = await _schemeService.List(schemeId);

            // assert
            actualValue.Should().BeEquivalentTo(expectedSchemeViewsList);
        }

        [Test]
        public async Task WhenCreateScheme_CreatedSchemeView()
        {
            // arrange
            var currentDate = DateTime.UtcNow;
            var userId = 1;
            var schemeId = 1;

            var expectedSchemeView = new SchemeView
            {
                Id = schemeId,
                Name = "GetScheme",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = null,
                DeletedBy = null
            };

            var schemeMock = new Scheme
            {
                Id = schemeId,
                Name = "GetScheme",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = null,
                DeletedBy = null
            };

            _schemeRepositoryMock.Setup(arg => arg.GetAsync(schemeId))
                            .ReturnsAsync(schemeMock);

            // act
            var actualValue = await _schemeService.GetAsync(schemeId);

            // assert
            actualValue.Should().BeEquivalentTo(expectedSchemeView);
        }

        [Test]
        public async Task WhenGetScheme_GotSchemeView()
        {
            // arrange
            var currentDate = DateTime.UtcNow;
            var userId = 1;

            var expectedSchemeView = new SchemeView
            {
                Id = 1,
                Name = "GetScheme",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = null,
                DeletedBy = null
            };

            var schemeMock = new Scheme
            {
                Id = 1,
                Name = "GetScheme",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = null,
                DeletedBy = null
            };

            _schemeRepositoryMock.Setup(arg => arg.GetAsync(1))
                            .ReturnsAsync(schemeMock);

            // act
            var actualValue = await _schemeService.GetAsync(1);

            // assert
            actualValue.Should().BeEquivalentTo(expectedSchemeView);
        }

        [Test]
        public async Task WhenUpdateScheme_UpdatedSchemeView()
        {
            // arrange
            var currentDate = DateTime.UtcNow;
            var userId = 1;

            var expectedSchemeView = new SchemeView
            {
                Id = 1,
                Name = "UpdateScheme",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = null,
                DeletedBy = null
            };

            var schemeMock = new Scheme
            {
                Id = 1,
                Name = "UpdateScheme",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = null,
                DeletedBy = null
            };

            _schemeRepositoryMock.Setup(arg => arg.UpdateAsync(userId, It.Is<Scheme>(s => s.Id == schemeMock.Id)))
                            .ReturnsAsync(schemeMock);

            // act
            var actualValue = await _schemeService.UpdateAsync(1, expectedSchemeView);

            // assert
            actualValue.Should().BeEquivalentTo(expectedSchemeView);
        }

        [Test]
        public void WhenUpdateScheme_UpdateNotExistedScheme()
        {
            // arrange
            var currentDate = DateTime.UtcNow;
            var userId = 1;

            var expectedSchemeView = new SchemeView
            {
                Id = 3,
                Name = "UpdateScheme",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = null,
                DeletedBy = null
            };

            var schemeMock = new Scheme
            {
                Id = 4,
                Name = "UpdateScheme",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = null,
                DeletedBy = null
            };

            _schemeRepositoryMock.Setup(arg => arg.UpdateAsync(userId, It.Is<Scheme>(s => s.Id != schemeMock.Id)))
                .Returns(Task.FromException<Scheme>(new InvalidOperationException()));

            // assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _schemeService.UpdateAsync(userId, expectedSchemeView));
        }

        [Test]
        public async Task WhenDeleteScheme_DeletedSchemeView()
        {
            // arrange
            var currentDate = DateTime.UtcNow;
            var userId = 1;
            var schemeId = 2;

            var expectedSchemeView = new SchemeView
            {
                Id = schemeId,
                Name = "DeleteScheme",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = currentDate,
                DeletedBy = userId
            };

            var schemeMock = new Scheme
            {
                Id = schemeId,
                Name = "DeleteScheme",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = currentDate,
                DeletedBy = userId
            };

            _schemeRepositoryMock.Setup(arg => arg.DeleteAsync(userId, schemeId))
                            .ReturnsAsync(schemeMock);

            // act
            var actualValue = await _schemeService.DeleteAsync(userId, schemeId);

            // assert
            var isEquivalent = actualValue.DeletedAt >= expectedSchemeView.DeletedAt &&
                               actualValue.DeletedBy == expectedSchemeView.DeletedBy;
            Assert.IsTrue(isEquivalent);
        }

        [Test]
        public void WhenDeleteScheme_DeleteNotExistedScheme()
        {
            // arrange
            var userId = 1;
            var schemeId = 2;

            _schemeRepositoryMock.Setup(arg => arg.DeleteAsync(userId, schemeId))
                .Returns(Task.FromException<Scheme>(new InvalidOperationException()));

            // assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _schemeService.DeleteAsync(userId, schemeId));
        }

        [Test]
        public async Task WhenDeletePermanentScheme_ReceivedDeletePermanentSchemeView()
        {
            // arrange
            var schemeId = 1;

            _schemeRepositoryMock.Setup(arg => arg.DeletePermanentAsync(schemeId));

            // act
            await _schemeService.DeletePermanentAsync(schemeId);

            // assert
            _schemeRepositoryMock.Verify(s => s.DeletePermanentAsync(schemeId));
        }

        [Test]
        public async Task WhenRestoreScheme_RestoredSchemeView()
        {
            // arrange
            var currentDate = DateTime.UtcNow;
            var userId = 1;
            var schemeId = 1;
            var expectSchemeId = 1;

            var expectedSchemeView = new SchemeView
            {
                Id = schemeId,
                Name = "RestoreScheme",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = currentDate,
                DeletedBy = userId
            };

            var schemeMock = new Scheme
            {
                Id = schemeId,
                Name = "RestoreScheme",
                CreatedAt = currentDate,
                CreatedBy = userId,
                ModifiedAt = currentDate,
                ModifiedBy = userId,
                DeletedAt = currentDate,
                DeletedBy = userId
            };

            _schemeRepositoryMock.Setup(arg => arg.RestoreAsync(userId, expectSchemeId))
                            .ReturnsAsync(schemeMock);

            // act
            var actualValue = await _schemeService.RestoreAsync(userId, expectSchemeId);

            // assert
            actualValue.Should().BeEquivalentTo(expectedSchemeView);
        }

        [Test]
        public void WhenRestoreScheme_RestoreNotExistedScheme()
        {
            // arrange
            var userId = 1;
            var schemeId = 2;

            _schemeRepositoryMock.Setup(arg => arg.RestoreAsync(userId, schemeId))
                .Returns(Task.FromException<Scheme>(new InvalidOperationException()));

            // assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _schemeService.RestoreAsync(userId, schemeId));
        }
    }
}
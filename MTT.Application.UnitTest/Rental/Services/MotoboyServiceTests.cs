using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using FluentResults;
using Moq;
using Moq.AutoMock;
using MTT.Application.Rental.Services;
using MTT.Domain.Rental;
using MTT.Domain.Rental.Entities;
using MTT.Domain.Rental.Services;
using MTT.Domain.Rental.ValueObjects;
using MTT.Domain.Shared;

namespace MTT.Application.UnitTest.Rental.Services
{
    public class MotoboyServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly IFixture _fixture;
        private readonly IMotoboyService _service;

        public MotoboyServiceTests()
        {
            _mocker = new AutoMocker();
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _service = _mocker.CreateInstance<MotoboyService>();
        }

        [Fact]
        public async Task Get_ShouldReturnMotoboy_WhenMotoboyExists()
        {
            // Arrange
            var motoboy = _fixture.Create<Motoboy>();


            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(uow => uow.Motoboy.GetAsync(motoboy.Id))
                .ReturnsAsync(motoboy);

            // Act
            var result = await _service.GetAsync(motoboy.Id.ToString());

            // Assert
            result.Should().Be(motoboy);
        }

        [Fact]
        public async Task Add_ShouldReturnValidationError_WhenMotoboyIsInvalid()
        {
            // Arrange
            var motoboy = _fixture
                .Create<Motoboy>()
                .WithName(string.Empty);

            // Act
            var result = await _service.AddAsync(motoboy, "cnhImage64Base");

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Message == "Name is required");
        }

        [Fact]
        public async Task Add_ShouldReturnCNPJAlreadyExistError_WhenCNPJAlreadyExists()
        {
            // Arrange
            var motoboy = _fixture.Create<Motoboy>();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(uow => uow.Motoboy.List())
                .Returns(new[] { motoboy }.AsQueryable());

            // Act
            var result = await _service.AddAsync(motoboy, "cnhImage64Base");

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Message == Errors.CNPJAddedAlready.Message);
        }

        [Fact]
        public async Task Add_ShouldReturnCNHAlreadyExistError_WhenCNHAlreadyExists()
        {
            // Arrange
            var motoboy = _fixture.Create<Motoboy>();

            var existingMotoboy = _fixture
                .Create<Motoboy>()
                .WithCNH(motoboy.CNH);

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(uow => uow.Motoboy.List())
                .Returns(new[] { existingMotoboy }.AsQueryable());

            // Act
            var result = await _service.AddAsync(motoboy, "cnhImage64Base");

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Message == Errors.CNHAlreadyExist.Message);
        }

        [Fact]
        public async Task Add_ShouldReturnCNHCategoryNotAllowedForRentalError_WhenCNHCategoryNotAllowed()
        {
            // Arrange
            var motoboy = _fixture
                .Create<Motoboy>()
                .WithCNH(new CNH(123, CNHCategory.C));

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(uow => uow.Motoboy.List())
                .Returns(Enumerable.Empty<Motoboy>().AsQueryable());

            // Act
            var result = await _service.AddAsync(motoboy, "cnhImage64Base");

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Message == Errors.CNHCategoryNotAllowedForRental.Message);
        }

        [Fact]
        public async Task Add_ShouldReturnCNHUploadUnexpectedError_WhenUploadFails()
        {
            // Arrange
            var motoboy = _fixture.Create<Motoboy>()
                .WithCNH(new CNH(123, CNHCategory.A));

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(uow => uow.Motoboy.List())
                .Returns(Enumerable.Empty<Motoboy>().AsQueryable());

            _mocker.GetMock<IStorageService>()
                .Setup(s => s.UploadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(Result.Fail(Errors.CNHUploadUnexpectedError));

            // Act
            var result = await _service.AddAsync(motoboy, "cnhImage64Base");

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Message == Errors.CNHUploadUnexpectedError.Message);
        }

        [Fact]
        public async Task Add_ShouldAddMotoboy_WhenValid()
        {
            // Arrange
            var motoboy = _fixture.Create<Motoboy>()
                .WithCNH(new CNH(123, CNHCategory.A));

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(uow => uow.Motoboy.List())
                .Returns(Enumerable.Empty<Motoboy>().AsQueryable());

            _mocker.GetMock<IStorageService>()
                .Setup(s => s.UploadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(Result.Ok());

            // Act
            var result = await _service.AddAsync(motoboy, "cnhImage64Base");

            // Assert
            result.IsSuccess.Should().BeTrue();

            _mocker.GetMock<IRentalUnitOfWork>().Verify(uow => uow.Motoboy.AddAsync(motoboy), Times.Once);
            _mocker.GetMock<IRentalUnitOfWork>().Verify(uow => uow.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdateCNH_ShouldReturnNotFoundError_WhenMotoboyDoesNotExist()
        {
            // Arrange
            var motoboy = _fixture.Create<Motoboy>();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(uow => uow.Motoboy.GetAsync(motoboy.Id))
                .ReturnsAsync((Motoboy)null);

            // Act
            var result = await _service.UpdateCNHAsync(motoboy.Id.ToString(), "cnhImage64Base");

            // Assert
            result.IsFailed.Should().BeTrue();
            result.Errors.Should().Contain(e => e.Message == Errors.MotoboyNotFound.Message);
        }

        [Fact]
        public async Task UpdateCNH_ShouldUploadCNH_WhenMotoboyExists()
        {
            // Arrange
            var motoboy = _fixture.Create<Motoboy>();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(uow => uow.Motoboy.GetAsync(motoboy.Id))
                .ReturnsAsync(motoboy);

            _mocker.GetMock<IStorageService>()
                .Setup(s => s.UploadAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(Result.Ok());

            // Act
            var result = await _service.UpdateCNHAsync(motoboy.Id.ToString(), "cnhImage64Base");

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mocker.GetMock<IStorageService>().Verify(s => s.UploadAsync(motoboy.Id.ToString(), "cnhImage64Base"), Times.Once);
        }

    }

}
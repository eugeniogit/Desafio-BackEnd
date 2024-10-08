using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using MTT.Application.Rental.Services;
using MTT.Domain.Rental;
using MTT.Domain.Rental.Entities;
using MTT.Domain.Rental.Services;

namespace MTT.Application.UnitTest.Rental.Services
{
    public class MotoServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly IFixture _fixture;
        private readonly IMotoService _service;

        public MotoServiceTests()
        {
            _mocker = new AutoMocker();
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _service = _mocker.CreateInstance<MotoService>();
        }

        [Fact]
        public async Task Add_ShouldReturnFail_WhenMotoIsInvalid()
        {
            // Arrange
            var moto = _fixture
                .Create<Moto>()
                .WithTag(string.Empty);

            // Act
            var result = await _service.AddAsync(moto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Message == "Tag is required");
        }

        [Fact]
        public async Task Add_ShouldReturnFail_WhenMotoAlreadyExists()
        {
            // Arrange
            var moto = _fixture.Create<Moto>();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Moto.GetByTagAsync(moto.Tag))
                .ReturnsAsync(moto);

            // Act
            var result = await _service.AddAsync(moto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Message == Errors.MotoAddedAlready.Message);
        }

        [Fact]
        public async Task Add_ShouldReturnOk_WhenMotoIsValidAndDoesNotExist()
        {
            // Arrange
            var moto = _fixture.Create<Moto>();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Moto.GetAsync(moto.Id))
                .ReturnsAsync((Moto)null);

            // Act
            var result = await _service.AddAsync(moto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mocker.GetMock<IRentalUnitOfWork>().Verify(u => u.Moto.AddAsync(moto), Times.Once);
            _mocker.GetMock<IRentalUnitOfWork>().Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public void List_ShouldReturnAllMotos()
        {
            // Arrange
            var motos = _fixture
                .CreateMany<Moto>()
                .ToList();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Moto.List())
                .Returns(motos.AsQueryable());

            // Act
            var result = _service.List();

            // Assert
            result.Should().BeEquivalentTo(motos);
        }

        [Fact]
        public async Task GetByTag_ShouldReturnMoto_WhenMotoExists()
        {
            // Arrange
            var moto = _fixture.Create<Moto>();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Moto.GetAsync(moto.Id))
                .ReturnsAsync(moto);

            // Act
            var result = await _service.GetAsync(moto.Id.ToString());

            // Assert
            result.Should().Be(moto);
        }

        [Fact]
        public async Task UpdateTag_ShouldReturnFail_WhenMotoDoesNotExist()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            var newTag = _fixture.Create<string>();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Moto.GetAsync(id))
                .ReturnsAsync((Moto)null);

            // Act
            var result = await _service.UpdateTagAsync(id.ToString(), newTag);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Message == Errors.MotoNotFound.Message);
        }

        [Fact]
        public async Task UpdateTag_ShouldReturnOk_WhenMotoExists()
        {
            // Arrange
            var moto = _fixture.Create<Moto>();
            var newTag = _fixture.Create<string>();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Moto.GetAsync(moto.Id))
                .ReturnsAsync(moto);

            // Act
            var result = await _service.UpdateTagAsync(moto.Id.ToString(), newTag);

            // Assert
            result.IsSuccess.Should().BeTrue();

            _mocker.GetMock<IRentalUnitOfWork>().Verify(u => u.Moto.Update(moto), Times.Once);
            _mocker.GetMock<IRentalUnitOfWork>().Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnFail_WhenMotoDoesNotExist()
        {
            // Arrange
            var id = _fixture.Create<Guid>();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Moto.GetAsync(id))
                .ReturnsAsync((Moto)null);

            // Act
            var result = await _service.DeleteAsync(id.ToString());

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Message == Errors.MotoNotFound.Message);
        }

        [Fact]
        public async Task Delete_ShouldReturnFail_WhenMotoIsRented()
        {
            // Arrange
            var rental = _fixture
                .Create<Domain.Rental.Entities.Rental>();

			var moto = _fixture
                .Create<Moto>()
                .WithRental(rental);

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Moto.GetAsync(moto.Id))
                .ReturnsAsync(moto);

            // Act
            var result = await _service.DeleteAsync(moto.Id.ToString());

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(e => e.Message == Errors.MotoRented.Message);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenMotoIsNotRented()
        {
            // Arrange
            var moto = _fixture.Create<Moto>();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Moto.GetAsync(moto.Id))
                .ReturnsAsync(moto);

            // Act
            var result = await _service.DeleteAsync(moto.Id.ToString());

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mocker.GetMock<IRentalUnitOfWork>().Verify(u => u.Moto.Delete(moto), Times.Once);
            _mocker.GetMock<IRentalUnitOfWork>().Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

    }

}
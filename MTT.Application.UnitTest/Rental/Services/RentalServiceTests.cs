using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Moq.AutoMock;
using MTT.Application.Rental.Services;
using MTT.Domain.Rental.Entities;
using MTT.Domain.Rental;
using MTT.Domain.Rental.Services;
using MTT.Domain.Rental.ValueObjects;
using FluentAssertions;

namespace MTT.Application.UnitTest.Rental.Services
{
    public class RentalServiceTests
    {
        private readonly AutoMocker _mocker;
        private readonly IFixture _fixture;
        private readonly IRentalService _service;

        public RentalServiceTests()
        {
            _mocker = new AutoMocker();
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _service = _mocker.CreateInstance<RentalService>();
        }

        [Fact]
        public async Task Get_ShouldReturnRental_WhenRentalExists()
        {
            // Arrange
            var rental = _fixture.Create<Domain.Rental.Entities.Rental>();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Rental.GetAsync(rental.Id))
                .ReturnsAsync(rental);

            // Act
            var result = await _service.GetAsync(rental.Id.ToString());

            // Assert
            result.Should().Be(rental);
        }

        [Fact]
        public async Task Add_ShouldReturnFail_WhenMotoboyNotFound()
        {
            // Arrange
            var plan = _fixture.Create<RentalPlan>();
            var motoboyId = _fixture.Create<Guid>();
            var motoId = _fixture.Create<Guid>();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Motoboy.GetAsync(motoboyId))
                .ReturnsAsync((Motoboy)null);

            // Act
            var result = await _service.AddAsync(plan, motoboyId.ToString(), motoId.ToString(), DateTime.UtcNow);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(error => error.Message == Errors.MotoboyNotFound.Message);
        }

        [Fact]
        public async Task Add_ShouldReturnFail_WhenCNHCategoryNotAllowedForRental()
        {
            // Arrange
            var plan = _fixture.Create<RentalPlan>();
            var motoId = _fixture.Create<Guid>();

            var motoboy = _fixture
                .Create<Motoboy>()
                .WithCNH(new CNH(123, CNHCategory.C));

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Motoboy.GetAsync(motoboy.Id))
                .ReturnsAsync(motoboy);

            // Act
            var result = await _service.AddAsync(plan, motoboy.Id.ToString(), motoId.ToString(), DateTime.UtcNow);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(c => c.Message == Errors.CNHCategoryNotAllowedForRental.Message);
        }

        [Fact]
        public async Task Add_ShouldReturnFail_WhenMotoNotFound()
        {
            // Arrange
            var plan = _fixture.Create<RentalPlan>();
            var motoId = _fixture.Create<Guid>();

            var motoboy = _fixture
                .Create<Motoboy>()
                .WithCNH(new CNH(123, CNHCategory.A));

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Motoboy.GetAsync(motoboy.Id))
                .ReturnsAsync(motoboy);

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Moto.GetAsync(motoId))
                .ReturnsAsync((Moto)null);

            // Act
            var result = await _service.AddAsync(plan, motoboy.Id.ToString(), motoId.ToString(), DateTime.UtcNow);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(c => c.Message == Errors.MotoNotFound.Message);
        }

        [Fact]
        public async Task Add_ShouldReturnOk_WhenRentalIsCreated()
        {
            // Arrange
            var plan = _fixture.Create<RentalPlan>();

            var motoboy = _fixture
                .Create<Motoboy>()
                .WithCNH(new CNH(123, CNHCategory.A));

            var moto = _fixture.Create<Moto>();
            var rental = _fixture.Create<Domain.Rental.Entities.Rental>();

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Motoboy.GetAsync(motoboy.Id))
                .ReturnsAsync(motoboy);

            _mocker.GetMock<IRentalUnitOfWork>()
               .Setup(u => u.Moto.GetAsync(moto.Id))
               .ReturnsAsync(moto);

            _mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Rental.AddAsync(It.IsAny<Domain.Rental.Entities.Rental>()))
                .ReturnsAsync(rental);

            // Act
            var result = await _service.AddAsync(plan, motoboy.Id.ToString(), moto.Id.ToString(), DateTime.UtcNow);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(rental.Id);
            _mocker.GetMock<IRentalUnitOfWork>().Verify(u => u.Rental.AddAsync(It.IsAny<Domain.Rental.Entities.Rental>()), Times.Once);
            _mocker.GetMock<IRentalUnitOfWork>().Verify(u => u.SaveChangesAsync(default), Times.Once);
        }

		[Fact]
		public async Task Complete_ShouldReturnFail_WhenRentalNotFound()
		{
			// Arrange
			var rentalId = Guid.NewGuid().ToString();
			_mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Rental.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Domain.Rental.Entities.Rental)null);

			// Act
			var result = await _service.CompleteAsync(rentalId, DateTime.UtcNow);

			// Assert
			result.IsSuccess.Should().BeFalse();
			result.Errors.Should().Contain(c => c.Message == Errors.RentalNotFound.Message);
		}

		[Fact]
		public async Task Complete_ShouldReturnOk_WhenRentalIsCompleted()
		{
			// Arrange
			var rental = _fixture.Create<Domain.Rental.Entities.Rental>();

			_mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Rental.GetAsync(It.IsAny<Guid>())).ReturnsAsync(rental);
			_mocker.GetMock<IRentalUnitOfWork>()
                .Setup(u => u.Rental.Update(It.IsAny<Domain.Rental.Entities.Rental>()));

			// Act
			var result = await _service.CompleteAsync(rental.Id.ToString(), DateTime.UtcNow);

			// Assert
			result.IsSuccess.Should().BeTrue();
			_mocker.GetMock<IRentalUnitOfWork>().Verify(u => u.Rental.Update(It.IsAny<Domain.Rental.Entities.Rental>()), Times.Once);
			_mocker.GetMock<IRentalUnitOfWork>().Verify(u => u.SaveChangesAsync(default), Times.Once);
		}

	}

}
using Contracts.DTOs;
using Domain.Exceptions;
using Domain.Models;
using Infrastructure.Interfaces;
using LanguageExt;
using Moq;
using WebApi.Services;

namespace UnitTests.WebApi.Services;

public class CustomerServiceTests
{
    private readonly Mock<IGenericRepository<Customer>> _mockRepository;
    private readonly CustomerService _customerService;
    private readonly Customer _testCustomer;

    public CustomerServiceTests()
    {
        _mockRepository = new Mock<IGenericRepository<Customer>>();
        _customerService = new CustomerService(_mockRepository.Object);

        // Initialize the test customer
        _testCustomer = new Customer
        {
            Id = Guid.NewGuid(),
            LoyaltyDiscount = 0.1m,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    [Theory]
    [InlineData("2")]
    [InlineData("-1.1")]
    public async Task AddAsync_ShouldReturnValidationException_WhenLoyaltyDiscountIsOutOfRange(string loyaltyDiscountStr)
    {
        // Convert the string to a decimal
        var loyaltyDiscount = decimal.Parse(loyaltyDiscountStr);

        // Arrange
        var dto = new CustomerCreateDto { LoyaltyDiscount = loyaltyDiscount };

        // Act
        var result = await _customerService.AddAsync(dto);

        // Assert
        Assert.True(result.IsLeft);
    }

    [Theory]
    [InlineData("0")]
    [InlineData("1")]
    [InlineData("0.54")]
    public async Task AddAsync_ShouldCallAddMethod_WhenLoyaltyDiscountIsValid(string loyaltyDiscountStr)
    {
        // Convert the string to a decimal
        var loyaltyDiscount = decimal.Parse(loyaltyDiscountStr);

        // Arrange
        var dto = new CustomerCreateDto { LoyaltyDiscount = loyaltyDiscount };
        var expectedCustomer = new Customer { Id = Guid.NewGuid(), LoyaltyDiscount = loyaltyDiscount, CreatedAt = DateTimeOffset.UtcNow };

        _mockRepository.Setup(repo => repo.Add(It.IsAny<Customer>()))
            .ReturnsAsync((Customer customer) => customer);

        // Act
        var result = await _customerService.AddAsync(dto);

        var resultLoyaltyDiscount = result.Match(
            Right: customer => customer.LoyaltyDiscount,
            Left: exception => throw exception
        );

        // Assert
        Assert.True(result.IsRight);
        Assert.Equal(expectedCustomer.LoyaltyDiscount, resultLoyaltyDiscount);
        _mockRepository.Verify(repo => repo.Add(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
    {
        // Arrange
        var customerId = new Guid("00000001-0002-0003-0004-000500060007");
        var expectedCustomer = new Customer
        {
            Id = customerId,
            LoyaltyDiscount = 0,
            CreatedAt = default
        };
        _mockRepository.Setup(repo => repo.GetById(customerId)).ReturnsAsync(expectedCustomer);

        // Act
        var result = await _customerService.GetByIdAsync(customerId);

        // Assert
        Assert.Equal(expectedCustomer, result);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNotFoundException_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        _mockRepository.Setup(repo => repo.GetById(customerId)).ReturnsAsync((Customer?)null);

        // Act
        var result = await _customerService.GetByIdAsync(customerId);

        // Assert
        Assert.True(result.IsLeft);
        Assert.IsType<NotFoundException>(result.Match(
            Right: _ => null,
            Left: ex => ex
        ));
    }

    [Fact]
    public async Task ListAsync_ShouldReturnCustomers_WhenCustomersExist()
    {
        // Arrange
        const int offset = 0;
        const int limit = 2;
        var expectedCustomers = new List<Customer>
        {
            new()
            {
                Id = new Guid("00000001-0002-0003-0004-000500060007"),
                LoyaltyDiscount = 0,
                CreatedAt = default
            },
            new()
            {
                Id= new Guid("00000001-0002-0003-0004-000500060008"),
                LoyaltyDiscount = 0,
                CreatedAt = default
            }
        };
        _mockRepository.Setup(repo => repo.ListAsync(offset, limit)).ReturnsAsync(expectedCustomers);

        // Act
        var result = await _customerService.ListAsync(offset, limit);

        // Assert
        Assert.Equal(expectedCustomers, result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedCustomer_WhenCustomerExistsAndLoyaltyDiscountIsWithinRange()
    {
        // Arrange
        var customerId = new Guid("00000001-0002-0003-0004-000500060007");
        var expectedCustomer = new Customer { Id = customerId, LoyaltyDiscount = 0.5m, CreatedAt = default};
        _mockRepository.Setup(repo => repo.GetById(customerId)).ReturnsAsync(expectedCustomer);

        var customerUpdateDto = new CustomerUpdateDto { LoyaltyDiscount = 0.6m };

        // Act
        var result = await _customerService.UpdateAsync(customerId, customerUpdateDto);

        var resultLoyaltyDiscount = result.Match(
            Right: customer => customer.LoyaltyDiscount,
            Left: exception => throw exception
        );

        // Assert
        Assert.Equal(customerUpdateDto.LoyaltyDiscount, resultLoyaltyDiscount);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnValidationException_WhenLoyaltyDiscountIsOutOfRange()
    {
        // Arrange
        var customerId = new Guid("00000001-0002-0003-0004-000500060007");
        var expectedCustomer = new Customer { Id = customerId, LoyaltyDiscount = 0.5m, CreatedAt = default};
        _mockRepository
            .Setup(repo => repo.GetById(customerId)).ReturnsAsync(expectedCustomer);

        var customerUpdateDto = new CustomerUpdateDto { LoyaltyDiscount = 1.1m };

        // Act
        var result = await _customerService.UpdateAsync(customerId, customerUpdateDto);

        // Assert
        Assert.True(result.Match(
            Right: _ => false,
            Left: _ => true
        ));
        Assert.IsType<ValidationException>(result.Match(
            Right: _ => null,
            Left: ex => ex
        ));
    }

    [Fact]
    public async Task Delete_ShouldReturnUnit_WhenCustomerExists()
    {
        // Arrange
        var customerId = new Guid("00000001-0002-0003-0004-000500060007");
        var expectedCustomer = new Customer { Id = customerId, LoyaltyDiscount = 0.1m, CreatedAt = default };
        _mockRepository.Setup(repo => repo.GetById(customerId)).ReturnsAsync(expectedCustomer);

        // Act
        var result = await _customerService.Delete(customerId);

        // Assert
        Assert.True(result.Match(
            Right: _ => true,
            Left: _ => false
        ));
        Assert.Equal(Unit.Default, result.Match(
            Right: u => u,
            Left: _ => Unit.Default
        ));
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFoundException_WhenCustomerDoesNotExist()
    {
        // Arrange
        var customerId = new Guid("00000001-0002-0003-0004-000500060007");
        _mockRepository.Setup(repo => repo.GetById(customerId)).ReturnsAsync((Customer?)null);

        // Act
        var result = await _customerService.Delete(customerId);

        // Assert
        var ex = result.Match(
            Right: _ => new NotFoundException("",""),
            Left: ex => ex
        );
        Assert.IsType<NotFoundException>(ex);
    }

}

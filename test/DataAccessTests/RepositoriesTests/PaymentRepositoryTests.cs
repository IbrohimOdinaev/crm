using static test.DataAccessTests.Helpers.EntityHelper;
using crm.DataAccess.DataBase;
using crm.DataAccess.Entities;
using crm.DataAccess.IRepositories;
using crm.DataAccess.Repositories;

namespace test.DataAccessTests.RepositoriesTests;

public class PaymentRepositoryTests
{
    IPaymentRepository _paymentRepository = new PaymentRepository();

    [Fact]
    public async Task GetbyIdAsync_ReturnPayment()
    {
        //Arrange
        DataStorage.Payments.Clear();
        Payment payment = (Payment)CreateDefaultEntity<Payment>();
        DataStorage.Payments.Add(payment);

        //Act
        var result = await _paymentRepository.GetByIdAsync(payment.Id);

        //Arrange
        Assert.Equal(payment, result);
    }

    [Fact]
    public async Task AddAsync_ValidValue()
    {
        //Arrange
        Payment payment = (Payment)CreateDefaultEntity<Payment>();

        //Act
        await _paymentRepository.AddAsync(payment);

        //Assert
        Assert.Contains(payment, DataStorage.Payments);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsListPayment()
    {
        //Arrange
        DataStorage.Payments.Clear();
        List<Payment> payments = new()
        {
            (Payment)CreateDefaultEntity<Payment>(),
            (Payment)CreateDefaultEntity<Payment>(),
            (Payment)CreateDefaultEntity<Payment>(),
            (Payment)CreateDefaultEntity<Payment>()
        };
        DataStorage.Payments.AddRange(payments);

        //Act
        var result = await _paymentRepository.GetAllAsync();

        //Assert
        Assert.Equal(payments, result);
    }

    [Fact]
    public async Task GetByUserIdAsync_ReturnUsersPayments()
    {
        //Arrange
        DataStorage.Payments.Clear();
        Guid userId = Guid.NewGuid();
        DataStorage.Payments.AddRange(new List<Payment>()
        {
            new Payment() {UserId = userId },
            new Payment() {UserId = userId },
            new Payment() {UserId = userId },
            new Payment() {UserId = Guid.NewGuid()},
            new Payment() {UserId = Guid.NewGuid()}
        });

        //Act
        var result = (await _paymentRepository.GetByUserIdAsync(userId)).Count();

        //Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public async Task GetByUserIdAsync_ReturnsEmptyList()
    {
        //Arrange
        DataStorage.Payments.Clear();
        Guid userId = Guid.NewGuid();
        DataStorage.Payments.AddRange(new List<Payment>()
        {
            new Payment() {UserId = userId },
            new Payment() {UserId = userId },
            new Payment() {UserId = userId },
            new Payment() {UserId = Guid.NewGuid()},
            new Payment() {UserId = Guid.NewGuid()}
        });

        //Act
        var result = await _paymentRepository.GetByUserIdAsync(Guid.NewGuid());

        //Assert
        Assert.Empty(result);
    }
}

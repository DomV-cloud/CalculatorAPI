using Calculator.Core.Models;
using Calculator.Data.DatabaseContext;
using Calculator.Data.Models;
using Calculator.Infrastructure.Interfaces;
using Calculator.Services.Implementation;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Calculator.UnitTests;

[TestClass]
public class CalculationLoggingServiceTest
{
    private Mock<CalculatorDbContext> _mockContext;
    private Mock<ILoggingService> _mockLoggingService;
    private CalculationLoggingService _service;

    [TestInitialize]
    public void TestInitialize()
    {
        var options = new DbContextOptionsBuilder<CalculatorDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _mockContext = new Mock<CalculatorDbContext>(options);
        _mockLoggingService = new Mock<ILoggingService>();
        _service = new CalculationLoggingService(_mockContext.Object, _mockLoggingService.Object);
    }

    [TestMethod]
    public async Task GetAllLogs_ShouldReturnLogsAndLogMessages()
    {
        // Arrange
        var logs = new List<CalculationLog>
            {
                new CalculationLog { Guid = new Guid("2309a0c3-a877-450c-9c3f-d3ceb6c3e3ba"), Expression = ExpressionType.Addition, FirstOperand = 1, SecondOperand = 2, Result = 3, TimeStamp = DateTime.Now }
            };
        var mockDbSet = CreateMockDbSet(logs);
        _mockContext.Setup(c => c.CalculationLogs).Returns(mockDbSet.Object);

        // Act
        var result = await _service.GetAllLogs();

        // Assert
        Assert.AreEqual(logs.Count, result.Count());
        CollectionAssert.AreEqual(logs, result.ToList());
        _mockLoggingService.Verify(log => log.LogInformationAsync("Fetching all calculation logs from the database..."), Times.Once);
        _mockLoggingService.Verify(log => log.LogInformationAsync("Fetched {0} calculation logs from the database.", logs.Count), Times.Once);
    }

    [TestMethod]
    public async Task LogCalculation_ShouldAddLogAndLogMessages()
    {
        // Arrange
        var calculationModel = new CalculationModel
        {
            Expression = ExpressionType.Addition,
            FirstOperand = 1,
            SecondOperand = 2,
            Result = 3
        };

        var mockDbSet = CreateMockDbSet(new List<CalculationLog>());
        _mockContext.Setup(c => c.CalculationLogs).Returns(mockDbSet.Object);

        // Act
        await _service.LogCalculation(calculationModel);

        // Assert
        _mockContext.Verify(c => c.CalculationLogs.AddAsync(It.IsAny<CalculationLog>(), default), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        _mockLoggingService.Verify(log => log.LogInformationAsync("Saving {0} to the database...", nameof(calculationModel)), Times.Once);
        _mockLoggingService.Verify(log => log.LogInformationAsync("Saving changes to the database..."), Times.Once);
        _mockLoggingService.Verify(log => log.LogInformationAsync("Changes saved successfully."), Times.Once);
    }

    [TestMethod]
    public async Task LogCalculation_ShouldLogErrorOnException()
    {
        // Arrange
        var calculationModel = new CalculationModel
        {
            Expression = ExpressionType.Addition,
            FirstOperand = 1,
            SecondOperand = 2,
            Result = 3
        };
        var exception = new Exception("Test exception");
        _mockContext.Setup(c => c.CalculationLogs.AddAsync(It.IsAny<CalculationLog>(), default)).ThrowsAsync(exception);

        // Act & Assert
        var ex = await Assert.ThrowsExceptionAsync<Exception>(() => _service.LogCalculation(calculationModel));
        Assert.AreEqual(exception, ex);
        _mockLoggingService.Verify(log => log.LogErrorAsync("An error occurred while saving the calculation log to the database.", exception), Times.Once);
    }

    private Mock<DbSet<T>> CreateMockDbSet<T>(IList<T> sourceList) where T : class
    {
        var queryable = sourceList.AsQueryable();
        var dbSet = new Mock<DbSet<T>>();
        dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
        dbSet.Setup(d => d.AddAsync(It.IsAny<T>(), default)).Callback<T, CancellationToken>((s, ct) => sourceList.Add(s));
        return dbSet;
    }
}

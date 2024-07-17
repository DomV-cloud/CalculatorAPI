using Calculator.Core.Models;
using Calculator.Data.DatabaseContext;
using Calculator.Data.Interfaces;
using Calculator.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Calculator.Services.Implementation
{
    public class CalculationLoggingService : ICalculationLogging
    {
        private readonly CalculatorDbContext _context;
        private readonly ILogger<CalculationLoggingService> _logger;

        public CalculationLoggingService(
            CalculatorDbContext context,
            ILogger<CalculationLoggingService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<CalculationLog>> GetAllLogs()
        {
            try
            {
                _logger.LogInformation("Fetching all calculation logs from the database...");
                var logs = await _context.CalculationLogs.ToListAsync();
                _logger.LogInformation("Fetched {Count} calculation logs from the database.", logs.Count);
                return logs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching calculation logs from the database.");
                throw;
            }
        }

        public async Task LogCalculation(CalculationModel calculationModel)
        {
            if (calculationModel is null)
            {
                _logger.LogError("calculationModel is null");
                throw new ArgumentNullException(nameof(calculationModel), "Calculation model cannot be null.");
            }

            var log = new CalculationLog
            {
                Expression = calculationModel.Expression,
                FirstOperand = calculationModel.FirstOperand,
                SecondOperand = calculationModel.SecondOperand,
                Result = calculationModel.Result,
                TimeStamp = DateTime.Now,
            };

            try
            {
                _logger.LogInformation("Saving {ModelName} to the database...", nameof(calculationModel));
                await _context.CalculationLogs.AddAsync(log);

                _logger.LogInformation("Saving changes to the database...");
                await _context.SaveChangesAsync();
                _logger.LogInformation("Changes saved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the calculation log to the database.");
                throw;
            }
        }
    }
}

using Calculator.Core.Models;
using Calculator.Data.DatabaseContext;
using Calculator.Data.Interfaces;
using Calculator.Data.Models;
using Calculator.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Calculator.Services.Implementation
{
    public class CalculationLoggingService : ICalculationLogging
    {
        private readonly CalculatorDbContext _context;
        private readonly ILoggingService _loggingService;

        public CalculationLoggingService(
            CalculatorDbContext context,
            ILoggingService loggingService)
        {
            _context = context;
            _loggingService = loggingService;
        }

        public async Task<IEnumerable<CalculationLog>> GetAllLogs()
        {
            try
            {
                await _loggingService.LogInformationAsync("Fetching all calculation logs from the database...");
                var logs = _context.CalculationLogs
                    .OrderByDescending( x => x.TimeStamp)
                    .ToList();
                await _loggingService.LogInformationAsync("Fetched {0} calculation logs from the database.", logs.Count);
                return logs;
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync("An error occurred while fetching calculation logs from the database.", ex);
                throw;
            }
        }

        public async Task LogCalculation(CalculationModel calculationModel)
        {
            if (calculationModel is null)
            {
                await _loggingService.LogErrorAsync("calculationModel is null");
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
                await _loggingService.LogInformationAsync("Saving {0} to the database...", nameof(calculationModel));
                await _context.CalculationLogs.AddAsync(log);

                await _loggingService.LogInformationAsync("Saving changes to the database...");
                await _context.SaveChangesAsync();
                await _loggingService.LogInformationAsync("Changes saved successfully.");
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync("An error occurred while saving the calculation log to the database.", ex);
                throw;
            }
        }
    }
}

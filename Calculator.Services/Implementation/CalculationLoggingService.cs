using Calculator.Core.Models;
using Calculator.Data.DatabaseContext;
using Calculator.Data.Interfaces;
using Calculator.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculator.Services.Implementation
{
    public class CalculationLoggingService : ICalculationLogging
    {
        private readonly CalculatorDbContext _context;

        public CalculationLoggingService(CalculatorDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CalculationLog>> GetAllLogs()
        {
            return await _context.CalculationLogs.ToListAsync();
        }

        public async Task LogCalculation(CalcaulationModel calcaulationModel)
        {
            ArgumentNullException.ThrowIfNull(calcaulationModel);

            var log = new CalculationLog
            {
                Expression = calcaulationModel.Expression,
                FirstOperand = calcaulationModel.FirstOperand,
                SecondOperand = calcaulationModel.SecondOperand,
                Result = calcaulationModel.Result,
                TimeStamp = DateTime.Now,
            };

            await _context.CalculationLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}

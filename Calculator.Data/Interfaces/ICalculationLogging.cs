using Calculator.Core.Models;
using Calculator.Data.Models;

namespace Calculator.Data.Interfaces
{
    public interface ICalculationLogging
    {
        public Task LogCalculation(CalcaulationModel calcaulationModel);
        Task<IEnumerable<CalculationLog>> GetAllLogs();
    }
}

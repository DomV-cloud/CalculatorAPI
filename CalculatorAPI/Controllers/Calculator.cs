using Microsoft.AspNetCore.Mvc;
using Calculator.Infrastructure.Interfaces;
using System;
using System.Threading.Tasks;
using Calculator.Core.Interfaces;
using Calculator.Data.Interfaces;
using Calculator.Core.Models;
using Calculator.Infrastructure.Services;

namespace Calculator.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculationLogging _calculationLogging;
        private readonly ICalculationService _calculationService;
        private readonly ILoggingService _loggingService;

        public CalculatorController(
            ICalculationLogging calculationLogging,
            ICalculationService calculationService,
            ILoggingService loggingService)
        {
            _calculationLogging = calculationLogging;
            _calculationService = calculationService;
            _loggingService = loggingService;
            _calculationService.OnError += SendError;
        }

        private void SendError(Exception ex)
        {
            _loggingService.LogErrorAsync($"Exception with message {ex.Message} was thrown ");
        }

        [HttpPost("calculate", Name = "calculate")]
        public async Task<IActionResult> Calculate(ExpressionType expressionType, double firstOperand, double secondOperand, bool returnInteger)
        {
            try
            {
                await _loggingService.LogInformationAsync("Starting calculation for expression: {0}, operands: {1}, {2}", expressionType, firstOperand, secondOperand);
                var calculation = _calculationService.Calculate(expressionType, firstOperand, secondOperand, returnInteger);

                await _loggingService.LogInformationAsync("Saving result to database...");
                await _calculationLogging.LogCalculation(calculation);

                return Ok(calculation);
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync("An error occurred during calculation. Exception: {0}", ex.Message);
                return StatusCode(500, "An error occurred during calculation.");
            }
        }

        [HttpGet("logs", Name = "logs")]
        public async Task<IActionResult> GetAllLogs()
        {
            try
            {
                await _loggingService.LogInformationAsync("Fetching all calculation logs...");
                var allLogs = await _calculationLogging.GetAllLogs();
                await _loggingService.LogInformationAsync("Fetched {0} calculation logs from the database.", allLogs.Count());
                return Ok(allLogs);
            }
            catch (Exception ex)
            {
                await _loggingService.LogErrorAsync("An error occurred while fetching calculation logs. Exception: {0}", ex.Message);
                return StatusCode(500, "An error occurred while fetching logs.");
            }
        }
    }
}

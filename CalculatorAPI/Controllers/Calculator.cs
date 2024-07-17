using Calculator.Core.Interfaces;
using Calculator.Core.Models;
using Calculator.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CalculatorAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculationLogging _calculationLogging;
        private readonly ICalculationService _calculationService;
        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(
            ICalculationLogging calculationLogging,
            ICalculationService calculationService,
            ILogger<CalculatorController> logger)
        {
            _calculationLogging = calculationLogging;
            _calculationService = calculationService;
            _logger = logger;
        }

        [HttpPost("calculate", Name = "calculate")]
        public async Task<IActionResult> Calculate(ExpressionType expressionType, double firstOperand, double secondOperand, bool returnInteger)
        {
            try
            {
                _logger.LogInformation("Starting calculation for expression: {ExpressionType}, operands: {FirstOperand}, {SecondOperand}", expressionType, firstOperand, secondOperand);
                var calculation = _calculationService.Calculate(expressionType, firstOperand, secondOperand, returnInteger);

                _logger.LogInformation("Calculation successful. Result: {Result}", calculation.Result);
                _logger.LogInformation("Saving result to database...");
                await _calculationLogging.LogCalculation(calculation);
                _logger.LogInformation("Result saved to database successfully.");

                return Ok(calculation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating the expression: {ExpressionType}, operands: {FirstOperand}, {SecondOperand}", expressionType, firstOperand, secondOperand);
                return StatusCode(500, "An error occurred during calculation.");
            }
        }

        [HttpGet("logs", Name = "logs")]
        public async Task<IActionResult> GetAllLogs()
        {
            try
            {
                _logger.LogInformation("Fetching all calculation logs...");
                var allLogs = await _calculationLogging.GetAllLogs();
                _logger.LogInformation("Fetched {Count} calculation logs successfully.", allLogs.Count());
                return Ok(allLogs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching calculation logs.");
                return StatusCode(500, "An error occurred while fetching logs.");
            }
        }
    }
}

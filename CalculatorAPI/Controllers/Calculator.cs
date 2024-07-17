using Calculator.Core.Interfaces;
using Calculator.Core.Models;
using Calculator.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorAPI.Controllers
{
    public class Calculator : Controller
    {
        private readonly ICalculationLogging _calculationLogging;
        private readonly ICalculationService _caltulationService;

        public Calculator(ICalculationLogging calculationLogging, ICalculationService caltulationService)
        {
            _calculationLogging = calculationLogging;
            _caltulationService = caltulationService;
        }

        [HttpPost("calculate", Name = "calculate")]
        public async Task<IActionResult> Calculate(ExpressionType expressionType, double firstOperand, double secondOperand, bool returnInteger)
        {
            try
            {
                var calculation = _caltulationService.Calculate(expressionType, firstOperand, secondOperand, returnInteger);
                await _calculationLogging.LogCalculation(calculation);
                return Ok(calculation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Vyskytla se chyba při výpočtu.");
            }
        }

        [HttpGet("logs", Name = "logs")]
        public async Task<IActionResult> GetAllLogs()
        {
            try
            {
                var allLogs = await _calculationLogging.GetAllLogs();
                return Ok(allLogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Vyskytla se chyba při výpočtu.");
            }
        }
    }
}

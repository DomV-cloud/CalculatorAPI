using Calculator.Core.Interfaces;
using Calculator.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorAPI.Controllers
{
    public class Calculator : Controller
    {
        private readonly ICalculationLogging _calculationLogging;
        private readonly ICaltulationService _caltulationService;

        public Calculator(ICalculationLogging calculationLogging, ICaltulationService caltulationService)
        {
            _calculationLogging = calculationLogging;
            _caltulationService = caltulationService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

using Calculator.Core.Interfaces;
using Calculator.Core.Models;
using Calculator.Infrastructure.Interfaces;

namespace Calculator.Services.Implementation
{
    public class CalculationService : ICalculationService
    {
        public event Action<Exception>? OnError;
        private readonly ILoggingService _loggingService;
        private double result = 0;

        public CalculationService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public CalculationModel Calculate(ExpressionType expression, double firstOperand, double secondOperand, bool returnInteger)
        {
            if (!Enum.IsDefined(typeof(ExpressionType), expression))
            {
                _loggingService.LogErrorAsync("Invalid value for {0}", nameof(expression));
                throw new ArgumentException($"Invalid value for {nameof(expression)}.", nameof(expression));
            }

            try
            {
                _loggingService.LogInformationAsync("Performing calculation for expression: {0}, operands: {1}, {2}", expression, firstOperand, secondOperand);

                result = expression switch
                {
                    ExpressionType.Addition => firstOperand + secondOperand,
                    ExpressionType.Subtraction => firstOperand - secondOperand, 
                    ExpressionType.Multiplication => firstOperand * secondOperand,
                    ExpressionType.Division when secondOperand != 0 => firstOperand / secondOperand,
                    ExpressionType.Division => throw new DivideByZeroException("Cannot divide by zero"),
                    _ => throw new InvalidOperationException("Unsupported operation")
                };

                SetReturnIntegers(returnInteger);

                _loggingService.LogInformationAsync("Calculation successful. Result: {0}", result);

                return new CalculationModel
                {
                    Expression = expression,
                    FirstOperand = firstOperand,
                    SecondOperand = secondOperand,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                _loggingService.LogErrorAsync("An error occurred during the calculation for expression: {0}, operands: {1}, {2}", ex, expression, firstOperand, secondOperand);
                OnError?.Invoke(ex);
                throw;
            }
        }

        public void SetReturnIntegers(bool returnInteger)
        {
            if (returnInteger)
            {
                result = Math.Round(result);
            }
        }
    }
}

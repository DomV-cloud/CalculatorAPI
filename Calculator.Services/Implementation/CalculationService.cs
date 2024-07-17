using Calculator.Core.Interfaces;
using Calculator.Core.Models;
using Microsoft.Extensions.Logging;
using System;

namespace Calculator.Services.Implementation
{
    public class CalculationService : ICalculationService
    {
        public event Action<Exception>? OnError;
        private readonly ILogger<CalculationService> _logger;
        private double result = 0;

        public CalculationService(ILogger<CalculationService> logger)
        {
            _logger = logger;
        }

        public CalculationModel Calculate(ExpressionType expression, double firstOperand, double secondOperand, bool returnInteger)
        {
            if (!Enum.IsDefined(typeof(ExpressionType), expression))
            {
                _logger.LogError("Invalid value for {Expression}", nameof(expression));
                throw new ArgumentException($"Invalid value for {nameof(expression)}.", nameof(expression));
            }

            try
            {
                _logger.LogInformation("Performing calculation for expression: {Expression}, operands: {FirstOperand}, {SecondOperand}", expression, firstOperand, secondOperand);

                result = expression switch
                {
                    ExpressionType.Addition => firstOperand + secondOperand,
                    ExpressionType.Substraction => firstOperand - secondOperand,
                    ExpressionType.Multiplication => firstOperand * secondOperand,
                    ExpressionType.Division when secondOperand != 0 => firstOperand / secondOperand,
                    ExpressionType.Division => throw new DivideByZeroException("Cannot divide by zero"),
                    _ => throw new InvalidOperationException("Unsupported operation")
                };

                SetReturnIntegers(returnInteger);

                _logger.LogInformation("Calculation successful. Result: {Result}", result);

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
                _logger.LogError(ex, "An error occurred during the calculation for expression: {Expression}, operands: {FirstOperand}, {SecondOperand}", expression, firstOperand, secondOperand);
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

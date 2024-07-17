using Calculator.Core.Interfaces;
using Calculator.Core.Models;

namespace Calculator.Services.Implementation
{
    public class CalculationService : ICalculationService
    {
        public event Action<Exception> OnError;
        double result = 0;

        public CalculationModel Calculate(ExpressionType expression, double firstOperand, double secondOperand, bool returnInteger)
        {
            if (!Enum.IsDefined(typeof(ExpressionType), expression))
            {
                throw new ArgumentException($"Invalid value for {nameof(expression)}.", nameof(expression));
            }

            try
            {
                // faster way how to use switch
                double result = expression switch
                {
                    ExpressionType.Addition => firstOperand + secondOperand,
                    ExpressionType.Substraction => firstOperand - secondOperand,
                    ExpressionType.Multiplication => firstOperand * secondOperand,
                    ExpressionType.Division when secondOperand != 0 => firstOperand / secondOperand,
                    ExpressionType.Division => throw new DivideByZeroException("Cannot divide by zero"),
                    _ => throw new InvalidOperationException("Unsupported operation")
                };

                SetReturnIntegers(returnInteger);

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

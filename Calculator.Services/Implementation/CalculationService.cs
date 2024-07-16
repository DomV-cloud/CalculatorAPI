using Calculator.Core.Interfaces;
using Calculator.Core.Models;

namespace Calculator.Services.Implementation
{
    public class CalculationService : ICaltulationService
    {
        public event Action<Exception> OnError;
        private bool returnIntegers = false;
        double result = 0;

        public CalcaulationModel Calculate(ExpressionType expression, double firstOperand, double secondOperand)
        {
            try
            {
                switch (expression)
                {
                    case ExpressionType.Addition:
                        result = firstOperand + secondOperand;
                        break;
                    case ExpressionType.Substraction:
                        result = firstOperand - secondOperand;
                        break;
                    case ExpressionType.Multiplication:
                        result = firstOperand * secondOperand;
                        break;
                    case ExpressionType.Division:
                        if (secondOperand == 0)
                        {
                            throw new DivideByZeroException("Cannot divide by zero");
                        }
                        result = firstOperand / secondOperand;
                        break;
                }

                if (returnIntegers)
                {
                    //SetReturnIntegers();
                }

                return new CalcaulationModel
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

        public void SetReturnIntegers(bool returnIntegers)
        {
            if (returnIntegers)
            {
                result = Math.Round(result); // oddělit ještě jednou servisou...
            }
        }
    }
}

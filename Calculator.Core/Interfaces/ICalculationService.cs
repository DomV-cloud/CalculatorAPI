using Calculator.Core.Models;

namespace Calculator.Core.Interfaces
{
    public interface ICalculationService
    {
        event Action<Exception> OnError;
        public void SetReturnIntegers(bool returnInteger);
        public CalculationModel Calculate(ExpressionType expression, double firstOperand, double secondOperand, bool returnIntegers);
    }
}

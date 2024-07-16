using Calculator.Core.Models;

namespace Calculator.Core.Interfaces
{
    public interface ICaltulationService
    {
        event Action<Exception> OnError;
        public void SetReturnIntegers(bool returnIntegers);
        public CalcaulationModel Calculate(ExpressionType expression, double firstOperand, double secondOperand);
    }
}

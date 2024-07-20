using Calculator.Core.Models;

namespace CalculatorAPI.DTO
{
    public record CalculationRequestDto
    {
        public ExpressionType ExpressionType { get; set; }
        public double FirstOperand { get; set; }
        public double SecondOperand { get; set; }
        public bool ReturnInteger { get; set; }
    };
}

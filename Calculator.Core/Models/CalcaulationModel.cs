namespace Calculator.Core.Models
{
    public class CalcaulationModel
    {
        public ExpressionType Expression { get; set; }
        public double FirstOperand { get; set; }
        public double SecondOperand { get; set; }
        public double Result { get; set; }
    }

    public enum ExpressionType
    {
        Addition,
        Substraction,
        Multiplication,
        Division
    }
}

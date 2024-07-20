using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Calculator.Core.Models
{
    public class CalculationModel
    {
        public ExpressionType Expression { get; set; }
        public double FirstOperand { get; set; }
        public double SecondOperand { get; set; }
        public double Result { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ExpressionType
    {
        [Display(Name = "+")]
        Addition,

        [Display(Name = "-")]
        Subtraction,

        [Display(Name = "*")]
        Multiplication,

        [Display(Name = "/")]
        Division
    }
}

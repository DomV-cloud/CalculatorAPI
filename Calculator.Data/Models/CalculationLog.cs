
using Calculator.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calculator.Data.Models
{
    public class CalculationLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; } = Guid.NewGuid();

        public ExpressionType Expression { get; set; }

        public double FirstOperand { get; set; }

        public double SecondOperand { get; set; }
        
        public double Result { get; set; }

        public DateTime TimeStamp { get; set; }

    }
}

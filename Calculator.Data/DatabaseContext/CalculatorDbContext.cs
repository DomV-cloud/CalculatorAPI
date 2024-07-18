using Calculator.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Calculator.Data.DatabaseContext
{
    public class CalculatorDbContext : DbContext
    {
        public virtual DbSet<CalculationLog> CalculationLogs { get; set; }

        public CalculatorDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}

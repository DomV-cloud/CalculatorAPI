using Calculator.Core.Models;
using Calculator.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Calculator.Data.DatabaseContext
{
    public class CalculatorDbContext : DbContext
    {
        public virtual DbSet<CalculationLog> CalculationLogs { get; set; }

        public CalculatorDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalculationLog>()
                .Property(e => e.Expression)
                .HasConversion(new EnumToStringConverter<ExpressionType>());
        }
    }
}


using Calculator.Core.Interfaces;
using Calculator.Data.DatabaseContext;
using Calculator.Data.Interfaces;
using Calculator.Services.Implementation;
using Microsoft.EntityFrameworkCore;

namespace CalculatorAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddDbContext<CalculatorDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));

            builder.Services.AddScoped<ICalculationLogging, CalculationLoggingService>();
            builder.Services.AddScoped<ICalculationService, CalculationService>();
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

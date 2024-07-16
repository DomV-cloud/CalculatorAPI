﻿// <auto-generated />
using System;
using Calculator.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Calculator.Data.Migrations
{
    [DbContext(typeof(CalculatorDbContext))]
    partial class CalculatorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Calculator.Data.Models.CalculationLog", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Expression")
                        .HasColumnType("int");

                    b.Property<double>("FirstOperand")
                        .HasColumnType("float");

                    b.Property<double>("Result")
                        .HasColumnType("float");

                    b.Property<double>("SecondOperand")
                        .HasColumnType("float");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Guid");

                    b.ToTable("CalculationLogs");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DragoAnt.EntityFrameworkCore.Data.Main;

namespace DragoAnt.EntityFrameworkCore.DbContext.Main.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20220413194856_AddCurrencyData")]
    partial class AddCurrencyData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DragoAnt.EntityFrameworkCore.Data.Main.Currency", b =>
                {
                    b.Property<string>("Iso3LetterCode")
                        .HasMaxLength(3)
                        .IsUnicode(false)
                        .HasColumnType("char(3)")
                        .IsFixedLength(true);

                    b.Property<byte>("DecimalDigits")
                        .HasColumnType("tinyint");

                    b.Property<string>("Description")
                        .HasMaxLength(150)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(150)");

                    b.Property<int>("IsoNumericCode")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Iso3LetterCode");

                    b.ToTable("Currency");

                    b.HasData(
                        new
                        {
                            Iso3LetterCode = "TST",
                            DecimalDigits = (byte)2,
                            Description = "Test currency",
                            IsoNumericCode = 1,
                            Type = 1
                        },
                        new
                        {
                            Iso3LetterCode = "TS2",
                            DecimalDigits = (byte)2,
                            Description = "Test currency 2",
                            IsoNumericCode = 2,
                            Type = 1
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

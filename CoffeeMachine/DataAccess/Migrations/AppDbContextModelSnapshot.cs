﻿// <auto-generated />
using System;
using CoffeeMachine.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CoffeeMachine.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CoffeeMachine.DataAccess.Entites.CoffeeStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("RefillDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("CoffeeStocks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Quantity = 5,
                            RefillDate = new DateTimeOffset(new DateTime(2024, 9, 28, 10, 25, 49, 761, DateTimeKind.Unspecified).AddTicks(157), new TimeSpan(0, 12, 0, 0, 0))
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

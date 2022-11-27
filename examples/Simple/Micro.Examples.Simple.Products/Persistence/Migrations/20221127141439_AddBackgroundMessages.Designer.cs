﻿// <auto-generated />
using System;
using Micro.Examples.Simple.Products.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Micro.Examples.Simple.Products.Persistence.Migrations
{
    [DbContext(typeof(ProductsDbContext))]
    [Migration("20221127141439_AddBackgroundMessages")]
    partial class AddBackgroundMessages
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Micro.BackgroundJobs.EntityFrameworkCore.Persistence.BackgroundJob", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("InvisibleUntil")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedAt")
                        .HasColumnType("datetime2");

                    b.Property<long?>("ProcessingDuration")
                        .HasColumnType("bigint");

                    b.Property<string>("Queue")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("RetryAttempt")
                        .HasColumnType("int");

                    b.Property<int>("RetryMaxCount")
                        .HasColumnType("int");

                    b.Property<string>("ServerId")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("State", "ProcessedAt");

                    b.HasIndex("State", "Queue", "InvisibleUntil", "CreatedAt");

                    b.ToTable("BackgroundJobs", "Micro");
                });

            modelBuilder.Entity("Micro.Examples.Simple.Products.Domain.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
﻿// <auto-generated />
using System;
using LimbusIdentityApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LimbusIdentityApi.Migrations
{
    [DbContext(typeof(IdentityDbContext))]
    [Migration("20241010204710_LessNullables")]
    partial class LessNullables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LimbusIdentityApi.Data.Identity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DefenseLevel")
                        .HasColumnType("int");

                    b.Property<string>("Fatal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Health")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ineffective")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxSpeed")
                        .HasColumnType("int");

                    b.Property<int>("MinSpeed")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sinner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Identities");
                });

            modelBuilder.Entity("LimbusIdentityApi.Data.Passive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cost")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdentityId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Support")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId");

                    b.ToTable("Passives");
                });

            modelBuilder.Entity("LimbusIdentityApi.Data.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CoinEffects")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IdentityId")
                        .HasColumnType("int");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxRoll")
                        .HasColumnType("int");

                    b.Property<int>("MinRoll")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OffenseLevel")
                        .HasColumnType("int");

                    b.Property<string>("Sin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SkillEffect")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdentityId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("LimbusIdentityApi.Data.Passive", b =>
                {
                    b.HasOne("LimbusIdentityApi.Data.Identity", null)
                        .WithMany("Passives")
                        .HasForeignKey("IdentityId");
                });

            modelBuilder.Entity("LimbusIdentityApi.Data.Skill", b =>
                {
                    b.HasOne("LimbusIdentityApi.Data.Identity", null)
                        .WithMany("Skills")
                        .HasForeignKey("IdentityId");
                });

            modelBuilder.Entity("LimbusIdentityApi.Data.Identity", b =>
                {
                    b.Navigation("Passives");

                    b.Navigation("Skills");
                });
#pragma warning restore 612, 618
        }
    }
}

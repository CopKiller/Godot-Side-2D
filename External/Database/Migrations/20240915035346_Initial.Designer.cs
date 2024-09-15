﻿// <auto-generated />
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240915035346_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0-rc.1.24451.1");

            modelBuilder.Entity("Side2D.Models.AccountModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Side2D.Models.Player.Attributes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Agility")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Defense")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Intelligence")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Strength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Willpower")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Attributes");
                });

            modelBuilder.Entity("Side2D.Models.Player.Vitals", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Health")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Mana")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxHealth")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxMana")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Vitals");
                });

            modelBuilder.Entity("Side2D.Models.PlayerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AccountModelId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AttributesId")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Direction")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<float>("JumpVelocity")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Speed")
                        .HasColumnType("REAL");

                    b.Property<int>("VitalsId")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Vocation")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AccountModelId");

                    b.HasIndex("AttributesId");

                    b.HasIndex("PositionId");

                    b.HasIndex("VitalsId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Side2D.Models.Vectors.Vector2C", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("X")
                        .HasColumnType("REAL");

                    b.Property<float>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Vector2C");
                });

            modelBuilder.Entity("Side2D.Models.PlayerModel", b =>
                {
                    b.HasOne("Side2D.Models.AccountModel", null)
                        .WithMany("Players")
                        .HasForeignKey("AccountModelId");

                    b.HasOne("Side2D.Models.Player.Attributes", "Attributes")
                        .WithMany()
                        .HasForeignKey("AttributesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Side2D.Models.Vectors.Vector2C", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Side2D.Models.Player.Vitals", "Vitals")
                        .WithMany()
                        .HasForeignKey("VitalsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attributes");

                    b.Navigation("Position");

                    b.Navigation("Vitals");
                });

            modelBuilder.Entity("Side2D.Models.AccountModel", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}

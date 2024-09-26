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
    [Migration("20240925211536_FixDatas")]
    partial class FixDatas
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
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

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

                    b.Property<int>("PlayerModelId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Strength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Willpower")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlayerModelId")
                        .IsUnique();

                    b.ToTable("Attributes");
                });

            modelBuilder.Entity("Side2D.Models.Player.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayerModelId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("X")
                        .HasColumnType("REAL");

                    b.Property<float>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("PlayerModelId")
                        .IsUnique();

                    b.ToTable("Position");
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

                    b.Property<int>("PlayerModelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlayerModelId")
                        .IsUnique();

                    b.ToTable("Vitals");
                });

            modelBuilder.Entity("Side2D.Models.PlayerModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountModelId")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Direction")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<float>("JumpVelocity")
                        .HasColumnType("REAL");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<int>("SlotNumber")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Speed")
                        .HasColumnType("REAL");

                    b.Property<byte>("Vocation")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AccountModelId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Side2D.Models.Player.Attributes", b =>
                {
                    b.HasOne("Side2D.Models.PlayerModel", "PlayerModel")
                        .WithOne("Attributes")
                        .HasForeignKey("Side2D.Models.Player.Attributes", "PlayerModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlayerModel");
                });

            modelBuilder.Entity("Side2D.Models.Player.Position", b =>
                {
                    b.HasOne("Side2D.Models.PlayerModel", "PlayerModel")
                        .WithOne("Position")
                        .HasForeignKey("Side2D.Models.Player.Position", "PlayerModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlayerModel");
                });

            modelBuilder.Entity("Side2D.Models.Player.Vitals", b =>
                {
                    b.HasOne("Side2D.Models.PlayerModel", "PlayerModel")
                        .WithOne("Vitals")
                        .HasForeignKey("Side2D.Models.Player.Vitals", "PlayerModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlayerModel");
                });

            modelBuilder.Entity("Side2D.Models.PlayerModel", b =>
                {
                    b.HasOne("Side2D.Models.AccountModel", "AccountModel")
                        .WithMany("Players")
                        .HasForeignKey("AccountModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountModel");
                });

            modelBuilder.Entity("Side2D.Models.AccountModel", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("Side2D.Models.PlayerModel", b =>
                {
                    b.Navigation("Attributes")
                        .IsRequired();

                    b.Navigation("Position")
                        .IsRequired();

                    b.Navigation("Vitals")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

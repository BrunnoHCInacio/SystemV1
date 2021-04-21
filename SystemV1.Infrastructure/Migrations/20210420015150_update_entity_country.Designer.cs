﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SystemV1.Infrastructure.Data;

namespace SystemV1.Infrastructure.Migrations
{
    [DbContext(typeof(SqlContext))]
    [Migration("20210420015150_update_entity_country")]
    partial class update_entity_country
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("SystemV1.Domain.Entitys.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<string>("Complement")
                        .HasColumnType("text");

                    b.Property<Guid?>("CountryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DateChange")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateRegister")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("District")
                        .HasColumnType("text");

                    b.Property<int>("IdUserChange")
                        .HasColumnType("integer");

                    b.Property<int>("IdUserRegister")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Number")
                        .HasColumnType("text");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("StateId")
                        .HasColumnType("uuid");

                    b.Property<string>("Street")
                        .HasColumnType("text");

                    b.Property<int>("ZipCode")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("CountryId");

                    b.HasIndex("ProviderId");

                    b.HasIndex("StateId");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DateChange")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateRegister")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Document")
                        .HasColumnType("text");

                    b.Property<int>("IdUserChange")
                        .HasColumnType("integer");

                    b.Property<int>("IdUserRegister")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CellPhoneNumber")
                        .HasColumnType("text");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DateChange")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateRegister")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Ddd")
                        .HasColumnType("text");

                    b.Property<string>("Ddi")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<int>("IdUserChange")
                        .HasColumnType("integer");

                    b.Property<int>("IdUserRegister")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<Guid>("ProviderId")
                        .HasColumnType("uuid");

                    b.Property<string>("TypeContact")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.HasIndex("ProviderId");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DateChange")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateRegister")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("IdState")
                        .HasColumnType("uuid");

                    b.Property<int>("IdUserChange")
                        .HasColumnType("integer");

                    b.Property<int>("IdUserRegister")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DateChange")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateRegister")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("IdUserChange")
                        .HasColumnType("integer");

                    b.Property<int>("IdUserRegister")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid?>("ProviderId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.ProductItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DateChange")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateRegister")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("IdUserChange")
                        .HasColumnType("integer");

                    b.Property<int>("IdUserRegister")
                        .HasColumnType("integer");

                    b.Property<string>("ImageZip")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSold")
                        .HasColumnType("boolean");

                    b.Property<string>("Modelo")
                        .HasColumnType("text");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductItem");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Provider", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DateChange")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateRegister")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Document")
                        .HasColumnType("text");

                    b.Property<int>("IdUserChange")
                        .HasColumnType("integer");

                    b.Property<int>("IdUserRegister")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Provider");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.State", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CountryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DateChange")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateRegister")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("IdUserChange")
                        .HasColumnType("integer");

                    b.Property<int>("IdUserRegister")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("State");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Address", b =>
                {
                    b.HasOne("SystemV1.Domain.Entitys.Client", "Client")
                        .WithMany("Addresses")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SystemV1.Domain.Entitys.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.HasOne("SystemV1.Domain.Entitys.Provider", "Provider")
                        .WithMany("Addresses")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SystemV1.Domain.Entitys.State", "State")
                        .WithMany()
                        .HasForeignKey("StateId");

                    b.Navigation("Client");

                    b.Navigation("Country");

                    b.Navigation("Provider");

                    b.Navigation("State");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Contact", b =>
                {
                    b.HasOne("SystemV1.Domain.Entitys.Client", "Client")
                        .WithMany("Contacts")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SystemV1.Domain.Entitys.Provider", "Provider")
                        .WithMany("Contacts")
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Product", b =>
                {
                    b.HasOne("SystemV1.Domain.Entitys.Provider", "Provider")
                        .WithMany()
                        .HasForeignKey("ProviderId");

                    b.Navigation("Provider");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.ProductItem", b =>
                {
                    b.HasOne("SystemV1.Domain.Entitys.Product", "Product")
                        .WithMany("ProductItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.State", b =>
                {
                    b.HasOne("SystemV1.Domain.Entitys.Country", "Country")
                        .WithMany("States")
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Client", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Contacts");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Country", b =>
                {
                    b.Navigation("States");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Product", b =>
                {
                    b.Navigation("ProductItems");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Provider", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Contacts");
                });
#pragma warning restore 612, 618
        }
    }
}

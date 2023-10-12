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
    [Migration("20221219223538_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("SystemV1.Domain.Entitys.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CityId")
                        .HasColumnType("uuid");

                    b.Property<string>("Complement")
                        .HasColumnType("text");

                    b.Property<string>("District")
                        .HasColumnType("text");

                    b.Property<string>("Number")
                        .HasColumnType("text");

                    b.Property<Guid>("PeopleId")
                        .HasColumnType("uuid");

                    b.Property<string>("Street")
                        .HasColumnType("text");

                    b.Property<string>("ZipCode")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("PeopleId");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Audit.EntityAudit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuditedEntity")
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EntityName")
                        .HasColumnType("text");

                    b.Property<Guid>("IdUser")
                        .HasColumnType("uuid");

                    b.Property<int>("TypeOperation")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("EntityAudit");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.City", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("StateId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("StateId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Client", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("PeopleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PeopleId");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CellPhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("Ddd")
                        .HasColumnType("text");

                    b.Property<string>("Ddi")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<Guid>("PeopleId")
                        .HasColumnType("uuid");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<int>("TypeContact")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PeopleId");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.People", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Document")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("TypePeople")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("ProviderId")
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

                    b.Property<string>("ImageZip")
                        .HasColumnType("text");

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

                    b.Property<Guid>("PeopleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PeopleId");

                    b.ToTable("Providers");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.State", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("State");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Address", b =>
                {
                    b.HasOne("SystemV1.Domain.Entitys.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SystemV1.Domain.Entitys.People", "People")
                        .WithMany("Addresses")
                        .HasForeignKey("PeopleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("People");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.City", b =>
                {
                    b.HasOne("SystemV1.Domain.Entitys.State", "State")
                        .WithMany("Cities")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("State");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Client", b =>
                {
                    b.HasOne("SystemV1.Domain.Entitys.People", "People")
                        .WithMany()
                        .HasForeignKey("PeopleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("People");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Contact", b =>
                {
                    b.HasOne("SystemV1.Domain.Entitys.People", "People")
                        .WithMany("Contacts")
                        .HasForeignKey("PeopleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("People");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Product", b =>
                {
                    b.HasOne("SystemV1.Domain.Entitys.Provider", "Provider")
                        .WithMany()
                        .HasForeignKey("ProviderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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

            modelBuilder.Entity("SystemV1.Domain.Entitys.Provider", b =>
                {
                    b.HasOne("SystemV1.Domain.Entitys.People", "People")
                        .WithMany()
                        .HasForeignKey("PeopleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("People");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.State", b =>
                {
                    b.HasOne("SystemV1.Domain.Entitys.Country", "Country")
                        .WithMany("States")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Country", b =>
                {
                    b.Navigation("States");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.People", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Contacts");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.Product", b =>
                {
                    b.Navigation("ProductItems");
                });

            modelBuilder.Entity("SystemV1.Domain.Entitys.State", b =>
                {
                    b.Navigation("Cities");
                });
#pragma warning restore 612, 618
        }
    }
}
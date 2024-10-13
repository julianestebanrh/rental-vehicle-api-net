﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using VehicleRental.Infrastructure;

#nullable disable

namespace VehicleRental.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240721015602_InitialDatabase")]
    partial class InitialDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("VehicleRental.Domain.Rentals.Rental", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime?>("CancelledOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("cancelled_on_utc");

                    b.Property<DateTime?>("CompletedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("completed_on_utc");

                    b.Property<DateTime?>("ConfirmedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("confirmed_on_utc");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on_utc");

                    b.Property<DateTime?>("RejectedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("rejected_on_utc");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid?>("VehicleId")
                        .HasColumnType("uuid")
                        .HasColumnName("vehicle_id");

                    b.HasKey("Id")
                        .HasName("pk_rentals");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_rentals_user_id");

                    b.HasIndex("VehicleId")
                        .HasDatabaseName("ix_rentals_vehicle_id");

                    b.ToTable("rentals", (string)null);
                });

            modelBuilder.Entity("VehicleRental.Domain.Reviews.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Comment")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("comment");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on_utc");

                    b.Property<int?>("Rating")
                        .HasColumnType("integer")
                        .HasColumnName("rating");

                    b.Property<Guid?>("RentalId")
                        .HasColumnType("uuid")
                        .HasColumnName("rental_id");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid?>("VehicleId")
                        .HasColumnType("uuid")
                        .HasColumnName("vehicle_id");

                    b.HasKey("Id")
                        .HasName("pk_reviews");

                    b.HasIndex("RentalId")
                        .HasDatabaseName("ix_reviews_rental_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_reviews_user_id");

                    b.HasIndex("VehicleId")
                        .HasDatabaseName("ix_reviews_vehicle_id");

                    b.ToTable("reviews", (string)null);
                });

            modelBuilder.Entity("VehicleRental.Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .HasMaxLength(2000)
                        .HasColumnType("character varying(2000)")
                        .HasColumnName("password");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_users_email");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("VehicleRental.Domain.Vehicles.Vehicle", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int[]>("Amenities")
                        .IsRequired()
                        .HasColumnType("integer[]")
                        .HasColumnName("amenities");

                    b.Property<DateTime?>("LastRentedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_rented_on_utc");

                    b.Property<string>("Model")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("model");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<string>("Vin")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("vin");

                    b.HasKey("Id")
                        .HasName("pk_vehicles");

                    b.ToTable("vehicles", (string)null);
                });

            modelBuilder.Entity("VehicleRental.Domain.Rentals.Rental", b =>
                {
                    b.HasOne("VehicleRental.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_rentals_users_user_id");

                    b.HasOne("VehicleRental.Domain.Vehicles.Vehicle", null)
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .HasConstraintName("fk_rentals_vehicles_vehicle_id");

                    b.OwnsOne("VehicleRental.Domain.Rentals.DateRange", "Duration", b1 =>
                        {
                            b1.Property<Guid>("RentalId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<DateOnly>("End")
                                .HasColumnType("date")
                                .HasColumnName("duration_end");

                            b1.Property<DateOnly>("Start")
                                .HasColumnType("date")
                                .HasColumnName("duration_start");

                            b1.HasKey("RentalId");

                            b1.ToTable("rentals");

                            b1.WithOwner()
                                .HasForeignKey("RentalId")
                                .HasConstraintName("fk_rentals_rentals_id");
                        });

                    b.OwnsOne("VehicleRental.Domain.Shared.Money", "AmenitiesUpCharge", b1 =>
                        {
                            b1.Property<Guid>("RentalId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric")
                                .HasColumnName("amenities_up_charge_amount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("amenities_up_charge_currency");

                            b1.HasKey("RentalId");

                            b1.ToTable("rentals");

                            b1.WithOwner()
                                .HasForeignKey("RentalId")
                                .HasConstraintName("fk_rentals_rentals_id");
                        });

                    b.OwnsOne("VehicleRental.Domain.Shared.Money", "MaintenanceFee", b1 =>
                        {
                            b1.Property<Guid>("RentalId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric")
                                .HasColumnName("maintenance_fee_amount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("maintenance_fee_currency");

                            b1.HasKey("RentalId");

                            b1.ToTable("rentals");

                            b1.WithOwner()
                                .HasForeignKey("RentalId")
                                .HasConstraintName("fk_rentals_rentals_id");
                        });

                    b.OwnsOne("VehicleRental.Domain.Shared.Money", "PriceForPeriod", b1 =>
                        {
                            b1.Property<Guid>("RentalId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric")
                                .HasColumnName("price_for_period_amount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("price_for_period_currency");

                            b1.HasKey("RentalId");

                            b1.ToTable("rentals");

                            b1.WithOwner()
                                .HasForeignKey("RentalId")
                                .HasConstraintName("fk_rentals_rentals_id");
                        });

                    b.OwnsOne("VehicleRental.Domain.Shared.Money", "TotalPrice", b1 =>
                        {
                            b1.Property<Guid>("RentalId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric")
                                .HasColumnName("total_price_amount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("total_price_currency");

                            b1.HasKey("RentalId");

                            b1.ToTable("rentals");

                            b1.WithOwner()
                                .HasForeignKey("RentalId")
                                .HasConstraintName("fk_rentals_rentals_id");
                        });

                    b.Navigation("AmenitiesUpCharge");

                    b.Navigation("Duration");

                    b.Navigation("MaintenanceFee");

                    b.Navigation("PriceForPeriod");

                    b.Navigation("TotalPrice");
                });

            modelBuilder.Entity("VehicleRental.Domain.Reviews.Review", b =>
                {
                    b.HasOne("VehicleRental.Domain.Rentals.Rental", null)
                        .WithMany()
                        .HasForeignKey("RentalId")
                        .HasConstraintName("fk_reviews_rentals_rental_id");

                    b.HasOne("VehicleRental.Domain.Users.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_reviews_users_user_id");

                    b.HasOne("VehicleRental.Domain.Vehicles.Vehicle", null)
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .HasConstraintName("fk_reviews_vehicles_vehicle_id");
                });

            modelBuilder.Entity("VehicleRental.Domain.Vehicles.Vehicle", b =>
                {
                    b.OwnsOne("VehicleRental.Domain.Vehicles.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("VehicleId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("address_city");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("address_country");

                            b1.Property<string>("Department")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("address_department");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("address_street");

                            b1.Property<string>("ZipCode")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("address_zip_code");

                            b1.HasKey("VehicleId");

                            b1.ToTable("vehicles");

                            b1.WithOwner()
                                .HasForeignKey("VehicleId")
                                .HasConstraintName("fk_vehicles_vehicles_id");
                        });

                    b.OwnsOne("VehicleRental.Domain.Shared.Money", "MaintenanceFee", b1 =>
                        {
                            b1.Property<Guid>("VehicleId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric")
                                .HasColumnName("maintenance_fee_amount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("maintenance_fee_currency");

                            b1.HasKey("VehicleId");

                            b1.ToTable("vehicles");

                            b1.WithOwner()
                                .HasForeignKey("VehicleId")
                                .HasConstraintName("fk_vehicles_vehicles_id");
                        });

                    b.OwnsOne("VehicleRental.Domain.Shared.Money", "Price", b1 =>
                        {
                            b1.Property<Guid>("VehicleId")
                                .HasColumnType("uuid")
                                .HasColumnName("id");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric")
                                .HasColumnName("price_amount");

                            b1.Property<string>("Currency")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("price_currency");

                            b1.HasKey("VehicleId");

                            b1.ToTable("vehicles");

                            b1.WithOwner()
                                .HasForeignKey("VehicleId")
                                .HasConstraintName("fk_vehicles_vehicles_id");
                        });

                    b.Navigation("Address");

                    b.Navigation("MaintenanceFee");

                    b.Navigation("Price");
                });
#pragma warning restore 612, 618
        }
    }
}

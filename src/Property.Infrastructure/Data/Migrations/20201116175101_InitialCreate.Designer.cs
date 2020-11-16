﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Property.Infrastructure.Data;

namespace Data.Migrations
{
    [DbContext(typeof(PropertyContext))]
    [Migration("20201116175101_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Property.Core.Property", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID")
                        .HasName("PropertyID");

                    b.ToTable("tbl_Property");
                });

            modelBuilder.Entity("Property.Core.Property", b =>
                {
                    b.OwnsOne("Property.Core.PropertyReference", "Reference", b1 =>
                        {
                            b1.Property<int>("PropertyID")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .UseIdentityColumn();

                            b1.Property<string>("Reference")
                                .IsRequired()
                                .HasMaxLength(250)
                                .HasColumnType("nvarchar(250)")
                                .HasColumnName("PropertyReference");

                            b1.HasKey("PropertyID");

                            b1.ToTable("tbl_Property");

                            b1.WithOwner()
                                .HasForeignKey("PropertyID");
                        });

                    b.Navigation("Reference");
                });
#pragma warning restore 612, 618
        }
    }
}
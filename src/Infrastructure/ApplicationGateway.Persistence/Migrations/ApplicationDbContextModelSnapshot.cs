﻿// <auto-generated />
using System;
using ApplicationGateway.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApplicationGateway.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ApplicationGateway.Domain.Entities.Snapshot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<int>("Id"), null, null, null, null, null, null);

                    b.Property<string>("Comment")
                        .HasMaxLength(450)
                        .IsUnicode(false)
                        .HasColumnType("character varying(450)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(450)
                        .IsUnicode(false)
                        .HasColumnType("character varying(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Gateway")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("character varying(20)");

                    b.Property<bool?>("IsActive")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("JsonData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(450)
                        .IsUnicode(false)
                        .HasColumnType("character varying(450)");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ObjectKey")
                        .HasColumnType("uuid");

                    b.Property<string>("ObjectName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .IsUnicode(false)
                        .HasColumnType("character varying(40)");

                    b.HasKey("Id");

                    b.ToTable("Snapshot", (string)null);
                });

            modelBuilder.Entity("ApplicationGateway.Domain.TykData.Transformers", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Gateway")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModifiedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TemplateName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TransformerTemplate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Transformers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("b0788d2f-8003-43c1-92a4-edc76a7c5dde"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Gateway = "TykGateway",
                            TemplateName = "CreateApi",
                            TransformerTemplate = "{\n  \"name\": \"#valueof(Name)\",\n  \"use_keyless\": true,\n  \"active\": true,\n  \"proxy\": {\n    \"listen_path\": \"#valueof(ListenPath)\",\n    \"target_url\": \"#valueof(TargetUrl)\",\n    \"strip_listen_path\": true\n  },\n  \"version_data\": {\n    \"not_versioned\": true,\n    \"default_version\": \"Default\",\n    \"versions\": {\n      \"Default\": {\n        \"name\": \"Default\",\n        \"use_extended_paths\": true\n      }\n    }\n  }\n}"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

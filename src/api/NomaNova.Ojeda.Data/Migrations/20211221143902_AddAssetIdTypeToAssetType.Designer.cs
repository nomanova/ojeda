﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NomaNova.Ojeda.Data.Context;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NomaNova.Ojeda.Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20211221143902_AddAssetIdTypeToAssetType")]
    partial class AddAssetIdTypeToAssetType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.AssetIdTypes.AssetIdType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Properties")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("WithManualEntry")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("AssetIdTypes");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.Assets.Asset", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AssetTypeId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AssetTypeId");

                    b.ToTable("Assets");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.Assets.FieldValue", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AssetId")
                        .HasColumnType("text");

                    b.Property<string>("FieldId")
                        .HasColumnType("text");

                    b.Property<string>("FieldSetId")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AssetId");

                    b.HasIndex("FieldId");

                    b.HasIndex("FieldSetId");

                    b.ToTable("FieldValues");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.AssetTypes.AssetType", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AssetIdTypeId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AssetIdTypeId");

                    b.ToTable("AssetTypes");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.AssetTypes.AssetTypeFieldSet", b =>
                {
                    b.Property<string>("FieldSetId")
                        .HasColumnType("text");

                    b.Property<string>("AssetTypeId")
                        .HasColumnType("text");

                    b.Property<long>("Order")
                        .HasColumnType("bigint");

                    b.HasKey("FieldSetId", "AssetTypeId");

                    b.HasIndex("AssetTypeId");

                    b.ToTable("AssetTypeFieldSets");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.Fields.Field", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Properties")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.FieldSets.FieldSet", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("FieldSets");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.FieldSets.FieldSetField", b =>
                {
                    b.Property<string>("FieldId")
                        .HasColumnType("text");

                    b.Property<string>("FieldSetId")
                        .HasColumnType("text");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("boolean");

                    b.Property<long>("Order")
                        .HasColumnType("bigint");

                    b.HasKey("FieldId", "FieldSetId");

                    b.HasIndex("FieldSetId");

                    b.ToTable("FieldSetFields");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.Assets.Asset", b =>
                {
                    b.HasOne("NomaNova.Ojeda.Core.Domain.AssetTypes.AssetType", "AssetType")
                        .WithMany("Assets")
                        .HasForeignKey("AssetTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("AssetType");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.Assets.FieldValue", b =>
                {
                    b.HasOne("NomaNova.Ojeda.Core.Domain.Assets.Asset", "Asset")
                        .WithMany("FieldValues")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NomaNova.Ojeda.Core.Domain.Fields.Field", "Field")
                        .WithMany("FieldValues")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("NomaNova.Ojeda.Core.Domain.FieldSets.FieldSet", "FieldSet")
                        .WithMany("FieldValues")
                        .HasForeignKey("FieldSetId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Asset");

                    b.Navigation("Field");

                    b.Navigation("FieldSet");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.AssetTypes.AssetType", b =>
                {
                    b.HasOne("NomaNova.Ojeda.Core.Domain.AssetIdTypes.AssetIdType", "AssetIdType")
                        .WithMany("AssetTypes")
                        .HasForeignKey("AssetIdTypeId");

                    b.Navigation("AssetIdType");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.AssetTypes.AssetTypeFieldSet", b =>
                {
                    b.HasOne("NomaNova.Ojeda.Core.Domain.AssetTypes.AssetType", "AssetType")
                        .WithMany("AssetTypeFieldSets")
                        .HasForeignKey("AssetTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NomaNova.Ojeda.Core.Domain.FieldSets.FieldSet", "FieldSet")
                        .WithMany("AssetTypeFieldSets")
                        .HasForeignKey("FieldSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssetType");

                    b.Navigation("FieldSet");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.FieldSets.FieldSetField", b =>
                {
                    b.HasOne("NomaNova.Ojeda.Core.Domain.Fields.Field", "Field")
                        .WithMany("FieldSetFields")
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NomaNova.Ojeda.Core.Domain.FieldSets.FieldSet", "FieldSet")
                        .WithMany("FieldSetFields")
                        .HasForeignKey("FieldSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("FieldSet");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.AssetIdTypes.AssetIdType", b =>
                {
                    b.Navigation("AssetTypes");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.Assets.Asset", b =>
                {
                    b.Navigation("FieldValues");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.AssetTypes.AssetType", b =>
                {
                    b.Navigation("AssetTypeFieldSets");

                    b.Navigation("Assets");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.Fields.Field", b =>
                {
                    b.Navigation("FieldSetFields");

                    b.Navigation("FieldValues");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.FieldSets.FieldSet", b =>
                {
                    b.Navigation("AssetTypeFieldSets");

                    b.Navigation("FieldSetFields");

                    b.Navigation("FieldValues");
                });
#pragma warning restore 612, 618
        }
    }
}

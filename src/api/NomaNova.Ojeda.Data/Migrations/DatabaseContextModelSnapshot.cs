﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NomaNova.Ojeda.Data.Context;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace NomaNova.Ojeda.Data.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.FieldSets.FieldSet", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("FieldSets");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.FieldSets.FieldSetField", b =>
                {
                    b.Property<string>("FieldId")
                        .HasColumnType("text");

                    b.Property<string>("FieldSetId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Order")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("FieldId", "FieldSetId");

                    b.HasIndex("FieldSetId");

                    b.ToTable("FieldSetFields");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.Fields.Field", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Fields");
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

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.FieldSets.FieldSet", b =>
                {
                    b.Navigation("FieldSetFields");
                });

            modelBuilder.Entity("NomaNova.Ojeda.Core.Domain.Fields.Field", b =>
                {
                    b.Navigation("FieldSetFields");
                });
#pragma warning restore 612, 618
        }
    }
}

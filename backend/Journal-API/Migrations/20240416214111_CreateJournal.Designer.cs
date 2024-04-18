﻿// <auto-generated />
using System;
using Journal_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Journal_API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240416214111_CreateJournal")]
    partial class CreateJournal
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.3.24172.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Journal_API.Models.Journal", b =>
                {
                    b.Property<Guid>("JournalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string[]>("Categories")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.Property<string>("EntryContent")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("JournalId");

                    b.ToTable("Journals");
                });
#pragma warning restore 612, 618
        }
    }
}

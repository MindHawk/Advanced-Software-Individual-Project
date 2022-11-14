﻿// <auto-generated />
using System;
using ForumServiceDAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ForumServiceAPI.Migrations
{
    [DbContext(typeof(ForumContext))]
    [Migration("20221111132050_second")]
    partial class second
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ForumServiceModels.Forum", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<Guid>("adminId")
                        .HasColumnType("uuid");

                    b.HasKey("Name");

                    b.ToTable("Forums");
                });
#pragma warning restore 612, 618
        }
    }
}

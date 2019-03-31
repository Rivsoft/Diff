﻿// <auto-generated />
using System;
using Diff.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Diff.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20190331134757_InitialDatabase")]
    partial class InitialDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Diff.Data.Models.DiffAnalysis", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Analized");

                    b.Property<byte[]>("Left");

                    b.Property<byte[]>("Right");

                    b.HasKey("Id");

                    b.ToTable("DiffAnalysis");
                });

            modelBuilder.Entity("Diff.Data.Models.DiffSegment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("DiffAnalysisId");

                    b.Property<int>("Length");

                    b.Property<int>("Offset");

                    b.HasKey("Id");

                    b.HasIndex("DiffAnalysisId");

                    b.ToTable("DiffSegment");
                });

            modelBuilder.Entity("Diff.Data.Models.DiffSegment", b =>
                {
                    b.HasOne("Diff.Data.Models.DiffAnalysis")
                        .WithMany("Segments")
                        .HasForeignKey("DiffAnalysisId");
                });
#pragma warning restore 612, 618
        }
    }
}
﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkyHawk.DataMock.Contexts;

#nullable disable

namespace SkyHawk.DataMock.Migrations
{
    [DbContext(typeof(SkyHawkDbContextMock))]
    [Migration("20240310192807_Add string length constraints")]
    partial class Addstringlengthconstraints
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("SkyHawk.Data.Entities.Activity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<int>("ServerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.ToTable("Activities");
                });

            modelBuilder.Entity("SkyHawk.Data.Entities.ServerInstance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan?>("AutoStart")
                        .HasColumnType("date");

                    b.Property<TimeSpan?>("AutoStop")
                        .HasColumnType("date");

                    b.Property<string>("ContainerId")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT")
                        .IsFixedLength();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Image")
                        .HasColumnType("BLOB");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Port")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("SkyHawk.Data.Entities.Snapshot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT")
                        .IsFixedLength();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Snapshots");
                });

            modelBuilder.Entity("SkyHawk.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanMakeSnapshots")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("MaxRunningServers")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxServers")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SkyHawk.Data.Entities.Activity", b =>
                {
                    b.HasOne("SkyHawk.Data.Entities.ServerInstance", "Server")
                        .WithMany("Activities")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Server");
                });

            modelBuilder.Entity("SkyHawk.Data.Entities.ServerInstance", b =>
                {
                    b.HasOne("SkyHawk.Data.Entities.User", "Owner")
                        .WithMany("Servers")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("SkyHawk.Data.Entities.Snapshot", b =>
                {
                    b.HasOne("SkyHawk.Data.Entities.User", "Owner")
                        .WithMany("Shapshots")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("SkyHawk.Data.Entities.ServerInstance", b =>
                {
                    b.Navigation("Activities");
                });

            modelBuilder.Entity("SkyHawk.Data.Entities.User", b =>
                {
                    b.Navigation("Servers");

                    b.Navigation("Shapshots");
                });
#pragma warning restore 612, 618
        }
    }
}

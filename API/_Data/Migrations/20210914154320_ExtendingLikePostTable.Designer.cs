﻿// <auto-generated />
using System;
using API._Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API._Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210914154320_ExtendingLikePostTable")]
    partial class ExtendingLikePostTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("API._Entities.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("KnownAs")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("BLOB");

                    b.Property<string>("Photo")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhotoPublicId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("API._Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Photo")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhotoPublicId")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostContent")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostTitle")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostrName")
                        .HasColumnType("TEXT");

                    b.Property<string>("PostrPhoto")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("API._Entities.UserPostComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CommentTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("PostId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PostrId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PostrName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Read")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserPhoto")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("UsersPostComments");
                });

            modelBuilder.Entity("API._Entities.UserPostLikes", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PostId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LikedTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("PostrId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PostrName")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Read")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserPhoto")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "PostId");

                    b.HasIndex("PostId");

                    b.ToTable("UsersPostLikes");
                });

            modelBuilder.Entity("API._Entities.Post", b =>
                {
                    b.HasOne("API._Entities.AppUser", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("API._Entities.UserPostComment", b =>
                {
                    b.HasOne("API._Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API._Entities.AppUser", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API._Entities.UserPostLikes", b =>
                {
                    b.HasOne("API._Entities.Post", "Post")
                        .WithMany("LikedBy")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("API._Entities.AppUser", "User")
                        .WithMany("LikesPost")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("API._Entities.AppUser", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("LikesPost");

                    b.Navigation("Posts");
                });

            modelBuilder.Entity("API._Entities.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("LikedBy");
                });
#pragma warning restore 612, 618
        }
    }
}

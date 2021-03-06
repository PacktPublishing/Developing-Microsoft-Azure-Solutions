﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Leaderboard
{
    public partial class LeaderboardContext : DbContext
    {
        public LeaderboardContext()
        {
        }

        public LeaderboardContext(DbContextOptions<LeaderboardContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Board> Boards { get; set; }
        public virtual DbSet<BoardUser> BoardUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Board>(entity =>
            {
                entity.ToTable("Board");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Boards)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Board__UserId__60A75C0F");
            });

            modelBuilder.Entity<BoardUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("BoardUser");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(1024);

                entity.Property(e => e.LastWorkout).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

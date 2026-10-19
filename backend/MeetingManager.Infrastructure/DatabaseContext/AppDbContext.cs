using MeetingManager.Domain.Entities;
using MeetingManager.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingManager.Infrastructure.DatabaseContext
{
    public class AppDbContext : IdentityDbContext<
    User,
    Role,
    Guid,
    IdentityUserClaim<Guid>,
    UserRole,
    IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>,
    IdentityUserToken<Guid>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> AppUsers => Set<User>();
        public DbSet<Role> AppRoles => Set<Role>();
        public DbSet<UserRole> AppUserRoles => Set<UserRole>();
        public DbSet<Meeting> Meetings => Set<Meeting>();
        public DbSet<MeetingParticipant> MeetingParticipants => Set<MeetingParticipant>();
        public DbSet<UserAvailability> UserAvailabilities => Set<UserAvailability>();
        public DbSet<UserTimeOff> UserTimeOffs => Set<UserTimeOff>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresEnum<AttendanceStatus>();
            modelBuilder.HasPostgresEnum<Domain.Enums.DayOfWeek>();

            modelBuilder.Ignore<IdentityUserClaim<Guid>>();
            modelBuilder.Ignore<IdentityUserLogin<Guid>>();
            modelBuilder.Ignore<IdentityUserToken<Guid>>();
            modelBuilder.Ignore<IdentityRoleClaim<Guid>>();

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.Ignore(u => u.PhoneNumberConfirmed);
                entity.Ignore(u => u.TwoFactorEnabled);
                entity.Ignore(u => u.PhoneNumber);
                entity.Ignore(u => u.Email);
                entity.Ignore(u => u.EmailConfirmed);
                entity.Ignore(u => u.ConcurrencyStamp);
                entity.Ignore(u => u.SecurityStamp);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("user_roles");
            });

            modelBuilder.Entity<Meeting>(entity =>
            {
                entity.ToTable("meetings");
            });

            modelBuilder.Entity<MeetingParticipant>(entity =>
            {
                entity.ToTable("meeting_participants");
                entity.HasIndex(x => new { x.MeetingId, x.UserId }).IsUnique();
            });

            modelBuilder.Entity<UserAvailability>(entity =>
            {
                entity.ToTable("user_availabilities");
            });

            modelBuilder.Entity<UserTimeOff>(entity =>
            {
                entity.ToTable("user_time_offs");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(w => w.Ignore(CoreEventId.MappedEntityTypeIgnoredWarning));
        }
    }
}

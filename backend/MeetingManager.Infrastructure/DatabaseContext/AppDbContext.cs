using MeetingManager.Domain.Entities;
using MeetingManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MeetingManager.Infrastructure.DatabaseContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<Meeting> Meetings => Set<Meeting>();
        public DbSet<MeetingParticipant> MeetingParticipants => Set<MeetingParticipant>();
        public DbSet<UserAvailability> UserAvailabilities => Set<UserAvailability>();
        public DbSet<UserTimeOff> UserTimeOffs => Set<UserTimeOff>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasPostgresEnum<AttendanceStatus>();
            modelBuilder.HasPostgresEnum<Domain.Enums.DayOfWeek>();

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
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
    }
}

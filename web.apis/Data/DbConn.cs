using common.data.Enums;
using data.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using web.apis.Models;

namespace web.apis
{
    public class DbConn : IdentityDbContext<ApplicationUser>
	{
		public DbConn(DbContextOptions<DbConn> options) : base(options)
		{
		}

        private void OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<LogEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Log || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                var auditEntry = new LogEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.UserId = userId;
                auditEntries.Add(auditEntry);
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.LogType = LogType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.LogType = LogType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.ChangedColumns.Add(propertyName);
                                auditEntry.LogType = LogType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }
                            break;
                    }
                }
            }
            foreach (var auditEntry in auditEntries)
            {
                Logs.Add(auditEntry.ToAudit());
            }
        }

        public virtual async Task<int> SaveChangesAsync(string userId = null)
        {
            OnBeforeSaveChanges(userId);
            var result = await base.SaveChangesAsync();
            return result;
        }

        public virtual async Task<int> SaveChanges(string userId = null)
        {
            OnBeforeSaveChanges(userId);
            var result = await base.SaveChangesAsync();
            return result;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
        }

        private void SeedSystemAsAdmin(ModelBuilder builder)
        {
            var user = new ApplicationUser()
            {
                Id = "bac4fac1-c546-41de-aebc-a14da689a0099",
                FirstName = "System",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Email = "superadmin@prus.com",
                NormalizedUserName = "SUPERADMIN@SYSTEM.DOM",
                NormalizedEmail = "SUPERADMIN@SYSTEM.DOM",
                UserName = "SYSTEMAAA",
                UserRoleEnum = UserRoleEnum.Administrator,
                Department = "ISD"
            };

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            var passwordhash = passwordHasher.HashPassword(user, "Sy5t3m001@");
            user.PasswordHash = passwordhash;

            builder.Entity<ApplicationUser>().HasData(user);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Id = "ADMIN", Name = "ADMIN", ConcurrencyStamp = "1", NormalizedName = "Admin" }
                );
        }

        private void SeedUserRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>() { RoleId = "ADMIN", UserId = "bac4fac1-c546-41de-aebc-a14da689a0099" }
                );
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            this.SeedSystemAsAdmin(builder);
            this.SeedRoles(builder);
            this.SeedUserRoles(builder);
            this.SeedEmailTemplates(builder);           

            base.OnModelCreating(builder);
        }

        private void SeedEmailTemplates(ModelBuilder builder)
        {
            builder.Entity<EmailTemplate>().HasData(new EmailTemplate("new_registration", "Basic API: New User", "<p>Welcome to Basic API <b>{user}</b> —a platform where creativity meets innovation</p>") { Id = 1 });
        }
        
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
        public DbSet<EntityPulse> EntityPulses { get; set; }
    }
}


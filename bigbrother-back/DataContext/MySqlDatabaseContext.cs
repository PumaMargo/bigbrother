using bigbrother_back.Models.DataModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace bigbrother_back.DataContext
{
    public class MySqlDatabaseContext : DatabaseContext
    {
        string ConnectionString { get; init; }

        public MySqlDatabaseContext(DbContextOptions<MySqlDatabaseContext> options, IConfiguration config)
            : base(options)
        {
            ConnectionString = config.GetConnectionString("MySqlConnection");

            Database.EnsureCreated();
        }

        public override async Task RecreateDatabaseAsync()
        {
            await Database.EnsureDeletedAsync();
            await Database.EnsureCreatedAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(b =>
            {
                var account = new Account()
                {
                    Id = 1,
                    Login = "admin@admin.com",
                    Role = AccountRole.Administrator,
                    Name = "Administrator",
                };
                account.BuildPasswordHash("bbAdmin");

                b.HasData(account);
            });
        }
    }
}

using bigbrother_back.Models.DataModel;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Security.Principal;

namespace bigbrother_back.DataContext
{
    public abstract class DatabaseContext : DbContext
    {
        #region Properties

        public DbSet<Account> Accounts { get; set; } = null!;

        public DbSet<Marker> Markers { get; set; } = null!;

        public DbSet<Place> Places { get; set; } = null!;

        public DbSet<Tag> Tags { get; set; } = null!;

        #endregion

        public DatabaseContext(DbContextOptions options)
          : base(options)
        {
        }

        public abstract Task RecreateDatabaseAsync();
    }
}

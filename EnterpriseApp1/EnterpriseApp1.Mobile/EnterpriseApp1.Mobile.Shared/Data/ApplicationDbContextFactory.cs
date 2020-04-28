using EnterpriseApp1.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.IO;
using Xamarin.Essentials;
using static System.IO.Path;

namespace EnterpriseApp1.Mobile.Shared
{
    public static class ApplicationDbContextFactory
    {
        private const string DbFolderName = "data";
        private const string DatabaseName = "data.db";
        public static ApplicationDbContext Context;

        public static void DatabaseInit()
        {
            if (Context != null) return;

            string dbLocation = Combine(FileSystem.AppDataDirectory, DbFolderName);

            if (!Directory.Exists(dbLocation))
                Directory.CreateDirectory(dbLocation);

            string databasePath = Combine(dbLocation, DatabaseName);

            DbContextOptionsBuilder<ApplicationDbContext> builder = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite($"Filename={databasePath}");

            Context = new ApplicationDbContext(builder.Options);

            if (Context.Database.GetAppliedMigrations().Any())
                Context.Database.Migrate();

            Context.Database.EnsureCreated();
        }
    }
}
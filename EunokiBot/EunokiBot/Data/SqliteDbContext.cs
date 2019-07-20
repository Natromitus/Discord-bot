using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EunokiBot
{
    public class SqliteDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string sDbLocation = Assembly.GetEntryAssembly().Location.Replace(@"EunokiBot.dll", @"Database");
            options.UseSqlite($"Data Source={sDbLocation}DemoDb.db");
        }

    }
}

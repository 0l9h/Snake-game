using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Snake
{
    class SnakeContext: DbContext
    {
        public SnakeContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<Record> Records { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Integrated Security=true; Initial Catalog=SnakeRecordsDB");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Test.Domain.Models;

namespace Test.Domain.Outputs
{
    public class EFDbContext : DbContext
    {
        public DbSet<QueryData> Queries { get; set; }

        public DbSet<ResultData> Results { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }
    }

}

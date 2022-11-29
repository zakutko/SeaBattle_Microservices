using GameHistory.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameHistory.DAL.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Models.GameHistory> GameHistories { get; set; }   
    }
}
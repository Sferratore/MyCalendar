using Microsoft.EntityFrameworkCore;
using MyCalendar.Models;

namespace MyCalendar.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<TaskReminder> TaskReminders { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

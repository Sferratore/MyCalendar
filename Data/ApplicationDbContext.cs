using Microsoft.EntityFrameworkCore;
using MyCalendar.Models;

namespace MyCalendar.Data
{
    public class ApplicationDbContext : DbContext
    {
        /* Constructor.
         * Passing DbContext configuration options via DI.
         */
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        public DbSet<CalendarAnnotation> CalendarAnnotations { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

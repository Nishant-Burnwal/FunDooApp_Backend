using Microsoft.EntityFrameworkCore;
using ModelLayer.Entity;

namespace DatabaseLayer.Data
{
    /*
     * DbContext is a core class in Entity Framework Core.
       It:
     * Represents a session with the database

    1. Allows you to:

    2. Query data
    3. Save data

    4. Track changes

    5. Map C# classes to database tables

    Real-World Analogy

    Imagine:

    DbContext = a generic database engine

    FunDooContext = your specific database setup

    options = the database address + rules
     */
    public class FunDooContext : DbContext
    {
        public FunDooContext(DbContextOptions<FunDooContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}

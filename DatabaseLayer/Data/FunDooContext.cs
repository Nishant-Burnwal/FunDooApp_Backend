using Microsoft.EntityFrameworkCore;
using ModelLayer.Entity;

namespace DatabaseLayer.Data
{
    public class FunDooContext : DbContext
    {
        public FunDooContext(DbContextOptions<FunDooContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        public DbSet<Label> Labels { get; set; }
        public DbSet<NoteLabel> NoteLabels { get; set; }
        public DbSet<Collaborator> Collaborators { get; set; }


    }
}

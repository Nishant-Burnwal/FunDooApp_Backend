using System;
using System.Collections.Generic;

namespace ModelLayer.Entity
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ChangedAt { get; set; }

        // Navigation Properties (for later)
        public ICollection<Note> Notes { get; set; }
    }
}
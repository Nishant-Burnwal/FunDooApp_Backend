using DatabaseLayer.Data;
using DatabaseLayer.Interface;
using ModelLayer.Entity;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseLayer.Repository
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
        private readonly FunDooContext _context;

        public CollaboratorRepository(FunDooContext context)
        {
            _context = context;
        }

        // Add Collaborator
        public Collaborator AddCollaborator(Collaborator collaborator)
        {
            _context.Collaborators.Add(collaborator);
            _context.SaveChanges();
            return collaborator;
        }

        // Get collaborators of a note
        public IEnumerable<Collaborator> GetCollaboratorsByNote(int noteId)
        {
            return _context.Collaborators
                .Where(c => c.NoteId == noteId)
                .ToList();
        }

        // Remove collaborator by ID
        public bool RemoveCollaborator(int collaboratorId, int ownerUserId)
        {
            var collaborator = _context.Collaborators
                .FirstOrDefault(c => c.CollaboratorId == collaboratorId &&
                                     c.OwnerUserId == ownerUserId);

            if (collaborator == null)
                return false;

            _context.Collaborators.Remove(collaborator);
            _context.SaveChanges();
            return true;
        }

        // Remove collaborator by Email
        public bool RemoveCollaboratorByEmail(int noteId, string email, int ownerUserId)
        {
            var collaborator = _context.Collaborators
                .FirstOrDefault(c => c.NoteId == noteId &&
                                     c.CollaboratorEmail == email &&
                                     c.OwnerUserId == ownerUserId);

            if (collaborator == null)
                return false;

            _context.Collaborators.Remove(collaborator);
            _context.SaveChanges();
            return true;
        }

        // Notes shared with user (by email)
        public IEnumerable<Collaborator> GetSharedNotesByEmail(string email)
        {
            return _context.Collaborators
                .Where(c => c.CollaboratorEmail == email)
                .ToList();
        }
    }
}

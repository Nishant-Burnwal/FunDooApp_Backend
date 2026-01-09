using ModelLayer.Entity;
using System.Collections.Generic;

namespace DatabaseLayer.Interface
{
    public interface ICollaboratorRepository
    {
        Collaborator AddCollaborator(Collaborator collaborator);
        IEnumerable<Collaborator> GetCollaboratorsByNote(int noteId);
        bool RemoveCollaborator(int collaboratorId, int ownerUserId);
        bool RemoveCollaboratorByEmail(int noteId, string email, int ownerUserId);
        IEnumerable<Collaborator> GetSharedNotesByEmail(string email);
    }
}

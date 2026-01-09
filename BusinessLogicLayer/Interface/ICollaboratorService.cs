using ModelLayer.DTOs.Collaborators;
using System.Collections.Generic;

namespace BusinessLogicLayer.Interface
{
    public interface ICollaboratorService
    {
        CollaboratorResponseDTO AddCollaborator(
            int noteId,
            string email,
            int ownerUserId
        );

        IEnumerable<CollaboratorResponseDTO> GetCollaborators(int noteId, int ownerUserId);

        bool RemoveCollaborator(int collaboratorId, int ownerUserId);

        bool RemoveCollaboratorByEmail(int noteId, string email, int ownerUserId);

        IEnumerable<int> GetSharedNoteIds(string email);
    }
}

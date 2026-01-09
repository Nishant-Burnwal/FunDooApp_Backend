using ModelLayer.DTOs.Notes;
using System.Collections.Generic;

namespace BusinessLogicLayer.Interface
{
    public interface INoteService
    {
        NoteResponseDTO CreateNote(CreateNoteDTO dto, int userId);
        IEnumerable<NoteResponseDTO> GetAllNotes(int userId);
        NoteResponseDTO UpdateNote(int noteId, UpdateNoteDTO dto, int userId);

        bool PinNote(int noteId, int userId, bool value);
        bool ArchiveNote(int noteId, int userId, bool value);

        bool MoveToTrash(int noteId, int userId);
        bool RestoreFromTrash(int noteId, int userId);
        IEnumerable<NoteResponseDTO> GetTrashedNotes(int userId);

        bool PermanentDelete(int noteId, int userId);
    }
}

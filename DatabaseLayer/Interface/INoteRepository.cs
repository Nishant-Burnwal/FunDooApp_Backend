using ModelLayer.Entity;
using System.Collections.Generic;

namespace DatabaseLayer.Interface
{
    public interface INoteRepository
    {
        // Create / Read
        Note CreateNote(Note note);
        IEnumerable<Note> GetAllNotes(int userId);
        Note GetNoteById(int noteId, int userId);

        // Update
        Note UpdateNote(Note note);
        bool UpdatePin(int noteId, int userId, bool isPin);
        bool UpdateArchive(int noteId, int userId, bool isArchive);

        // Trash & Restore
        bool MoveToTrash(int noteId, int userId);
        bool RestoreFromTrash(int noteId, int userId);
        IEnumerable<Note> GetTrashedNotes(int userId);

        // Permanent delete
        bool PermanentDelete(int noteId, int userId);
    }
}

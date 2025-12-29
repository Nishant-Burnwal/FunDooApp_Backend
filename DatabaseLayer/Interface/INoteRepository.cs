using ModelLayer.Entity;
using System.Collections.Generic;

namespace DatabaseLayer.Interface
{
    public interface INoteRepository
    {
        Note CreateNote(Note note);
        IEnumerable<Note> GetAllNotes(int userId);
        Note GetNoteById(int noteId, int userId);
        Note UpdateNote(Note note);
        bool SoftDeleteNote(int noteId, int userId);
        bool UpdatePin(int noteId, int userId, bool isPin);
        bool UpdateArchive(int noteId, int userId, bool isArchive);
        IEnumerable<Note> GetTrashedNotes(int userId);
        bool RestoreNote(int noteId, int userId);
        bool PermanentDelete(int noteId, int userId);

    }
}

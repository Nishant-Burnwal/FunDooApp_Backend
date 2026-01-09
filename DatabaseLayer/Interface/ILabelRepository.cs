using ModelLayer.Entity;
using System.Collections.Generic;

namespace DatabaseLayer.Interface
{
    public interface ILabelRepository
    {
        // Label CRUD
        Label CreateLabel(Label label);
        IEnumerable<Label> GetLabelsByUser(int userId);
        Label GetLabelById(int labelId, int userId);
        Label UpdateLabel(Label label);
        bool DeleteLabel(int labelId, int userId);

        // Label - Note mapping
        bool AddLabelToNote(int labelId, int noteId, int userId);
        bool RemoveLabelFromNote(int labelId, int noteId, int userId);
        IEnumerable<Note> GetNotesByLabel(int labelId, int userId);
    }
}

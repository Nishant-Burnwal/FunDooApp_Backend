using DatabaseLayer.Data;
using DatabaseLayer.Interface;
using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseLayer.Repository
{
    public class LabelRepository : ILabelRepository
    {
        private readonly FunDooContext _context;

        public LabelRepository(FunDooContext context)
        {
            _context = context;
        }

        // Create Label
        public Label CreateLabel(Label label)
        {
            _context.Labels.Add(label);
            _context.SaveChanges();
            return label;
        }

        // Get all labels of user
        public IEnumerable<Label> GetLabelsByUser(int userId)
        {
            return _context.Labels
                .Where(l => l.UserId == userId)
                .OrderBy(l => l.LabelName)
                .ToList();
        }

        // 🔹 Get label by id
        public Label GetLabelById(int labelId, int userId)
        {
            return _context.Labels
                .FirstOrDefault(l => l.LabelId == labelId && l.UserId == userId);
        }

        // Update label
        public Label UpdateLabel(Label label)
        {
            _context.Labels.Update(label);
            _context.SaveChanges();
            return label;
        }

        // Delete label
        public bool DeleteLabel(int labelId, int userId)
        {
            var label = GetLabelById(labelId, userId);
            if (label == null) return false;

            // Remove mappings first
            var mappings = _context.NoteLabels
                .Where(nl => nl.LabelId == labelId && nl.UserId == userId);

            _context.NoteLabels.RemoveRange(mappings);
            _context.Labels.Remove(label);
            _context.SaveChanges();
            return true;
        }

        // Add label to note
        public bool AddLabelToNote(int labelId, int noteId, int userId)
        {
            bool exists = _context.NoteLabels.Any(nl =>
                nl.LabelId == labelId &&
                nl.NoteId == noteId &&
                nl.UserId == userId);

            if (exists) return false;

            _context.NoteLabels.Add(new NoteLabel
            {
                LabelId = labelId,
                NoteId = noteId,
                UserId = userId
            });

            _context.SaveChanges();
            return true;
        }

        // Remove label from note
        public bool RemoveLabelFromNote(int labelId, int noteId, int userId)
        {
            var mapping = _context.NoteLabels.FirstOrDefault(nl =>
                nl.LabelId == labelId &&
                nl.NoteId == noteId &&
                nl.UserId == userId);

            if (mapping == null) return false;

            _context.NoteLabels.Remove(mapping);
            _context.SaveChanges();
            return true;
        }

        // Get notes by label
        public IEnumerable<Note> GetNotesByLabel(int labelId, int userId)
        {
            return _context.NoteLabels
                .Where(nl => nl.LabelId == labelId && nl.UserId == userId)
                .Join(
                    _context.Notes,
                    nl => nl.NoteId,
                    n => n.NotesId,
                    (nl, n) => n
                )
                .Where(n => !n.IsTrash)
                .ToList();
        }
    }
}

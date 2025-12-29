using DatabaseLayer.Data;
using DatabaseLayer.Interface;
using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseLayer.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly FunDooContext _context;

        public NoteRepository(FunDooContext context)
        {
            _context = context;
        }

        public Note CreateNote(Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
            return note;
        }

        public IEnumerable<Note> GetAllNotes(int userId)
        {
            return _context.Notes
                .Where(n => n.UserId == userId && !n.IsTrash)
                .OrderByDescending(n => n.CreatedAt)
                .ToList();
        }

        public Note GetNoteById(int noteId, int userId)
        {
            return _context.Notes
                .FirstOrDefault(n => n.NotesId == noteId && n.UserId == userId);
        }

        public Note UpdateNote(Note note)
        {
            _context.Notes.Update(note);
            _context.SaveChanges();
            return note;
        }

        public bool SoftDeleteNote(int noteId, int userId)
        {
            var note = GetNoteById(noteId, userId);
            if (note == null) return false;

            note.IsTrash = true;
            note.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return true;
        }
        public bool UpdatePin(int noteId, int userId, bool isPin)
        {
            var note = GetNoteById(noteId, userId);
            if (note == null) return false;

            note.IsPin = isPin;
            note.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return true;
        }

        public bool UpdateArchive(int noteId, int userId, bool isArchive)
        {
            var note = GetNoteById(noteId, userId);
            if (note == null) return false;

            note.IsArchive = isArchive;

            // Google Keep rule: archived notes are not pinned
            if (isArchive)
                note.IsPin = false;

            note.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return true;
        }
        public IEnumerable<Note> GetTrashedNotes(int userId)
        {
            return _context.Notes
                .Where(n => n.UserId == userId && n.IsTrash)
                .OrderByDescending(n => n.UpdatedAt)
                .ToList();
        }

        public bool RestoreNote(int noteId, int userId)
        {
            var note = GetNoteById(noteId, userId);
            if (note == null) return false;

            note.IsTrash = false;
            note.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return true;
        }

        public bool PermanentDelete(int noteId, int userId)
        {
            var note = GetNoteById(noteId, userId);
            if (note == null) return false;

            _context.Notes.Remove(note);
            _context.SaveChanges();
            return true;
        }

    }
}

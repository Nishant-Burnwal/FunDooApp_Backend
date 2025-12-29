using BusinessLogicLayer.Interface;
using DatabaseLayer.Interface;
using ModelLayer.DTOs.Notes;
using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Service
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public NoteResponseDTO CreateNote(CreateNoteDTO dto, int userId)
        {
            var note = new Note
            {
                Title = dto.Title,
                Description = dto.Description,
                Reminder = dto.Reminder,
                Colour = dto.Colour,
                Image = dto.Image,

                IsPin = false,
                IsArchive = false,
                IsTrash = false,

                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserId = userId
            };

            return Map(_noteRepository.CreateNote(note));
        }


        public IEnumerable<NoteResponseDTO> GetAllNotes(int userId)
        {
            return _noteRepository.GetAllNotes(userId).Select(Map);
        }

        public NoteResponseDTO UpdateNote(int noteId, UpdateNoteDTO dto, int userId)
        {
            var note = _noteRepository.GetNoteById(noteId, userId);
            if (note == null) return null;

            note.Title = dto.Title;
            note.Description = dto.Description;
            note.Reminder = dto.Reminder;
            note.Colour = dto.Colour;
            note.UpdatedAt = DateTime.Now;

            return Map(_noteRepository.UpdateNote(note));
        }

        public bool DeleteNote(int noteId, int userId)
        {
            return _noteRepository.SoftDeleteNote(noteId, userId);
        }

        private NoteResponseDTO Map(Note note)
        {
            return new NoteResponseDTO
            {
                NotesId = note.NotesId,
                Title = note.Title,
                Description = note.Description,
                IsPin = note.IsPin,
                IsArchive = note.IsArchive,
                IsTrash = note.IsTrash,
                Colour = note.Colour,
                CreatedAt = note.CreatedAt
            };
        }

        public bool PinNote(int noteId, int userId, bool value)
        {
            return _noteRepository.UpdatePin(noteId, userId, value);
        }

        public bool ArchiveNote(int noteId, int userId, bool value)
        {
            return _noteRepository.UpdateArchive(noteId, userId, value);
        }
        public IEnumerable<NoteResponseDTO> GetTrashedNotes(int userId)
        {
            return _noteRepository.GetTrashedNotes(userId).Select(Map);
        }

        public bool RestoreNote(int noteId, int userId)
        {
            return _noteRepository.RestoreNote(noteId, userId);
        }

        public bool PermanentDelete(int noteId, int userId)
        {
            return _noteRepository.PermanentDelete(noteId, userId);
        }

    }
}

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
        private readonly ICacheService _cacheService;

        private string NotesCacheKey(int userId) => $"notes_user_{userId}";
        private string TrashCacheKey(int userId) => $"notes_user_{userId}_trash";

        public NoteService(INoteRepository noteRepository, ICacheService cacheService)
        {
            _noteRepository = noteRepository;
            _cacheService = cacheService;
        }

        public NoteResponseDTO CreateNote(CreateNoteDTO dto, int userId)
        {
            _cacheService.Remove(NotesCacheKey(userId));

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
            var cacheKey = NotesCacheKey(userId);

            var cached = _cacheService.Get<IEnumerable<NoteResponseDTO>>(cacheKey);
            if (cached != null)
                return cached;

            var notes = _noteRepository.GetAllNotes(userId)
                .Select(Map)
                .ToList();

            _cacheService.Set(cacheKey, notes, TimeSpan.FromMinutes(5));
            return notes;
        }

        public NoteResponseDTO UpdateNote(int noteId, UpdateNoteDTO dto, int userId)
        {
            _cacheService.Remove(NotesCacheKey(userId));

            var note = _noteRepository.GetNoteById(noteId, userId);
            if (note == null) return null;

            note.Title = dto.Title;
            note.Description = dto.Description;
            note.Reminder = dto.Reminder;
            note.Colour = dto.Colour;
            note.UpdatedAt = DateTime.Now;

            return Map(_noteRepository.UpdateNote(note));
        }

        public bool PinNote(int noteId, int userId, bool value)
        {
            _cacheService.Remove(NotesCacheKey(userId));
            return _noteRepository.UpdatePin(noteId, userId, value);
        }

        public bool ArchiveNote(int noteId, int userId, bool value)
        {
            _cacheService.Remove(NotesCacheKey(userId));
            return _noteRepository.UpdateArchive(noteId, userId, value);
        }

        public bool MoveToTrash(int noteId, int userId)
        {
            var result = _noteRepository.MoveToTrash(noteId, userId);
            if (result)
            {
                _cacheService.Remove(NotesCacheKey(userId));
                _cacheService.Remove(TrashCacheKey(userId));
            }
            return result;
        }

        public bool RestoreFromTrash(int noteId, int userId)
        {
            _cacheService.Remove(NotesCacheKey(userId));
            _cacheService.Remove(TrashCacheKey(userId));
            return _noteRepository.RestoreFromTrash(noteId, userId);
        }

        public IEnumerable<NoteResponseDTO> GetTrashedNotes(int userId)
        {
            var cacheKey = TrashCacheKey(userId);

            var cached = _cacheService.Get<IEnumerable<NoteResponseDTO>>(cacheKey);
            if (cached != null)
                return cached;

            var notes = _noteRepository.GetTrashedNotes(userId)
                .Select(Map)
                .ToList();

            _cacheService.Set(cacheKey, notes, TimeSpan.FromMinutes(5));
            return notes;
        }

        public bool PermanentDelete(int noteId, int userId)
        {
            _cacheService.Remove(NotesCacheKey(userId));
            _cacheService.Remove(TrashCacheKey(userId));
            return _noteRepository.PermanentDelete(noteId, userId);
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
    }
}

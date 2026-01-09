using BusinessLogicLayer.Interface;
using DatabaseLayer.Interface;
using ModelLayer.DTOs.Collaborators;
using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Service
{
    public class CollaboratorService : ICollaboratorService
    {
        private readonly ICollaboratorRepository _collaboratorRepository;
        private readonly INoteRepository _noteRepository;

        public CollaboratorService(
            ICollaboratorRepository collaboratorRepository,
            INoteRepository noteRepository)
        {
            _collaboratorRepository = collaboratorRepository;
            _noteRepository = noteRepository;
        }

        // Add collaborator
        public CollaboratorResponseDTO AddCollaborator(
            int noteId,
            string email,
            int ownerUserId)
        {
            // 1. Ensure note belongs to owner
            var note = _noteRepository.GetNoteById(noteId, ownerUserId);
            if (note == null)
                return null;

            // 2️. Prevent duplicate collaborator
            var existing = _collaboratorRepository
                .GetCollaboratorsByNote(noteId)
                .Any(c => c.CollaboratorEmail == email);

            if (existing)
                return null;

            var collaborator = new Collaborator
            {
                NoteId = noteId,
                OwnerUserId = ownerUserId,
                CollaboratorEmail = email,
                CreatedAt = DateTime.Now
            };

            var created = _collaboratorRepository.AddCollaborator(collaborator);

            return Map(created);
        }

        // Get collaborators of a note
        public IEnumerable<CollaboratorResponseDTO> GetCollaborators(
            int noteId,
            int ownerUserId)
        {
            // Validate ownership
            var note = _noteRepository.GetNoteById(noteId, ownerUserId);
            if (note == null)
                return Enumerable.Empty<CollaboratorResponseDTO>();

            return _collaboratorRepository
                .GetCollaboratorsByNote(noteId)
                .Select(Map);
        }

        // Remove collaborator by ID
        public bool RemoveCollaborator(int collaboratorId, int ownerUserId)
        {
            return _collaboratorRepository.RemoveCollaborator(
                collaboratorId,
                ownerUserId
            );
        }

        // Remove collaborator by email
        public bool RemoveCollaboratorByEmail(
            int noteId,
            string email,
            int ownerUserId)
        {
            return _collaboratorRepository.RemoveCollaboratorByEmail(
                noteId,
                email,
                ownerUserId
            );
        }

        //  Get shared note IDs for collaborator
        public IEnumerable<int> GetSharedNoteIds(string email)
        {
            return _collaboratorRepository
                .GetSharedNotesByEmail(email)
                .Select(c => c.NoteId);
        }

        // Mapper
        private CollaboratorResponseDTO Map(Collaborator collaborator)
        {
            return new CollaboratorResponseDTO
            {
                CollaboratorId = collaborator.CollaboratorId,
                NoteId = collaborator.NoteId,
                Email = collaborator.CollaboratorEmail,
                CreatedAt = collaborator.CreatedAt
            };
        }
    }
}

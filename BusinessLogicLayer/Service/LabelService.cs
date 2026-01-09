using BusinessLogicLayer.Interface;
using DatabaseLayer.Interface;
using ModelLayer.DTOs.Labels;
using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogicLayer.Service
{
    public class LabelService : ILabelService
    {
        private readonly ILabelRepository _labelRepository;

        public LabelService(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        // Create Label
        public LabelResponseDTO CreateLabel(CreateLabelDTO dto, int userId)
        {
            var label = new Label
            {
                LabelName = dto.LabelName,
                UserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return Map(_labelRepository.CreateLabel(label));
        }

        // Get All Labels
        public IEnumerable<LabelResponseDTO> GetAllLabels(int userId)
        {
            return _labelRepository
                .GetLabelsByUser(userId)
                .Select(Map);
        }

        // Update Label
        public LabelResponseDTO UpdateLabel(int labelId, UpdateLabelDTO dto, int userId)
        {
            var label = _labelRepository.GetLabelById(labelId, userId);
            if (label == null) return null;

            label.LabelName = dto.LabelName;
            label.UpdatedAt = DateTime.Now;

            return Map(_labelRepository.UpdateLabel(label));
        }

        // Delete Label
        public bool DeleteLabel(int labelId, int userId)
        {
            return _labelRepository.DeleteLabel(labelId, userId);
        }

        // Add Label to Note
        public bool AddLabelToNote(int labelId, int noteId, int userId)
        {
            return _labelRepository.AddLabelToNote(labelId, noteId, userId);
        }

        // Remove Label from Note
        public bool RemoveLabelFromNote(int labelId, int noteId, int userId)
        {
            return _labelRepository.RemoveLabelFromNote(labelId, noteId, userId);
        }

        // Get NoteIds by Label
        public IEnumerable<int> GetNoteIdsByLabel(int labelId, int userId)
        {
            return _labelRepository
                .GetNotesByLabel(labelId, userId)
                .Select(n => n.NotesId);
        }

        // Mapper
        private LabelResponseDTO Map(Label label)
        {
            return new LabelResponseDTO
            {
                LabelId = label.LabelId,
                LabelName = label.LabelName,
                CreatedAt = label.CreatedAt
            };
        }
    }
}

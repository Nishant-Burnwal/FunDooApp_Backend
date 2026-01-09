using ModelLayer.DTOs.Labels;
using System.Collections.Generic;

namespace BusinessLogicLayer.Interface
{
    public interface ILabelService
    {
        LabelResponseDTO CreateLabel(CreateLabelDTO dto, int userId);
        IEnumerable<LabelResponseDTO> GetAllLabels(int userId);
        LabelResponseDTO UpdateLabel(int labelId, UpdateLabelDTO dto, int userId);
        bool DeleteLabel(int labelId, int userId);

        bool AddLabelToNote(int labelId, int noteId, int userId);
        bool RemoveLabelFromNote(int labelId, int noteId, int userId);
        IEnumerable<int> GetNoteIdsByLabel(int labelId, int userId);
    }
}

using BusinessLogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs.Labels;
using ModelLayer.Utility;
using System.Security.Claims;

namespace FunDoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LabelsController : ControllerBase
    {
        private readonly ILabelService _labelService;

        public LabelsController(ILabelService labelService)
        {
            _labelService = labelService;
        }

        // Get UserId from JWT
        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        // 1️. Create Label
        [HttpPost]
        public IActionResult CreateLabel(CreateLabelDTO dto)
        {
            var label = _labelService.CreateLabel(dto, GetUserId());

            return Ok(new ApiResponse<LabelResponseDTO>
            {
                Success = true,
                Message = "Label created successfully",
                Data = label
            });
        }

        // 2️. Get All Labels of User
        [HttpGet]
        public IActionResult GetAllLabels()
        {
            var labels = _labelService.GetAllLabels(GetUserId());

            return Ok(new ApiResponse<IEnumerable<LabelResponseDTO>>
            {
                Success = true,
                Message = "Labels fetched successfully",
                Data = labels
            });
        }

        // 3️. Update Label
        [HttpPut("{labelId}")]
        public IActionResult UpdateLabel(int labelId, UpdateLabelDTO dto)
        {
            var label = _labelService.UpdateLabel(labelId, dto, GetUserId());

            if (label == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Label not found"
                });
            }

            return Ok(new ApiResponse<LabelResponseDTO>
            {
                Success = true,
                Message = "Label updated successfully",
                Data = label
            });
        }

        // 4️. Delete Label
        [HttpDelete("{labelId}")]
        public IActionResult DeleteLabel(int labelId)
        {
            bool result = _labelService.DeleteLabel(labelId, GetUserId());

            if (!result)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Label not found"
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Label deleted successfully"
            });
        }

        // 5️. Add Label to Note
        [HttpPost("{labelId}/notes/{noteId}")]
        public IActionResult AddLabelToNote(int labelId, int noteId)
        {
            bool result = _labelService.AddLabelToNote(labelId, noteId, GetUserId());

            if (!result)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Label already added or invalid note/label"
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Label added to note"
            });
        }

        // 6️. Remove Label from Note
        [HttpDelete("{labelId}/notes/{noteId}")]
        public IActionResult RemoveLabelFromNote(int labelId, int noteId)
        {
            bool result = _labelService.RemoveLabelFromNote(labelId, noteId, GetUserId());

            if (!result)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Label-note mapping not found"
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Label removed from note"
            });
        }

        // 7️. Get Notes by Label (returns noteIds)
        [HttpGet("{labelId}/notes")]
        public IActionResult GetNotesByLabel(int labelId)
        {
            var noteIds = _labelService.GetNoteIdsByLabel(labelId, GetUserId());

            return Ok(new ApiResponse<IEnumerable<int>>
            {
                Success = true,
                Message = "Notes fetched for label",
                Data = noteIds
            });
        }
    }
}

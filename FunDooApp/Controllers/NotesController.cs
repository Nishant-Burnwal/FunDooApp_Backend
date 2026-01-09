using BusinessLogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs.Notes;
using ModelLayer.Utility;
using System.Security.Claims;

namespace FunDoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        // Extract UserId from JWT
        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        // Create Note
        [HttpPost]
        public IActionResult Create(CreateNoteDTO dto)
        {
            var note = _noteService.CreateNote(dto, GetUserId());

            return Ok(new ApiResponse<NoteResponseDTO>
            {
                Success = true,
                Message = "Note created successfully",
                Data = note
            });
        }

        // Get all active notes
        [HttpGet]
        public IActionResult GetAll()
        {
            var notes = _noteService.GetAllNotes(GetUserId());

            return Ok(new ApiResponse<IEnumerable<NoteResponseDTO>>
            {
                Success = true,
                Message = "Notes fetched successfully",
                Data = notes
            });
        }

        // Update note
        [HttpPut("note/{noteId}")]
        public IActionResult Update(int noteId, UpdateNoteDTO dto)
        {
            var note = _noteService.UpdateNote(noteId, dto, GetUserId());
            if (note == null)
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Note not found"
                });

            return Ok(new ApiResponse<NoteResponseDTO>
            {
                Success = true,
                Message = "Note updated successfully",
                Data = note
            });
        }

        // Move note to trash
        [HttpPut("{noteId}/trash")]
        public IActionResult MoveToTrash(int noteId)
        {
            bool result = _noteService.MoveToTrash(noteId, GetUserId());
            if (!result)
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Note not found"
                });

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Note moved to trash"
            });
        }

        // Get trashed notes
        [HttpGet("trash")]
        public IActionResult GetTrashedNotes()
        {
            var notes = _noteService.GetTrashedNotes(GetUserId());

            return Ok(new ApiResponse<IEnumerable<NoteResponseDTO>>
            {
                Success = true,
                Message = "Trashed notes fetched successfully",
                Data = notes
            });
        }

        // Restore note from trash
        [HttpPut("{noteId}/restore")]
        public IActionResult RestoreFromTrash(int noteId)
        {
            bool result = _noteService.RestoreFromTrash(noteId, GetUserId());
            if (!result)
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Note not found"
                });

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Note restored successfully"
            });
        }

        // Permanent delete
        [HttpDelete("{noteId}/permanent")]
        public IActionResult PermanentDelete(int noteId)
        {
            bool result = _noteService.PermanentDelete(noteId, GetUserId());
            if (!result)
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Note not found"
                });

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Note permanently deleted"
            });
        }

        // Pin / Unpin
        [HttpPatch("{noteId}/pin")]
        public IActionResult Pin(int noteId, UpdateNoteStateDTO dto)
        {
            bool result = _noteService.PinNote(noteId, GetUserId(), dto.Value);
            if (!result)
                return NotFound();

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = dto.Value ? "Note pinned" : "Note unpinned"
            });
        }

        // Archive / Unarchive
        [HttpPatch("{noteId}/archive")]
        public IActionResult Archive(int noteId, UpdateNoteStateDTO dto)
        {
            bool result = _noteService.ArchiveNote(noteId, GetUserId(), dto.Value);
            if (!result)
                return NotFound();

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = dto.Value ? "Note archived" : "Note unarchived"
            });
        }
    }
}

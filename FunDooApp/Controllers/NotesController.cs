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

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        [HttpPost]
        public IActionResult Create(CreateNoteDTO dto)
        {
            var note = _noteService.CreateNote(dto, GetUserId());
            return Ok(new ApiResponse<NoteResponseDTO>
            {
                Success = true,
                Message = "Note created",
                Data = note
            });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var notes = _noteService.GetAllNotes(GetUserId());
            return Ok(new ApiResponse<IEnumerable<NoteResponseDTO>>
            {
                Success = true,
                Message = "Notes fetched",
                Data = notes
            });
        }

        [HttpPut("{noteId}")]
        public IActionResult Update(int noteId, UpdateNoteDTO dto)
        {
            var note = _noteService.UpdateNote(noteId, dto, GetUserId());
            if (note == null) return NotFound();

            return Ok(new ApiResponse<NoteResponseDTO>
            {
                Success = true,
                Message = "Note updated",
                Data = note
            });
        }

        [HttpDelete("{noteId}")]
        public IActionResult Delete(int noteId)
        {
            var result = _noteService.DeleteNote(noteId, GetUserId());
            if (!result) return NotFound();

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Note moved to trash"
            });
        }

        [HttpPatch("{noteId}/pin")]
        public IActionResult Pin(int noteId, UpdateNoteStateDTO dto)
        {
            bool result = _noteService.PinNote(noteId, GetUserId(), dto.Value);
            if (!result) return NotFound();

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = dto.Value ? "Note pinned" : "Note unpinned"
            });
        }

        [HttpPatch("{noteId}/archive")]
        public IActionResult Archive(int noteId, UpdateNoteStateDTO dto)
        {
            bool result = _noteService.ArchiveNote(noteId, GetUserId(), dto.Value);
            if (!result) return NotFound();

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = dto.Value ? "Note archived" : "Note unarchived"
            });
        }
        [HttpGet("trash")]
        public IActionResult GetTrash()
        {
            var notes = _noteService.GetTrashedNotes(GetUserId());

            return Ok(new ApiResponse<IEnumerable<NoteResponseDTO>>
            {
                Success = true,
                Message = "Trashed notes fetched",
                Data = notes
            });
        }
        [HttpPatch("{noteId}/restore")]
        public IActionResult Restore(int noteId)
        {
            bool result = _noteService.RestoreNote(noteId, GetUserId());
            if (!result) return NotFound();

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Note restored"
            });
        }
        [HttpDelete("{noteId}/permanent")]
        public IActionResult PermanentDelete(int noteId)
        {
            bool result = _noteService.PermanentDelete(noteId, GetUserId());
            if (!result) return NotFound();

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Note permanently deleted"
            });
        }

    }
}

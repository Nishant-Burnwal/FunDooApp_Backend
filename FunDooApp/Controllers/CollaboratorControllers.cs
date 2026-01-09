using BusinessLogicLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs.Collaborators;
using ModelLayer.Utility;
using System.Security.Claims;

namespace FunDoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CollaboratorsController : ControllerBase
    {
        private readonly ICollaboratorService _collaboratorService;

        public CollaboratorsController(ICollaboratorService collaboratorService)
        {
            _collaboratorService = collaboratorService;
        }

        // Get UserId from JWT
        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        // 1️. Add collaborator to a note
        [HttpPost("{noteId}")]
        public IActionResult AddCollaborator(
            int noteId,
            AddCollaboratorDTO dto)
        {
            var result = _collaboratorService.AddCollaborator(
                noteId,
                dto.Email,
                GetUserId()
            );

            if (result == null)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Unable to add collaborator"
                });
            }

            return Ok(new ApiResponse<CollaboratorResponseDTO>
            {
                Success = true,
                Message = "Collaborator added successfully",
                Data = result
            });
        }

        // 2️. Get collaborators of a note
        [HttpGet("{noteId}")]
        public IActionResult GetCollaborators(int noteId)
        {
            var collaborators = _collaboratorService
                .GetCollaborators(noteId, GetUserId());

            return Ok(new ApiResponse<IEnumerable<CollaboratorResponseDTO>>
            {
                Success = true,
                Message = "Collaborators fetched successfully",
                Data = collaborators
            });
        }

        // 3️. Remove collaborator by ID
        [HttpDelete("by-id/{collaboratorId}")]
        public IActionResult RemoveCollaborator(int collaboratorId)
        {
            bool result = _collaboratorService.RemoveCollaborator(
                collaboratorId,
                GetUserId()
            );

            if (!result)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Collaborator not found"
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Collaborator removed successfully"
            });
        }

        // 4️. Remove collaborator by Email
        [HttpDelete("{noteId}/{email}")]
        public IActionResult RemoveCollaboratorByEmail(
            int noteId,
            string email)
        {
            bool result = _collaboratorService.RemoveCollaboratorByEmail(
                noteId,
                email,
                GetUserId()
            );

            if (!result)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Collaborator not found"
                });
            }

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Collaborator removed successfully"
            });
        }

        // 5️. Get notes shared with logged-in user
        [HttpGet("shared-notes")]
        public IActionResult GetSharedNotes()
        {
            var noteIds = _collaboratorService
                .GetSharedNoteIds(User.FindFirst(ClaimTypes.Email).Value);

            return Ok(new ApiResponse<IEnumerable<int>>
            {
                Success = true,
                Message = "Shared notes fetched successfully",
                Data = noteIds
            });
        }
    }
}
